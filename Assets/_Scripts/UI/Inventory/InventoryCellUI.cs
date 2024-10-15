using UnityEngine;

public class InventoryCellUI : MonoBehaviour
{
    private ItemUI _itemUI;

    public void SetItem(ItemUI itemPrefab, InventoryItem item)
    {
        if(_itemUI == null)
            _itemUI = Instantiate(itemPrefab, transform);
        _itemUI.Init(item);
    }

    public void SetItem(ItemUI newItem)
    {
        _itemUI = newItem;
        _itemUI.transform.localPosition = Vector3.zero; 
    }

    public void ClearCell()
    {
        Destroy(_itemUI.gameObject);
        _itemUI = null;   
    }
}
