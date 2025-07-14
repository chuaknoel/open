using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;

public enum ItemType
{
    None, // 기타
    Consume, // 소모품
    Equip, // 장비 아이템
    Important, // 중요 아이템(퀘스트,열쇠,문서 등등)
    Collectibles // 수집품
}
public enum ItemGrade
{
    Common, // 일반
    Uncommon, //고급
    Rare, // 희귀
    Epic, // 영웅
    Legendary // 전설
}
[System.Serializable]
[CreateAssetMenu(fileName ="NewItem", menuName ="Item")]
public class Item  
{
    [Header("Info")]
    [SerializeField] protected string itemName;
    [SerializeField] protected ItemType type;
    [SerializeField] protected ItemGrade itemGrade;
    [SerializeField] protected string itemDescription;
    [SerializeField] protected Sprite image;
    [SerializeField] protected int inventoryIndex;
   
    [Header("Stacking")]
    [SerializeField] protected int count;
    [SerializeField] protected int maxCount;

    public string ItemName => itemName;
    public ItemType Type => type;
    public ItemGrade ItemGrade => itemGrade;
    public string ItemDescription => itemDescription;
    public Sprite Image => image;

    public int Count => count;
    public int MaxCount => maxCount;
    public int InventoryIndex => inventoryIndex;

    public Item(string itemName,ItemType type, ItemGrade itemGrade, string itemDescription, Sprite image,
    int inventoryIndex, int count, int maxCount)
    {
        this.itemName = itemName;
        this.type = type;
        this.itemGrade = itemGrade;
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
