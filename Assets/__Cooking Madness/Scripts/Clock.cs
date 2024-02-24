using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class Clock : MonoBehaviour, IClockControl
{
    public GameObject fireClock;
    public GameObject normalClock;
    public Image loadingBar;

    public int cookDuration;
    public int beForeFireDuration;
    [Inject]
    SignalBus _singnalBus;
    IMeatStateChanger _meatStateChanger;
    private void Start()
    {
     
        StartCoroutine(FillBar());

    }
    public void SetMeatStateChanger(IMeatStateChanger meatStateChanger)
    {
        _meatStateChanger = meatStateChanger;
    }
    IEnumerator FillBar()
    {
        float startTime = Time.time;
        float endNormalCock = startTime + cookDuration;
        float elapsedTime = 0f;

        while (Time.time <= endNormalCock)
        {
            elapsedTime = Time.time - startTime;
            float fill = Mathf.Clamp01(elapsedTime / cookDuration);
            loadingBar.fillAmount = fill;
            yield return null;
        }
        startTime = Time.time;
        float fireTime = startTime + beForeFireDuration;

        // Ensure it's fully filled if there's any discrepancy
        loadingBar.fillAmount = 0.0f;
        loadingBar.color = Color.yellow;
        normalClock.SetActive(false);
        fireClock.SetActive(true);

        //  ChangeToGrilled();
        _meatStateChanger.ChangeToGrilled();

        while (Time.time <= fireTime)
        {
            elapsedTime = Time.time - startTime;
            float fill = Mathf.Clamp01(elapsedTime / beForeFireDuration);
            loadingBar.fillAmount = fill;
            yield return null;
        }
        //  ChangeToBurned();
        _meatStateChanger.ChangeToBurned();

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

    public class Factory : PlaceholderFactory<Clock> { }
}
