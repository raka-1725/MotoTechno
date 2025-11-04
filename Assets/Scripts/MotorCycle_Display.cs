using UnityEngine;

public class MotorCycle_Display : MonoBehaviour
{
    [SerializeField] private MotorCycleCusomization mMotoSpecCustom;
    public GameObject mRearWing;
    public GameObject mFrontWinglet;
    public Material mBodyColorMaterial;

    private void Update()
    {
        UpdateDisplay();
    }
    private void UpdateDisplay()
    {

        if (mMotoSpecCustom.RearWing == true)
        {
            mRearWing.gameObject.SetActive(true);
        }
        else
        {
            mRearWing.gameObject.SetActive(false);
        }

        if (mMotoSpecCustom.FrontWinglet == true)
        {
            mFrontWinglet.gameObject.SetActive(true);
        }
        else
        {
            mFrontWinglet.gameObject.SetActive(false);
        }
        //bodycolor

        if (mMotoSpecCustom.DefaultBodyColor == false)
        {
            mBodyColorMaterial.color = mMotoSpecCustom.mCustomBodyColor;
        }
        else
        {
            mBodyColorMaterial.color = mMotoSpecCustom.mDefaultBodyColor;
        }

    }
}
