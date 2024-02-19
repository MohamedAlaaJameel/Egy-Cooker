using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

public class LevelUIManager : MonoBehaviour
{
    
    #region Injected Methods
    IEventManager eventManager;
    [Inject]
    private ITextProvider textProvider;

    public void SetTextProvider(ITextProvider provider)
    {
        textProvider = provider;
    }
    public void SetEventManager(IEventManager eventManager)
    {
        this.eventManager = eventManager;
    }
    #endregion




    private static LevelUIManager instance;

    public static LevelUIManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<LevelUIManager>();
                if (instance == null)
                {
                    GameObject obj = new GameObject("LevelUIManager");
                    instance = obj.AddComponent<LevelUIManager>();
                }
            }
            return instance;
        }
    }

 
    //injected by levelManager

    public void SetScoreText(string score)
    {
        // Use the text provider to set the text
        if (textProvider != null)
        {
            textProvider.SetText(score);
        }
        else
        {
            Debug.LogError("You didnot inject the text provider by level manager");
        }
    }

    //private void OnEnable()
    //{
    //    if (eventManager==null)
    //    {
    //        Debug.Log("Null eventManager OnEnable has been called ");
    //    }
    //    eventManager.Subscribe<ScoreChangnedEventArg>(SetScoreText);

    //}
    //private void OnDisable()
    //{
    //    eventManager.Unsubscribe<ScoreChangnedEventArg>(SetScoreText);

    //}
}
