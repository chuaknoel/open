using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.PackageManager.UI;
using UnityEngine;

/// <summary>
/// ?袁⑷퍥 UI????뽮쉐????????온?귐뗫릭????쎄쾿?깆????낅빍??
/// </summary>
public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public Canvas canvas;
    private UIStack uiStack;

    public BaseWindow destroyItemWindow;

    private void Awake()
    {
        Instance = this;

        Resources.Load<TMP_FontAsset>("Fonts/NEXON_Warhaven_Regular SDF");
        uiStack = GetComponent<UIStack>();
    }
    /// <summary>
    /// Window ??뽮쉐??
    /// </summary>
    /// <param name="window">??뽮쉐?酉釉?筌?/param>
    public void OpenWindow(BaseWindow window)
    {
        if (window.UIType == UIType.Inventroy)
        {
            window.GetComponent<BaseWindow>().OpenInventroy(); 
        }
        else
        {
            window.GetComponent<BaseWindow>().OpenUI();
        }
        uiStack.StackUI(window);
    }
    /// <summary>
    ///  Window ??쑵??源딆넅
    /// </summary>
    /// <param name="window">??쑵??源딆넅??筌?/param>
    public void CloseWindow(BaseWindow window)
    {
        if (window.UIType == UIType.Inventroy)
        {
            window.GetComponent<BaseWindow>().CloseInventroy();
        }
        else
        {
            window.GetComponent<BaseWindow>().CloseUI();
        }       
        uiStack.GetUI();
    }
    private void OnDestroy()
    {
        Instance = null;
    }
}