using UnityEngine;

/// <summary> Player�ɒ͂܂�镨�𐧌䂷��N���X </summary>
public class BroughtObject : MonoBehaviour
{
    [Header("�Ȃ����")]
    [Tooltip("�E��"), SerializeField] GameObject _rHand = default;
    [Tooltip("����"), SerializeField] GameObject _lHand = default;
    [Tooltip("����ƂȂ��邽�߂�FixedJoint")] FixedJoint _leftFJ = default;
    [Tooltip("�E��ƂȂ��邽�߂�FixedJoint")] FixedJoint _rightFJ = default;
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

            //*****Player�ɂ��܂ꂽ�Ƃ�*****
            //***�E��***
            if (_pState.pState.HasFlag(PlayerState.PlayerStatus.RGrab) && !_rightFJ)
            {
                _rightFJ = this.gameObject.AddComponent<FixedJoint>();  //FixedJoint���A�^�b�`����
                _rightFJ.connectedBody = _rHand.GetComponent<Rigidbody>();�@//���Joint����
                _rb.Sleep();    //Player���U��񂳂�Ȃ��悤�ARigidBody��؂�
                gameObject.layer = LayerMask.NameToLayer("BroughtObjects"); //Player�ƃR���C�_�[���Փ˂��Ȃ��悤�ɂ���
            }
            //***����***
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
            //*****�����Ă�����Ԃ��������낵�� or ��������A�肩�痣���悤�ɂ���*****
            //***�E��***
            if (_rightFJ && (!_pState.pState.HasFlag(PlayerState.PlayerStatus.RGrab) || _pState.pState.HasFlag(PlayerState.PlayerStatus.Throw)))
            {
                _rb.WakeUp();   //�~�߂Ă���RigidBody���N������
                Destroy(_rightFJ);  //Joint����
                gameObject.layer = LayerMask.NameToLayer("Default");    //Player�ƃR���C�_�[��������悤�ɖ߂�
            }
            //***����***
            if (_leftFJ && (!_pState.pState.HasFlag(PlayerState.PlayerStatus.LGrab) || _pState.pState.HasFlag(PlayerState.PlayerStatus.Throw)))
            {
                _rb.WakeUp();
                Destroy(_leftFJ);
                gameObject.layer = LayerMask.NameToLayer("Default");
            }

        }
    }
}
