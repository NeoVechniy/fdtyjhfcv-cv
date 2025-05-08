using UnityEngine;

[RequireComponent (typeof(Rigidbody))]

public class EnemyController : MonoBehaviour
{
    private Rigidbody _rb;

    [SerializeField] private Transform _player;
    [SerializeField] private Transform _target;

    [SerializeField] private float _speed;
    [SerializeField] private float _searchRadius;
    [SerializeField] private float _safeDistance;
    [SerializeField] public float _HP;

    private float _playerDistance;
    private float _targetDistance;

    private Vector3 _direction;
    private bool _isPlayer;

    private LineRenderer _lineRenderer1;
    private LineRenderer _lineRenderer2;
    private Ray _ray;
    private RaycastHit _raycastHit;
    private GameObject _shootPoint1;
    private GameObject _shootPoint2;

    private float _shootTime;
    [SerializeField] private float _shootDeltaTime = 1;
    private bool _isCharged = true;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.useGravity = false;
        _rb.drag = 1;
        _rb.angularDrag = 1;

        _speed = 1000f;
        _searchRadius = 100f;
        _safeDistance = 20f;

        _isCharged = true;
        _shootTime = 0;

        _shootPoint1 = transform.GetChild(0).gameObject;
        _shootPoint2 = transform.GetChild(1).gameObject;

        _lineRenderer1 = _shootPoint1.GetComponent<LineRenderer>();
        _lineRenderer2 = _shootPoint2.GetComponent<LineRenderer>();

        _lineRenderer1.positionCount = 2;
        _lineRenderer1.startWidth = 0.1f;
        _lineRenderer1.endWidth = 0.1f;

        _lineRenderer2.positionCount = 2;
        _lineRenderer2.startWidth = 0.1f;
        _lineRenderer2.endWidth = 0.1f;
    }

    void Update()
    {
        if (!_isCharged)
        {
            _shootTime += Time.deltaTime;
            if (_shootTime >= _shootDeltaTime)
            {
                _isCharged = true;
                _shootTime = 0;
            }
        }

        if (_HP <= 0)
        {
            DestroyShip();
        }

        _playerDistance = Vector3.Distance(transform.position, _player.position);
        _targetDistance = Vector3.Distance(transform.position, _target.position);

        if (_playerDistance < _searchRadius || _targetDistance < _searchRadius)
        {
            Vector3 targetPosition = LaserShoot();

            if (targetPosition != Vector3.zero)
            {
                _lineRenderer1.enabled = true;
                _lineRenderer1.SetPosition(0, _shootPoint1.transform.position);
                _lineRenderer1.SetPosition(1, targetPosition);

                _lineRenderer2.enabled = true;
                _lineRenderer2.SetPosition(0, _shootPoint2.transform.position);
                _lineRenderer2.SetPosition(1, targetPosition);
            }
            else
            {
                _lineRenderer1.enabled = false;
                _lineRenderer2.enabled = false;
            }
        }

        if (_playerDistance < _searchRadius)
        {
            _isPlayer = true;
        }
        else
        {
            _isPlayer = false;
        }
    }

    private void FixedUpdate()
    {
        if (_isPlayer)
        {
            _direction = _player.position - transform.position;
            if (_playerDistance > _safeDistance)
            {
                _rb.AddForce(transform.forward * _speed * Time.fixedDeltaTime);
            }
        }
        else
        {
            _direction = _target.position - transform.position;
            if (_targetDistance > _safeDistance)
            {
                _rb.AddForce(transform.forward * _speed * Time.fixedDeltaTime);
            }
        }

        transform.rotation = Quaternion.Slerp(transform.rotation,
            Quaternion.LookRotation(_direction), 0.1f);
    }

    public float GetHP()
    {
        return _HP;
    }

    public void GetDamage(float damage)
    {
        _HP -= damage;
    }

    public void DestroyShip()
    {
        Destroy(gameObject);
    }

    private Vector3 LaserShoot()
    {
        _ray = new Ray(_shootPoint1.transform.position, _shootPoint1.transform.forward);
        Debug.DrawRay(_ray.origin, _ray.direction * 1000);

        if (Physics.Raycast(_ray, out _raycastHit))
        {
            if (_raycastHit.collider.gameObject.tag == "Enemy" && _isCharged)
            {
                _isCharged = false;
                _raycastHit.collider.gameObject.GetComponent<EnemyController>().GetDamage(Random.Range(1, 10));
            }
            else if (_raycastHit.collider.gameObject.tag == "Player" && _isCharged)
            {
                _raycastHit.collider.gameObject.GetComponent<ShipController>().GetDamage(Random.Range(1, 10));
                _isCharged = false;
            }
            else if (_raycastHit.collider.gameObject.tag == "Station" && _isCharged)
            {
                _raycastHit.collider.gameObject.GetComponentInParent<StationController>().GetDamage(Random.Range(1, 10));
                _isCharged = false;
            }
            return _raycastHit.point;
        }
        else
        {
            return Vector3.zero;
        }
    }
}
