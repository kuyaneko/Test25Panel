using System.Collections;

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

#if UNITY_SWITCH

using nn.hid;

#endif

//using Rzp;


//	Switch準拠
public enum PadType
{
	None         = 0,

	A            = 1,
	B            = 2,
	X            = 3,
	Y            = 4,

	StickL       = 5,	//	Ｌスティック押し込み
	StickR       = 6,	//	Ｒスティック押し込み

	L            = 7,
	R            = 8,
	ZL           = 9,
	ZR           = 10,

	Plus         = 11,	//	（START）
	Minus        = 12,	//	（SELECT）

	Left         = 13,
	Up           = 14,
	Right        = 15,
	Down         = 16,

	StickL_Left  = 17,
	StickL_Up    = 18,
	StickL_Right = 19,
	StickL_Down  = 20,

	StickR_Left  = 21,
	StickR_Up    = 22,
	StickR_Right = 23,
	StickR_Down  = 24,

	LeftSL       = 25,
	LeftSR       = 26,
	RightSL      = 27,
	RightSR      = 28,
}


public enum PadMode
{
	LongPress	= 0,
	Press		= 1,
	Release		= 2,
}


public enum JoyConSide
{
	Left	= 0,
	Right	= 1,
};


public class PadCtrl : SingletonMonoBehaviour<PadCtrl>
{
	private long[] key_type_ =
	{
#if UNITY_EDITOR || UNITY_STANDALONE_WIN

		(long)Key.None,                 //  0 None

		(long)Key.Z,                    //  1 A
		(long)Key.X,                    //  2 B
		(long)Key.C,                    //  3 X
		(long)Key.V,                    //  4 Y

		(long)Key.Q,                    //  5 StickL
		(long)Key.E,                    //  6 StickR

		(long)Key.Numpad1,              //  7 L
		(long)Key.Numpad3,              //  8 R

		(long)Key.Numpad7,              //  9 ZL
		(long)Key.Numpad9,              // 10 ZR

		(long)Key.NumpadPlus,           // 11 Start
		(long)Key.NumpadMinus,          // 12 Select

		(long)Key.LeftArrow,            // 13 Left
		(long)Key.UpArrow,              // 14 Up
		(long)Key.RightArrow,           // 15 Right
		(long)Key.DownArrow,            // 16 Down

		(long)Key.A,                    // 17 StickL_Left
		(long)Key.W,                    // 18 StickL_Up
		(long)Key.D,                    // 19 StickL_Right
		(long)Key.S,                    // 20 StickL_Down

		(long)Key.Numpad4,              // 21 StickR_Left
		(long)Key.Numpad8,              // 22 StickR_Up
		(long)Key.Numpad6,              // 23 StickR_Right
		(long)Key.Numpad2,              // 24 StickR_Down

		(long)Key.U,                    // 25 LeftSL
		(long)Key.J,                    // 26 LeftSR
		(long)Key.L,                    // 27 RightSL
		(long)Key.O,                    // 28 RightSR

#elif UNITY_SWITCH

		(long)NpadButton.None,          //  0 None

		(long)NpadButton.A,             //  1 A
		(long)NpadButton.B,             //  2 B
		(long)NpadButton.X,             //  3 X
		(long)NpadButton.Y,             //  4 Y

		(long)NpadButton.StickL,        //  5 StickL
		(long)NpadButton.StickR,        //  6 StickR

		(long)NpadButton.L,             //  7 L
		(long)NpadButton.R,             //  8 R

		(long)NpadButton.ZL,            //  9 ZL
		(long)NpadButton.ZR,            // 10 ZR

		(long)NpadButton.Plus,          // 11 Start
		(long)NpadButton.Minus,         // 12 Select

		(long)NpadButton.Left,          // 13 Left
		(long)NpadButton.Up,            // 14 Up
		(long)NpadButton.Right,         // 15 Right
		(long)NpadButton.Down,          // 16 Down

		(long)NpadButton.StickLLeft,    // 17 StickL_Left
		(long)NpadButton.StickLUp,      // 18 StickL_Up
		(long)NpadButton.StickLRight,   // 19 StickL_Right
		(long)NpadButton.StickLDown,    // 20 StickL_Down

		(long)NpadButton.StickRLeft,    // 21 StickR_Left
		(long)NpadButton.StickRUp,      // 22 StickR_Up
		(long)NpadButton.StickRRight,   // 23 StickR_Right
		(long)NpadButton.StickRDown,    // 24 StickR_Down

		(long)NpadButton.LeftSL,        // 25 LeftSL
		(long)NpadButton.LeftSR,        // 26 LeftSR
		(long)NpadButton.RightSL,       // 27 RightSL
		(long)NpadButton.RightSR,       // 28 RightSR

#endif
	};

	private float _long_press_time;

	private Coroutine vibration_coroutine_;

	private bool GetKeyMode( Key key, PadMode mode )
	{
		bool is_key = false;

#if UNITY_EDITOR || UNITY_STANDALONE_WIN
		Keyboard keyboard = Keyboard.current;

		if( keyboard == null )
		{
			return false;
		}

		var button = keyboard[ key ];

		switch ( mode )
		{
			case PadMode.LongPress:	is_key = button.isPressed;				break;
			case PadMode.Press:		is_key = button.wasPressedThisFrame;	break;
			case PadMode.Release:	is_key = button.wasReleasedThisFrame;	break;
		}
#endif

		return is_key;
	}


	private bool GetKeyMode( PadType type, PadMode mode )
	{
		long key = key_type_[ (int)type ];

		if( key == 0 )
		{
			return false;
		}

		return GetKeyMode( (Key)key, mode );
	}


	public bool GetPadMode( PadType type, PadMode mode )
	{
		bool is_key = false;

#if UNITY_EDITOR || UNITY_STANDALONE_WIN
		Gamepad pad = Gamepad.current;

		if( pad == null )
		{
			is_key = JoyStickCtrl.instance.GetJoyStickMode( type, mode );
		}
		else
		{
			ButtonControl[] button_table =
			{
				null,

				pad.buttonSouth,
				pad.buttonEast,
				pad.buttonWest,
				pad.buttonNorth,

				pad.leftStickButton,
				pad.rightStickButton,

				pad.leftShoulder,
				pad.rightShoulder,

				pad.leftTrigger,
				pad.rightTrigger,

				pad.startButton,
				pad.selectButton,

				pad.dpad.left,
				pad.dpad.up,
				pad.dpad.right,
				pad.dpad.down,

				pad.leftStick.left,
				pad.leftStick.up,
				pad.leftStick.right,
				pad.leftStick.down,

				pad.rightStick.left,
				pad.rightStick.up,
				pad.rightStick.right,
				pad.rightStick.down,

				null,
				null,
				null,
				null,
			};

			var button = button_table[ (int)type ];

			if( button == null )
			{
				return false;
			}

			switch( mode )
			{
				case PadMode.LongPress:	is_key = button.isPressed;				break;
				case PadMode.Press:		is_key = button.wasPressedThisFrame;	break;
				case PadMode.Release:	is_key = button.wasReleasedThisFrame;	break;
			}
		}

#elif UNITY_SWITCH

		var button = key_type_[ (int)type ];

		switch( mode )
		{
			case PadMode.LongPress:	is_key = nintendo_pad.npadState.GetButton    ((NpadButton)button); break;
			case PadMode.Press:		is_key = nintendo_pad.npadState.GetButtonDown((NpadButton)button); break;
			case PadMode.Release:	is_key = nintendo_pad.npadState.GetButtonUp  ((NpadButton)button); break;
		}
#endif

		return is_key || GetKeyMode( type, mode );
	}


	//	長押し
	public bool IsLongPress( params object[] list )
	{
		float interval = GetInterval( list );

		if( IsPress( list ) )
		{
			_long_press_time = interval * 2;

			return true;
		}

#if UNITY_EDITOR || DEVELOPMENT_BUILD
		//if( MenuTest.instance != null && MenuTest.instance.IsOpen() )
		//{
		//	return false;
		//}
#endif
		bool flag = false;

		foreach( var press in list )
		{
			if( press is object[] obj && IsLongPress( obj ) )
			{
				flag = true;
				break;
			}

			if( press is Key key && GetKeyMode( key, PadMode.LongPress ) )
			{
				flag = true;
				break;
			}

			if( press is PadType type && GetPadMode( type, PadMode.LongPress ) )
			{
				flag = true;
				break;
			}
		}

		if( flag )
		{
			_long_press_time -= Time.deltaTime;

			if( _long_press_time <= 0.0f )
			{
				_long_press_time += interval;

				return true;
			}
		}

		return false;
	}


	private float GetInterval( object[] list )
	{
		var interval = list[ list.Length - 1 ];

		if( interval is float )
		{
			return (float)interval;
		}

		return 0.0f;
	}


	//	押した瞬間
	public bool IsPress( params object[] list )
	{
#if UNITY_EDITOR || DEVELOPMENT_BUILD
		//if( MenuTest.instance != null && MenuTest.instance.IsOpen() )
		//{
		//	return false;
		//}
#endif
		foreach( var press in list )
		{
			if( press is object[] obj && IsPress( obj ) )
			{
				return true;
			}

			if( press is Key key && GetKeyMode( key, PadMode.Press ) )
			{
				return true;
			}

			if( press is PadType type && GetPadMode( type, PadMode.Press ) )
			{
				return true;
			}
		}

		return false;
	}


	//	いずれかを押した瞬間
	public bool IsPressAny()
	{
		for( int i = 0; i < key_type_.Length; i++ )
		{
			if( IsPress( (PadType)i ) )
			{
				return true;
			}
		}

#if UNITY_EDITOR || UNITY_STANDALONE_WIN
		Keyboard keyboard = Keyboard.current;

		if( keyboard != null )
		{
			return keyboard.anyKey.wasPressedThisFrame;
		}
#endif
		return false;
	}


	//	離した瞬間
	public bool IsRelease( params object[] list )
	{
#if UNITY_EDITOR || DEVELOPMENT_BUILD
		//if( MenuTest.instance != null && MenuTest.instance.IsOpen() )
		//{
		//	return false;
		//}
#endif
		foreach( var release in list )
		{
			if( release is object[] obj && IsRelease( obj ) )
			{
				return true;
			}

			if( release is Key key && GetKeyMode( key, PadMode.Release ) )
			{
				return true;
			}

			if( release is PadType type && GetPadMode( type, PadMode.Release ) )
			{
				return true;
			}
		}

		return false;
	}


	public Vector2 GetLStick()
	{
		PadMode mode = PadMode.LongPress;

		Vector2 vec = Vector2.zero;

		if( GetKeyMode( PadType.StickL_Left, mode ) )
		{
			vec.x = -1;
		}
		else if( GetKeyMode( PadType.StickL_Right, mode ) )
		{
			vec.x = 1;
		}

		if( GetKeyMode( PadType.StickL_Up, mode ) )
		{
			vec.y = 1;
		}
		else if( GetKeyMode( PadType.StickL_Down, mode ) )
		{
			vec.y = -1;
		}

		if( vec == Vector2.zero )
		{
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
			Gamepad pad = Gamepad.current;

			if( pad == null )
			{
				vec = JoyStickCtrl.instance.GetLStick();
			}
			else
			{
				vec = pad.leftStick.ReadValue();
			}

#elif UNITY_SWITCH

			var stick = nintendo_pad.npadState.analogStickL;

			vec.x = stick.x;
			vec.y = stick.y;

			vec /= AnalogStickState.Max;
#endif

			vec.x = GetRange( vec.x );
			vec.y = GetRange( vec.y );
		}

		return vec;
	}


	public Vector2 GetRStick()
	{
		PadMode mode = PadMode.LongPress;

		Vector2 vec = Vector2.zero;

		if( GetKeyMode( PadType.StickR_Left, mode ) )
		{
			vec.x = -1;
		}
		else if( GetKeyMode( PadType.StickR_Right, mode ) )
		{
			vec.x = 1;
		}

		if( GetKeyMode( PadType.StickR_Up, mode ) )
		{
			vec.y = 1;
		}
		else if( GetKeyMode( PadType.StickR_Down, mode ) )
		{
			vec.y = -1;
		}

		if( vec == Vector2.zero )
		{
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
			Gamepad pad = Gamepad.current;

			if( pad == null )
			{
				vec = JoyStickCtrl.instance.GetRStick();
			}
			else
			{
				vec = pad.rightStick.ReadValue();
			}

#elif UNITY_SWITCH

			var stick = nintendo_pad.npadState.analogStickR;

			vec.x = stick.x;
			vec.y = stick.y;

			vec /= AnalogStickState.Max;
#endif

			vec.x = GetRange( vec.x );
			vec.y = GetRange( vec.y );
		}

		return vec;
	}


	private float GetRange( float value )
	{
		if( value < -0.2f )
		{
			return value;
		}

		if( value > 0.2f )
		{
			return value;
		}

		return 0.0f;
	}


	public void PlayVibration( float time = 0.2f )
	{
		float power = 0.35f;

#if UNITY_EDITOR || UNITY_STANDALONE_WIN
		Gamepad pad = Gamepad.current;

		if( pad == null )
		{
			return;
		}

		pad.SetMotorSpeeds( power, power );

#elif UNITY_SWITCH

		nintendo_pad_.PlayVibration( power, power );
#endif

		if( time > 0.0f )
		{
			vibration_coroutine_ = StartCoroutine( CoroutineVibration( time ) );
		}
	}


	private IEnumerator CoroutineVibration( float time )
	{
		yield return new WaitForSeconds( time );

		StopVibration();
	}


	public void StopVibration()
	{
		if (vibration_coroutine_ != null)
		{
			StopCoroutine(vibration_coroutine_);
			vibration_coroutine_ = null;
		}

#if UNITY_EDITOR || UNITY_STANDALONE_WIN
		Gamepad pad = Gamepad.current;

		if( pad == null )
		{
			return;
		}

		pad.SetMotorSpeeds( 0.0f, 0.0f );

#elif UNITY_SWITCH
		nintendo_pad_.StopVibration();
#endif
	}

#if UNITY_SWITCH
	/// <summary>
	/// JoyConセンサーの情報を取得
	/// </summary>
	/// <param name="in_side">右か左か</param>
	public nn.hid.SixAxisSensorState GetSensorStatus(JoyConSide in_side)
	{
		return nintendo_pad.state[(int)in_side];
	}



	// テストでしか使ってないので後々消える予定
	public Quaternion GetnPadQuaternion(JoyConSide in_side)
	{
		return nintendo_pad.quaternion[(int)in_side];
	}


	/// <summary>
	/// 接続コントローラーの状態を取得する
	/// </summary>
	/// <param name="in_npad_num">NpadId(0～7)</param>
	public nn.hid.NpadStyle GetControllerMode(int in_npad_num = 0)
	{
		return nintendo_pad.GetControllerStyle(in_npad_num);
	}


	/// <summary>
	/// コントローラーアプレットを開く
	/// </summary>
	/// <returns>アプレットに設定した条件を満たした設定をされたか</returns>
	public void OpenControllerSupportApplet()
	{
		nintendo_pad.ControllerSupport();
	}

	/// <summary>
	/// Switchコントローラーが有効な状態か
	/// </summary>
	/// <returns></returns>
	public bool GetEnableNpad()
	{
		return nintendo_pad.npadStyle != NpadStyle.None;
	}


	/// <summary>
	/// Switchの操作形態のサポートを切り替える
	/// </summary>
	/// <param name="is_in_game">インゲームか</param>
	public void SetSwitchSupportStyle(bool is_in_game)
	{
		nintendo_pad.SetSupportStyle(is_in_game);
	}
#endif
}
