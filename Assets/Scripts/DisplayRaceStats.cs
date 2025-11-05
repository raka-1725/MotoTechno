using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class DisplayRaceStats : MonoBehaviour
{
    [Header("Sector/Lap")]
    [SerializeField] private TextMeshProUGUI mLapText;
    [SerializeField] private TextMeshProUGUI mTotalLapText;

    [SerializeField] private TextMeshProUGUI mLapTimeText;
    [SerializeField] private TextMeshProUGUI mBestLapTimeText;
    [SerializeField] private GameObject mBestLapBGImage;
    [SerializeField] private TextMeshProUGUI mSectorTimeText;
    [SerializeField] private GameObject mFinalLap;

    private float mBestLapTime;
    public int mPosition;

    [Header("Position")]
    [SerializeField] private TextMeshProUGUI mPositionText;

    private void Start()
    {
        mFinalLap.SetActive(false);
        mBestLapBGImage.SetActive(false);
        mBestLapTimeText.SetText("");
        mLapText.text = ($"Lap : {1}");
    }

    public void UpdateSetcorUI(float sectortime)
    {
        mSectorTimeText.text = ($"Sector : {sectortime.ToString("F2")}");
    }

    public void UpdatePositionUI(int position) 
    {
        mPosition = position;
        
        mPositionText.text = ($"Position : P{position.ToString()}");
    }
    public void SetTotalLaps(int totalLaps)
    {
        mTotalLapText.text = $"/ {totalLaps}";
    }
    public void UpdateLapUI(int laps, float laptime)
    {
        mLapText.text = ($"Lap : {laps.ToString()}");
        if (laptime <= 0f) { return; }
        if (!mBestLapBGImage.activeSelf)
        {
            mBestLapBGImage.SetActive(true);
            mBestLapTime = laptime;
            UpdateBestLapUI(mBestLapTime);
            return;
        }
        if (laptime < mBestLapTime)
        {
            mBestLapTime = laptime;
            UpdateBestLapUI(mBestLapTime);
        }
    }

    public void UpdateLapTimeUI(float laptime)
    {
        int minutes = Mathf.FloorToInt(laptime / 60f);
        int seconds = Mathf.FloorToInt(laptime % 60f);
        int milliseconds = Mathf.FloorToInt((laptime * 1000f) % 1000f);
        string formattedTime = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);
        mLapTimeText.text = formattedTime;
    }

    public void UpdateBestLapUI(float laptime) 
    {
        int minutes = Mathf.FloorToInt(laptime / 60f);
        int seconds = Mathf.FloorToInt(laptime % 60f);
        int milliseconds = Mathf.FloorToInt((laptime * 1000f) % 1000f);
        string formattedTime = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);
        mBestLapBGImage.SetActive(true);
        mBestLapTimeText.text = formattedTime.ToString();
    }

    public void IndicateFinalLap() 
    {
        mFinalLap.SetActive(true);
    }
}
