using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FinishRaceUI : MonoBehaviour
{
    DisplayRaceStats mDisplayStats;
    RaceManager mRaceManager;
    private int mPosition;
    private int mCreditAward;

    public float fadeDuration = 1.2f;

    [SerializeField] private GameObject mFinishUI;
    [SerializeField] private Image mBGFinalPosition;
    [SerializeField] private TextMeshProUGUI mPositionText;
    [SerializeField] private TextMeshProUGUI mCreditAwardedText;

    private void Awake()
    {
        mDisplayStats = GetComponent<DisplayRaceStats>();
        mRaceManager = FindAnyObjectByType<RaceManager>();
        mFinishUI.SetActive(false);
    }
    private void Update()
    {
        mPosition = mDisplayStats.mPosition;
        mCreditAward = mRaceManager.awardCredit;
    }
    public void Finish()
    {
        ResetUI();
        StartCoroutine(ShowPosition(mPosition, mCreditAward));
        mFinishUI.SetActive(true);
    }

    private IEnumerator ShowPosition(int finalPos, int creditAwarded)
    {
        Color bgColor = mBGFinalPosition.color;
        bgColor.a = 0f;
        mBGFinalPosition.color = bgColor;

        Color textColor = mPositionText.color;
        mPositionText.color = textColor;

        mPositionText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        textColor.a = 0f;

        if (finalPos == 1) 
        {
            mPositionText.text = $"{finalPos} st";
        }
        else if (finalPos == 2)
        {
            mPositionText.text = $"{finalPos} nd";
        }
        else if (finalPos == 3)
        {
            mPositionText.text = $"{finalPos} rd";
        }
        else
        {
            mPositionText.text = $"{finalPos} th";
        }
        mPositionText.gameObject.SetActive(true);
        mCreditAwardedText.gameObject.SetActive(true);
        mCreditAwardedText.SetText($"{mCreditAward} credits awarded !!");
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsed / fadeDuration);
            bgColor.a = alpha;
            mBGFinalPosition.color = bgColor;
            yield return null;
        }
    }
    public void ResetUI()
    {
        if (mFinishUI != null) mFinishUI.SetActive(false);

        if (mBGFinalPosition != null)
        {
            Color bgColor = mBGFinalPosition.color;
            bgColor.a = 0f;
            mBGFinalPosition.color = bgColor;
        }

        if (mPositionText != null)
        {
            Color textColor = mPositionText.color;
            textColor.a = 0f;
            mPositionText.color = textColor;
            mPositionText.gameObject.SetActive(false);
        }
    }
}
