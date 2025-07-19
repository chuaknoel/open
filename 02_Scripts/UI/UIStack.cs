using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// UI 창들을 순차적으로 쌓는데 필요한 스크립트입나다.
/// </summary>
public class UIStack : MonoBehaviour
{
    public Stack<BaseWindow> uiStack = new Stack<BaseWindow>();

    /// <summary>
    /// UI 창을 쌓는 함수입니다.
    /// </summary>
    /// <param name="window">UI 창</param>
    public void StackUI(BaseWindow window)
    {
        uiStack.Push(window);
    }
    /// <summary>
    /// 가장 위에 있는 UI창을 반환하는 함수입니다.
    /// </summary>
    /// <returns></returns>
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
