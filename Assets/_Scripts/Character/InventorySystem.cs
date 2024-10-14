using System;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    [SerializeField] private int _maxCells;
    [SerializeField] private int _maxItemsInCell;

    public InventoryItem[] _inventoryItems;

    private void Awake()
    {
        _inventoryItems = new InventoryItem[_maxCells];
    }

    public void AddItems(IPickable pickable, ItemObject itemObject, int count = 1)
    {
        int index = -1;
        bool isCountChanged = false;

        if(CheckSameItem(itemObject, ref index))
        {
            if (IsCellHaveSpace(index, count))
            {
                AddItemsToCell(index, count);
                pickable.DestroyObject();
                return;
            }
            else
            {
                AddItemsToCell(index, _maxItemsInCell - _inventoryItems[index].Count);
                count = GetCountRest(index, count);
                isCountChanged = true; 
            }
        }

        index = GetFirstEmptyCellIndex();
        if (index > -1)
        {
            SetItemToCell(index, itemObject, count);
            pickable.DestroyObject();
            return;
        }

        if(isCountChanged)
        {
            pickable.DestroyObject();
        }

        Debug.Log("Inventory Full");
    }

    private void SetItemToCell(int index, ItemObject itemObject, int count)
    {
        _inventoryItems[index].ItemObject = itemObject;
        AddItemsToCell(index, count);
    }

    private void AddItemsToCell(int index, int count)
    {
        _inventoryItems[index].Count += count;
    }

    private int GetCountRest(int index, int count) => _inventoryItems[index].Count + count - _maxItemsInCell;

    private bool IsCellHaveSpace(int index, int count) => _inventoryItems[index].Count + count <= _maxItemsInCell;

    private bool CheckSameItem(ItemObject itemObject, ref int index)
    {
        for(int i = 0; i < _inventoryItems.Length; i++)
        {
            if ( _inventoryItems[i].ItemObject == itemObject && _inventoryItems[i].Count < _maxItemsInCell)
            {
                index = i;
                return true;
            }
        }
        return false;
    }

    private int GetFirstEmptyCellIndex()
    {
        for (int i = 0; i < _inventoryItems.Length; i++)
        {
            if (_inventoryItems[i].Count == 0)
                return i;
        }
        return -1;
    }

    public void RemoveItems(ItemObject item, int count = 1)
    {
        for(int i = 0; i < _inventoryItems.Length; i++)
        {
            if (_inventoryItems[i].ItemObject == item)
            {
                RemoveItems(i, count);
                return;
            }
        }
    }

    public void RemoveItems(int index, int count = 1)
    {
        if (_inventoryItems[index].Count > 0)
        {
            _inventoryItems[index].Count -= 0;
            ClearCell(index);
        }
    }

    private void ClearCell(int index)
    {
        if (_inventoryItems[index].Count <= 0)
        {
            _inventoryItems[index].Count = 0;
            _inventoryItems[index].ItemObject = null;
        }
    }

    public void SwapItems(int firstIndex, int secondIndex)
    {
        var temp = _inventoryItems[firstIndex];
        _inventoryItems[firstIndex] = _inventoryItems[secondIndex];
        _inventoryItems[secondIndex] = temp;
    }
}

[Serializable]
public struct InventoryItem
{
    public ItemObject ItemObject;
    public int Count;
}