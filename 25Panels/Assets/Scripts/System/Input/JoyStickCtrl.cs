using UnityEngine;
using UnityEngine.InputSystem;


public class JoyStickCtrl : SingletonMonoBehaviour<JoyStickCtrl>
{
	private string[] button_ =
	{
		null,

		"joystick button 2",
		"joystick button 1",
		"joystick button 3",
		"joystick button 0",

		"joystick button 10",
		"joystick button 11",

		"joystick button 4",
		"joystick button 5",

		"joystick button 6",
		"joystick button 7",

		"joystick button 9",
		"joystick button 8",

		"5th axis",
		"6th axis",
		"5th axis",
		"6th axis",

		"X axis",
		"Y axis",
		"X axis",
		"Y axis",

		"3rd axis",
		"4th axis",
		"3rd axis",
		"4th axis",

		null,
		null,
		null,
		null,
	};

	private bool[] get_key_press_ = new bool[ 29 ];


	public void ClearKey()
	{
		for( int i = 0; i < get_key_press_.Length; i++ )
		{
			get_key_press_[ i ] = false;
		}
	}


	public bool GetJoyStickMode( PadType type, PadMode mode )
	{
		Joystick joystick = Joystick.current;

		if( joystick == null )
		{
			return false;
		}

		string button_type = button_[(int)type];

		bool is_key = false;

		if( type <= PadType.Minus )
		{
			switch (mode)
			{
				case PadMode.LongPress:	is_key = Input.GetKey( button_type ); break;
				case PadMode.Press:		is_key = Input.GetKeyDown( button_type ); break;
				case PadMode.Release:	is_key = Input.GetKeyUp( button_type ); break;
			}
		}
		else
		{
			float axis = Input.GetAxis( button_type );

			switch( type )
			{
				case	PadType.Left:
				case	PadType.StickL_Left:
				case	PadType.StickR_Left:
					if( axis < -0.2f ){ is_key = true; }
					break;

				case	PadType.Up:
				case	PadType.StickL_Up:
				case	PadType.StickR_Up:
					if( axis > 0.2f ){ is_key = true; }
					break;

				case	PadType.Right:
				case	PadType.StickL_Right:
				case	PadType.StickR_Right:
					if( axis > 0.2f ){ is_key = true; }
					break;

				case	PadType.Down:
				case	PadType.StickL_Down:
				case	PadType.StickR_Down:
					if( axis < -0.2f ){ is_key = true; }
					break;
			}

			if( mode == PadMode.Press )
			{
				if( is_key )
				{
					if( get_key_press_[ (int)type ] )
					{
						is_key = false;
					}
					else
					{
						get_key_press_[ (int)type ] = true;
					}
				}
				else
				{
					get_key_press_[ (int)type ] = false;
				}
			}
		}

		return is_key;
	}


	public Vector2 GetLStick()
	{
		Joystick stick = Joystick.current;

		if( stick == null )
		{
			return Vector2.zero;
		}

		Vector2 vec = Vector2.zero;

		vec.x = Input.GetAxis( button_[ (int)PadType.StickL_Left ] );
		vec.x = GetRange( vec.x );

		vec.y = Input.GetAxis( button_[ (int)PadType.StickL_Up ] );
		vec.y = GetRange( vec.y );

		return vec;
	}


	public Vector2 GetRStick()
	{
		Joystick stick = Joystick.current;

		if( stick == null )
		{
			return Vector2.zero;
		}

		Vector2 vec = Vector2.zero;

		vec.x = Input.GetAxis( button_[ (int)PadType.StickR_Right ] );
		vec.x = GetRange( vec.x );

		vec.y = Input.GetAxis( button_[ (int)PadType.StickR_Down ] );
		vec.y = GetRange( vec.y );

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
}
