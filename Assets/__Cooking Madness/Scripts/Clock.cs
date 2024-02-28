using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class Clock : MonoBehaviour, IChangeFoodStates
{
    public GameObject fireClock;
    public GameObject normalClock;
    public Image loadingBar;

    private Meat _meet;

    public void SetFood(Meat meat)
    {
        _meet = meat;
        transform.SetParent(meat.transform);
    }

    private void Start()
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
        HideClock();
    }
    public void HideClock()
    {
        gameObject.SetActive(false);
    }

    public void StopClock()
    {
        StopAllCoroutines(); // Example of stopping coroutines
        HideClock();
    }



    public void ChangeFoodState(IFoodState foodState, Meat food)
    {
        food.TransitionTo(foodState);
    }

 

    public class Factory : PlaceholderFactory<Clock> { }
}
