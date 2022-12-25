using System.Collections;
using UnityEngine;

/// <summary> Playerが物をつかむ動きを制御するクラス </summary>
public class PlayerGrab : MonoBehaviour
{
    Animator _playerAnim = default;
    PlayerState _pState = default;

    //*****関連*****
    [Header("腕を動かすスピード"), SerializeField, Range(0f, 0.05f)] float _upDownSpeed = 0;
    //***右手***
    [Header("右手用の移動先"), SerializeField] Transform _rightHandTarget = default;
    [Header("右手のPositionに対するウェイト")] float _rightPositionWeight = 0;
    [Header("右手のRotationに対するウェイト")] float _rightRotationWeight = 0;
    //***左手***
    [Header("左手用の移動先"), SerializeField] Transform _leftHandTarget = default;
    [Header("左手のPositionに対するウェイト")] float _leftPositionWeight = 0;
    [Header("左手のRotationに対するウェイト")] float _leftRotationWeight = 0;

    private void Start()
    {
        _pState =  GetComponent<PlayerState>();
        _playerAnim = GetComponent<Animator>();
    }
    private void Update()
    {
        //***左腕を上げる***
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
        ///***右腕を上げる***
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
        //***右手のIK設定***
        _playerAnim.SetIKPositionWeight(AvatarIKGoal.RightHand, _rightPositionWeight);
        _playerAnim.SetIKRotationWeight(AvatarIKGoal.RightHand, _rightRotationWeight);
        _playerAnim.SetIKPosition(AvatarIKGoal.RightHand, _rightHandTarget.position);
        _playerAnim.SetIKRotation(AvatarIKGoal.RightHand, _rightHandTarget.rotation);
        //***左手のIK設定***
        _playerAnim.SetIKPositionWeight(AvatarIKGoal.LeftHand, _leftPositionWeight);
        _playerAnim.SetIKRotationWeight(AvatarIKGoal.LeftHand, _leftRotationWeight);
        _playerAnim.SetIKPosition(AvatarIKGoal.LeftHand, _leftHandTarget.position);
        _playerAnim.SetIKRotation(AvatarIKGoal.LeftHand, _leftHandTarget.rotation);
    }

    /// <summary> 指定した値にウェイトを step ずつ変更する </summary>
    /// <param name="targetWeight"></param>
    /// <param name="step"></param>
    /// <returns></returns>
    IEnumerator ChangeWeightRoutine(string whichiHand, float targetWeight, float step)
    {
        //*****右腕*****
        if (whichiHand == "Right")
        {
            if (_rightPositionWeight < targetWeight)
            {
                //***上げるとき***
                while (_rightPositionWeight < targetWeight)
                {
                    _rightPositionWeight += step;
                    _rightRotationWeight = _rightPositionWeight;
                    yield return null;
                    if (Input.GetKeyUp(KeyCode.E))  //途中でキーを離したらコルーチンをやめる
                    {
                        yield break;
                    }
                }
            }
            else
            {
                //***下げるとき***
                while (_rightPositionWeight > targetWeight)
                {
                    _rightPositionWeight -= step;
                    _rightRotationWeight = _rightPositionWeight;
                    yield return null;
                    if (Input.GetKeyDown(KeyCode.E))    //途中でキーを押したらコルーチンをやめる
                    {
                        yield break;
                    }
                }
            }
        }
        //*****左腕*****
        else if (whichiHand == "Left")
        {
            if (_leftPositionWeight < targetWeight)
            {
                //***上げるとき***
                while (_leftPositionWeight < targetWeight)
                {
                    _leftPositionWeight += step;
                    _leftRotationWeight = _leftPositionWeight;
                    yield return null;
                    if (Input.GetKeyUp(KeyCode.Q))  //途中でキーを離したらコルーチンをやめる
                    {
                        yield break;
                    }
                }
            }
            else
            {
                //***下げるとき***
                while (_leftPositionWeight > targetWeight)
                {
                    _leftPositionWeight -= step;
                    _leftRotationWeight = _leftPositionWeight;
                    yield return null;
                    if (Input.GetKeyDown(KeyCode.Q))    //途中でキーを押したらコルーチンをやめる
                    {
                        yield break;
                    }
                }
            }

        }
    }

}
