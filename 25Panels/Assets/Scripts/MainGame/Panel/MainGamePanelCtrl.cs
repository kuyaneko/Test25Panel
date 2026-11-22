using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainGamePanelCtrl : MonoBehaviour
{
    private const int PanelCountPerSide = 5;
    private const int TotalPanelCount = PanelCountPerSide * PanelCountPerSide;

    [SerializeField]
    private PanelObject _panelPrefab;

    [SerializeField]
    private GridLayoutGroup _gridLayout;

    private readonly List<PanelObject> _panels = new List<PanelObject>();
    private int _nextNumber = 1;

    public bool IsClear { get; private set; }

    public void Initialize()
    {
        if (_panelPrefab == null || _gridLayout == null)
        {
            Debug.LogError("MainGamePanelCtrl requires a PanelObject prefab and a GridLayoutGroup.", this);
            return;
        }

        ClearPanels();
        GeneratePanels();
    }

    private void ClearPanels()
    {
        foreach (Transform child in _gridLayout.transform)
        {
            Destroy(child.gameObject);
        }

        _panels.Clear();
        _nextNumber = 1;
        IsClear = false;
    }

    private void GeneratePanels()
    {
        _gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        _gridLayout.constraintCount = PanelCountPerSide;

        List<int> numbers = new List<int>(TotalPanelCount);
        for (int number = 1; number <= TotalPanelCount; number++)
        {
            numbers.Add(number);
        }

        for (int i = numbers.Count - 1; i > 0; i--)
        {
            int randomIndex = UnityEngine.Random.Range(0, i + 1);
            int number = numbers[i];
            numbers[i] = numbers[randomIndex];
            numbers[randomIndex] = number;
        }

        foreach (int number in numbers)
        {
            PanelObject panel = Instantiate(_panelPrefab, _gridLayout.transform);
            panel.Initialize(number, OnPanelClicked);
            _panels.Add(panel);
        }
    }

    private void OnPanelClicked(PanelObject panel)
    {
        if (IsClear || panel.Number != _nextNumber)
        {
            return;
        }

        panel.SetPanelActive(false);
        _nextNumber++;

        if (_nextNumber > TotalPanelCount)
        {
            IsClear = true;
        }
    }
}
