using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class Clock : MonoBehaviour, IChangeFoodStates
{
   
    [Inject]
    SignalBus _signalBus;
    public GameObject fireClock;
    public GameObject normalClock;
    public Image loadingBar;
    private Meat _meet;
    public int ID { get => GetInstanceID(); }

    public void SetFood(Meat meat)
    {
        _meet = meat;
        transform.SetParent(meat.transform);
    }

    private void StartClock()
    {
     
        StartCoroutine(FillBar());

    }
  
    IEnumerator FillBar()
    {
        float startTime = Time.time;
        float endNormalCock = startTime + _meet.GrillDuration;
        float elapsedTime = 0f;

        while (Time.time <= endNormalCock)
        {
            elapsedTime = Time.time - startTime;
            float fill = Mathf.Clamp01(elapsedTime / _meet.GrillDuration);
            loadingBar.fillAmount = fill;
            yield return null;
        }
        startTime = Time.time;
        float fireTime = startTime + _meet.BurnDuration;

        // Ensure it's fully filled if there's any discrepancy
        loadingBar.fillAmount = 0.0f;
        loadingBar.color = Color.yellow;
        normalClock.SetActive(false);
        fireClock.SetActive(true);

        //  ChangeToGrilled();
        ChangeFoodState(new GrilledState(), _meet);

        while (Time.time <= fireTime)
        {
            elapsedTime = Time.time - startTime;
            float fill = Mathf.Clamp01(elapsedTime / _meet.BurnDuration);
            loadingBar.fillAmount = fill;
            yield return null;
        }
        //  ChangeToBurned();
        ChangeFoodState(new BurnedState(), _meet);

        loadingBar.fillAmount = 1.0f;
        StopClock();
    }

    public void StopClock()
    {
        StopAllCoroutines(); // Example of stopping coroutines
        _signalBus.Fire(ID);
        HideClock();
    }
    public void HideClock()
    {
        gameObject.SetActive(false);
    }


    public void ChangeFoodState(IFoodState foodState, Meat food)
    {
        food.TransitionTo(foodState);
    }

    public void InitClockData()
    {
        loadingBar.fillAmount = 0.0f;
        gameObject.SetActive(true);
        loadingBar.color = Color.green;
        normalClock.SetActive(true);
        fireClock.SetActive(false);



    }

    public class Pool : MemoryPool<Meat, Clock> {
        protected override void Reinitialize(Meat food, Clock clock)
        {
            clock.InitClockData();
            clock.SetFood(food);
            clock.StartClock();
        }
  
    }
}
