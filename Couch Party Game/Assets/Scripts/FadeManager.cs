using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class FadeManager : MonoBehaviour
{
    public UnityAction onFadedIn, onFadedOut;
    [SerializeField] Image fadePanel;


    [SerializeField] float defaultFadeDuration = 1;
    Coroutine fadeRoutine;

    [SerializeField] bool unfadeOnStart;
    // Start is called before the first frame update
    void Start()
    {
        if (unfadeOnStart)
        {
            FadeOut();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDisable()
    {
        if(fadeRoutine != null)
        {
            StopCoroutine(fadeRoutine);
            fadeRoutine = null;
            if(onFadedIn != null)
            {
                onFadedIn.Invoke();
            }
            if(onFadedOut != null)
            {
                onFadedOut.Invoke();
            }
        }
    }

    public void FadeInOut()
    {
        if (fadeRoutine != null)
        {
            StopCoroutine(fadeRoutine);
        }
        fadeRoutine = StartCoroutine(FadeInOutRoutine(defaultFadeDuration));
    }

    public void FadeInOut(float duration)
    {
        if(fadeRoutine != null)
        {
            StopCoroutine(fadeRoutine);
        }
        fadeRoutine = StartCoroutine(FadeInOutRoutine(duration));
    }

    public void FadeIn()
    {
        if (fadeRoutine != null)
        {
            StopCoroutine(fadeRoutine);
        }
        fadeRoutine = StartCoroutine(FadeInRoutine(defaultFadeDuration));
    }

    public void FadeIn(float duration)
    {
        if (fadeRoutine != null)
        {
            StopCoroutine(fadeRoutine);
        }
        fadeRoutine = StartCoroutine(FadeInRoutine(duration));
    }

    public void FadeOut()
    {
        if (fadeRoutine != null)
        {
            StopCoroutine(fadeRoutine);
        }
        fadeRoutine = StartCoroutine(FadeOutRoutine(defaultFadeDuration));
    }

    public void FadeOut(float duration)
    {
        if (fadeRoutine != null)
        {
            StopCoroutine(fadeRoutine);
        }
        fadeRoutine = StartCoroutine(FadeOutRoutine(duration));
    }

    IEnumerator FadeInOutRoutine(float duration)
    {
        Debug.Log("ZZZ");
        fadePanel.raycastTarget = true;
        Color newColor = fadePanel.color;

        float modifyAmount = 1;
        modifyAmount /= duration;
        modifyAmount /= 2;

        while(fadePanel.color.a < 1)
        {
            //Debug.Log(modifyAmount);
            newColor.a += modifyAmount * Time.deltaTime;
            fadePanel.color = newColor;
            yield return null;
        }

        if(onFadedIn != null)
        {
            onFadedIn.Invoke();
        }

        fadePanel.raycastTarget = false;

        modifyAmount = -1;
        modifyAmount /= duration;
        modifyAmount /= 2;

        while (fadePanel.color.a > 0)
        {
            newColor.a += modifyAmount * Time.deltaTime;
            fadePanel.color = newColor;
            yield return null;
        }

        if(onFadedOut != null)
        {
            onFadedOut.Invoke();
        }
    }

    IEnumerator FadeOutRoutine(float duration)
    {
        fadePanel.raycastTarget = false;
        Color newColor = fadePanel.color;
        float modifyAmount = -1;
        modifyAmount /= duration;
        modifyAmount /= 2;

        while (fadePanel.color.a > 0)
        {
            newColor.a += modifyAmount * Time.deltaTime;
            fadePanel.color = newColor;
            yield return null;
        }

        if (onFadedIn != null)
        {
            onFadedIn.Invoke();
        }
    }

    IEnumerator FadeInRoutine(float duration)
    {
        fadePanel.raycastTarget = true;
        Color newColor = fadePanel.color;

        float modifyAmount = 1;
        modifyAmount /= duration;
        modifyAmount /= 2;

        while (fadePanel.color.a < 1)
        {
            newColor.a += modifyAmount * Time.deltaTime;
            fadePanel.color = newColor;
            yield return null;
        }

        if (onFadedOut != null)
        {
            onFadedOut.Invoke();
        }
    }

}
