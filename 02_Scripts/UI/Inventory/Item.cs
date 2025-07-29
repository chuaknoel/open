using Enums;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;

[System.Serializable]
[CreateAssetMenu(fileName ="NewItem", menuName ="Item")]
public class Item  
{
    [Header("Info")]
    [SerializeField] protected string itemId;
    [SerializeField] protected string itemName;
    [SerializeField] protected ItemType type;
    [SerializeField] protected string itemDescription;
    [SerializeField] protected Sprite image;
    [SerializeField] protected int inventoryIndex;
   
    [Header("Stacking")]
    [SerializeField] protected int count;
    [SerializeField] protected int maxCount;
    
    public string ItemId => itemId;
    public string ItemName => itemName;
    public ItemType Type => type;
    public string ItemDescription => itemDescription;
    public Sprite Image => image;

    public int Count => count;
    public int MaxCount => maxCount;
    public int InventoryIndex => inventoryIndex;

    // 기본 생성자
    public Item() { }

    public Item(Item item)
    {
        this.itemId = item.ItemId;
        this.itemName = item.ItemName;
        this.type = item.Type;

        this.itemDescription = item.ItemDescription;
        this.image = item.Image;
        this.inventoryIndex = item.InventoryIndex;

        this.count = item.Count;
        this.maxCount = item.MaxCount;
    }
    public Item(
    string itemId,
    string itemName,
    ItemType type,
    string itemDescription,
    Sprite image,
    int inventoryIndex,
    int count,
    int maxCount
    )
    {
        this.itemId = itemId;
        this.itemName = itemName;
        this.type = type;
        this.itemDescription = itemDescription;
        this.image = image;
        this.inventoryIndex = inventoryIndex;
        this.count = count;
        this.maxCount = maxCount;
    }
    public virtual void CreateItem(int index)
    {
        Move(index);
        if (count == 0) { count = 1; }
    }
    public virtual void Add(int value)
    {
        int result = count + value;
        if (result >= maxCount) { count = maxCount; }
        else { count += value; }
    }
    public virtual void Remove(int value)
    {
        int result = count - value;
        if (result <= 0) { count = 0; }
        else { count -= value; }
    }
    public void Move(int index)
    {
        inventoryIndex = index;
    }
    public virtual void Use( )
    {
        Remove(1);
    }
}
