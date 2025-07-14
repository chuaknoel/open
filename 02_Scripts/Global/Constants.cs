using UnityEngine;

public static class Constants
{
    public static class BGM
    {

    }

    public static class SFX
    {
        //public static readonly string Jump = "Ambient/Jump";
    }

    public static class Voice
    {

    }

    public static class Ambient
    {

    }

    public static class AnimatorHash
    {
        public static readonly int LookDirHash = Animator.StringToHash("LookDir");
        public static readonly int MoveStateHash = Animator.StringToHash("MoveState");
        public static readonly int AttackStateHash = Animator.StringToHash("AttackState");
        public static readonly int DamagedStateHash = Animator.StringToHash("DamagedState");
        public static readonly int DashStateHash = Animator.StringToHash("DashState");
        public static readonly int DeathTriggerHash = Animator.StringToHash("DeathTrigger");
        public static readonly int DeathStateHash = Animator.StringToHash("DeathState");
        public static readonly int IdleStateHash = Animator.StringToHash("IdleState");
        public static readonly int SkillStateHash = Animator.StringToHash("SkillState");
        public static readonly int SkillButtonHash = Animator.StringToHash("SkillButton");

        public static readonly int EnemyIdleStateHash = Animator.StringToHash("EnemyIdleState");
        public static readonly int EnemyMoveStateHash = Animator.StringToHash("EnemyMoveState");
        public static readonly int EnemyAttackStateHash = Animator.StringToHash("EnemyAttackState");
        public static readonly int EnemyDamagedStateHash = Animator.StringToHash("EnemyDamagedState");
        public static readonly int EnemyDeathStateHash = Animator.StringToHash("EnemyDeathState");
    }
}