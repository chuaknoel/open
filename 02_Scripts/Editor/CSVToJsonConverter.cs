using UnityEngine;
using UnityEditor; // Unity 에디터 기능을 사용하기 위해 필수입니다.
using System.IO;   // 파일 읽기/쓰기를 위해 필수입니다.
using System.Collections.Generic;
using System.Linq; // Linq 사용을 위해 추가합니다.
using System.Text.RegularExpressions; // 정규식 사용을 위해 추가합니다.

/// <summary>
/// Unity 에디터 환경에서 CSV 데이터 파일을 JSON 파일로 변환하는 기능을 제공하는 클래스입니다.
/// 이 스크립트는 'Assets/Editor' 폴더 안에 있어야 합니다.
/// 상단 메뉴의 [My Tools > Convert All CSV to JSON]을 통해 실행할 수 있습니다.
/// </summary>
public class CSVToJsonConverter
{
    // 원본 CSV 파일들이 위치한 경로입니다.
    private static string csvPath = Path.Combine(Application.dataPath, "Resources", "CSV");
    // 변환된 JSON 파일들을 저장할 경로입니다.
    private static string jsonPath = Path.Combine(Application.dataPath, "Resources", "Json");

    /// <summary>
    /// Unity 에디터 상단 메뉴에 "My Tools/Convert All CSV to JSON" 항목을 추가하고,
    /// 클릭 시 ConvertAllFiles 메소드를 실행하도록 합니다.
    /// 모든 게임 데이터 CSV 파일을 JSON으로 일괄 변환하는 메인 진입점입니다.
    /// </summary>
    [MenuItem("My Tools/Convert All CSV to JSON")]
    public static void ConvertAllFiles()
    {
        // JSON을 저장할 폴더가 존재하지 않으면 새로 생성합니다.
        if (!Directory.Exists(jsonPath))
        {
            Directory.CreateDirectory(jsonPath);
            Logger.Log($"'{jsonPath}' 폴더를 생성했습니다.");
        }

        // 각 CSV 파일에 대해 변환 작업을 수행합니다.
        // 기존 CSVConverter 메소드들을 사용하는 변환 방식으로 통일합니다.
        ConvertLegacy<WeaponData>("Weapons", LoadWeapons);
        ConvertLegacy<ArmorData>("armor", LoadArmors, "Armors"); // armor.csv -> Armors.json
        ConvertLegacy<MaterialData>("Materials(final)", CSVConverter.LoadMaterials, "Materials"); // Materials(final).csv -> Materials.json
        ConvertLegacy<CompanionData>("Companions", CSVConverter.LoadCompanions);
        ConvertLegacy<EnemyData>("Enemy", CSVConverter.LoadEnemies);
        ConvertLegacy<NPCData>("NPC", CSVConverter.LoadNPCs);

        // Skill 데이터는 별도의 메소드로 처리합니다.
        ConvertSkills();

        // 작업 완료 후 사용자에게 알림창을 띄웁니다.
        EditorUtility.DisplayDialog("CSV to JSON 변환 완료",
            "모든 CSV 파일이 성공적으로 JSON으로 변환되었습니다.\n" +
            $"저장 위치: {jsonPath}",
            "확인");

        // Unity 에디터가 새로 생성된 파일들을 인식하도록 AssetDatabase를 새로고침합니다.
        AssetDatabase.Refresh();
    }

    /// <summary>
    /// 기존 CSVConverter 클래스의 메소드를 사용하는 레거시 데이터 타입을 위한 제네릭 변환 메소드입니다.
    /// IGetCSVData 인터페이스를 구현하지 않는 기존 데이터 클래스들을 위해 사용됩니다.
    /// CSV 파일을 읽어 지정된 파서로 딕셔너리를 생성한 후, JSON 파일로 저장합니다.
    /// </summary>
    /// <typeparam name="T">변환할 데이터 클래스 타입</typeparam>
    /// <param name="csvFileName">변환할 CSV 파일의 이름 (확장자 제외)</param>
    /// <param name="parser">CSV 텍스트를 딕셔너리로 변환하는 파서 함수</param>
    /// <param name="jsonFileName">저장할 JSON 파일 이름 (null일 경우 csvFileName과 동일하게 설정)</param>
    private static void ConvertLegacy<T>(string csvFileName, System.Func<string, Dictionary<string, T>> parser, string jsonFileName = null)
    {
        string csvFilePath = Path.Combine(csvPath, $"{csvFileName}.csv");

        if (string.IsNullOrEmpty(jsonFileName))
        {
            jsonFileName = csvFileName;
        }
        string jsonFilePath = Path.Combine(jsonPath, $"{jsonFileName}.json");

        if (File.Exists(csvFilePath))
        {
            string csvText = File.ReadAllText(csvFilePath);
            Dictionary<string, T> dataDictionary = parser(csvText);

            List<T> dataList = new List<T>(dataDictionary.Values);
            string json = JsonUtility.ToJson(new Serialization<T>(dataList), true);

            File.WriteAllText(jsonFilePath, json);
            Logger.Log($"<color=green>성공:</color> {csvFileName}.csv -> {jsonFileName}.json 변환 완료.");
        }
        else
        {
            Logger.LogError($"<color=red>실패:</color> {csvFilePath} 파일을 찾을 수 없습니다.");
        }
    }

    /// <summary>
    /// SkillData 전용 변환 메소드입니다.
    /// Skills.csv 파일의 복잡한 구조와 Enum 파싱을 위해 별도로 구현된 메소드입니다.
    /// 정규식 기반 CSV 파서를 사용하여 쉼표가 포함된 필드도 올바르게 처리합니다.
    /// </summary>
    private static void ConvertSkills()
    {
        string csvFileName = "Skills";
        string jsonFileName = "Skills";
        string csvFilePath = Path.Combine(csvPath, $"{csvFileName}.csv");
        string jsonFilePath = Path.Combine(jsonPath, $"{jsonFileName}.json");

        if (File.Exists(csvFilePath))
        {
            string csvText = File.ReadAllText(csvFilePath);
            Dictionary<string, SkillData> dataDictionary = LoadSkills(csvText);

            List<SkillData> dataList = new List<SkillData>(dataDictionary.Values);
            string json = JsonUtility.ToJson(new Serialization<SkillData>(dataList), true);

            File.WriteAllText(jsonFilePath, json);
            Logger.Log($"<color=green>성공:</color> {csvFileName}.csv -> {jsonFileName}.json 변환 완료.");
        }
        else
        {
            Logger.LogError($"<color=red>실패:</color> {csvFilePath} 파일을 찾을 수 없습니다.");
        }
    }

    #region Weapon Parser

    /// <summary>
    /// 업데이트된 Weapons.csv 파일을 파싱하여 WeaponData 딕셔너리를 생성합니다.
    /// 새로운 CSV 구조: ID, Type, Rank, RanktoInt, Atk, Name, Description, Area, PrefabAddress
    /// 기존 WeaponData 클래스 구조에 맞게 매핑합니다.
    /// </summary>
    /// <param name="csvText">Weapons.csv 파일의 전체 텍스트 내용</param>
    /// <returns>무기 ID를 키로 하는 WeaponData 딕셔너리</returns>
    public static Dictionary<string, WeaponData> LoadWeapons(string csvText)
    {
        var db = new Dictionary<string, WeaponData>();
        var lines = csvText.Split(new[] { "\r\n", "\r", "\n" }, System.StringSplitOptions.RemoveEmptyEntries);

        // 헤더 라인을 건너뛰고 데이터 라인부터 처리
        for (int i = 1; i < lines.Length; i++)
        {
            var values = lines[i].Split(',');
            if (values.Length < 9) continue;

            try
            {
                var data = new WeaponData
                {
                    ID = values[0].Trim(),
                    Type = values[1].Trim(),
                    Rank = values[2].Trim(),
                    RanktoInt = int.Parse(values[3].Trim()),
                    Atk = int.Parse(values[4].Trim()),
                    Name = values[5].Trim(),
                    Description = values[6].Trim(),
                    Area = ParseVector2(values[7].Trim()),
                    PrefabAddress = values[8].Trim()
                };
                db[data.ID] = data;
            }
            catch (System.Exception ex)
            {
                Logger.LogError($"Weapon CSV 파싱 오류 (라인 {i}, ID: {values[0]}): {ex.Message}");
            }
        }

        Logger.Log($"총 {db.Count}개의 무기 데이터를 성공적으로 파싱했습니다.");
        return db;
    }

    /// <summary>
    /// Vector2 문자열을 파싱하는 헬퍼 메소드입니다.
    /// "x;y" 형식의 문자열을 Vector2로 변환합니다.
    /// CSV에서 따옴표로 감싸진 경우도 처리합니다.
    /// </summary>
    /// <param name="s">파싱할 문자열 (예: "5.0;3.0" 또는 '"5.0;3.0"')</param>
    /// <returns>Vector2 값</returns>
    private static Vector2 ParseVector2(string s)
    {
        try
        {
            // 따옴표 제거
            string cleanedString = s.Trim('"').Trim();
            var parts = cleanedString.Split(';');
            if (parts.Length == 2)
            {
                float x = float.Parse(parts[0].Trim());
                float y = float.Parse(parts[1].Trim());
                return new Vector2(x, y);
            }
        }
        catch (System.Exception ex)
        {
            Logger.LogError($"Vector2 파싱 실패 - 입력값: '{s}', 오류: {ex.Message}");
        }
        return Vector2.zero;
    }

    #endregion

    #region Armor Parser

    /// <summary>
    /// 업데이트된 armor.csv 파일을 파싱하여 ArmorData 딕셔너리를 생성합니다.
    /// 새로운 CSV 구조: ID, Type, Rank, RanktoInt, DEF, Name, Description, PrefabAddress
    /// 기존 ArmorData 클래스 구조에 맞게 매핑합니다.
    /// </summary>
    /// <param name="csvText">armor.csv 파일의 전체 텍스트 내용</param>
    /// <returns>방어구 ID를 키로 하는 ArmorData 딕셔너리</returns>
    public static Dictionary<string, ArmorData> LoadArmors(string csvText)
    {
        var db = new Dictionary<string, ArmorData>();
        var lines = csvText.Split(new[] { "\r\n", "\r", "\n" }, System.StringSplitOptions.RemoveEmptyEntries);

        // 헤더 라인을 건너뛰고 데이터 라인부터 처리
        for (int i = 1; i < lines.Length; i++)
        {
            var values = lines[i].Split(',');
            if (values.Length < 8) continue;

            try
            {
                var data = new ArmorData
                {
                    ID = values[0].Trim(),
                    Type = values[1].Trim(),
                    Rank = values[2].Trim(),
                    RanktoInt = int.Parse(values[3].Trim()),
                    DEF = int.Parse(values[4].Trim()),
                    Name = values[5].Trim(),
                    Description = values[6].Trim(),
                    PrefabAddress = values[7].Trim()
                };
                db[data.ID] = data;
            }
            catch (System.Exception ex)
            {
                Logger.LogError($"Armor CSV 파싱 오류 (ID: {values[0]}): {string.Join(",", values)} | 오류 메시지: {ex.Message}");
            }
        }
        return db;
    }

    #endregion

    #region Skill Parser

    /// <summary>
    /// 쉼표가 포함된 필드를 처리하기 위해 정규식을 사용하는 CSV 파서입니다.
    /// 따옴표로 감싸진 필드 내부의 쉼표는 구분자로 인식하지 않도록 처리합니다.
    /// 스킬 설명과 같이 복잡한 텍스트가 포함된 CSV 데이터를 안전하게 파싱할 수 있습니다.
    /// </summary>
    /// <param name="csvText">파싱할 CSV 텍스트 전체</param>
    /// <returns>각 행을 문자열 배열로 분할한 데이터 컬렉션</returns>
    private static IEnumerable<string[]> ParseCsv(string csvText)
    {
        var lines = csvText.Split(new[] { "\r\n", "\r", "\n" }, System.StringSplitOptions.RemoveEmptyEntries);

        var csvParser = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)");

        foreach (var line in lines.Skip(1)) // 헤더(첫 줄) 건너뛰기
        {
            string[] fields = csvParser.Split(line);
            for (int i = 0; i < fields.Length; i++)
            {
                fields[i] = fields[i].TrimStart('\"').TrimEnd('\"');
            }
            yield return fields;
        }
    }

    /// <summary>
    /// 세미콜론(;)으로 구분된 정수 목록을 파싱하는 헬퍼 메소드입니다.
    /// 스킬의 선행 조건이나 연관 스킬 ID 목록을 처리할 때 사용됩니다.
    /// 빈 문자열이나 "0" 값인 경우 빈 리스트를 반환합니다.
    /// </summary>
    /// <param name="s">파싱할 문자열 (예: "101;202;301")</param>
    /// <returns>정수 리스트</returns>
    private static List<int> ParseIntList(string s)
    {
        var list = new List<int>();
        if (string.IsNullOrEmpty(s) || s == "0") return list;

        var parts = s.Split(';');
        foreach (var part in parts)
        {
            if (int.TryParse(part.Trim(), out int val))
            {
                list.Add(val);
            }
        }
        return list;
    }

    /// <summary>
    /// Skills.csv 파일을 파싱하여 SkillData 딕셔너리를 생성합니다.
    /// 복잡한 스킬 데이터 구조와 Enum 타입 변환을 처리하며,
    /// 파싱 오류가 발생한 경우 해당 행을 건너뛰고 오류 로그를 출력합니다.
    /// </summary>
    /// <param name="csvText">Skills.csv 파일의 전체 텍스트 내용</param>
    /// <returns>스킬 코드를 키로 하는 SkillData 딕셔너리</returns>
    public static Dictionary<string, SkillData> LoadSkills(string csvText)
    {
        var db = new Dictionary<string, SkillData>();
        foreach (var values in ParseCsv(csvText))
        {
            if (values.Length < 21) continue;

            try
            {
                var data = new SkillData
                {
                    skillCode = int.Parse(values[0]),
                    skillName = values[1],
                    skillDescription = values[5],
                    skillLevel = int.Parse(values[6]),
                    amount = float.Parse(values[7]),
                    duration = float.Parse(values[8]),
                    hitCount = int.Parse(values[9]),
                    coolTime = float.Parse(values[10]),
                    cunsumeMana = int.Parse(values[11]),
                    range = float.Parse(values[12]),
                    // Enum 파싱 전 Trim()을 호출하여 문자열의 앞뒤 공백을 제거합니다.
                    skillType = (Enums.SkillType)System.Enum.Parse(typeof(Enums.SkillType), values[13].Trim(), true),
                    targetType = (Enums.SkillTargetType)System.Enum.Parse(typeof(Enums.SkillTargetType), values[14].Trim(), true),
                    damageType = (Enums.SKillDamageType)System.Enum.Parse(typeof(Enums.SKillDamageType), values[15].Trim(), true),
                    skillPrefabAddress = values[16],
                    skillImageAddress = values[17],
                    requiredLevel = int.Parse(values[18]),
                    requiredSkills = ParseIntList(values[19]),
                    requiredPoint = int.Parse(values[20]),
                    isLock = true
                };
                db[data.skillCode.ToString()] = data;
            }
            catch (System.Exception ex)
            {
                Logger.LogError($"Skill CSV 파싱 오류 (SkillCode: {values[0]}): {string.Join(",", values)} | 오류 메시지: {ex.Message}");
            }
        }
        return db;
    }
    #endregion
}

/// <summary>
/// JsonUtility가 최상위 레벨의 리스트(List)나 딕셔너리(Dictionary)를 직접 JSON으로 변환하지 못하는 문제를
/// 해결하기 위한 래퍼(Wrapper) 클래스입니다. 
/// Unity의 JsonUtility는 단일 객체만 직렬화할 수 있어, 컬렉션을 변환하려면 이처럼 감싸야 합니다.
/// </summary>
/// <typeparam name="T">리스트에 포함될 데이터 타입</typeparam>
[System.Serializable]
public class Serialization<T>
{
    [SerializeField]
    private List<T> items;

    /// <summary>
    /// 래핑된 리스트를 반환하는 메소드입니다.
    /// JSON 역직렬화 후 실제 데이터에 접근할 때 사용됩니다.
    /// </summary>
    /// <returns>래핑된 리스트 데이터</returns>
    public List<T> ToList() { return items; }

    /// <summary>
    /// Serialization 클래스의 생성자입니다.
    /// 리스트를 래핑하여 JSON 직렬화가 가능한 형태로 만듭니다.
    /// </summary>
    /// <param name="items">래핑할 리스트 데이터</param>
    public Serialization(List<T> items)
    {
        this.items = items;
    }
}