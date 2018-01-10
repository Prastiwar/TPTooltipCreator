using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TP_TooltipCreator;

public class ExampleTooltipScript : MonoBehaviour
{
    TPTooltipCreator creator;
    [SerializeField] Transform StaticTransform;
    CanvasGroup canvas;
    bool isFading;

    // Use this for initialization
    void Awake ()
    {
        creator = FindObjectOfType<TPTooltipCreator>();
        canvas = creator.TooltipLayout.GetComponent<CanvasGroup>();

        creator.SetTooltipActive(false);
        creator.TooltipLayout.SetButtonClick(0, BtnClick);
        creator.SetOnEnterObserver(Extend);
        creator.StaticTransform = StaticTransform;
        creator.SetAnimation(Animate);
    }

    void BtnClick()
    {
        Debug.Log("Button pressed!");
    }

    void Animate()
    {
        StartCoroutine(Waiter());
    }

    // Method which will execute on tooltip OnObserver
    void Extend()
    {
        creator.TooltipLayout.SetText(0, creator.OnObserver.name);
        creator.TooltipLayout.SetText(1, creator.OnObserver.name + "'s description");

        int length = 6;
        for (int i = 2; i < length; i++)
        {
            creator.TooltipLayout.GetText(i).text = "Rand. Modifier: " + Random.Range(0, 100);
        }
    }

    IEnumerator Waiter()
    {
        yield return new WaitWhile(() => isFading);
        StartCoroutine(Fading());
    }

    // Very simple fading
    IEnumerator Fading()
    {
        isFading = true;
        if (creator.OnObserver != null)
        {
            canvas.alpha = 0;
            creator.SetTooltipActive(true);
            while (canvas.alpha < 1f)
            {
                yield return new WaitForSeconds(0.01f);
                canvas.alpha += 0.12f;
            }
        }
        else
        {
            canvas.alpha = 1;
            while (canvas.alpha > 0f)
            {
                yield return new WaitForSeconds(0.01f);
                canvas.alpha -= 0.12f;
            }
            creator.SetTooltipActive(false);
        }
        isFading = false;
    }
}
