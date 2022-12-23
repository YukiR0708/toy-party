using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //*****移動＆ジャンプ関連*****
    Rigidbody _playerRb = default;
    Animator _playerAnim = default;
    [SerializeField, Header("移動スピード")] float _speed = default;
    [Tooltip("左右入力")] float _hInput = default;
    [Tooltip("前後入力")] float _vInput = default;
    [SerializeField, Header("ジャンプ力")] float _jumpForce = default;
    [SerializeField, Header("着地判定")] bool _onGround = default;


    //*****腕の動き（IK）関連*****
    [Header("腕を動かすスピード"), SerializeField, Range(0f, 0.05f)] float _upDownSpeed = 0;
    //***右手***
    [Header("右手用の空オブジェクト"), SerializeField] Transform _rightHandTarget = default;
    [SerializeField, Header("右手のPositionに対するウェイト")] float _rightPositionWeight = 0;
    [Header("右手のRotationに対するウェイト")] float _rightRotationWeight = 0;
    //***左手***
    [Header("左手用の空オブジェクト"), SerializeField] Transform _leftHandTarget = default;
    [Header("左手のPositionに対するウェイト")] float _leftPositionWeight = 0;
    [Header("左手のRotationに対するウェイト")] float _leftRotationWeight = 0;


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
        //*****cameraの前方を常にPlayerの正面として移動する*****
        //↓カメラのローカル空間のベクトルをワールド空間のベクトルへ変換
        Vector3 pForward = Camera.main.transform.TransformDirection(Vector3.forward);
        Vector3 pRight = Camera.main.transform.TransformDirection(Vector3.right);

        if (_playerAnim)
        {
            _playerAnim.SetFloat("speed", Mathf.Abs(_hInput) + Mathf.Abs(_vInput)); //AnimatorControllerに移動速度を渡す

            //***移動***
            Vector3 playerDir = (_hInput * pRight + _vInput * pForward) * _speed * Time.deltaTime;
            playerDir.y = 0;
            Quaternion playerRot = Camera.main.transform.rotation;
            playerRot.x = 0;
            playerRot.z = 0;
            transform.rotation = Quaternion.Slerp(transform.rotation, playerRot, 1.0f); // カメラの正面を向く
            _playerRb.AddForce(playerDir);

            //***攻撃***
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                _playerAnim.SetTrigger("attack");
            }

            //***ジャンプ***
            if (_onGround && Input.GetKeyDown(KeyCode.Space))
            {
                _playerAnim.SetTrigger("jump");
                _playerRb.AddForce(Vector2.up * _jumpForce);
                _onGround = false;
            }

            //***左腕を上げる***
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
    ///// <summary> 着地判定のフラグ処理/// </summary>
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