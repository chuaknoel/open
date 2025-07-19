using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialWindow : BaseWindow
{
    public override UIType UIType => UIType.ParentWindow;
    [SerializeField] private bool isFirstGameStart = true;

    // Start is called before the first frame update
    void Start()
    {
        if(isFirstGameStart)
        {
            ShowTutorialWindow();
        }
    }

   private void ShowTutorialWindow()
    {
        base.OpenParentWindow();

        isFirstGameStart = false;
    }
}
