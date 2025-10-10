using UnityEngine;
using UnityEngine.UI;

public class CameraSwitcher : MonoBehaviour
{
    [SerializeField] private Camera mFirstPersonCam;
    [SerializeField] private Camera mThirdPersonCam;

    [SerializeField] private Button mCamSwitchButton;
    private bool bFPcam;

    private void Awake()
    {
        mCamSwitchButton.onClick.AddListener(SwitchCam);
    }

    private void SwitchCam() 
    {
        if (bFPcam) 
        {
            mFirstPersonCam.enabled = true;
            mThirdPersonCam.enabled = false;
        }
        if (!bFPcam) 
        {
            mFirstPersonCam.enabled = false;
            mThirdPersonCam.enabled = true;
        }
            bFPcam = !bFPcam;
    }

}
