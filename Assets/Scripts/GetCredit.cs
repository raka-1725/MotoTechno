using TMPro;
using UnityEngine;

public class GetCredit : MonoBehaviour
{
    public int credit;

    [SerializeField] private TextMeshProUGUI mCreditText;
    private void Update()
    {
        credit = Mathf.Clamp(PlayerCredits.Instance.credit,0 , 100000);

        mCreditText.SetText($"Credits : {credit}");
    }
}
