using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class ShipController : MonoBehaviour
{
    [SerializeField] private float _force = 1000f;
    [SerializeField] private float _rotationSpeed = 10f;
    [SerializeField] private float _maxSpeed = 100f;
    [SerializeField] private float _HP = 100f;

    [SerializeField] private Rigidbody _rb;

    private float _verticalInput; //вперед назад
    private float _horizontalInput; //вправо влево рыскание
    private float _rollInput; //вправо влево крен
    private float _pitchInput; //вверх вниз

    private bool _cursorVisible = false;

    [SerializeField] private ParticleSystem _particleSystem;
    private ParticleSystem.EmissionModule _emissionModule;
    void Start()
    {
        
        Cursor.visible = _cursorVisible;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift)) 
        {
            _verticalInput = 1;
        }
        else
        {
            _verticalInput = 0;
        }
        
        if (Input.GetKey(KeyCode.E))
        {
            _horizontalInput = 1;
        }
        else if (Input.GetKey(KeyCode.Q))
        {
            _horizontalInput = -1;
        }
        else 
        {
            _horizontalInput = 0; 
        }

        _rollInput = Input.GetAxis("Horizontal");
        _pitchInput = Input.GetAxis("Vertical");

        if (_cursorVisible)
        {
            Cursor.visible = _cursorVisible;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.visible = _cursorVisible;
            Cursor.lockState = CursorLockMode.Locked;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _cursorVisible = !_cursorVisible;
        }

        _emissionModule.rateOverTime = Mathf.Lerp(10, 10, _rb.velocity.magnitude);

    }

    private void FixedUpdate()
    {
        _rb.AddTorque(transform.up * _horizontalInput * 0.2f * _rotationSpeed * 3 * Time.fixedDeltaTime, ForceMode.VelocityChange);
        _rb.AddTorque(transform.forward * -_rollInput * _rotationSpeed * Time.fixedDeltaTime, ForceMode.VelocityChange);
        _rb.AddTorque(transform.right * -_pitchInput * _rotationSpeed * Time.fixedDeltaTime, ForceMode.VelocityChange);

        if (_verticalInput > 0)
        {
            _rb.AddForce(transform.forward * _force * _verticalInput);
        }
        if (_verticalInput < 0)
        {
            _rb.AddForce(transform.forward * _force * 0.3f * _verticalInput);
        }
    }

    public float GetHP()
    {
        return _HP;
    }

    public void GetDamage(float damage)
    {
        _HP -= damage;
    }

}
