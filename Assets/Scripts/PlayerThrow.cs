using UnityEngine;

/// <summary> Player�����𓊂��铮���𐧌䂷��N���X </summary>
public class PlayerThrow : MonoBehaviour
{
    PlayerState _pState = default;
    [SerializeField] ThrownObject _throwObj = default;

    private void Start()
    {
        _pState = GetComponent<PlayerState>();
    }
    private void OnCollisionEnter(Collision other)
    {
        //*****�������镨�ɐڐG������A����ɂ��Ă���ThrownObject���擾����*****
        if (other.gameObject.CompareTag("Toy"))
        {
            _throwObj = other.gameObject.GetComponent<ThrownObject>();

        }

    }

    private void OnCollisionExit(Collision other)
    {
        //*****�������镨�ƐڐG���Ȃ��Ȃ�����AThrownObject�̎擾����������*****
        if (other.gameObject.CompareTag("Toy") && LayerMask.LayerToName(other.gameObject.layer) == "Default")   //���C���[���ʂȂƂ�(�����Ă���Ƃ�)�͉������Ȃ�
        {
            _throwObj = null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //*****��������ł�Ƃ��ɍ��N���b�N�œ�����*****
        if ((_pState.pState.HasFlag(PlayerState.PlayerStatus.LGrab) || _pState.pState.HasFlag(PlayerState.PlayerStatus.RGrab)) && _throwObj)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                //�\���郂�[�V����������
                //�O���̃}�[�J�[������
            }

            if (Input.GetButtonUp("Fire1"))
            {
                _pState.pState |= PlayerState.PlayerStatus.Throw; 
                _throwObj.Thrown(); //������
            }

        }

    }
}
