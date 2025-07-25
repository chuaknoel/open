using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestAnimation : MonoBehaviour
{
    private Animator animator;

    private bool canDamaged = true;

    private bool isIdleState = false;
    private bool isMoveState = false;
    private bool isAttackState =  false;

    private bool isMove = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            PlayIdleState();
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            PlayMoveState();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            PlayAttackState();
        }

        if (Input.GetKeyDown(KeyCode.R) && canDamaged)
        {
            animator.SetBool("EnemyIdleState", false);
            animator.SetBool("EnemyMoveState", false);
            animator.SetBool("EnemyAttackState", false);

            canDamaged = false;
            animator.SetBool("EnemyDamagedState", true);

            StartCoroutine(DamagedAnimation());
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            animator.SetBool("EnemyDeathState", true);
        }


        if ( Input.GetKey(KeyCode.L) && !isMove)
        {
            animator.SetInteger("LookDir", 0);
            isMove = true;
        }

        if (Input.GetKey(KeyCode.I) && !isMove)
        {
            animator.SetInteger("LookDir", 1);
            isMove = true;
        }

        if (Input.GetKey(KeyCode.J) && !isMove)
        {
            animator.SetInteger("LookDir", 2);
            isMove = true;
        }

        if (Input.GetKey(KeyCode.K) && !isMove)
        {
            animator.SetInteger("LookDir", 3);
            isMove = true;
        }


    }

    private void PlayIdleState()
    {
        animator.SetBool("EnemyIdleState", true);
        animator.SetBool("EnemyMoveState", false);
        animator.SetBool("EnemyAttackState", false);

        isIdleState = true;
        isMoveState = false;
        isAttackState = false;
    }

    private void PlayMoveState()
    {
        animator.SetBool("EnemyIdleState", false);
        animator.SetBool("EnemyMoveState", true);
        animator.SetBool("EnemyAttackState", false);

        isIdleState = false;
        isMoveState = true;
        isAttackState = false;
    }

    private void PlayAttackState()
    {
        animator.SetBool("EnemyIdleState", false);
        animator.SetBool("EnemyMoveState", false);
        animator.SetBool("EnemyAttackState", true);

        isIdleState = false;
        isMoveState = false;
        isAttackState = true;
    }
    private IEnumerator DamagedAnimation()
    {
        yield return new WaitForSeconds(1.0f);

        animator.SetBool("EnemyDamagedState", false);
        canDamaged = true;

        if (isIdleState)
        {
            PlayIdleState();
        }
        else if (isMoveState)
        {
            PlayMoveState();
        }
        else if (isAttackState)
        {
            PlayAttackState();
        }
    }
    //public void ExitBattleScene()
    //{
    //    //플레이어가 죽으면 return

    //    activatedEnemyCount--;
    //    currentPlayerPosition = player.transform.position;

    //    if (activatedEnemyCount == 0)
    //    {
    //        StartCoroutine(LoadMainSceneRoutine());
    //    }
    //}

    //private IEnumerator LoadMainSceneRoutine()
    //{
    //    SceneManager.LoadScene("TutorialScene");

    //    yield return new WaitUntil(() => SceneManager.GetActiveScene().name == "TutorialScene");

    //    GetPlayer();
    //    AddExternalEnemyList();

    //    externalEnemy.transform.gameObject.SetActive(false);
    //    player.transform.position = currentPlayerPosition;
    //}
}
