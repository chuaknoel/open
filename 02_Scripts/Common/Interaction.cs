using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interaction : MonoBehaviour
{
    private Player player;

    public IInteractable currentInteraction; //현재 인터럭션 되고 있는 오브젝트
    public LayerMask interactionMask;        //인터럭션이 되어야 오브젝트를 구분하는 마스크

    public List<IInteractable> inRangeInteractions = new List<IInteractable>(); //인터럭션 감지 범위 내에 있는 오브젝트

    private float lastUpdateTime = 0f; //마지막 업데이트 시간
    private float updateDelay = 0.1f;  //업데이트 딜레이

    public void Init(Player player)
    {
        this.player = player;
        player.playerActions.Interaction.started += OnInteraction;
    }

    public void OnUpdate()
    {
        if (player.Controller.inputDir == Vector3.zero) return; //플레이어가 움직이고 있을때만 업데이트

        if (Time.time - lastUpdateTime < updateDelay) return;   //딜레이 마다 한번씩만 업데이트 하기 위한 코드
        lastUpdateTime = Time.time;

        IInteractable temp = FindNearestInteraction();           //플레이어가 움직일때 범위내 가장 가까운 인터럭션 오브젝트를 검색

        if (temp == currentInteraction) return;                 //가장 가까운 오브젝트가 현재 인터럭션 중인 오브젝트면 리턴

        currentInteraction?.SetInterface(false);
        currentInteraction = temp;                              //가장 가까운 인터럭션 오브젝트 변경
        currentInteraction?.SetInterface(true);

    }

    public void RotateInterction(float angle)
    {
        transform.localRotation = Quaternion.Euler(0, 0, angle);
    }

    public void GetInteraction(Collider2D collision)
    {
        if (collision.TryGetComponent<IInteractable>(out IInteractable newInteraction))
        {
            if (!newInteraction.IsInteractable) return; //현재 인터럭션 불가능한 상태면 등록하지 않는다.

            inRangeInteractions.Add(newInteraction);    //감지된 오브젝트를 등록

            if (currentInteraction != null) return;     //현재 감지중인 오브젝트가 있으면 리턴처리

            currentInteraction?.SetInterface(false);    //새로운 인터렉션 오브젝트를 찾았다면 기존에 켜져있던 인터렉션 오브젝트의 인터페이스를 오프하여야한다.

            currentInteraction = newInteraction;
            currentInteraction.SetInterface(true);

            //Logger.Log(((MonoBehaviour)currentInteraction).gameObject.name);
        }
    }

    public void OnInteraction(InputAction.CallbackContext context)
    {
        Logger.Log("OnInteraction");
        if (currentInteraction != null)
        {
            currentInteraction.SetInterface(false);
            currentInteraction.OnInteraction();

            currentInteraction = FindNearestInteraction();  //인터럭션 동작 후 가장 가까운 인터럭션 오브젝트를 다시 찾아옴
            currentInteraction?.SetInterface(true);
        }
    }

    private IInteractable FindNearestInteraction() //감지중인 오브젝트 중 가장 가까운 오브젝트 검출
    {
        IInteractable nearest = null;
        float minDis = float.MaxValue;

        foreach(var interaction in inRangeInteractions)
        {
            float distance = Vector2.Distance(transform.position, ((MonoBehaviour)interaction).transform.position);
            if(distance < minDis)
            {
                minDis = distance;
                nearest = interaction;
            }
        }
        return nearest;
    }

    private void RemoveInteraction(Collider2D collision)
    {
        if (collision.TryGetComponent<IInteractable>(out IInteractable exitInteraction))
        {
            inRangeInteractions.Remove(exitInteraction);
            if (exitInteraction == currentInteraction)          //감지 범위 밖으로 나간 오브젝트가 현재 인터럭션 중이 오브젝트였다면
            {
                currentInteraction?.SetInterface(false);
                //Logger.Log(((MonoBehaviour)currentInteraction).gameObject.name);
                currentInteraction = FindNearestInteraction();   //범위 내 가장 가까운 오브젝트를 찾아준다.
                currentInteraction?.SetInterface(true);
            }
        }
    }

    private void OnDestroy()
    {
        player.playerActions.Interaction.started -= OnInteraction;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GetInteraction(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        RemoveInteraction(collision);
    }
}
