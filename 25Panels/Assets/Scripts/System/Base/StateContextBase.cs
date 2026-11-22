using UnityEngine;

/// <summary>
/// ステート管理クラスの基底
/// </summary>
/// <typeparam name="Context_t">継承したクラス</typeparam>
/// <typeparam name="Start_t">最初に実行されるステート</typeparam>
public abstract class StateContextBase<Context_t, Start_t> : MonoBehaviour
	where Context_t : StateContextBase<Context_t, Start_t>
	where Start_t : StateBase<Context_t>, new()
{
	protected virtual void Awake()
	{
		_isAwake = true;

		_currentState = NewState<Start_t>();

		Debug.Log($"{typeof(Context_t)} : first state {_currentState.GetType().Name}");

		_currentState.OnAwake();

		DoTransition();
	}

	protected virtual void Start()
	{
		_isAwake = false;

		_currentState.OnEnter();

		DoTransition();
	}

	protected virtual void Update()
	{
		_currentState.OnUpdate();

		DoTransition();
	}

	protected virtual void LateUpdate()
	{
		_currentState.OnLateUpdate();

		DoTransition();
	}

	protected virtual void FixedUpdate()
	{
		_currentState.OnFixedUpdate();

		DoTransition();
	}

	protected virtual void OnDestroy()
	{
		_currentState.OnExit();
	}

	protected virtual void OnPause()
	{
		_currentState.OnPause();
	}

	protected virtual void OnResume()
	{
		_currentState.OnResume();
	}

	protected virtual State_t NewState<State_t>() where State_t : StateBase<Context_t>, new()
	{
		var state = new State_t();
		state.SetContext((Context_t)this);

		return state;
	}

	protected virtual void SetState<State_t>() where State_t : StateBase<Context_t>, new()
	{
		_nextState = NewState<State_t>();
	}

	private void DoTransition()
	{
		if (_nextState == null)
		{
			return;
		}

		_currentState.OnExit();

		Debug.Log($"{typeof(Context_t)} : change state {_currentState.GetType().Name} -> {_nextState.GetType().Name}");

		_prevState = _currentState;
		_currentState = _nextState;
		_nextState = null;

		if (_isAwake == true)
		{
			_currentState.OnAwake();
		}
		else
		{
			_currentState.OnEnter();
		}

		_prevState = null;

		DoTransition();
	}

	protected StateBase<Context_t> _currentState = null;
	protected StateBase<Context_t> _nextState = null;
	protected StateBase<Context_t> _prevState = null;
	private bool _isAwake = true;
}


/// <summary>
/// ステートクラスの基底
/// </summary>
/// <typeparam name="Context_t">ステート管理クラス</typeparam>
public abstract class StateBase<Context_t> where Context_t : class
{
	public void SetContext(Context_t context) { owner = context; }

	public virtual void OnAwake() { }
	public virtual void OnEnter() { }
	public virtual void OnUpdate() { }
	public virtual void OnFixedUpdate() { }
	public virtual void OnExit() { }
	public virtual void OnPause() { }
	public virtual void OnResume() { }
	public virtual void OnLateUpdate() { }

	public Context_t owner { get; private set; } = null;
}