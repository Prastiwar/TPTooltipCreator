using System.Collections;
using UnityEngine;
using TP_Tooltip;

public class ExampleTooltipScript : MonoBehaviour
{
    TPTooltipCreator creator;
    [SerializeField] Transform StaticTransform;
    CanvasGroup canvas;

    // Use this for initialization
    void Awake ()
    {
        creator = FindObjectOfType<TPTooltipCreator>();
        canvas = creator.TooltipLayout.GetComponent<CanvasGroup>();

        creator.SetTooltipActive(false);
        creator.TooltipLayout.SetButtonClick(0, BtnClick);
        creator.SetOnEnterObserver(Extend);
        creator.StaticTransform = StaticTransform;
        creator.SetOnActive(FadeOnActive);
    }

    void FadeOnActive(bool active)
    {
        StartCoroutine(Fading(active));
    }

    // Very simple fading
    IEnumerator Fading(bool active)
    {
        canvas.alpha = 0;
        creator.SetTooltipActive(active);

        if (active)
        {
            while (canvas.alpha < 1f)
            {
                yield return new WaitForSeconds(0.01f);
                canvas.alpha += 0.12f;
            }
        }
    }
    
    void BtnClick()
    {
        Debug.Log("Button pressed!");
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
}