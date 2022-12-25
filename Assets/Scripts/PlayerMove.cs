using System;
using System.Collections;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{

    PlayerState _pState = default;
    //*****移動＆ジャンプ関連*****
    Rigidbody _playerRb = default;
    Animator _playerAnim = default;
    [SerializeField, Header("移動スピード")] float _speed = default;
    [Tooltip("左右入力")] float _hInput = default;
    [Tooltip("前後入力")] float _vInput = default;
    [SerializeField, Header("ジャンプ力")] float _jumpForce = default;
    [SerializeField, Header("着地判定")] bool _onGround = default;

    void Start()
    {
        _playerRb = GetComponent<Rigidbody>();
        _playerAnim = GetComponent<Animator>();
        _pState = GetComponent<PlayerState>();
    }

    private void Update()
    {
        _hInput = Input.GetAxisRaw("Horizontal");
        _vInput = Input.GetAxisRaw("Vertical");
    }

    void LateUpdate()
    {
        //*****cameraの前方を常にPlayerの正面として移動する*****
        //↓カメラのローカル空間のベクトルをワールド空間のベクトルへ変換
        Vector3 pForward = Camera.main.transform.TransformDirection(Vector3.forward);
        Vector3 pRight = Camera.main.transform.TransformDirection(Vector3.right);

        if (_playerAnim)
        {
            _playerAnim.SetFloat("speed", Mathf.Abs(_hInput) + Mathf.Abs(_vInput)); //AnimatorControllerに移動速度を渡す

            //***移動***
            Vector3 playerDir = (_hInput * pRight + _vInput * pForward) * _speed * Time.deltaTime;  //移動方向を決定
            playerDir.y = 0;

            if (0 < MathF.Abs(playerDir.x) + Math.Abs(playerDir.z)) //pStateを変更
            {
                _pState.pState |= PlayerState.PlayerStatus.Move;
            }
            else
            {
                _pState.pState &= ~PlayerState.PlayerStatus.Move;
            }

            Quaternion playerRot = Camera.main.transform.rotation;
            playerRot.x = 0;
            playerRot.z = 0;
            transform.rotation = Quaternion.Slerp(transform.rotation, playerRot, 1.0f); // カメラの正面を向く
            _playerRb.AddForce(playerDir);

            //***攻撃***
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                _pState.pState |= PlayerState.PlayerStatus.Attack;
                _playerAnim.SetTrigger("attack");
            }
            else if(Input.GetKeyUp(KeyCode.LeftShift))
            {
                _pState.pState &= ~PlayerState.PlayerStatus.Attack;
            }

            //***ジャンプ***
            if (_onGround && Input.GetKeyDown(KeyCode.Space))
            {
                _pState.pState |= PlayerState.PlayerStatus.Jump;
                _playerAnim.SetBool("jump", true);
                _playerRb.AddForce(Vector2.up * _jumpForce);
                _onGround = false;
            }

        }
    }
    ///// <summary> 着地判定のフラグ処理/// </summary>
    ///// <param name="other"></param>
    private void OnCollisionEnter(Collision other)
    {
        var name = other.gameObject.tag;
        if (name == "Ground" || name == "Box")
        {
            _pState.pState &= ~PlayerState.PlayerStatus.Jump;
            _playerAnim.SetBool("jump", false);
            _onGround = true;
        }
    }
}