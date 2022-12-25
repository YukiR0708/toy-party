using System;
using UnityEngine;

/// <summary> Playerの状態管理用の列挙型 </summary>
public class PlayerState : MonoBehaviour
{
    [Tooltip("Playerの状態")] public PlayerStatus pState = PlayerStatus.Idle;

    [Flags]
    public enum PlayerStatus
    {
        Idle = 1 << 0,  //停止
        Move = 1 << 1,　//移動
        Jump = 1 << 2,　//ジャンプ
        RGrab = 1 << 3,　//右手でつかむ
        LGrab = 1 << 4, //左手でつかむ
        Attack = 1 << 5, //攻撃
        Throw = 1 << 6, //投げる
    }

}
