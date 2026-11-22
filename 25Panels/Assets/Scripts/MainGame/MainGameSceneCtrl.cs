using UnityEngine;
using UnityEngine.UI;

public partial class MainGameSceneCtrl : StateCtrlBase<MainGameSceneCtrl, MainGameSceneCtrl.MainGameInitState>
{
    [SerializeField]
    private MainGamePanelCtrl _panelCtrl;

    public bool IsClear => _panelCtrl != null && _panelCtrl.IsClear;

    public void InitializePanels()
    {
        if (_panelCtrl == null)
        {
            Debug.LogError("MainGameSceneCtrl requires a MainGamePanelCtrl reference.", this);
            return;
        }

        _panelCtrl.Initialize();
    }
}
