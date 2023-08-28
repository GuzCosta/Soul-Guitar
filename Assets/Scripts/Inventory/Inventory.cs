using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum ItemCategory { Items, Guitars, Figurines }

public class Inventory : MonoBehaviour, ISavable
{
    [SerializeField] List<ItemSlot> itemsSlots;
    [SerializeField] List<ItemSlot> guitarsSlots;
    [SerializeField] List<ItemSlot> figurinesSlots;

    List<List<ItemSlot>> allSlots;

    public event Action OnUpdated;

    private void Awake()
    {
        allSlots = new List<List<ItemSlot>>() { itemsSlots, guitarsSlots, figurinesSlots };
    }

    public static List<string> ItemCategories { get; set; } = new List<string>()
    {
        "ITEMS", "GUITARS", "FIGURINES"
    };

    public List<ItemSlot> GetSlotsByCategory(int categoryIndex)
    {
        return allSlots[categoryIndex];
    }

    public ItemBase GetItem(int itemIndex, int categoryIndex)
    {
        var currentSlots = GetSlotsByCategory(categoryIndex);
        return currentSlots[itemIndex].Item;
    }

    /*public ItemBase UseItem(int itemIndex, Guitar selectedGuitar, int selectedCategory)
    {
        var item = GetItem(itemIndex, selectedCategory);
        bool itemUsed = item.Use(selectedGuitar);
        if (itemUsed)
        {
            if (!item.IsReusable)
                RemoveItem(item);

            return item;
        }
        return null;
    }*/

    public void AddItem(ItemBase item, int count = 1)
    {
        int category = (int)GetCategoryFromItem(item);
        var currentSlots = GetSlotsByCategory(category);

        var itemSlot = currentSlots.FirstOrDefault(slot => slot.Item == item);
        if (itemSlot != null)
        {
            itemSlot.Count += count;
        }
        else
        {
            currentSlots.Add(new ItemSlot()
            {
                Item = item,
                Count = count
            });
        }

        OnUpdated?.Invoke();
    }

    public void RemoveItem(ItemBase item)
    {
        int category = (int)GetCategoryFromItem(item);
        var currentSlots = GetSlotsByCategory(category);

        var itemSlot = currentSlots.First(slot => slot.Item == item);
        itemSlot.Count--;
        if (itemSlot.Count == 0)
            currentSlots.Remove(itemSlot);

        OnUpdated?.Invoke();
    }

    public bool HasItem(ItemBase item)
    {
        int category = (int)GetCategoryFromItem(item);
        var currentSlots = GetSlotsByCategory(category);

        return currentSlots.Exists(slot => slot.Item == item);
    }

    ItemCategory GetCategoryFromItem(ItemBase item)
    {
        if (item is Strings)
            return ItemCategory.Items;
        else if (item is Guitars)
            return ItemCategory.Guitars;
        else
            return ItemCategory.Figurines;

    }

    public static Inventory GetInventory()
    {
        return FindObjectOfType<PlayerController>().GetComponent<Inventory>();
    }

    public object CaptureState()
    {
        var saveData = new InventorySaveData()
        {
            items = itemsSlots.Select(i => i.GetSaveData()).ToList(),
            guitars = guitarsSlots.Select(i => i.GetSaveData()).ToList(),
            figurines = figurinesSlots.Select(i => i.GetSaveData()).ToList(),
        };
        Debug.Log("Saved data: " + saveData.ToString());
        return saveData;
    }

    public void RestoreState(object state)
    {
        var saveData = state as InventorySaveData;
        Debug.Log("Restore data: " + saveData.ToString());

        itemsSlots = saveData.items.Select(i => new ItemSlot(i)).ToList();
        guitarsSlots = saveData.guitars.Select(i => new ItemSlot(i)).ToList();
        figurinesSlots = saveData.figurines.Select(i => new ItemSlot(i)).ToList();

        allSlots = new List<List<ItemSlot>>() { itemsSlots, guitarsSlots, figurinesSlots };

        OnUpdated?.Invoke();
    }
}

[Serializable]
public class ItemSlot
{
    [SerializeField] ItemBase item;
    [SerializeField] int count;

    public ItemSlot()
    {

    }

    public ItemSlot(ItemSaveData saveData)
    {
        item = ItemDB.GetObjectByName(saveData.name);
        count = saveData.count;
    }
    public ItemSaveData GetSaveData()
    {
        var saveData = new ItemSaveData()
        {
            name = item.name,
            count = count
        };

        return saveData;
    }

    public ItemBase Item { get => item; set => item = value; }
    public int Count { get => count; set => count = value; }
}

[Serializable]
public class ItemSaveData
{
    public string name;
    public int count;
}

[Serializable]
public class InventorySaveData
{
    public List<ItemSaveData> items;
    public List<ItemSaveData> guitars;
    public List<ItemSaveData> figurines;

    public override string ToString()
    {
        string r = "";
        foreach (var item in items)
        {
            r = r + $"Item: {item.name} Count: {item.count}\n";
        }

        foreach (var guitar in guitars)
        {
            r = r + $"Guitar: {guitar.name} Count: {guitar.count}\n";
        }
        foreach (var figurine in figurines)
        {
            r = r + $"Figurines: {figurine.name} Count: {figurine.count}\n";
        }
        return r;
    }
}
