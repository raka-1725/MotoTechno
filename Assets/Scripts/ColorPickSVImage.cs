using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class ColorPickSVImage : MonoBehaviour, IDragHandler, IPointerClickHandler
{
    [SerializeField] private Image pickerImage;
    [SerializeField] private RawImage SVImage;
    private ColorPicker mColorPicker;
    private RectTransform mRectTransform;
    private RectTransform mPickerTransform;


    private void Awake()
    {
        SVImage = GetComponent<RawImage>();
        mColorPicker = FindAnyObjectByType<ColorPicker>();
        mRectTransform = GetComponent<RectTransform>();

        mPickerTransform = pickerImage.GetComponent<RectTransform>();
        mPickerTransform.position = new Vector2(-(mRectTransform.sizeDelta.x * 0.5f), -(mRectTransform.sizeDelta.y) * 0.5f);

    }

    private void UpdateColor(PointerEventData eventData) 
    {
        Vector2 localPoint;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(mRectTransform, eventData.position, eventData.pressEventCamera, out localPoint);

        float width = mRectTransform.rect.width;
        float height = mRectTransform.rect.height;

        localPoint.x = Mathf.Clamp(localPoint.x, -width / 2f, width / 2f);
        localPoint.y = Mathf.Clamp(localPoint.y, -height / 2f, height / 2f);

        float xNormal = (localPoint.x + width / 2f) / width;
        float yNormal = (localPoint.y + height / 2f) / height;
        
        mPickerTransform.localPosition = localPoint;

        Color currentColor = Color.HSVToRGB(mColorPicker.currentHue, xNormal, yNormal);
        pickerImage.color = currentColor;

        mColorPicker.SetSV(xNormal, yNormal);

    }
    public void OnDrag(PointerEventData eventData)
    {
        UpdateColor(eventData);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        UpdateColor(eventData);
    }
}
