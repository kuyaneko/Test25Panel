using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class PanelObject : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private Image           _panel;
    [SerializeField]
    private TextMeshProUGUI _panelText;

    public int Number { get; private set; }

    private Action<PanelObject> _onClicked;

    public void SetNumber(int in_num)
    {
        Number = in_num;
        Setnum(in_num.ToString());
    }

    public void Initialize(int in_num, Action<PanelObject> in_onClicked)
    {
        SetNumber(in_num);
        _onClicked = in_onClicked;
        SetPanelActive(true);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            _onClicked?.Invoke(this);
        }
    }

    private void Setnum(string in_num)
    {
        _panelText.SetText(in_num);
    }

    public void SetPanelActive(bool isActive)
    {
        _panel.color = isActive ? Color.white : Color.gray;
    }
}
