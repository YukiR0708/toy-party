using UnityEngine;

/// <summary> Playerに投げられる物を制御するクラス </summary>
public class ThrownObject : MonoBehaviour
{
    [Header("投げる速度"), SerializeField] float _throwSpeed = default;
    [Header("投げる角度"), SerializeField] float _throwTheta = default;
    Rigidbody _rb = default;
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }
    public void Thrown()
    {
        //*****カメラの正面方向に向かって投げるようにする*****
        Vector3 _forward = Camera.main.transform.TransformDirection(Vector3.forward); //カメラの正面方向をとる
        Vector3 _up = Camera.main.transform.TransformDirection(Vector3.up);　//カメラの上方向をとる
        _rb.velocity = _forward * _throwSpeed * Mathf.Cos(_throwTheta) + _up * _throwSpeed * Mathf.Sin(_throwTheta);    //斜方投射する
    }
}
