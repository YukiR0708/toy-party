using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// ゲームを管理するコンポーネント。シングルトン パターンで作られている。
/// 呼ぶ時は、SingletonSystem.Instance.（メソッド/プロパティ） のようにして呼ぶこと。
/// </summary>
public class SingletonSystem : MonoBehaviour
{
    /// <summary>インスタンスを取得するためのパブリック変数</summary>
    public static SingletonSystem Instance = default;
    /// <summary>シーンが切り替わった時のプレイヤーの向き</summary>
    Vector3 _playerDirection = Vector3.forward;
    /// <summary>シーンが切り替わった時にプレイヤーが移動する Transform の名前</summary>
    string _pointNameOnSceneLoaded = "";
    //GameObject _player = default;


    /// <summary>
    /// シーンが切り替わる時のプレイヤーの向き
    /// </summary>
    public Vector3 PlayerDirection
    {
        get { return _playerDirection; }
        set { _playerDirection = value; }
    }

    /// <summary>
    /// シーンが切り替わった時にプレイヤーの初期位置となる Transform の名前
    /// </summary>
    public string PointNameOnSceneLoaded
    {
        get { return _pointNameOnSceneLoaded; }
        set { _pointNameOnSceneLoaded = value; }
    }

    void Awake()//システムだからStartより先にいてもらわないと困る
    {
        // この処理は Start() に書いてもよいが、Awake() に書くことが多い。
        // 参考: イベント関数の実行順序 https://docs.unity3d.com/ja/2019.4/Manual/ExecutionOrder.html
        if (Instance)
        {
            // インスタンスが既にある場合は、破棄する
            Debug.LogWarning($"SingletonSystem のインスタンスは既に存在するので、{gameObject.name} は破棄します。");
            Destroy(this.gameObject);
        }
        else
        {
            // このクラスのインスタンスが無かった場合は、自分を DontDestroyOnload に置く
            Instance = this;
            SceneManager.sceneLoaded += OnSceneLoaded;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    /// <summary>
    /// シーンがロードされた時に呼ぶ。
    /// シーンが切り替わった時のプレイヤーの場所と向きを制御する。
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
                Debug.LogError("Player が見つかりません。");
            }
        }

        if (player)
        {
            player.transform.forward = this.PlayerDirection;
        }
    }

}
