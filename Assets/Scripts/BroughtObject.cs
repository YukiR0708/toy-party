using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BroughtObject : MonoBehaviour
{
    [SerializeField] GameObject _rHand = default;
    [SerializeField] GameObject _lHand = default;
    GameObject _defaultParent = default;

    private void Start()
    {
        _defaultParent = transform.parent.gameObject;
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player" && transform.parent.gameObject == _defaultParent)
        {
            if (Input.GetKey(KeyCode.E))
            {
                transform.SetParent(_rHand.transform);
            }
            else if (Input.GetKey(KeyCode.Q))
            {
                transform.SetParent(_lHand.transform);
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.E))
        {
            if (transform.parent.gameObject == _rHand)
            {
                transform.SetParent(_defaultParent.transform);
            }
        }
        else if (Input.GetKeyUp(KeyCode.Q))
        {
            if (transform.parent.gameObject == _lHand)
            {
                transform.SetParent(_defaultParent.transform);
            }
        }



    }
}
