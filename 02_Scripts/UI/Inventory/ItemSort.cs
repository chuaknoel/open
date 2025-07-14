using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using static UnityEditor.Progress;

public class ItemSort
{
    // 등급 내림차순 정렬
    public List<Item> SortByItemGrade(List<Item> items) =>
        BubbleSort(items, (a, b) => a.ItemGrade < b.ItemGrade);

    // 이름 오름차순 정렬
    public List<Item> SortByItemName(List<Item> items) =>
        BubbleSort(items, (a, b) =>
            string.Compare(a.ItemName, b.ItemName, CultureInfo.CurrentCulture, CompareOptions.None) > 0);

    // 버블 정렬
    private List<Item> BubbleSort(List<Item> items, Func<Item, Item, bool> compare )
    {
        List<Item> sortedItems = new List<Item>(items);

        // 버블 정렬
        for (int i = 0; i < sortedItems.Count - 1; i++)
        {
            for (int j = 0; j < sortedItems.Count - 1 - i; j++)
            {
                if (compare(sortedItems[j], sortedItems[j+1]))
                {
                    Item temp = sortedItems[j];
                    sortedItems[j] = sortedItems[j + 1];
                    sortedItems[j + 1] = temp;
                }
            }
        }
        return sortedItems;
    }
}
