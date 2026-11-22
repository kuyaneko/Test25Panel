using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;


public class SystemCtrl : MonoBehaviour
{
    private static SceneCtrl.Scene start_scene_id = SceneCtrl.Scene.TITLE;

    [SerializeField]
    private GameObject[] debug_menu_;

    public int debug_info { get; private set; } = 0;
#if UNITY_EDITOR

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
	private static void Initialize()
	{
		Scene scene = SceneManager.GetActiveScene();

		Debug.Log("BeforeSceneLoad( path " + scene.path + ", name " + scene.name + ", index " + scene.buildIndex + " )");

		if( ! scene.path.StartsWith( "Assets/Scenes/" ) )
		{
			return;
		}

		if( scene.name == "" || scene.buildIndex == SceneManager.sceneCountInBuildSettings )
		{
			start_scene_id = SceneCtrl.Scene.NONE;
		}
		else if( scene.name != "System" )
		{
			start_scene_id = (SceneCtrl.Scene)scene.buildIndex;

			SceneManager.UnloadSceneAsync( "Scenes/" + scene.name );
			//	Systemを追加
			SceneManager.LoadScene( "Scenes/System" );
		}
	}

#endif


	void Awake()
	{
		Debug.unityLogger.logEnabled = Debug.isDebugBuild;

		// ゲームのフレームレート設定
		Application.targetFrameRate = 60;

#if UNITY_EDITOR || DEVELOPMENT_BUILD
		// デバッグメニュー生成
		for( int i = 0; i < debug_menu_.Length; i++ )
		{
			Instantiate( debug_menu_[ i ], transform );
		}
#endif
	}


	private void Start()
	{
		// シーン読み込む前の処理をここで実行
		StartScene();
	}


	private void Update()
	{
		UpdateEscape();

		UpdateScreenshot();
	}


	private void StartScene()
	{
		if( start_scene_id == SceneCtrl.Scene.NONE )
		{
			return;
		}

		SceneCtrl.instance.LoadScene( start_scene_id );
	}


	private void UpdateEscape()
	{
		if( PadCtrl.instance.IsPress( Key.Escape ) )
		{
#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE_WIN
			Application.Quit();
#endif
		}
	}


	private void UpdateScreenshot()
	{
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
		if( PadCtrl.instance.IsPress( Key.P ) )
		{
			var path = Application.dataPath;
			path += System.DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".jpg";

			ScreenCapture.CaptureScreenshot( path );
		}
#endif
	}
}
