using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanionInfo
{
    [Header("Info")]
    private string companionName;
    private bool joining;
    private int trustLevel;
    private string dialogueKey;
    private string descKey;
    private string skillName;
    private string skillDescKey;

    [Header("AbilityData")]
    private int attackPower;
    private int defensePower;
    private int moveSpeed;
    private int hp;
    private int mp;
    private float evasionRate; // 회피율

    public string CompanionName => companionName;
    public bool Joining => joining;
    public int TrustLevel => trustLevel;
    public string DialogueKey => dialogueKey;
    public string DescKey => descKey;
    public string SkillName => skillName;
    public string SkillsDescKey => skillDescKey;    
    public int AttackPower => attackPower;
    public int DefensePower => defensePower;
    public int MoveSpeed => moveSpeed;
    public float HP => hp;
    public float MP => mp;
    public float EvasionRate => evasionRate;

    public CompanionInfo(string _name, bool _isJoined, int _trust, string _dialogue, string _descKey, string _skill, string _skillDesc,
        int _attackPower, int _defensePower, int _moveSpeed, int _hp, int _mp, float _evasionRate) 
    {
        companionName = _name;
        joining = _isJoined;
        trustLevel = _trust;
        dialogueKey = _dialogue;
        descKey = _descKey;
        skillName = _skill;
        skillDescKey = _skillDesc;
        attackPower = _attackPower;
        defensePower = _defensePower;
        moveSpeed = _moveSpeed;
        hp = _hp;
        mp = _mp;
        evasionRate = _evasionRate;
    }
}
