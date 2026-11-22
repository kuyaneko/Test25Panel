using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeBase : MonoBehaviour
{
	protected enum State
	{
		NONE = 0,

		IN,		// フェードイン中
		OUT,	// フェードアウト中
	};

	protected State		_state;
	protected Action	_callback;

	[SerializeField]
	protected Image		_image;


	public void SetColor(Color in_color)
	{
		in_color.a = _image.color.a;

		_image.color = in_color;
	}


	public bool IsFadeEnd()
	{
		return _state == State.NONE;
	}

	// フェードアウト→インでフェード種類が違う時用の調整処理
	public void SetEnableFade(bool in_enable)
	{
		Color setColor = _image.color;
		setColor.a = (in_enable == true ? 1.0f : 0.0f);

		_image.color = setColor;
		_image.enabled = in_enable;
	}


	public virtual void FadeIn(Action in_callback)
	{
	}

	public virtual void FadeOut(Action in_callback)
	{
	}

	public virtual bool IsActive()
	{
		// overrideして使うこと
		return false;
	}
}
