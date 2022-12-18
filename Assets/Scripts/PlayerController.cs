using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    ///*****移動＆ジャンプ関連*****
    Rigidbody _playerRb = default;
    Animator _playerAnim = default;
    [SerializeField, Header("移動スピード")] float _speed = default;
    [Tooltip("左右入力")] float _hInput = default;
    [Tooltip("前後入力")] float _vInput = default;
    [SerializeField, Header("ジャンプ力")] float _jumpForce = default;
    [SerializeField, Header("着地判定")] bool _onGround = default;


    void Start()
    {
        _playerRb = gameObject.GetComponent<Rigidbody>();
        _playerAnim = gameObject.GetComponent<Animator>();
    }

    private void Update()
    {
        _hInput = Input.GetAxisRaw("Horizontal");
        _vInput = Input.GetAxisRaw("Vertical");


    }

    void LateUpdate()
    {
        //*****PlayerのTPS移動(cameraの前方を常に正面とする)処理*****
        //↓カメラのローカル空間のベクトルをワールド空間のベクトルへ変換
        Vector3 pForward = Camera.main.transform.TransformDirection(Vector3.forward);
        Vector3 pRight = Camera.main.transform.TransformDirection(Vector3.right);

        if (_playerAnim)
        {
            _playerAnim.SetFloat("speed", Mathf.Abs(_hInput * _speed) + Mathf.Abs(_vInput * _speed)); //AnimatorControllerに移動速度を渡す

            //*****移動 & 攻撃*****
            Vector3 playerDir = (_hInput * pRight + _vInput * pForward) * _speed * Time.deltaTime;
            playerDir.y = 0;
            _playerRb.AddForce(playerDir);

            //攻撃
            if (Input.GetKeyDown(KeyCode.Space))
            {
                //_playerRb.AddForce(Vector2.up * _jumpForce);
                _playerAnim.SetTrigger("attack");
                //_onGround = false;
            }
        }

    }
    ///// <summary> 着地判定のフラグ処理/// </summary>
    ///// <param name="other"></param>
    //private void OnCollisionEnter(Collision other)
    //{
    //    if (other.gameObject.tag == "Ground")
    //    {
    //        _onGround = true;
    //    }
    //}
}