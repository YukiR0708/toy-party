using UnityEngine;

/// <summary> Playerが物を投げる動きを制御するクラス </summary>
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
        //*****投げられる物に接触したら、それについているThrownObjectを取得する*****
        if (other.gameObject.CompareTag("Toy"))
        {
            _throwObj = other.gameObject.GetComponent<ThrownObject>();

        }

    }

    private void OnCollisionExit(Collision other)
    {
        //*****投げられる物と接触しなくなったら、ThrownObjectの取得を解除する*****
        if (other.gameObject.CompareTag("Toy") && LayerMask.LayerToName(other.gameObject.layer) == "Default")   //レイヤーが別なとき(持っているとき)は解除しない
        {
            _throwObj = null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //*****物をつかんでるときに左クリックで投げる*****
        if ((_pState.pState.HasFlag(PlayerState.PlayerStatus.LGrab) || _pState.pState.HasFlag(PlayerState.PlayerStatus.RGrab)) && _throwObj)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                //構えるモーションをする
                //軌道のマーカーをだす
            }

            if (Input.GetButtonUp("Fire1"))
            {
                _pState.pState |= PlayerState.PlayerStatus.Throw; 
                _throwObj.Thrown(); //投げる
            }

        }

    }
}
