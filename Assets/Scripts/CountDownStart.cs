using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class CountDownStart : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI mCountDownText;
    public float countDownDuration = 3f;
    public bool bStart;

    private float currentTime;
    private bool countdownRunning = false;

    public Action<CountDownStart> onRaceStart;
    public void StartCountDown() 
    {
        currentTime = countDownDuration;
        countdownRunning = true;
        StartCoroutine(CountDownCoroutine());
    }

    private IEnumerator CountDownCoroutine() 
    {
        yield return new WaitForSeconds(2);
        while (currentTime > 0) 
        {
            mCountDownText.text = currentTime.ToString();
            yield return new WaitForSeconds(1);
            currentTime--;
        }

        onRaceStart?.Invoke(this);
        bStart = true;
        mCountDownText.SetText("GO!!");
        yield return new WaitForSeconds(1.2f);
        mCountDownText.gameObject.SetActive(false);
    }


}
