using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //*****�ړ����W�����v�֘A*****
    Rigidbody _playerRb = default;
    Animator _playerAnim = default;
    [SerializeField, Header("�ړ��X�s�[�h")] float _speed = default;
    [Tooltip("���E����")] float _hInput = default;
    [Tooltip("�O�����")] float _vInput = default;
    [SerializeField, Header("�W�����v��")] float _jumpForce = default;
    [SerializeField, Header("���n����")] bool _onGround = default;


    //*****�r�̓����iIK�j�֘A*****
    [Header("�r�𓮂����X�s�[�h"), SerializeField, Range(0f, 0.05f)] float _upDownSpeed = 0;
    //***�E��***
    [Header("�E��p�̋�I�u�W�F�N�g"), SerializeField] Transform _rightHandTarget = default;
    [SerializeField, Header("�E���Position�ɑ΂���E�F�C�g")] float _rightPositionWeight = 0;
    [Header("�E���Rotation�ɑ΂���E�F�C�g")] float _rightRotationWeight = 0;
    //***����***
    [Header("����p�̋�I�u�W�F�N�g"), SerializeField] Transform _leftHandTarget = default;
    [Header("�����Position�ɑ΂���E�F�C�g")] float _leftPositionWeight = 0;
    [Header("�����Rotation�ɑ΂���E�F�C�g")] float _leftRotationWeight = 0;


    void Start()
    {
        _playerRb = GetComponent<Rigidbody>();
        _playerAnim = GetComponent<Animator>();
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
            Vector3 playerDir = (_hInput * pRight + _vInput * pForward) * _speed * Time.deltaTime;
            playerDir.y = 0;
            Quaternion playerRot = Camera.main.transform.rotation;
            playerRot.x = 0;
            playerRot.z = 0;
            transform.rotation = Quaternion.Slerp(transform.rotation, playerRot, 1.0f); // �J�����̐��ʂ�����
            _playerRb.AddForce(playerDir);

            //***�U��***
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                _playerAnim.SetTrigger("attack");
            }

            //***�W�����v***
            if (_onGround && Input.GetKeyDown(KeyCode.Space))
            {
                _playerAnim.SetTrigger("jump");
                _playerRb.AddForce(Vector2.up * _jumpForce);
                _onGround = false;
            }

            //***���r���グ��***
            if (Input.GetKeyDown(KeyCode.Q))
            {
                StartCoroutine(ChangeWeightRoutine("Left", 1f, _upDownSpeed));
            }
            else if (Input.GetKeyUp(KeyCode.Q))
            {
                StartCoroutine(ChangeWeightRoutine("Left", 0f, _upDownSpeed));
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                StartCoroutine(ChangeWeightRoutine("Right", 1f, _upDownSpeed));
            }
            else if (Input.GetKeyUp(KeyCode.E))
            {
                StartCoroutine(ChangeWeightRoutine("Right", 0f, _upDownSpeed));
            }
        }
    }
    ///// <summary> ���n����̃t���O����/// </summary>
    ///// <param name="other"></param>
    private void OnCollisionEnter(Collision other)
    {
        var name = other.gameObject.tag;
        if (name == "Ground" || name == "Stairs" || name == "Box")
        {
            _onGround = true;
        }
    }

    private void OnAnimatorIK(int layerIndex)
    {
        //***�E���IK�ݒ�***
        _playerAnim.SetIKPositionWeight(AvatarIKGoal.RightHand, _rightPositionWeight);
        _playerAnim.SetIKRotationWeight(AvatarIKGoal.RightHand, _rightRotationWeight);
        _playerAnim.SetIKPosition(AvatarIKGoal.RightHand, _rightHandTarget.position);
        _playerAnim.SetIKRotation(AvatarIKGoal.RightHand, _rightHandTarget.rotation);
        //***�����IK�ݒ�***
        _playerAnim.SetIKPositionWeight(AvatarIKGoal.LeftHand, _leftPositionWeight);
        _playerAnim.SetIKRotationWeight(AvatarIKGoal.LeftHand, _leftRotationWeight);
        _playerAnim.SetIKPosition(AvatarIKGoal.LeftHand, _leftHandTarget.position);
        _playerAnim.SetIKRotation(AvatarIKGoal.LeftHand, _leftHandTarget.rotation);
    }

    /// <summary> �w�肵���l�ɃE�F�C�g�� step ���ύX���� </summary>
    /// <param name="targetWeight"></param>
    /// <param name="step"></param>
    /// <returns></returns>
    IEnumerator ChangeWeightRoutine(string whichiHand, float targetWeight, float step)
    {
        //*****�E�r*****
        if (whichiHand == "Right")
        {
            if (_rightPositionWeight < targetWeight)
            {
                //***�グ��Ƃ�***
                while (_rightPositionWeight < targetWeight)
                {
                    _rightPositionWeight += step;
                    _rightRotationWeight = _rightPositionWeight;
                    yield return null;
                    if (Input.GetKeyUp(KeyCode.E))  //�r���ŃL�[�𗣂�����R���[�`������߂�
                    {
                        yield break;
                    }
                }
            }
            else
            {
                //***������Ƃ�***
                while (_rightPositionWeight > targetWeight)
                {
                    _rightPositionWeight -= step;
                    _rightRotationWeight = _rightPositionWeight;
                    yield return null;
                    if (Input.GetKeyDown(KeyCode.E))    //�r���ŃL�[����������R���[�`������߂�
                    {
                        yield break;
                    }
                }
            }
        }
        //*****���r*****
        else if (whichiHand == "Left")
        {
            if (_leftPositionWeight < targetWeight)
            {
                //***�グ��Ƃ�***
                while (_leftPositionWeight < targetWeight)
                {
                    _leftPositionWeight += step;
                    _leftRotationWeight = _leftPositionWeight;
                    yield return null;
                    if (Input.GetKeyUp(KeyCode.Q))  //�r���ŃL�[�𗣂�����R���[�`������߂�
                    {
                        yield break;
                    }
                }
            }
            else
            {
                //***������Ƃ�***
                while (_leftPositionWeight > targetWeight)
                {
                    _leftPositionWeight -= step;
                    _leftRotationWeight = _leftPositionWeight;
                    yield return null;
                    if (Input.GetKeyDown(KeyCode.Q))    //�r���ŃL�[����������R���[�`������߂�
                    {
                        yield break;
                    }
                }
            }

        }
    }



}