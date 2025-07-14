using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
using System;
using UnityEngine.UI;

[Serializable]
public class SkillData
{
    public int skillCode;       //스킬 고유 Code
    public string skillName;         //스킬 이름
    public string skillDescription;  //스킬 설명
    public Sprite skillImage;

    public int skillLevel;      //스킬의 레벨 : 스킬 포인트를 투자하여 강화 된 스킬 자체의 레벨

    public float amount;        //스킬의 효과 수치 : 공격 스킬이면 공격력, 치유 스킬이면 힐량, 
    public float duration;      //스킬 지속 시간

    public int hitCount;        //스킬의 효과 카운트 : 공격 스킬이면 히트 카운트 만큼 공격, 도트 스킬의 경우 히트 카운트 횟수만큼 피해

    public float coolTime;      //스킬 쿨타임
    public float remainingTime; //현재 스킬 쿨타임
    public int cunsumeMana;     //스킬 사용시 소모되는 마나

    public float range;         //스킬의 사정거리

    public SkillType skillType;         //스킬의 타입 : 공격, 버프, 디버프 등
    public SkillTargetType targetType;  //스킬 효과가 발현될 대상 : 플레이어, 에너미, 동료 등
    public SKillDamageType damageType;  //스킬의 데미지 타입 : 물리, 마법 등
    
    public string skillPrefabAddress;      //씬에서 사용할 프리팹의 어드레서블 주소
    public string skillImageAddress;       //스킬 이미지 주소
    public List<string> skillSoundAddress; //스킬 사운드 주소

    //제약 조건
    public int requiredLevel;                           //스킬 습득 가능 레벨
    public List<int> requiredSkills = new List<int>();  //스킬을 배우기 위한 선행 스킬 리스트 : 신성한 빛을 배우기 위해서 홀리 라이트를 배워야 한다 등
    public int requiredPoint;                           //스킬을 배우기 위해 필요한 포인트
    public bool isLock;                                 //스킬 해금 여부

    public SkillData() { }
}

public class SKillEffect
{
    public SkillEffectType type;//스킬 효과 타입 : Burn, Stun, Slow, 등
    public float amount;        //효과 계수 : 슬로우 30%, 중독 피해 15 등
    public float duration;      //지속 시간
    public float interval;      //간격 : 1초마다 적용 (도트/힐오버타임 등)
}