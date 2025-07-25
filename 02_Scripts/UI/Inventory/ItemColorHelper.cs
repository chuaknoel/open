using Enums;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 아이템 등급에 따른 배경 색상 정보를 담는 클래스입니다.
/// </summary>
[System.Serializable]
public class ItemGradeColorPair
{
    public ItemGrade itemGrade;
    public Color color;
}

/// <summary>
/// 아이템 등급에 따른 색상을 설정하는 클래스입니다.
/// </summary>
public class ItemColorHelper : MonoBehaviour
{
    [SerializeField] private List<ItemGradeColorPair> colorList;

    /// <summary>
    /// 아이템 등급에 따른 색상을 설정합니다.
    /// </summary>
    /// <param name="grade">아이템 등급</param>
    /// <returns></returns>
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
