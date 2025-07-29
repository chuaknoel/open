using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#region 데이터 모델 클래스 정의
//=======================================================================================
// 이 파일 안에서 모든 데이터 구조(모델)를 정의하여,
// 어떤 종류의 데이터가 있는지 한눈에 파악하고 관리하기 용이하도록 합니다.
// 각 클래스는 IGetCSVData를 상속받아 ID, NameKey, DescKey를 필수로 가집니다.
//=======================================================================================

/// <summary>
/// 무기 데이터의 구조를 정의하는 클래스입니다.
/// </summary>
[System.Serializable]  // ⬅️ 추가 필요
public class WeaponData : IGetCSVData
{
    public string ID { get; set; }    // 프로퍼티는 직렬화 안됨
    public string Type;
    public string Rank;
    public int RanktoInt;        // 새로 추가
    public int Atk;
    public string Name;
    public string Description;
    public Vector2 Area;
    public string PrefabAddress; // 새로 추가
}

/// <summary>
/// 재료 아이템 데이터의 구조를 정의하는 클래스입니다.
/// </summary>
[Serializable]
public class MaterialData : IGetCSVData
{
    public string ID { get; set; }
    public string NameKey { get; set; }
    public string DescKey { get; set; }
    public string Category;
    public int MaxCount;
}

/// <summary>
/// 플레이어블 캐릭터 데이터의 구조를 정의하는 클래스입니다.
/// </summary>
[Serializable]
public class CharacterData : IGetCSVData
{
    public string ID { get; set; }
    public string NameKey { get; set; }
    public string DescKey { get; set; }

    [Header("Base Stat")]
    public float baseAttack;
    public float baseDefence;

    public float baseMoveSpeed;

    public float baseHealth;
    public float currentHealth;

    public float baseMana;
    public float currentMana;

    public float baseEvasionRate; // 회피율

    [Header("Player Stat")]
    public int baseStr;
    public int baseDex;
    public int baseInt;
    public int baseSanctity;

    public int level;
    public int Exp;
    public int nextLevelExp;

    [Header("Player Skill")]
    public List<int> skillTree;
}

[System.Serializable]
public class NPCData : IGetCSVData
{
    public string ID { get; set; }
    public string Name;
    public bool IsMerchant;          // ★ 상인 여부 필드 추가
    public string InitialDialogue;
    public string LocationHint;
}

/// <summary>
/// 동료 캐릭터 데이터의 구조를 정의하는 클래스입니다.
/// </summary>
[Serializable]
public class CompanionData : IGetCSVData
{
    public string ID { get; set; }
    public string NameKey { get; set; }

    /// <summary>
    /// 동료의 특별 설명(SpecDescKey)을 담는 변수입니다.
    /// IGetCSVData 인터페이스의 DescKey 역할을 합니다.
    /// </summary>
    public string DescKey { get; set; }

    public bool IsJoined;
    public int TrustLevel;
    public string DialogueKey;

    public string SkillNameKey;
    public string SkillDescKey;

    public float AttackPower;
    public float DefensePower;
    public float MoveSpeed;
    public float Hp;
    public float Mp;
    public float EvasionRate; // 회피율
    public string CompanionPrefabAddress;
}

//[Serializable]
//public class QuestData : IGetCSVData
//{
//    public string ID { get; set; }
//    public string Type;           // "Main" 또는 "Sub"
//    public string Title;          // 퀘스트 제목
//    public string GiverNPC;       // 퀘스트를 준 NPC (메인퀘스트는 빈 값)
//    public string StoryDesc;      // 스토리적 설명
//    public string QuestContent;   // 실제 퀘스트 내용 (목표)
//    public string RewardItemID;   // 보상 아이템 ID (메인퀘스트는 빈 값)
//    public string CompletionStory; // 완료 후 짧은 이야기
//}


[Serializable]
public class EnemyData : IGetCSVData
{
    public string ID { get; set; }
    public string Name;
    public string Description;
    public int HP;
    public int ATK;
    public int DEF;
    public string DropItemID;

    public string enemyPrefabAddress;
}


[Serializable]
public class ArmorData : IGetCSVData
{
    public string ID { get; set; }
    public string Type;
    public string Rank;
    public int RanktoInt;        // ← 새로 추가
    public int DEF;
    public string Name;
    public string Description;
    public string PrefabAddress; // ← 새로 추가
}

#endregion

/// <summary>
/// 모든 종류의 CSV 텍스트를 C# 객체 딕셔너리로 변환하는 static 클래스입니다.
/// 게임 데이터 로딩 시 이 클래스의 메소드들이 호출됩니다.
/// </summary>
public static class CSVConverter
{
    /// <summary>
    /// CSV 텍스트를 파싱하는 내부 헬퍼 메소드입니다.
    /// </summary>
    private static IEnumerable<string[]> Parse(string csvText)
    {
        var lines = csvText.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
        return lines.Skip(1).Select(line => line.Split(','));
    }

    // --- 각 CSV 파일별 파싱 메소드 ---

    /// <summary>
    /// 새로운 Weapons.csv 구조에 맞춰 파싱 로직을 수정했습니다.
    /// </summary>
    public static Dictionary<string, WeaponData> LoadWeapons(string csvText)
    {
        var db = new Dictionary<string, WeaponData>();
        foreach (var values in Parse(csvText))
        {
            // 열 개수가 7개가 아닐 경우 데이터 오류로 보고 건너뜁니다.
            if (values.Length < 7) continue;

            try
            {
                var data = new WeaponData
                {
                    ID = values[0],
                    Type = values[1],
                    Rank = values[2],
                    Atk = int.Parse(values[3]),
                    Name = values[4],
                    Description = values[5],
                    Area = ParseVector2(values[6])
                };
                db[data.ID] = data;
            }
            catch { }
        }
        return db;
    }

    private static Vector2 ParseVector2(string s)
    {
        try
        {
            var parts = s.Split(';');
            if (parts.Length == 2) return new Vector2(float.Parse(parts[0]), float.Parse(parts[1]));
        }
        catch { }
        return Vector2.zero;
    }

    // 스킬 목록을 파싱하는 헬퍼 메서드 (예: "101;202;301")
    private static List<int> ParseIntList(string s)
    {
        var list = new List<int>();
        if (string.IsNullOrEmpty(s)) return list;

        var parts = s.Split(';');
        foreach (var part in parts)
        {
            if (int.TryParse(part, out int val))
            {
                list.Add(val);
            }
        }
        return list;
    }

    public static Dictionary<string, SkillData> LoadSkills(string csvText)
    {
        var db = new Dictionary<string, SkillData>();
        foreach (var values in Parse(csvText))
        {
            // CSV 열 개수에 맞게 수정하세요. (예시: 20개)
            if (values.Length < 20) continue;

            try
            {
                var data = new SkillData
                {
                    // CSV 열 순서에 맞춰 데이터를 읽어옵니다.
                    // 이 순서는 Skills.csv 파일의 실제 열 순서와 일치해야 합니다.
                    skillCode = int.Parse(values[0]),
                    skillName = values[1],
                    skillDescription = values[2],
                    skillLevel = int.Parse(values[3]),
                    amount = float.Parse(values[4]),
                    duration = float.Parse(values[5]),
                    hitCount = int.Parse(values[6]),
                    coolTime = float.Parse(values[7]),
                    cunsumeMana = int.Parse(values[8]),
                    range = float.Parse(values[9]),
                    skillType = (Enums.SkillType)System.Enum.Parse(typeof(Enums.SkillType), values[10]),
                    targetType = (Enums.SkillTargetType)System.Enum.Parse(typeof(Enums.SkillTargetType), values[11]),
                    damageType = (Enums.SKillDamageType)System.Enum.Parse(typeof(Enums.SKillDamageType), values[12]),
                    skillPrefabAddress = values[13],
                    skillImageAddress = values[14],
                    requiredLevel = int.Parse(values[15]),
                    requiredSkills = ParseIntList(values[16]), // "101;202" 같은 형식을 처리
                    requiredPoint = int.Parse(values[17]),
                    isLock = bool.Parse(values[18])
                };
                // skillImage는 CSV로 관리하기 어려우므로 여기서는 비워둡니다.
                // data.skillImage = ...

                // 딕셔너리의 키는 string이어야 하므로 skillCode를 문자열로 변환하여 사용합니다.
                db[data.skillCode.ToString()] = data;
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Skill CSV 파싱 오류: {string.Join(",", values)} | 오류: {ex.Message}");
            }
        }
        return db;
    }

    public static Dictionary<string, NPCData> LoadNPCs(string csvText)
    {
        var db = new Dictionary<string, NPCData>();
        foreach (var values in Parse(csvText))
        {
            // 열 개수가 5개가 아닐 경우 데이터 오류로 보고 건너뜁니다.
            if (values.Length < 5) continue;

            try
            {
                var data = new NPCData
                {
                    ID = values[0],
                    Name = values[1],
                    IsMerchant = bool.Parse(values[2]), // ★ 상인 여부 파싱 추가
                    InitialDialogue = values[3],
                    LocationHint = values[4]
                };
                db[data.ID] = data;
            }
            catch { /* 데이터 형식 오류 시 해당 줄은 건너뜁니다. */ }
        }
        return db;
    }

    public static Dictionary<string, MaterialData> LoadMaterials(string csvText)
    {
        var db = new Dictionary<string, MaterialData>();
        foreach (var values in Parse(csvText))
        {
            try
            {
                var data = new MaterialData { ID = values[0], Category = values[1], MaxCount = int.Parse(values[2]), NameKey = values[3], DescKey = values[4] };
                db[data.ID] = data;
            }
            catch { /* 데이터 형식 오류 시 해당 줄은 건너뜁니다. */ }
        }
        return db;
    }

    //public static Dictionary<string, QuestData> LoadQuests(string csvText)
    //{
    //    var db = new Dictionary<string, QuestData>();
    //    foreach (var values in Parse(csvText))
    //    {
    //        // 열 개수가 8개가 아닐 경우 데이터 오류로 보고 건너뜁니다.
    //        if (values.Length < 8) continue;

    //        try
    //        {
    //            var data = new QuestData
    //            {
    //                ID = values[0],
    //                Type = values[1],
    //                Title = values[2],
    //                GiverNPC = values[3],
    //                StoryDesc = values[4],
    //                QuestContent = values[5],
    //                RewardItemID = values[6],
    //                CompletionStory = values[7]
    //            };
    //            db[data.ID] = data;
    //        }
    //        catch { /* 데이터 형식 오류 시 해당 줄은 건너뜁니다. */ }
    //    }
    //    return db;
    //}

    public static Dictionary<string, CharacterData> LoadCharacters(string csvText)
    {
        var db = new Dictionary<string, CharacterData>();
        foreach (var values in Parse(csvText))
        {
            try
            {
                var data = new CharacterData { ID = values[0], NameKey = values[1], DescKey = values[2], baseHealth = int.Parse(values[3]), baseMoveSpeed = int.Parse(values[4]) };
                db[data.ID] = data;
            }
            catch { /* 데이터 형식 오류 시 해당 줄은 건너뜁니다. */ }
        }
        return db;
    }

    /// <summary>
    /// Companions.csv 파일을 파싱하여 CompanionData 딕셔너리를 생성합니다. (수정됨)
    /// </summary>
    public static Dictionary<string, CompanionData> LoadCompanions(string csvText)
    {
        var db = new Dictionary<string, CompanionData>();
        foreach (var values in Parse(csvText))
        {
            try
            {
                // CSV 파일의 열 순서에 맞춰 데이터를 파싱합니다.
                var data = new CompanionData
                {
                    // Companions.csv의 첫 번째 열을 ID로 가정합니다.
                    ID = values[0],                 
                    NameKey = values[1],
                    // IsJoined는 true/false 문자열을 bool 타입으로 변환합니다.
                    IsJoined = bool.Parse(values[2]),
                    TrustLevel = int.Parse(values[3]),
                    DialogueKey = values[4],
                    // SpecDescKey 열의 값을 DescKey 프로퍼티에 할당합니다.
                    DescKey = values[5],
                    SkillNameKey = values[6],
                    SkillDescKey = values[7],

                    AttackPower = float.Parse(values[8]),
                    DefensePower = float.Parse(values[9]),
                    MoveSpeed = float.Parse(values[10]),
                    Hp = float.Parse(values[11]),
                    Mp = float.Parse(values[12]),
                    EvasionRate = float.Parse(values[13]),
                    CompanionPrefabAddress = values[14]
                };
                db[data.ID] = data;
            }
            catch { Logger.Log("데이터 형식 오류 시 해당 줄은 건너뜁니다"); /* 데이터 형식 오류 시 해당 줄은 건너뜁니다. */ }
        }
        return db;
    }

    public static Dictionary<string, EnemyData> LoadEnemies(string csvText)
    {
        var db = new Dictionary<string, EnemyData>();
        foreach (var values in Parse(csvText))
        {
            // 열 개수가 7개가 아닐 경우 데이터 오류로 보고 건너뜁니다.
            if (values.Length < 7) continue;

            try
            {
                var data = new EnemyData
                {
                    ID = values[0],
                    Name = values[1],
                    Description = values[2],
                    HP = int.Parse(values[3]),
                    ATK = int.Parse(values[4]),
                    DEF = int.Parse(values[5]),
                    DropItemID = values[6]
                };
                db[data.ID] = data;
            }
            catch { /* 형식 오류가 있는 줄은 건너뜁니다 */ }
        }
        return db;
    }


    public static Dictionary<string, ArmorData> LoadArmors(string csvText)
    {
        var db = new Dictionary<string, ArmorData>();
        foreach (var values in Parse(csvText))
        {
            try
            {
                var data = new ArmorData
                {
                    ID = values[0],
                    Type = values[1],
                    Rank = values[2],
                    DEF = int.Parse(values[3]),
                    Name = values[4],
                    Description = values[5]
                };
                db[data.ID] = data;
            }
            catch { /* 형식 오류가 있는 줄은 건너뜁니다 */ }
        }
        return db;
    }

    public static Dictionary<string, string> LoadLocalization(string csvText, string langCode)
    {
        var db = new Dictionary<string, string>();
        var lines = csvText.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
        if (lines.Length < 1) return db;
        var headers = lines[0].Split(',');
        int langIndex = Array.IndexOf(headers, langCode);
        if (langIndex == -1) return db;
        foreach (var line in lines.Skip(1))
        {
            var values = line.Split(',');
            string key = values[0];
            if (string.IsNullOrEmpty(key) || values.Length <= langIndex) continue;
            db[key] = values[langIndex].Replace("\"\"", "\"");
        }
        return db;
    }
}