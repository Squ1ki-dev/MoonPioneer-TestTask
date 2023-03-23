using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Notification: MonoBehaviour 
{
    [SerializeField] private TMP_Text displayText;

    private IEnumerator coroutineClearText = null;

    private float _clearTextDelay = 5f;

    private const string BLUE_RECTANGLES = "blue cubes";
    private const string GREEN_RECTANGLES = "green cubes";
    private const string YELLOW_RECTANGLES = "yellow cubes";
    private const string UNKNOWN_ITEM = "unknown";
    private const string FULL_STORAGE_REASON = " Since the warehouse with produced rectangles is full";
    private const string NO_RESOURCE_REASON = " because there are not enough resources for further production";
    
    private void Awake() 
    {
        Factory[] allFactories = UnityEngine.Object.FindObjectsOfType<Factory>();
        foreach (Factory factory in allFactories) 
        {
            factory.onStop?.AddListener(OnStopFactory);
        }
    }

    private void OnStopFactory(Factory.ReasonStop reasonStop, Item.TypeItem typeItem) 
    {
        string itemName;
        switch (typeItem) 
        {
            case Item.TypeItem.Blue:
                itemName = BLUE_RECTANGLES;
                break;
            case Item.TypeItem.Green:
                itemName = GREEN_RECTANGLES;
                break;
            case Item.TypeItem.Yellow:
                itemName = YELLOW_RECTANGLES;
                break;
            default:
                itemName = UNKNOWN_ITEM;
                break;
        }

        string reason;
        switch (reasonStop) 
        {
            case Factory.ReasonStop.FullStorage:
                reason = FULL_STORAGE_REASON;
                break;
            case Factory.ReasonStop.NoResource:
                reason = NO_RESOURCE_REASON;
                break;
            default:
                reason = "";
                break;
        }

        string notification = $"Production factory {itemName} stopped {reason}";
        displayText.text = notification;

        coroutineClearText = coroutineClearText ?? ClearTextDelay(_clearTextDelay);
        StopCoroutine(coroutineClearText);
        StartCoroutine(coroutineClearText);
    }

    private IEnumerator ClearTextDelay(float delay) 
    {
        yield return new WaitForSeconds(delay);
        displayText.text = "";
    }
}
