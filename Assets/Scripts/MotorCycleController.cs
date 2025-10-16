using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

[RequireComponent(typeof(MotorCycleInputAction))]
public class MotorCycleController : MonoBehaviour
{
    MotorCycleInputAction mInputAction;
    TrailRenderer mSkidMarks;
    MotorCycleUI mMotoUI;

    private Vector2 mMoveInput;
    private float mHorizontalSteer; //horizontal input
    [SerializeField] private float mBrakeAccel; //vertical input
    [SerializeField] private float mCurrentSteerAngle;
    private float mCurrentBrakeForce;

    private Rigidbody rb;

    [Header("Mode")]
    public bool AkiraMoto; //special mode for the akira motorcycle (some of the axis are inverted)
    public bool InvertSteer;


    //Set ScriptableObject for vehicle stats
    [Header("Motor/Brake")]//motor
    [SerializeField] private MotorCycleCusomization mMotoSpecCustom;
    private float mMotorPower;
    private float mBrakeTorque;
    [SerializeField] private float mMaxSteerAngle;
    [SerializeField] private bool bRegenBrake;
    [SerializeField] private bool bOverTakePower;
    private bool bIsBraking;

    private float mExtraGripR;
    private float mExtraGripF;

    [Header("Battery")]
    [SerializeField] private float mBattery;
    private float mEnergyUseIndex;
    private float mRegenStrength;
    private float mOverTakePowerIndex;

    [Header("BrakeLight")]
    [SerializeField] private GameObject mBrakeLight_R;
    [SerializeField] private GameObject mBrakeLight_L;
    [SerializeField] private Material mBrakeLightMatOFF;
    [SerializeField] private Material mBrakeLightMatON;
    [SerializeField] private Material mBrakeLightMatREGENON;

    public enum DriveTypes
    {
        Rear = 1, Front = 2, All = 3
    }
    public DriveTypes mDriveTypes = DriveTypes.Rear;
    //public bool bCanReverse; // reverse function

    [Header("Visual Customization")]
    public GameObject mRearWing;
    public GameObject mFrontWinglet;
    public Material mBodyColorMaterial;

    [Header("Wheel Colliders")]
    [SerializeField] private WheelCollider mWColliderFront;
    [SerializeField] private WheelCollider mWColliderRear;

    [Header("Vehicle Stats")]
    public float Speed; // in km/h
    public float rawSpeed;
    [SerializeField] private float mZtiltAngle;

    [Header("Tyre Skid")]
    [SerializeField] private bool bSkidOn = false;
    [SerializeField] private float mSkidWidth = 0.08f;
    [SerializeField] private float mMinSkidSpeed = 10f;
    [Header("WheelRotation")]
    [SerializeField] private Transform mFrontWheelTransform;
    [SerializeField] private Transform mRearWheelTransform;

    private void Awake()
    {
        mInputAction = new MotorCycleInputAction();
        mInputAction.MotorCycle.MotoInput.performed += HandleMoveInput;
        mInputAction.MotorCycle.MotoInput.canceled += HandleMoveInput;
        mInputAction.MotorCycle.MotoRegen.performed += OnRegenInputPressed;
        mInputAction.MotorCycle.MotoRegen.canceled += OnRegenInputRelease;
        mInputAction.MotorCycle.MotoOverTake.performed += OnOverTakeInput;
        mInputAction.MotorCycle.MotoOverTake.canceled += OnOverTakeInputRelease;
        rb = GetComponent<Rigidbody>();

        //visual
        if (bSkidOn) 
        {
            mSkidMarks = GetComponentInChildren<TrailRenderer>();
            mSkidMarks.startWidth = mSkidWidth;
            mSkidMarks.emitting = false;
        }

        //Setting stats from specsheet;
        mMotorPower = mMotoSpecCustom.MaxPower;
        mBrakeTorque = mMotoSpecCustom.BrakeTorque;
        mBattery = mMotoSpecCustom.BatteryCapacity;
        mEnergyUseIndex = mMotoSpecCustom.EnergyUseIndex;
        mRegenStrength = mMotoSpecCustom.RegenStrength;
        mOverTakePowerIndex = mMotoSpecCustom.OverTakeIndex;
        mExtraGripF = mMotoSpecCustom.FrontWingletGripMultiplier ;
        mExtraGripR = mMotoSpecCustom.RearWingGripMultiplier;
        CheckVehicleStatsSet();

        //UI
        mMotoUI = GetComponent<MotorCycleUI>();

    }


    private void Update()
    {
        GetSpeed();
        SkidMark();
        
    }

    private void FixedUpdate()
    {
        HandleMotor();
        HandleSteer();
        RotateTire();
        BodyTilt();
    }
    private void LateUpdate()
    {
        mMotoUI.Speed(Speed);
        mMotoUI.BatteryPercentage(mBattery);
    }
    private void CheckVehicleStatsSet()
    {
        WheelFrictionCurve frontWheelFriction = mWColliderFront.forwardFriction;
        WheelFrictionCurve rearWheelFriction = mWColliderRear.forwardFriction;

        if (mMotoSpecCustom.RearWing == true)
        {
            mRearWing.gameObject.SetActive(true);
            rearWheelFriction.extremumSlip *= mExtraGripR;
            rearWheelFriction.asymptoteValue *= mExtraGripR;
        }
        else
        {
            mRearWing.gameObject.SetActive(false);
        }

        if (mMotoSpecCustom.FrontWinglet == true)
        {
            mFrontWinglet.gameObject.SetActive(true);
            frontWheelFriction.extremumSlip *= mExtraGripF;
            frontWheelFriction.asymptoteValue *= mExtraGripF;
        }
        else
        {
            mFrontWinglet.gameObject.SetActive(false);
        }

        mWColliderFront.forwardFriction = frontWheelFriction;
        mWColliderRear.forwardFriction = rearWheelFriction; 

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


    private void HandleMoveInput(InputAction.CallbackContext context)
    {
        mMoveInput = context.ReadValue<Vector2>();
    }

    private void OnRegenInputPressed(InputAction.CallbackContext context) 
    {
        bRegenBrake = true;
    }
    private void OnRegenInputRelease(InputAction.CallbackContext context)
    {
        bRegenBrake = false;
    }
    private void OnOverTakeInput(InputAction.CallbackContext context) 
    {
        bOverTakePower = true;
    }
    private void OnOverTakeInputRelease(InputAction.CallbackContext context) 
    {
        bOverTakePower = false;
    }
    private void HandleMotor()
    {
        mBrakeAccel = (mMoveInput.y);
        if (mBrakeAccel >= 0) { bIsBraking = false; }
        if (mBrakeAccel < 0) { bIsBraking = true; }
        float speedfactor = Mathf.InverseLerp(0, 200, Speed);
        float motorTorque = Mathf.Lerp(mMotorPower, 0, speedfactor);
        if (bOverTakePower) 
        {
            motorTorque = motorTorque * OverTakePower();
        }
        //Energy system
        EnergyUse(speedfactor);

        ///Debug.Log($"MotorInput : {-(mBrakeAccel * motorTorque)}, Brake : {mCurrentBrakeForce}");

        if (!bIsBraking && Speed > 0)
        {
            //mBrakeAccel = Mathf.Abs(mBrakeAccel);
        }
        if (mBattery <= 0)
        {
            mBrakeAccel = Mathf.Clamp(mBrakeAccel, -2, 0);
            mWColliderFront.motorTorque = 0f;
            mWColliderRear.motorTorque = 0f;
            Debug.Log("No energy");
        }
        switch (mDriveTypes)
        {
            case DriveTypes.Rear:
                mWColliderRear.motorTorque = -(mBrakeAccel * motorTorque);
                break;
            case DriveTypes.Front:
                mWColliderFront.motorTorque = -(mBrakeAccel * motorTorque);
                break;
            case DriveTypes.All:
                mWColliderFront.motorTorque = -(mBrakeAccel * motorTorque);
                mWColliderRear.motorTorque = -(mBrakeAccel * motorTorque);
                break;
            default:
                break;
        }
        if (Speed < 0) { motorTorque = 0; }
        //Debug.Log($"CurrentMotorPower {-(mBrakeAccel * motorTorque)}");


        BrakeLight();
        ApplyBraking();
        //Debug.Log($"MotorInput{ mBrakeAccel}");
    }

    private void EnergyUse(float speedfactor)
    {
        mBattery = Mathf.Clamp(mBattery, 0f, 100f);
        float energyUseMultiplier = OverTakePower();
        float energyuse = Mathf.Max(0f, mBrakeAccel) * mEnergyUseIndex * speedfactor * Time.deltaTime;
        
        mBattery -= energyuse;
        if (bRegenBrake && (Speed > 5f || Speed < 5f))
        {
            mBrakeAccel = -0.2f;
            float regen = mRegenStrength * speedfactor * Time.deltaTime;
            mBattery += regen;
            //Debug.Log($"Regen {regen}");
            if (bRegenBrake && Speed < 5f)
            {
                mBrakeAccel = 0;
            }
        }
    }

    private float OverTakePower() 
    {
        if (!bOverTakePower || bRegenBrake || bIsBraking) return mEnergyUseIndex;
        float overtakePower = mOverTakePowerIndex;
        return overtakePower;
    }

    private void BrakeLight()
    {
        Material currentBrakeLight = bIsBraking ? mBrakeLightMatON : mBrakeLightMatOFF;
        if (bRegenBrake) { currentBrakeLight = mBrakeLightMatREGENON; if (bIsBraking) { currentBrakeLight = mBrakeLightMatON; } }
        Renderer brakeLightRenderer_L = mBrakeLight_L.GetComponent<Renderer>();
        Renderer brakeLightRenderer_R = mBrakeLight_R.GetComponent<Renderer>();

        brakeLightRenderer_L.material = currentBrakeLight;
        brakeLightRenderer_R.material = currentBrakeLight;
    }

    private void ApplyBraking() 
    {
        mCurrentBrakeForce = bIsBraking ? mBrakeTorque : 0;
        mWColliderFront.brakeTorque = mCurrentBrakeForce;
        mWColliderRear.brakeTorque = mCurrentBrakeForce;
    }
    private void HandleSteer() 
    {
        mCurrentSteerAngle = mMaxSteerAngle * (InvertSteer ? -mMoveInput.x : mMoveInput.x);
        mWColliderFront.steerAngle = mCurrentSteerAngle;

        //Moving Tire
        //Debug.Log($"SteerInput{mCurrentSteerAngle}");
        float clampedSteerAngle = Mathf.Clamp(mCurrentSteerAngle, -10, 10);
        Vector3 euler = mFrontWheelTransform.localEulerAngles;
        mFrontWheelTransform.localEulerAngles = new Vector3(euler.x, clampedSteerAngle + (AkiraMoto ? +90 : +0), euler.z);
    }

    private void RotateTire() 
    {
        float rotationIndex = -(Speed * 36f);
        mFrontWheelTransform.Rotate(AkiraMoto ? Vector3.forward : Vector3.right, Time.deltaTime * rotationIndex);
        mRearWheelTransform.Rotate(AkiraMoto ? Vector3.forward : Vector3.right, Time.deltaTime * rotationIndex);
    }

    private void BodyTilt() 
    {
        float ZRotation = 0f;
            if (Speed > 0.1) 
            {
                ZRotation = Mathf.Clamp(mZtiltAngle * mCurrentSteerAngle * Speed, -8,8);
            }
        Quaternion targetRotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, ZRotation), 0.1f);
        Quaternion newRotation = Quaternion.Euler(targetRotation.eulerAngles.x, transform.eulerAngles.y, targetRotation.eulerAngles.z);
        transform.rotation = newRotation;
    }
    private void GetSpeed() 
    {
        Vector3 velocity = rb.linearVelocity;
        Speed = velocity.magnitude;
        rawSpeed = velocity.magnitude;
        Speed = Speed * 3.6f; //m/s to km/h
    }

    private void SkidMark() 
    {
        if (bSkidOn == false){ return; }
        if (Speed > mMinSkidSpeed) { mSkidMarks.emitting = true; }
        else { mSkidMarks.emitting = false; }
    }
    private void OnEnable()
    {
        mInputAction.Enable();
    }

    private void OnDisable()
    {
        mInputAction.Disable();
    }

}
