using System;
using System.Collections;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{

    PlayerState _pState = default;
    //*****�ړ����W�����v�֘A*****
    Rigidbody _playerRb = default;
    Animator _playerAnim = default;
    [SerializeField, Header("�ړ��X�s�[�h")] float _speed = default;
    [Tooltip("���E����")] float _hInput = default;
    [Tooltip("�O�����")] float _vInput = default;
    [SerializeField, Header("�W�����v��")] float _jumpForce = default;
    [SerializeField, Header("���n����")] bool _onGround = default;

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
        //*****camera�̑O�������Player�̐��ʂƂ��Ĉړ�����*****
        //���J�����̃��[�J����Ԃ̃x�N�g�������[���h��Ԃ̃x�N�g���֕ϊ�
        Vector3 pForward = Camera.main.transform.TransformDirection(Vector3.forward);
        Vector3 pRight = Camera.main.transform.TransformDirection(Vector3.right);

        if (_playerAnim)
        {
            _playerAnim.SetFloat("speed", Mathf.Abs(_hInput) + Mathf.Abs(_vInput)); //AnimatorController�Ɉړ����x��n��

            //***�ړ�***
            Vector3 playerDir = (_hInput * pRight + _vInput * pForward) * _speed * Time.deltaTime;  //�ړ�����������
            playerDir.y = 0;

            if (0 < MathF.Abs(playerDir.x) + Math.Abs(playerDir.z)) //pState��ύX
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
            transform.rotation = Quaternion.Slerp(transform.rotation, playerRot, 1.0f); // �J�����̐��ʂ�����
            _playerRb.AddForce(playerDir);

            //***�U��***
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                _pState.pState |= PlayerState.PlayerStatus.Attack;
                _playerAnim.SetTrigger("attack");
            }
            else if(Input.GetKeyUp(KeyCode.LeftShift))
            {
                _pState.pState &= ~PlayerState.PlayerStatus.Attack;
            }

            //***�W�����v***
            if (_onGround && Input.GetKeyDown(KeyCode.Space))
            {
                _pState.pState |= PlayerState.PlayerStatus.Jump;
                _playerAnim.SetBool("jump", true);
                _playerRb.AddForce(Vector2.up * _jumpForce);
                _onGround = false;
            }

        }
    }
    ///// <summary> ���n����̃t���O����/// </summary>
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