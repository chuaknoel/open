using UnityEngine;
using System.IO;
using System.Collections.Generic;
using UnityEngine.Rendering;

/// <summary>
/// 게임의 모든 핵심 데이터(아이템, 캐릭터 스탯 등)를 로드하고 관리하는 통합 매니저입니다.
/// 싱글턴 패턴으로 구현되어 어디서든 `DataManager.Instance`로 쉽게 접근할 수 있습니다.
/// </summary>
public class DataManager : MonoBehaviour
{
    public static DataManager Instance { get; private set; }

    public DefaultData defaultData;
    public SaveData saveData;
    public Dictionary<string, WeaponData> WeaponDB { get; private set; }
    public Dictionary<string, MaterialData> MaterialDB { get; private set; }
    //public Dictionary<string, CharacterData> CharacterDB { get; private set; }
    public Dictionary<string, CompanionData> CompanionDB { get; private set; }
    public Dictionary<string, EnemyData> EnemyDB { get; private set; }
    public Dictionary<string, ArmorData> ArmorDB { get; private set; }
    public Dictionary<string, QuestData> QuestDB { get; private set; }
    public Dictionary<string, NPCData> NPCDB { get; private set; }

    private string saveDataPath;
    private string saveFileName = "save.json";

    private bool isLoad;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void Init()
    {
        if (isLoad) return;
        isLoad = true;
        LoadAllData();
        saveDataPath = Application.persistentDataPath + saveFileName;
    }

    /// <summary>
    /// 게임 시작 시 Resources/CSV 폴더에 있는 모든 데이터 CSV 파일을 로드합니다.
    /// </summary>
    private void LoadAllData()
    {
        LoadData();

        WeaponDB = CSVConverter.LoadWeapons(LoadCsvFile("Weapons"));
        MaterialDB = CSVConverter.LoadMaterials(LoadCsvFile("Materials(final)"));
        //CharacterDB = CSVConverter.LoadCharacters(LoadCsvFile("Characters"));
        CompanionDB = CSVConverter.LoadCompanions(LoadCsvFile("Companions"));
        EnemyDB = CSVConverter.LoadEnemies(LoadCsvFile("Enemy"));
        ArmorDB = CSVConverter.LoadArmors(LoadCsvFile("Armors"));

        NPCDB = CSVConverter.LoadNPCs(LoadCsvFile("NPCs"));
    }

    /// <summary>
    /// Resources/CSV 폴더에서 특정 이름의 CSV 파일을 읽어 텍스트로 반환하는 헬퍼 메소드입니다.
    /// </summary>
    private string LoadCsvFile(string fileName)
    {

        TextAsset textAsset = Resources.Load<TextAsset>($"CSV/{fileName}");
        if (textAsset == null) return "";
        return textAsset.text;
    }

    public void LoadData()
    {
        if (!File.Exists(saveDataPath))
        {
            TextAsset defaulData = Resources.Load<TextAsset>("Json/Default");

            if (defaulData != null)
            {
                string loadData = defaulData.text;
                defaultData = JsonUtility.FromJson<DefaultData>(loadData);
                Logger.Log($"디폴트 데이터 로드 : {defaultData}");
            }
            else
            {
                Logger.LogError($"디폴트 데이터 로드에 실패 했습니다.");
            }
        }
        else
        {
            string data = File.ReadAllText(saveDataPath);
            saveData = JsonUtility.FromJson<SaveData>(data);
        }
    }
}