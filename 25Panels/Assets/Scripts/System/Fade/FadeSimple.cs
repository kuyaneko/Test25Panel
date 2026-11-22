using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeSimple : FadeBase
{
	[SerializeField]
	private float _time_max;
	private float _time_cnt;

	private float _target;



	public override void FadeIn(Action in_callback)
	{
		Debug.Log("FadeIn");

		_state = State.IN;
		_callback = in_callback;

		Fade(0.0f);
	}

	public override void FadeOut(Action in_callback)
	{
		Debug.Log("FadeOut");

		_state = State.OUT;
		_callback = in_callback;

		Fade(1.0f);
	}

	public override bool IsActive()
	{
		// 見えてる状態か
		return _image.color.a == 1.0f;
	}

	private void Fade(float target)
	{
		//ui_activity_.SetActive(true);
		base._image.enabled = true;

		_time_cnt = 0.0f;

		_target = target;

		StartCoroutine(PlayFade());
	}

	// フェード実行
	private IEnumerator PlayFade()
	{
		while (_time_cnt < 1.0f)
		{
			_time_cnt += (_time_max > 0.0f) ? (Time.deltaTime / _time_max) : (1.0f);

			Color color = _image.color;
			color.a = Mathf.Lerp(1.0f - _target, _target, _time_cnt);
			_image.color = color;

			yield return null;
		}

		OnEnd();
	}


	private void OnEnd()
	{
		if (_state == State.IN)
		{
			base._image.enabled = false;
			//ui_activity_.SetActive(false);
		}

		_state = State.NONE;
		_callback?.Invoke();
	}
}
