using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// �Q�[�����Ǘ�����R���|�[�l���g�B�V���O���g�� �p�^�[���ō���Ă���B
/// �ĂԎ��́ASingletonSystem.Instance.�i���\�b�h/�v���p�e�B�j �̂悤�ɂ��ČĂԂ��ƁB
/// </summary>
public class SingletonSystem : MonoBehaviour
{
    /// <summary>�C���X�^���X���擾���邽�߂̃p�u���b�N�ϐ�</summary>
    public static SingletonSystem Instance = default;
    /// <summary>�V�[�����؂�ւ�������̃v���C���[�̌���</summary>
    Vector3 _playerDirection = Vector3.forward;
    /// <summary>�V�[�����؂�ւ�������Ƀv���C���[���ړ����� Transform �̖��O</summary>
    string _pointNameOnSceneLoaded = "";
    //GameObject _player = default;


    /// <summary>
    /// �V�[�����؂�ւ�鎞�̃v���C���[�̌���
    /// </summary>
    public Vector3 PlayerDirection
    {
        get { return _playerDirection; }
        set { _playerDirection = value; }
    }

    /// <summary>
    /// �V�[�����؂�ւ�������Ƀv���C���[�̏����ʒu�ƂȂ� Transform �̖��O
    /// </summary>
    public string PointNameOnSceneLoaded
    {
        get { return _pointNameOnSceneLoaded; }
        set { _pointNameOnSceneLoaded = value; }
    }

    void Awake()//�V�X�e��������Start����ɂ��Ă����Ȃ��ƍ���
    {
        // ���̏����� Start() �ɏ����Ă��悢���AAwake() �ɏ������Ƃ������B
        // �Q�l: �C�x���g�֐��̎��s���� https://docs.unity3d.com/ja/2019.4/Manual/ExecutionOrder.html
        if (Instance)
        {
            // �C���X�^���X�����ɂ���ꍇ�́A�j������
            Debug.LogWarning($"SingletonSystem �̃C���X�^���X�͊��ɑ��݂���̂ŁA{gameObject.name} �͔j�����܂��B");
            Destroy(this.gameObject);
        }
        else
        {
            // ���̃N���X�̃C���X�^���X�����������ꍇ�́A������ DontDestroyOnload �ɒu��
            Instance = this;
            SceneManager.sceneLoaded += OnSceneLoaded;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    /// <summary>
    /// �V�[�������[�h���ꂽ���ɌĂԁB
    /// �V�[�����؂�ւ�������̃v���C���[�̏ꏊ�ƌ����𐧌䂷��B
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="mode"></param>
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        if (this.PointNameOnSceneLoaded != "")
        {
            var point = GameObject.Find(this.PointNameOnSceneLoaded);

            if (player)
            {
                player.transform.position = point.transform.position;
            }
            else
            {
                Debug.LogError("Player ��������܂���B");
            }
        }

        if (player)
        {
            player.transform.forward = this.PlayerDirection;
        }
    }

}
