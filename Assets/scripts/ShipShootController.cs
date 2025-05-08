using UnityEngine;

public class ShipShootController : MonoBehaviour
{
    private LineRenderer _lineRenderer1;
    private LineRenderer _lineRenderer2;

    private Ray _ray;
    private RaycastHit _raycastHit;

    private GameObject _shootPoint1;
    private GameObject _shootPoint2;

    private float _shootTime;
    private float _laserTime;
    [SerializeField] private float _shootDeltaTime = 1;
    [SerializeField] private float _laserDeltaTime = 1;
    [SerializeField] private float _laserRechargeTime = 1;
    private bool _isCharged = true;
    private bool _isLaserVisible = true;

    void Start()
    {
        _isCharged = true;
        _isLaserVisible = true;

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

    // Update is called once per frame
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
        if (!_isLaserVisible)
        {
            _laserTime += Time.deltaTime;
            if (_laserTime >= _laserRechargeTime)
            {
                _isLaserVisible = true;
                _laserTime = 0;
            }
        }

        if (Input.GetMouseButton(0) && _isLaserVisible)
        {
            Vector3 targetPosition = LaserShoot();

            if (targetPosition != Vector3.zero)
            {
                _laserTime += Time.deltaTime;
                if (_laserTime >= _laserDeltaTime)
                {
                    _isLaserVisible = false;
                    _laserTime = 0;
                }

                _lineRenderer1.enabled = true;
                _lineRenderer1.SetPosition(0, _shootPoint1.transform.position);
                _lineRenderer1.SetPosition(1, targetPosition);

                _lineRenderer2.enabled = true;
                _lineRenderer2.SetPosition(0, _shootPoint2.transform.position);
                _lineRenderer2.SetPosition(1, targetPosition);
            }
        }
        else
        {
            _lineRenderer1.enabled = false;
            _lineRenderer2.enabled = false;
        }
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
                _raycastHit.collider.gameObject.GetComponent<EnemyController>().GetDamage(Random.Range(5, 10));
            }
            return _raycastHit.point;
        }
        else
        {
            return transform.position + transform.forward * 100;
        }
    }
}
