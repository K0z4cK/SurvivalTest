using System;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : SingletonComponent<UIManager>
{
    [Header("Inventory")]
    [SerializeField] private InventoryUI _inventoryPanel;
    [SerializeField] private Button _showInventoryBtn;
    [SerializeField] private Button _hideInventoryBtn;

    private void Awake()
    {
        _showInventoryBtn.onClick.AddListener(_inventoryPanel.ShowPanel);
        _hideInventoryBtn.onClick.AddListener(_inventoryPanel.HidePanel);
    }

    public void SubscribeOnInventoryActions(ref Action<int, InventoryItem> onItemSet, ref Action<int> onItemClear)
    {
        onItemSet += _inventoryPanel.SetItemToCell;
        onItemClear += _inventoryPanel.ClearCell;
    }

}
