using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    ///*****�ړ����W�����v�֘A*****
    Rigidbody _playerRb = default;
    Animator _playerAnim = default;
    [SerializeField, Header("�ړ��X�s�[�h")] float _speed = default;
    [Tooltip("���E����")] float _hInput = default;
    [Tooltip("�O�����")] float _vInput = default;
    [SerializeField, Header("�W�����v��")] float _jumpForce = default;
    [SerializeField, Header("���n����")] bool _onGround = default;


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
        //*****Player��TPS�ړ�(camera�̑O������ɐ��ʂƂ���)����*****
        //���J�����̃��[�J����Ԃ̃x�N�g�������[���h��Ԃ̃x�N�g���֕ϊ�
        Vector3 pForward = Camera.main.transform.TransformDirection(Vector3.forward);
        Vector3 pRight = Camera.main.transform.TransformDirection(Vector3.right);

        if (_playerAnim)
        {
            _playerAnim.SetFloat("speed", Mathf.Abs(_hInput * _speed) + Mathf.Abs(_vInput * _speed)); //AnimatorController�Ɉړ����x��n��

            //*****�ړ� & �U��*****
            Vector3 playerDir = (_hInput * pRight + _vInput * pForward) * _speed * Time.deltaTime;
            playerDir.y = 0;
            _playerRb.AddForce(playerDir);

            //�U��
            if (Input.GetKeyDown(KeyCode.Space))
            {
                //_playerRb.AddForce(Vector2.up * _jumpForce);
                _playerAnim.SetTrigger("attack");
                //_onGround = false;
            }
        }

    }
    ///// <summary> ���n����̃t���O����/// </summary>
    ///// <param name="other"></param>
    //private void OnCollisionEnter(Collision other)
    //{
    //    if (other.gameObject.tag == "Ground")
    //    {
    //        _onGround = true;
    //    }
    //}
}