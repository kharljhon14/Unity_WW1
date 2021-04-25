using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeScreen : MonoBehaviour
{
    [SerializeField] private Image fadeScreen;
    [SerializeField] private float fadeSpeed;

    private void Start()
    {
        StartCoroutine(FadeIn());
    }

    public virtual IEnumerator FadeIn()
    {
        float timeStarted = Time.time;
        float timeSinceStarted = Time.time - timeStarted;
        float percentageComplete = timeSinceStarted / fadeSpeed;
        Color currentColor = fadeScreen.color;

        while (true)
        {
            timeSinceStarted = Time.time - timeStarted;
            percentageComplete = timeSinceStarted / fadeSpeed;
            currentColor.a = Mathf.Lerp(1, 0, percentageComplete);

            fadeScreen.color = currentColor;

            if (percentageComplete >= 1)
            {
                fadeScreen.raycastTarget = false;
                break;
            }

            yield return new WaitForEndOfFrame();
        }
    }

    public virtual IEnumerator FadeOut()
    {
        float timeStarted = Time.time;
        float timeSinceStarted = Time.time - timeStarted;
        float percentageComplete = timeSinceStarted / fadeSpeed;
        Color currentColor = fadeScreen.color;

        while (true)
        {
            timeSinceStarted = Time.time - timeStarted;
            percentageComplete = timeSinceStarted / fadeSpeed;
            currentColor.a = Mathf.Lerp(0, 1, percentageComplete);
            fadeScreen.color = currentColor;
            fadeScreen.raycastTarget = true;
            if (percentageComplete >= 1)
                break;

            yield return new WaitForEndOfFrame();
        }
       
    }
}
