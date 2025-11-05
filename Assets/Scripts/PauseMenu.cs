using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    MotorCycleInputAction mInputAction;
    [SerializeField] private GameObject mPauseScreen;
    
    private bool bPaused;

    private void Awake()
    {
        mInputAction = new MotorCycleInputAction();
        mInputAction.MotorCycle.PauseMenu.performed += OnPausePressed;
        mInputAction.MotorCycle.Enable();

        mPauseScreen.SetActive(false);
    }
    private void OnEnable() => mInputAction.Enable();
    private void OnDisable()
    {
        mInputAction.Disable();
        Time.timeScale = 1.0f;
    }
    
    private void OnPausePressed(InputAction.CallbackContext context) 
    {
        Debug.Log("Paused");
        bPaused = !bPaused;
        mPauseScreen.SetActive(bPaused);
        Paused();
    }

    public void Paused() 
    {
        Time.timeScale = bPaused ? 0f : 1f;
    }

    
}
