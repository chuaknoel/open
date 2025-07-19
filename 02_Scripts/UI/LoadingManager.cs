using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Analytics;
using Unity.VisualScripting;

/// <summary>
/// 게임씬을 로드하는 스크립트입니다.
/// </summary>
public class LoadingManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI loadingText;
    [SerializeField] private Slider loadingSlider;
    SceneLoader SceneLoader;
    private float time = 0;

    private void Start()
    {
        SceneLoader = GameObject.FindGameObjectWithTag("SceneLoader").GetComponent<SceneLoader>();
        StartCoroutine(LoadSceneRoutine(SceneLoader.ReturnSceneName()));
    }

    /// <summary>
    /// 게임씬을 로드합니다.
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public IEnumerator LoadSceneRoutine(string name)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(name);
        asyncOperation.allowSceneActivation = false;

        float loadingTime = Random.Range(2f,3f);

        while (!asyncOperation.isDone)
        {
            time += Time.deltaTime;

            // 로드 정보 표시
            loadingText.text = (Mathf.FloorToInt((time / loadingTime) * 100)).ToString() + " %";
            loadingSlider.value = time / loadingTime;

            if (time > loadingTime)
            {
                loadingSlider.value = 1f;
                asyncOperation.allowSceneActivation = true; // 씬 활성화
            }
            yield return null;
        }
    }
}
