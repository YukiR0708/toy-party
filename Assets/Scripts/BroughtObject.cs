using UnityEngine;

/// <summary> Playerに掴まれる物を制御するクラス </summary>
public class BroughtObject : MonoBehaviour
{
    [Header("つながる先")]
    [Tooltip("右手"), SerializeField] GameObject _rHand = default;
    [Tooltip("左手"), SerializeField] GameObject _lHand = default;
    [Tooltip("左手とつながるためのFixedJoint")] FixedJoint _leftFJ = default;
    [Tooltip("右手とつながるためのFixedJoint")] FixedJoint _rightFJ = default;
    Rigidbody _rb = default;
    PlayerState _pState = default;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _pState = other.gameObject.GetComponent<PlayerState>();

            //*****Playerにつかまれたとき*****
            //***右手***
            if (_pState.pState.HasFlag(PlayerState.PlayerStatus.RGrab) && !_rightFJ)
            {
                _rightFJ = this.gameObject.AddComponent<FixedJoint>();  //FixedJointをアタッチする
                _rightFJ.connectedBody = _rHand.GetComponent<Rigidbody>();　//手とJointする
                _rb.Sleep();    //Playerが振り回されないよう、RigidBodyを切る
                gameObject.layer = LayerMask.NameToLayer("BroughtObjects"); //Playerとコライダーが衝突しないようにする
            }
            //***左手***
            else if (_pState.pState.HasFlag(PlayerState.PlayerStatus.LGrab) && !_leftFJ)
            {
                _leftFJ = this.gameObject.AddComponent<FixedJoint>();
                _leftFJ.connectedBody = _lHand.GetComponent<Rigidbody>();
                _rb.Sleep();
                gameObject.layer = LayerMask.NameToLayer("BroughtObjects");
            }
        }
    }

    private void Update()
    {
        if (_pState)
        {
            //*****持っていた状態から手をおろした or 投げたら、手から離れるようにする*****
            //***右手***
            if (_rightFJ && (!_pState.pState.HasFlag(PlayerState.PlayerStatus.RGrab) || _pState.pState.HasFlag(PlayerState.PlayerStatus.Throw)))
            {
                _rb.WakeUp();   //止めていたRigidBodyを起動する
                Destroy(_rightFJ);  //Jointを絶つ
                gameObject.layer = LayerMask.NameToLayer("Default");    //Playerとコライダーが当たるように戻す
            }
            //***左手***
            if (_leftFJ && (!_pState.pState.HasFlag(PlayerState.PlayerStatus.LGrab) || _pState.pState.HasFlag(PlayerState.PlayerStatus.Throw)))
            {
                _rb.WakeUp();
                Destroy(_leftFJ);
                gameObject.layer = LayerMask.NameToLayer("Default");
            }

        }
    }
}
