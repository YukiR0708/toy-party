using System;
using UnityEngine;

/// <summary> Player�̏�ԊǗ��p�̗񋓌^ </summary>
public class PlayerState : MonoBehaviour
{
    [Tooltip("Player�̏��")] public PlayerStatus pState = PlayerStatus.Idle;

    [Flags]
    public enum PlayerStatus
    {
        Idle = 1 << 0,  //��~
        Move = 1 << 1,�@//�ړ�
        Jump = 1 << 2,�@//�W�����v
        RGrab = 1 << 3,�@//�E��ł���
        LGrab = 1 << 4, //����ł���
        Attack = 1 << 5, //�U��
        Throw = 1 << 6, //������
    }

}
