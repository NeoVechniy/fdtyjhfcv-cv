using UnityEngine;

public class StationController : MonoBehaviour
{
    [SerializeField] private float _stationHP;
    [SerializeField] private float _rotationSpeed;

    void Update()
    {
        transform.Rotate(0, _rotationSpeed * Time.deltaTime, 0);
    }

    public void GetDamage(float damage)
    {
        _stationHP -= damage;
    }

    public float GetHP()
    {
        return _stationHP;
    }
}
