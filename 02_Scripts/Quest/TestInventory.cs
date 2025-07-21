using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInventory : MonoBehaviour
{
    public void Add(string itemId, int amount = 1)
    { 
        QuestEvents.ItemCollected(itemId, amount);
    }

    public void AddHealthPotion()
    {
        Add("HealthPotion", 1);
    }
}
