using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;

using TMPro;


public class InputTextCtrl : SingletonMonoBehaviour<InputTextCtrl>
{
	[SerializeField]
	private GameObject          root_object_ = null;

	[SerializeField]
	private TMP_InputField      input_field_ = null;

	private string              result_text_ = "";
	private bool                is_inputted_ = false;




#if UNITY_SWITCH
	// switch専用
	private nn.swkbd.ShowKeyboardArg showKeyboardArg_;
#endif

	/// <summary>
	/// 初期化（ゲーム起動時用）
	/// </summary>
	public void Initialize()
	{
		result_text_ = "";
		is_inputted_ = false;

#if UNITY_SWITCH
		nn.swkbd.Swkbd.Initialize(ref showKeyboardArg_, false);
#endif
	}


	/// <summary>
	/// キーボード
	/// </summary>
	public void OpenWindow()
	{
		result_text_ = string.Empty;
		is_inputted_ = false;

#if UNITY_SWITCH && !UNITY_EDITOR
		OpenSW();
#else
		OpenPC();
#endif
	}


	/// <summary>
	/// 入力した文字を受け取る
	/// （引数に結果を1回入れたら入力済み状態が解除されます）
	/// </summary>
	/// <returns>入力されていたか</returns>
	public bool GetResult(ref string result_text)
	{
		if (is_inputted_ == true)
		{
			result_text = result_text_;
			is_inputted_ = false;
			return true;
		}

		return false;
	}


	/// <summary>
	/// 入力開始（PC）
	/// </summary>
	private void OpenPC()
	{
		root_object_.SetActive(true);

		input_field_.text = string.Empty;
		input_field_.Select();
		input_field_.ActivateInputField();

		StartCoroutine(CoroutineTextBoxWindow());
	}


	/// <summary>
	/// 入力開始（Switch）
	/// </summary>
	private void OpenSW()
	{
#if UNITY_SWITCH
		var result_string   = new System.Text.StringBuilder(nn.swkbd.Swkbd.TextMaxLength);
		var result          = nn.swkbd.Swkbd.ShowKeyboard(result_string, showKeyboardArg_, true);

		if (result.IsSuccess())
		{
			result_text_ = result_string.ToString();
		}

		is_inputted_ = true;
#endif
	}

	/// <summary>
	/// 入力ウィンドウ稼働用コルーチン（PC用）
	/// </summary>
	/// <returns></returns>
	private IEnumerator CoroutineTextBoxWindow()
	{
		while (true)
		{
			if (PadCtrl.instance.IsPress(Key.Enter))
			{
				result_text_ = input_field_.text;
				break;
			}
			yield return null;
		}

		is_inputted_ = true;
		root_object_.SetActive(false);
	}




#if UNITY_SWITCH// && !UNITY_EDITOR
	// switch専用

	public void SetAppletConfigDefault()
	{
		nn.swkbd.Swkbd.Initialize(ref showKeyboardArg_, false);
		//nn.swkbd.Swkbd.InitializeKeyboardConfig();
	}

	/// <summary>
	/// プロフィール画面用設定
	/// </summary>
	public void SetAppletConfigProfile()
	{

		// 初期値にリセットしてから設定する
		showKeyboardArg_.keyboardConfig = new nn.swkbd.KeyboardConfig();

		// アプレット起動時、後ろを暗くする
		showKeyboardArg_.keyboardConfig.isUseBlurBackground = true;
		// カーソルの位置を最後尾に
		showKeyboardArg_.keyboardConfig.initialCursorPos = nn.swkbd.InitialCursorPos.Last;
		// プレビューを1行に
		showKeyboardArg_.keyboardConfig.inputFormMode = nn.swkbd.InputFormMode.OneLine;
		// 改行を無効
		showKeyboardArg_.keyboardConfig.isUseNewLine = false;
		// 受け取る文字列をUTF-8に
		showKeyboardArg_.keyboardConfig.isUseUtf8 = false;
		// キーボードの種類を指定（番号のみの種類だと小数点が使えないのでASCIIを指定）
		showKeyboardArg_.keyboardConfig.keyboardMode = nn.swkbd.KeyboardMode.Numeric;

		//default_char = showKeyboardArg_.keyboardConfig.rightOptionalSymbolKey;
		showKeyboardArg_.keyboardConfig.rightOptionalSymbolKey = '.';
		// 入力最大＆最小文字数
		showKeyboardArg_.keyboardConfig.textMaxLength = 6;
		showKeyboardArg_.keyboardConfig.textMinLength = 0;
	}

	/// <summary>
	/// パスコード入力用設定
	/// </summary>
	public void SetAppletConfigPassCode()
	{
		// 初期値にリセットしてから設定する
		showKeyboardArg_.keyboardConfig = new nn.swkbd.KeyboardConfig();

		// アプレット起動時、後ろを暗くする
		showKeyboardArg_.keyboardConfig.isUseBlurBackground = true;
		// カーソルの位置を最後尾に
		showKeyboardArg_.keyboardConfig.initialCursorPos = nn.swkbd.InitialCursorPos.Last;
		// プレビューを1行に
		showKeyboardArg_.keyboardConfig.inputFormMode = nn.swkbd.InputFormMode.OneLine;
		// 改行を無効
		showKeyboardArg_.keyboardConfig.isUseNewLine = false;
		// 受け取る文字列をUTF-16に
		showKeyboardArg_.keyboardConfig.isUseUtf8 = false;
		// キーボードの種類を指定(ASC2)
		showKeyboardArg_.keyboardConfig.keyboardMode = nn.swkbd.KeyboardMode.ASCII;
		// 入力最大＆最小文字数
		//showKeyboardArg_.keyboardConfig.textMaxLength = 16;
		//showKeyboardArg_.keyboardConfig.textMinLength = 0;
	}

	/// <summary>
	/// 任意のキーボードアプレット設定を使用する
	/// </summary>
	/// <param name="in_config">任意のキーボードアプレット設定</param>
	public void SetAppletSettingCustom(nn.swkbd.KeyboardConfig in_config)
	{
		// 「nn.swkbd.KeyboardConfig」を宣言して引数に設定してもらうが、性質上適切にdefineで区切らないとエラーが起きるので注意

		// 初期値にリセットしてから設定する
		showKeyboardArg_.keyboardConfig = new nn.swkbd.KeyboardConfig();

		// 受け取った設定を適用
		showKeyboardArg_.keyboardConfig = in_config;
	}

#endif

}
