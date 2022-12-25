using System.Collections;
using UnityEngine;

/// <summary> Player���������ޓ����𐧌䂷��N���X </summary>
public class PlayerGrab : MonoBehaviour
{
    Animator _playerAnim = default;
    PlayerState _pState = default;

    //*****�֘A*****
    [Header("�r�𓮂����X�s�[�h"), SerializeField, Range(0f, 0.05f)] float _upDownSpeed = 0;
    //***�E��***
    [Header("�E��p�̈ړ���"), SerializeField] Transform _rightHandTarget = default;
    [Header("�E���Position�ɑ΂���E�F�C�g")] float _rightPositionWeight = 0;
    [Header("�E���Rotation�ɑ΂���E�F�C�g")] float _rightRotationWeight = 0;
    //***����***
    [Header("����p�̈ړ���"), SerializeField] Transform _leftHandTarget = default;
    [Header("�����Position�ɑ΂���E�F�C�g")] float _leftPositionWeight = 0;
    [Header("�����Rotation�ɑ΂���E�F�C�g")] float _leftRotationWeight = 0;

    private void Start()
    {
        _pState =  GetComponent<PlayerState>();
        _playerAnim = GetComponent<Animator>();
    }
    private void Update()
    {
        //***���r���グ��***
        if (Input.GetKeyDown(KeyCode.Q))
        {
            _pState.pState |= PlayerState.PlayerStatus.LGrab;
            StartCoroutine(ChangeWeightRoutine("Left", 1f, _upDownSpeed));
        }
        else if (Input.GetKeyUp(KeyCode.Q))
        {
            _pState.pState &= ~PlayerState.PlayerStatus.LGrab;
            StartCoroutine(ChangeWeightRoutine("Left", 0f, _upDownSpeed));
        }
        ///***�E�r���グ��***
        if (Input.GetKeyDown(KeyCode.E))
        {
            _pState.pState |= PlayerState.PlayerStatus.RGrab;
            StartCoroutine(ChangeWeightRoutine("Right", 1f, _upDownSpeed));
        }
        else if (Input.GetKeyUp(KeyCode.E))
        {
            _pState.pState &= ~PlayerState.PlayerStatus.RGrab;
            StartCoroutine(ChangeWeightRoutine("Right", 0f, _upDownSpeed));
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
