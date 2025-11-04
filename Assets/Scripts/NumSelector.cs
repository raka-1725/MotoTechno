using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NumSelector : MonoBehaviour
{
    [SerializeField] private int min;
    public int number;
    [SerializeField] private int max;

    [SerializeField] private Button incrementButton;
    [SerializeField] private Button decrementButton;

    [SerializeField] private TextMeshProUGUI mNumberText;

    private void Update()
    {
        CheckInteractbale();
    }

    private void CheckInteractbale()
    {
        incrementButton.interactable = !(number >= max);
        decrementButton.interactable = !(number <= min);
    }

    public void IncrementNum() 
    {
        number++;
        Mathf.Clamp(number, min, max);
        mNumberText.SetText(number.ToString());
    }

    public void DecrementNum() 
    {
        number--;
        Mathf.Clamp(number, min, max);
        mNumberText.SetText(number.ToString());
    }
}
