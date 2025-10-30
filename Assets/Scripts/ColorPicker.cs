using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ColorPicker : MonoBehaviour
{
    public float currentHue;
    public float currentSat;
    public float currentVal;

    [SerializeField] private RawImage mHueImage;
    [SerializeField] private RawImage mSatValImage;
    [SerializeField] private RawImage mOutputImage;

    [SerializeField] private Slider mHueSlider;

    [SerializeField] private TMP_InputField mHexInput;

    [SerializeField] private Texture2D mHueTexture;
    [SerializeField] private Texture2D mSatValTexture;
    [SerializeField] private Texture2D mOutputTexture;

    public Color selectedColor;

    private void Start()
    {
        CreateHueImage();
        CreateSVImage();
        CreateOutputImage();
        UpdateOutputImage();
    }
    private void CreateHueImage() 
    {
        mHueTexture = new Texture2D(1,128);
        mHueTexture.wrapMode = TextureWrapMode.Clamp;
        mHueTexture.name = "HueTexture";

        for (int i = 0; i < mHueTexture.height; i++)
        {
            mHueTexture.SetPixel(0, i, Color.HSVToRGB((float)i / mHueTexture.height, 1, 1));
        }

        mHueTexture.Apply();
        currentHue = 0;

        mHueImage.texture = mHueTexture; 
    }

    private void CreateSVImage()
    {
        mSatValTexture = new Texture2D(128, 128);
        mSatValTexture.wrapMode = TextureWrapMode.Clamp;
        mSatValTexture.name = "SatValTexture";

        for (int j = 0; j < mSatValTexture.height; j++) 
        {
            for (int k = 0; k < mSatValTexture.width; k++) 
            {
                mSatValTexture.SetPixel(k, j, Color.HSVToRGB(currentHue,(float) k / mSatValTexture.width, (float)j / mSatValTexture.height));
            }
        }

        mSatValTexture.Apply();
        currentSat = 0;
        currentVal = 0;

        mSatValImage.texture = mSatValTexture;
    }

    private void CreateOutputImage() 
    {
        mOutputTexture = new Texture2D(1, 16);
        mOutputTexture.wrapMode = TextureWrapMode.Clamp;
        mOutputTexture.name = "OutputTexture";

        Color currentColor = Color.HSVToRGB(currentHue, currentSat, currentVal);

        for (int i = 0; i < mOutputTexture.height; i++) 
        {
            mOutputTexture.SetPixel(0, i, currentColor);
        }

        mOutputTexture.Apply();

        mOutputImage.texture = mOutputTexture;
    }
    public void OnHueChanged(float newHue)
    {
        currentHue = newHue;
        UpdateSVImage();
        UpdateOutputImage();
    }
    private void UpdateOutputImage() 
    {
        Color currentColor = Color.HSVToRGB(currentHue, currentSat, currentVal);

        for (int i = 0; i < mOutputTexture.height; i++) 
        {
            mOutputTexture.SetPixel(0, i, currentColor);
        }

        mOutputTexture.Apply();
        mHexInput.text = ColorUtility.ToHtmlStringRGB(currentColor);
        selectedColor = currentColor;
    }

    public void SetSV(float S, float V) 
    {
        currentSat = S;
        currentVal = V;
        UpdateOutputImage();
        UpdateSVImage();
    }

    public void UpdateSVImage() 
    {
        currentHue = mHueSlider.value;

        for (int j = 0; j < mSatValTexture.height; j++)
        {
            for (int k = 0; k < mSatValTexture.width; k++)
            {
                mSatValTexture.SetPixel(k, j, Color.HSVToRGB(currentHue, (float)k / mSatValTexture.width, (float)j / mSatValTexture.height));
            }
        }

        mSatValTexture.Apply();
        UpdateOutputImage();
    }

    public void OnTextInput() 
    {
        if (mHexInput.text.Length < 6) { return; }

        Color newCol;
        if (ColorUtility.TryParseHtmlString("#" + mHexInput.text, out newCol))
            Color.RGBToHSV(newCol, out currentHue, out currentSat, out currentVal);

        mHueSlider.value = currentHue;
        mHexInput.text = "";
        UpdateOutputImage();
    }
}
