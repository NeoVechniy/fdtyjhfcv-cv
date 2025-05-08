using UnityEngine;
using UnityEngine.UI;

public class EnemyUIController : MonoBehaviour
{
    private Camera mainCamera;
    [SerializeField] private Slider _hpBar;
    [SerializeField] private GameObject _enemyUI;

    private void Start()
    {
        mainCamera = Camera.main;
        _hpBar.maxValue = gameObject.GetComponent<EnemyController>().GetHP();
        _hpBar.value = _hpBar.maxValue;
    }
    void Update()
    {
        _enemyUI.transform.LookAt(_enemyUI.transform.position + mainCamera.transform.rotation * Vector3.forward,
                       mainCamera.transform.rotation * Vector3.up);

        _hpBar.value = gameObject.GetComponent<EnemyController>().GetHP();
    }
}
