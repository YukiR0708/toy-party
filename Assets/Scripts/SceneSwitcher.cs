using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// �V�[����؂�ւ���@�\��񋟂���B
/// �v���C���[���V�[����؂�ւ���g���K�[�ɐڐG������A
/// �V�[�����ƈړ���ƂȂ� Transform �̖��O���w�肷��ƁA
/// �w�肵���V�[���ɐ؂�ւ��A�v���C���[�͎w�肵�� Transform �̏ꏊ�Ɉړ�����B
/// </summary>
public class SceneSwitcher : MonoBehaviour
{
    /// <summary>�ړ���̃V�[����</summary>
    [SerializeField] string _sceneName = "";
    /// <summary>�ړ���� Transform �̃I�u�W�F�N�g��</summary>
    [SerializeField] string _pointName = "";

    /// <summary>
    /// �V�[����؂�ւ���
    /// </summary>
    /// <param name="sceneName">���[�h����V�[����</param>
    /// <param name="pointName">�V�[�������[�h�������ɁA�v���C���[�͂��̖��O�� Transform �Ɉړ�����</param>
    /// <param name="direction">�v���C���[�̌����Ă�������B�V�[�����؂�ւ�������ɂ��̕������ێ�����</param>
    void SwitchScene(string sceneName, string pointName, Vector3 direction)
    {
        // �����ƈړ����ۑ�����
        SingletonSystem.Instance.PlayerDirection = direction;
        SingletonSystem.Instance.PointNameOnSceneLoaded = pointName;
        // �V�[����ǂݍ���Ő؂�ւ���
        SceneManager.LoadScene(sceneName);
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            SwitchScene(_sceneName, _pointName, collision.gameObject.transform.forward);
        }
    }
}
