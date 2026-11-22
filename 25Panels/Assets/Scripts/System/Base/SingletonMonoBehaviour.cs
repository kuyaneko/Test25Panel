using UnityEngine;


public class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
	public static T instance { get; private set; } = null;


	protected virtual void Awake()
	{
		var log = $"{(this as T).GetType()}: ";
			log += $"new instance {gameObject.name}";

		if( instance == null )
		{
			instance = this as T;
		}
		else
		{
			log += $", current instance {instance.gameObject.name}";

			//Destroy(this);
		}

		Debug.Log( "<size=24>" + log + "</size>" );
	}
}
