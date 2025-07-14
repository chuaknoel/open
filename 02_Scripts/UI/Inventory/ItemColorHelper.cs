using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemGradeColorPair
{
    public ItemGrade itemGrade;
    public Color color;
}
public class ItemColorHelper : MonoBehaviour
{
    [SerializeField] private List<ItemGradeColorPair> colorList;

    // 아이템 레벨에 따른 색상 설정
    public Color GetColor(ItemGrade grade)
    {
        foreach (var color in colorList)
        {
            if (color.itemGrade == grade)
                return color.color;
        }
        return Color.gray;
    }
}
