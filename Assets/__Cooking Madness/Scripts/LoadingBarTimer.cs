using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LoadingBarTimer : MonoBehaviour
{
    public GameObject fireClock;
    public GameObject normalClock;
    public GameObject loadingBarObject;

    Order Order;


    Image loadingBar;

   
    void Start()
    {
            loadingBar = loadingBarObject.GetComponent<Image>();
            StartCoroutine(FillBar());
    }

    public void SetOrder(Order meatObject)
    {
        this.Order = meatObject;

    }
 
    IEnumerator FillBar()
    {
        float startTime = Time.time;
        float endNormalCock = startTime + LevelManager.Instance.cookDuration;
        float elapsedTime = 0f;

        while (Time.time <= endNormalCock)
        {
            elapsedTime = Time.time - startTime;
            float fill = Mathf.Clamp01(elapsedTime / LevelManager.Instance.cookDuration);
            loadingBar.fillAmount = fill;
            yield return null;
        }
        startTime = Time.time;
        float fireTime = startTime + LevelManager.Instance.beForeFireDuration;

        // Ensure it's fully filled if there's any discrepancy
        loadingBar.fillAmount = 0.0f;
        loadingBar.color = Color.yellow;
        normalClock.SetActive(false);
        fireClock.SetActive(true);
        
        LevelManager.Instance.ChangeToGrilled(Order);


        while (Time.time <= fireTime)
        {
            elapsedTime = Time.time - startTime;
            float fill = Mathf.Clamp01(elapsedTime / LevelManager.Instance.beForeFireDuration);
            loadingBar.fillAmount = fill;
            yield return null;
        }
        LevelManager.Instance.ChangeToBurned(Order);

        loadingBar.fillAmount = 1.0f;
        HideClock();
    }
    public void HideClock()
    {
        gameObject.SetActive(false);
    }
}
