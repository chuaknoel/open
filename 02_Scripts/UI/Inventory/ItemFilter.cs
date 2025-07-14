using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemFilter
{
    public List<Item> FilterByKeyword(List<Item> items, string keyword) =>
        items.Where(i => i.ItemName.Contains(keyword, StringComparison.OrdinalIgnoreCase)).ToList();

    public List<Item> FilterByCategory(List<Item> items, ItemType type) =>
        items.Where(i => i.Type == type).ToList();
}
