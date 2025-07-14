using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIStack : MonoBehaviour
{
    public Stack<BaseWindow> uiStack = new Stack<BaseWindow>();

    public void StackUI(BaseWindow window)
    {
        uiStack.Push(window);
    }
    public BaseWindow GetUI()
    {
        if (uiStack.Count > 0) 
        {
            return uiStack.Pop();
        }
        else
        {
            return null;
        }
    }
}
