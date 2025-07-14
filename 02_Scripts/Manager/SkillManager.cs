using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager instance;

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
        instance = this;
        Init();
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
        return allSkillDictionary[skillCode];
    }

    private void OnDestroy()
    {
        instance = null;
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
