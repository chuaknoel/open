using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookAnimation : MonoBehaviour
{
    [SerializeField] private Animator bookAnimator;

    public void BookOpen()
    {
        bookAnimator.ResetTrigger("isOpen");
        bookAnimator.SetTrigger("isOpen");  
    }
    public void BookClose()
    {
        bookAnimator.ResetTrigger("isClose");
        bookAnimator.SetTrigger("isClose");
    }
}
