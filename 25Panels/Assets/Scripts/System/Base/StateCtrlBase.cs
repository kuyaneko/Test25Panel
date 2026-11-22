using UnityEngine;

/// <summary>
/// ステートマシンのベースクラスを拡張したやつ
/// シーンを管理するルートのステートへ継承させる
/// </summary>
/// <typeparam name="TStateCtrl">継承したクラス</typeparam>
/// <typeparam name="TInitState">最初に実行されるステート</typeparam>
public class StateCtrlBase<TStateCtrl, TInitState> : StateContextBase<TStateCtrl, TInitState>
	where TStateCtrl : StateCtrlBase<TStateCtrl, TInitState>
	where TInitState : StateBase<TStateCtrl>, new()
{
}
