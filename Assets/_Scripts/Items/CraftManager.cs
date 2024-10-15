using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CraftManager : SingletonComponent<CraftManager>
{
    public event Action<ItemCraftStruct> OnItemCrafted;

    [SerializeField] private ItemCrafts _itemCrafts;

    public void SubscribeOnItemCrafted(Action<ItemCraftStruct> onItemCrafted) => OnItemCrafted += onItemCrafted;
    public void UnsubscribeOnItemCrafted(Action<ItemCraftStruct> onItemCrafted) => OnItemCrafted -= onItemCrafted;

    public List<ItemCraftStruct> GetAvalibleCrafts(InventoryItem[] items)
    {
        List<ItemCraftStruct> avalibleCrafts = new List<ItemCraftStruct>();

        foreach (ItemCraftStruct craft in _itemCrafts.itemCrafts)
        {
            List<InventoryItem> itemsForCraft = GetCraftItems(craft.CraftRecipe, items);
            if (itemsForCraft.Count == craft.CraftRecipe.Count)
            {
                avalibleCrafts.Add(craft);
                continue;
            }
        }

        return avalibleCrafts;
    }

    public List<InventoryItem> GetCraftItems(List<InventoryItem> craftRecipe, InventoryItem[] inventoryItems)
    {
        List<InventoryItem> itemsForCraft = new List<InventoryItem>();

        foreach (InventoryItem item in craftRecipe)
        {
            InventoryItem avalibleItem = inventoryItems.FirstOrDefault(i => i.ItemObject == item.ItemObject && i.Count >= item.Count);
            if (avalibleItem.ItemObject != null && !itemsForCraft.Contains(avalibleItem))
            {
                itemsForCraft.Add(avalibleItem);
                continue;
            }
        }

        return itemsForCraft;
    }

    public void CraftItem(ItemCraftStruct craft)
    {
        OnItemCrafted?.Invoke(craft);
    }
}
