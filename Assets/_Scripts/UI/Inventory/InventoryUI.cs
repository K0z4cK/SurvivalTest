using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private ItemUI _itemPrefab;

    [Header("Transforms of cells")]
    [SerializeField] private Transform _cellsGrid;
    [SerializeField] private Transform _cellsHotbar;

    [Header("Hotbar")]
    [SerializeField] private GameObject _hudHotbar;
    [SerializeField] private Transform _hudHotbarPosition;
    [SerializeField] private Transform _inventoryHotbarPosition;

    [Header("Panel")]
    [SerializeField] private GameObject _panel;

    private List<InventoryCellUI> _inventoryCells = new List<InventoryCellUI>();

    private void Awake()
    {
        foreach (Transform cell in _cellsHotbar)
        {
            _inventoryCells.Add(cell.GetComponent<InventoryCellUI>());
        }
        foreach (Transform cell in _cellsGrid)
        {
            _inventoryCells.Add(cell.GetComponent<InventoryCellUI>());
        }
        HidePanel();
    }

    public void ShowPanel()
    {
        _hudHotbar.gameObject.SetActive(false);
        _cellsHotbar.position = _inventoryHotbarPosition.transform.position;
        _panel.SetActive(true);
    }

    public void HidePanel()
    {
        _hudHotbar.gameObject.SetActive(true);
        _cellsHotbar.position = _hudHotbarPosition.position;
        _panel.SetActive(false);
    }

    public void SetItemToCell(int index, InventoryItem item) => _inventoryCells[index].SetItem(_itemPrefab, item);

    public void ClearCell(int index) => _inventoryCells[index].ClearCell();
}
