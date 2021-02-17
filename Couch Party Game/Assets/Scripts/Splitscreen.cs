using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Splitscreen : MonoBehaviour
{
    [SerializeField] RawImage renderImage;

    public Player owner;
    public UnityAction<GameObject> onFadedIn, onFadedOut;

    Coroutine currentFadeRoutine;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDisable()
    {
        if(currentFadeRoutine != null)
        {
            StopCoroutine(currentFadeRoutine);
            currentFadeRoutine = null;
        }
        if(onFadedIn != null)
        {
            onFadedIn.Invoke(owner.gameObject);
        }
        if(onFadedOut != null)
        {
            onFadedOut.Invoke(owner.gameObject);
        }
    }

    public void FadeInOut(float duration)
    {
        if(currentFadeRoutine != null)
        {
            StopCoroutine(currentFadeRoutine);
        }
        currentFadeRoutine = StartCoroutine(FadeInOutRoutine(duration));
    }

    IEnumerator FadeInOutRoutine(float duration)
    {
        Color color = renderImage.color;
        Color targetColor = new Color(0, 0 , 0, color.a);

        Vector4 changeAmount = Vector4.zero;
        changeAmount.x = targetColor.r - color.r;
        changeAmount.y = targetColor.g - color.g;
        changeAmount.z = targetColor.b - color.b;
        changeAmount.w = targetColor.a - color.a;

        changeAmount /= (duration / 2);

        while(color.r > targetColor.r || color.g > targetColor.g || color.b > targetColor.b || color.a > targetColor.a)
        {
            yield return null;
            color.r += changeAmount.x * Time.deltaTime;
            color.g += changeAmount.y * Time.deltaTime;
            color.b += changeAmount.z * Time.deltaTime;
            color.a += changeAmount.w * Time.deltaTime;
            renderImage.color = color;
        }

        color = targetColor;
        renderImage.color = color;

        if (onFadedIn != null)
        {
            onFadedIn.Invoke(owner.gameObject);
        }

        targetColor = new Color(1, 1, 1, color.a);

        changeAmount = Vector4.zero;
        changeAmount.x = targetColor.r - color.r;
        changeAmount.y = targetColor.g - color.g;
        changeAmount.z = targetColor.b - color.b;
        changeAmount.w = targetColor.a - color.a;

        changeAmount /= (duration / 2);

        while (color.r < targetColor.r || color.g < targetColor.g || color.b < targetColor.b || color.a < targetColor.a)
        {
            yield return null;
            color.r += changeAmount.x * Time.deltaTime;
            color.g += changeAmount.y * Time.deltaTime;
            color.b += changeAmount.z * Time.deltaTime;
            color.a += changeAmount.w * Time.deltaTime;
            renderImage.color = color;
        }

        color = targetColor;
        renderImage.color = color;

        if (onFadedOut != null)
        {
            onFadedOut.Invoke(owner.gameObject);
        }
    }
}
