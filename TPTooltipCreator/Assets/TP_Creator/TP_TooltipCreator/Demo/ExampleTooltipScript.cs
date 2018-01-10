using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TP_TooltipCreator;

public class ExampleTooltipScript : MonoBehaviour
{
    TPTooltipCreator creator;
    [SerializeField] Transform StaticTransform;

    // Use this for initialization
    void Awake ()
    {
        creator = FindObjectOfType<TPTooltipCreator>();

        creator.SetTooltipActive(false);
        creator.TooltipLayout.SetButtonClick(0, BtnClick);
        creator.SetOnEnterObserver(Extend);
        creator.StaticTransform = StaticTransform;
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
            creator.TooltipLayout.GetText(i).text = "Rand. Modifier: " + Random.Range(0,100);
        }
    }
}
