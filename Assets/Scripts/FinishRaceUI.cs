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
    public void Finish(RaceManager raceManager)
    {
        ResetUI();

        mPosition = raceManager.position;
        mCreditAward = raceManager.awardCredit;

        Debug.Log($"pos {mPosition}, credit {mCreditAward}");

        SetPositionText(mPosition);
        mCreditAwardedText.text = $"{mCreditAward} credits awarded !!";

        mFinishUI.SetActive(true);
        StartCoroutine(FadeInUI());
    }

    private void SetPositionText(int finalPos)
    {
        switch (finalPos)
        {
            case 1: mPositionText.text = "1 st"; break;
            case 2: mPositionText.text = "2 nd"; break;
            case 3: mPositionText.text = "3 rd"; break;
            default: mPositionText.text = $"{finalPos} th"; break;
        }
    }

    private IEnumerator FadeInUI()
    {
        float elapsed = 0f;
        Color bgColor = mBGFinalPosition.color;
        bgColor.a = 0f;
        mBGFinalPosition.color = bgColor;

        Color positionColor = mPositionText.color;
        positionColor.a = 0f;
        mPositionText.color = positionColor;
        mPositionText.gameObject.SetActive(true);

        Color creditColor = mCreditAwardedText.color;
        creditColor.a = 0f;
        mCreditAwardedText.color = creditColor;
        mCreditAwardedText.gameObject.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsed / fadeDuration);

            bgColor.a = alpha;
            mBGFinalPosition.color = bgColor;

            positionColor.a = alpha;
            mPositionText.color = positionColor;

            creditColor.a = alpha;
            mCreditAwardedText.color = creditColor;

            yield return null;
        }
        bgColor.a = 1f;
        mBGFinalPosition.color = bgColor;
        positionColor.a = 1f;
        mPositionText.color = positionColor;
        creditColor.a = 1f;
        mCreditAwardedText.color = creditColor;
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

        if (mCreditAwardedText != null)
        {
            Color creditColor = mCreditAwardedText.color;
            creditColor.a = 0f;
            mCreditAwardedText.color = creditColor;
            mCreditAwardedText.gameObject.SetActive(false);
        }
    }
}
