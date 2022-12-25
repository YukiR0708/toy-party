using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// シーンを切り替える機能を提供する。
/// プレイヤーがシーンを切り替えるトリガーに接触したら、
/// シーン名と移動先となる Transform の名前を指定すると、
/// 指定したシーンに切り替わり、プレイヤーは指定した Transform の場所に移動する。
/// </summary>
public class SceneSwitcher : MonoBehaviour
{
    /// <summary>移動先のシーン名</summary>
    [SerializeField] string _sceneName = "";
    /// <summary>移動先の Transform のオブジェクト名</summary>
    [SerializeField] string _pointName = "";

    /// <summary>
    /// シーンを切り替える
    /// </summary>
    /// <param name="sceneName">ロードするシーン名</param>
    /// <param name="pointName">シーンをロードした時に、プレイヤーはこの名前の Transform に移動する</param>
    /// <param name="direction">プレイヤーの向いている方向。シーンが切り替わった時にこの方向を維持する</param>
    void SwitchScene(string sceneName, string pointName, Vector3 direction)
    {
        // 方向と移動先を保存する
        SingletonSystem.Instance.PlayerDirection = direction;
        SingletonSystem.Instance.PointNameOnSceneLoaded = pointName;
        // シーンを読み込んで切り替える
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
