using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Fader : MonoBehaviour
{
    [SerializeField] private Image faderImage;        //페이더 이미지
    private Color currentAlpha;     //페이드 연출에 필요한 알파 컬러


    public void FadeIn(float fadeInTime = 1f, Action onComplete = null)  //액션 함수가 없어도 작동하도록 처리
    {
        //Debug.Log("Fade In");
        StartCoroutine(FadeInRoutine(fadeInTime, onComplete));
    }

    public IEnumerator FadeInRoutine(float fadeInTime =1f, Action onComplete = null)
    {
        currentAlpha.a = 0;
        faderImage.color = currentAlpha;

        yield return faderImage.DOFade(1, fadeInTime).SetUpdate(true).WaitForCompletion();

        currentAlpha.a = 1;
        faderImage.color = currentAlpha;
        onComplete?.Invoke();
    }

    public void FadeOut(float fadeOutTime = 2f, Action onComplete = null)
    {
        //Debug.Log("Fade Out");
        StartCoroutine(FadeOutRoutine(fadeOutTime ,onComplete));
    }

    public IEnumerator FadeOutRoutine(float fadeOutTime =1, Action onComplete = null , Action doEarlyLogic = null , float earlyTime = 0.9f)
    {
        currentAlpha.a = 1;
        faderImage.color = currentAlpha;

        Tween tween = faderImage.DOFade(0, fadeOutTime).SetUpdate(true);

        bool isEarlyLogic = false;
        while (tween.IsActive() && !isEarlyLogic)
        {
            //Logger.Log(tween.ElapsedPercentage());
            if(tween.ElapsedPercentage() >= earlyTime)
            {
                isEarlyLogic = true;
                doEarlyLogic?.Invoke();
            }

            yield return null;
        }
        
        yield return faderImage.DOFade(0, fadeOutTime).SetUpdate(true).WaitForCompletion();

        currentAlpha.a = 0;                //while문 이후 확실하게 알파값을 처리해 주고 마무리한다.                                   
        faderImage.color = currentAlpha;
        onComplete?.Invoke();
    }
}
