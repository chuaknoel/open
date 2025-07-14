using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// 게임씬을 로드하는 스크립트입니다.
/// </summary>
public class LoadingManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI loadingText;
    [SerializeField] private Slider loadingSlider;
    private float time = 0;

    public void Start()
    {
        StartCoroutine(LoadScene("TutorialScene"));
    }

    /// <summary>
    /// 게임씬을 로드합니다.
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    IEnumerator LoadScene(string name)
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
