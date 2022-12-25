using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> Player‚Ìó‘ÔŠÇ——p‚Ì—ñ‹“Œ^ </summary>
public class PlayerState : MonoBehaviour
{
    public PlayerStatus pState = PlayerStatus.Idle;

    [Flags]
    public enum PlayerStatus
    {
        Idle = 1 << 0,
        Move = 1 << 1,
        Jump = 1 << 2,
        RGrab = 1 << 3,
        LGrab = 1 << 4,
        Attack = 1 << 5,
    }

}
