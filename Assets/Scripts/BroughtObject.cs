using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BroughtObject : MonoBehaviour
{
    [SerializeField] GameObject _rHand = default;
    [SerializeField] GameObject _lHand = default;
    FixedJoint _leftFJ = default;
    FixedJoint _rightFJ = default;
    Rigidbody _rb = default;
    PlayerState _pState = default;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            _pState = other.gameObject.GetComponent<PlayerState>();

            if (_pState.pState.HasFlag(PlayerState.PlayerStatus.RGrab) && !_rightFJ)
            {
                _rightFJ = this.gameObject.AddComponent<FixedJoint>();
                _rightFJ.connectedBody = _rHand.GetComponent<Rigidbody>();
                _rb.Sleep();
                gameObject.layer = LayerMask.NameToLayer("BroughtObjects");
            }
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
            if (!_pState.pState.HasFlag(PlayerState.PlayerStatus.RGrab) && _rightFJ)
            {
                _rb.WakeUp();
                Destroy(_rightFJ);
                gameObject.layer = LayerMask.NameToLayer("Default");
            }

            if (!_pState.pState.HasFlag(PlayerState.PlayerStatus.LGrab) && _leftFJ)
            {
                _rb.WakeUp();
                Destroy(_leftFJ);
                gameObject.layer = LayerMask.NameToLayer("Default");
            }

        }
    }
}
