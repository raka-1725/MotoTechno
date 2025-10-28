using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FinishRaceUI : MonoBehaviour
{
    DisplayRaceStats mDisplayStats;
    private int mPosition;

    public float fadeDuration = 1.2f;

    [SerializeField] private GameObject mFinishUI;
    [SerializeField] private Image mBGFinalPosition;
    [SerializeField] private TextMeshProUGUI mPositionText; 

    private void Awake()
    {
        mDisplayStats = GetComponent<DisplayRaceStats>();
        mFinishUI.SetActive(false);
    }
    private void Update()
    {
        mPosition = mDisplayStats.mPosition;
    }
    public void Finish()
    {
        StartCoroutine(ShowPosition(mPosition));
        mFinishUI.SetActive(true);

    }

    private IEnumerator ShowPosition(int finalPos)
    {
        yield return new WaitForSeconds(1f);
        Color bgColor = mBGFinalPosition.color;
        bgColor.a = 0f;
        mBGFinalPosition.color = bgColor;

        mPositionText.gameObject.SetActive(false);
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsed / fadeDuration);
            bgColor.a = alpha;
            mBGFinalPosition.color = bgColor;
            yield return null;
        }

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
    }
}
