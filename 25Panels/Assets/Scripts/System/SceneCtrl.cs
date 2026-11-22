using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneCtrl : SingletonMonoBehaviour<SceneCtrl>
{
	public enum Scene
	{
		NONE = -1,

		SYSTEM = 0,
		TITLE,
		MAINGAME,
		RESULT,
	}

	public Scene	scene_id_current { get; private set; }
	public Scene	scene_id_prev	 { get; private set; }


	[Serializable]
	public class AddSceneData
	{
		public Scene	scene;
		public Action	callback;
	}

	[SerializeField]
	private List<AddSceneData> _scene_data = new List<AddSceneData>();


	// シーン読み込み
	public void LoadScene( Scene in_scene, params object[] variables )
	{
		Debug.Log( "LoadScene:" + in_scene );

		StartCoroutine( CoroutineLoadScene( in_scene ) );
	}

	// シーン読み込み実行（コルーチン）
	private IEnumerator CoroutineLoadScene( Scene in_scene )
	{
		if( scene_id_current != Scene.NONE )
		{
			yield return CoroutineClearScene( scene_id_current );
		}

		scene_id_prev	 = scene_id_current;
		scene_id_current = in_scene;

		yield return CoroutineAddScene( in_scene, null );

		SetActiveScene( in_scene );
	}

	// シーン追加
	public void AddScene( Scene in_scene, Action in_callback )
	{
		Debug.Log( "AddScene:" + in_scene );

		StartCoroutine( CoroutineAddScene( in_scene, in_callback ) );
	}

	// シーン追加実行（コルーチン）
	private IEnumerator CoroutineAddScene( Scene in_scene, Action in_callback )
	{
		AddSceneData data = new AddSceneData();

		data.scene		= in_scene;
		data.callback	= in_callback;

		_scene_data.Insert( 0, data );

		var async = SceneManager.LoadSceneAsync( (int)in_scene, LoadSceneMode.Additive );

		yield return new WaitUntil( () => async.isDone );
	}

	// シーン削除実行（コルーチン）
	private IEnumerator CoroutineClearScene( Scene in_scene )
	{
		Debug.Log( "ClearScene:" + in_scene );

		var data = _scene_data.Find( p => p.scene == in_scene );

		if( data != null )
		{
			data.callback?.Invoke();

			var async = SceneManager.UnloadSceneAsync( (int)data.scene );

			yield return new WaitUntil( () => async.isDone );

			_scene_data.Remove( data );

			// 前のシーンを破棄した後１フレーム待つ
			yield return null;
		}
	}

	// 一番最初のシーンへ
	public void ReturnScene()
	{
		Debug.Log("ReturnScene");

		if( _scene_data.Count <= 1 )
		{
			return;
		}

		StartCoroutine( CoroutineReturnScene() );
	}

	// 一番最初に読み込んだシーンを読み込み
	public IEnumerator CoroutineReturnScene()
	{
		yield return CoroutineClearScene( _scene_data[ 0 ].scene );
	}


	public bool IsScene( Scene in_scene_id )
	{
		Debug.Log( "IsScene:" + in_scene_id );

		var data = _scene_data.Find( p => p.scene == in_scene_id );

		return ( data != null );
	}


	private void SetActiveScene( Scene in_scene )
	{
		var scene = SceneManager.GetSceneByBuildIndex( (int)in_scene );

		SceneManager.SetActiveScene( scene );
	}
}
