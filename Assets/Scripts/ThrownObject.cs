using UnityEngine;

/// <summary> Player�ɓ������镨�𐧌䂷��N���X </summary>
public class ThrownObject : MonoBehaviour
{
    [Header("�����鑬�x"), SerializeField] float _throwSpeed = default;
    [Header("������p�x"), SerializeField] float _throwTheta = default;
    Rigidbody _rb = default;
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }
    public void Thrown()
    {
        //*****�J�����̐��ʕ����Ɍ������ē�����悤�ɂ���*****
        Vector3 _forward = Camera.main.transform.TransformDirection(Vector3.forward); //�J�����̐��ʕ������Ƃ�
        Vector3 _up = Camera.main.transform.TransformDirection(Vector3.up);�@//�J�����̏�������Ƃ�
        _rb.velocity = _forward * _throwSpeed * Mathf.Cos(_throwTheta) + _up * _throwSpeed * Mathf.Sin(_throwTheta);    //�Ε����˂���
    }
}
