using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager Instance { get; private set; }

    public List<SkillData> skillDatas = new List<SkillData>();
    public Dictionary<int, Skill> allSkillDictionary = new Dictionary<int, Skill>();

    public Skill TestData;

    public Transform playerSkill;
    public Transform lionSkill;
    public Transform ElaraSkill;
    public Transform gerrettSkill;
    public Transform seraphineSkill;

    public Sprite tempImage;

    private void Awake()
    {
        if (Instance != null)
        {
            Logger.LogError($"[YourManager] Duplicate instance detected on '{gameObject.name}'. " +
                        $"This may indicate a missing unload or unintended duplicate. Destroying this instance.");
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void OnEnable()
    {
        if (Instance == this)
        {
            return;
        }
        else if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Logger.LogError($"[YourManager] Duplicate instance detected on '{gameObject.name}'");
        }
    }

    public void Init()
    {
        MakeSkill();
        SetSkill();
    }

    private void MakeSkill()
    {
        SkillData skill = new SkillData();
        skill.skillName = "스킬01";
        skill.skillDescription = "첫번째 스킬입니다.";
        skill.skillImage = tempImage;
        skillDatas.Add(skill);
    }

    public void SetSkill()
    {
        //skillDatas = LoadData.SkillData;
        foreach (var skill in skillDatas)
        {
            //allSkillDictionary[skill.skillCode] = 어드레서블Load(skill.skillPrefabAddress);
            allSkillDictionary[skill.skillCode] = TestData;
        }
    }

    public Skill FindSkill(int skillCode)
    {
        if (allSkillDictionary.ContainsKey(skillCode))
        {
            return allSkillDictionary[skillCode];
        }
        else
        {
            Logger.Log($"{skillCode} was not found in the skill registry. Please check if it's properly defined.");
            return null;
        }
    }

    public void UnLoad()
    {
        if (Instance == this)
        {
            Instance = null;
        }
        else if (Instance == null)
        {
            Logger.LogError($"[YourManager] UnLoad called, but Instance was already null. Possible duplicate unload or uninitialized state.");
        }
        else
        {
            Logger.LogError($"[YourManager] UnLoad called by a non-instance object: {gameObject.name}. Current Instance is on {Instance.gameObject.name}");
        }
    }
}

//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class SkillManager : MonoBehaviour
//{
//    public Sprite tempImage;
//    //public List<SkillData> skillList = new List<SkillData>();
//    public GameObject temporaryQuickSlot;
//    private void Start()
//    {
//        MakeSkill();
//    }
//    private void MakeSkill()
//    {
//        SkillData skill = new SkillData();
//        skill.skillName = "스킬01";
//        skill.skillDescription = "첫번째 스킬입니다.";
//        skill.skillImage = tempImage;
//        skillList.Add(skill);
//    }
//}
