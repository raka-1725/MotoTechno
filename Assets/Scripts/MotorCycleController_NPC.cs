using System.Collections;
using System.IO;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class MotorCycleController_NPC : Unity.MLAgents.Agent
{
    TrailRenderer mSkidMarks;
    NPC_Path mPath;
    private Vector2 mMoveInput;
    private float mHorizontalSteer; //horizontal input
    [SerializeField] private float mBrakeAccel; //vertical input
    [SerializeField] private float mCurrentSteerAngle;
    private float mCurrentBrakeForce;

    private float stuckTimer = 0f;

    [Header("Waypoints")]
    [SerializeField] private int currentWPindex = 0;
    [SerializeField] private float mWPprox = 3; 


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
        rb = GetComponent<Rigidbody>();
        mPath = FindAnyObjectByType<NPC_Path>();

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
        RequestDecision();
        CheckStuck();
        CheckWaypointProximity();
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

    private void HandleMotor()
    {
        mBrakeAccel = (mMoveInput.y);
        if (mBrakeAccel >= 0) { bIsBraking = false; }
        if (mBrakeAccel < 0) { bIsBraking = true; }
        float speedfactor = Mathf.InverseLerp(0, 200, Speed);
        float motorTorque = Mathf.Lerp(mMotorPower, 0, speedfactor);

        //Energy system
        EnergyUse(speedfactor);
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
                mWColliderRear.motorTorque = (mBrakeAccel * motorTorque);
                break;
            case DriveTypes.Front:
                mWColliderFront.motorTorque = (mBrakeAccel * motorTorque);
                break;
            case DriveTypes.All:
                mWColliderFront.motorTorque = (mBrakeAccel * motorTorque);
                mWColliderRear.motorTorque = (mBrakeAccel * motorTorque);
                break;
            default:
                break;
        }
        if (Speed < 0) { motorTorque = 0; }
        BrakeLight();
        ApplyBraking();
    }

    private void EnergyUse(float speedfactor)
    {
        mBattery = Mathf.Clamp(mBattery, 0f, 100f);
        float energyUseMultiplier = mEnergyUseIndex;
        float energyuse = Mathf.Max(0f, mBrakeAccel) * mEnergyUseIndex * speedfactor * Time.deltaTime;
        
        mBattery -= energyuse;
        if (bRegenBrake && (Speed > 5f || Speed < 5f))
        {
            mBrakeAccel = -0.2f;
            float regen = mRegenStrength * speedfactor * Time.deltaTime;
            mBattery += regen;
            if (bRegenBrake && Speed < 5f)
            {
                mBrakeAccel = 0;
            }
        }
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



    private void OnDrawGizmos()
    {
        if (mPath == null || mPath.waypoints == null || mPath.waypoints.Count == 0)
            return;
        if (currentWPindex < 0 || currentWPindex >= mPath.waypoints.Count)
            return;
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, mPath.waypoints[currentWPindex].position);

        Gizmos.color = Color.violetRed;
        Gizmos.DrawWireSphere(transform.position, mWPprox);
    }

    private void CheckWaypointProximity()
    {
        if (mPath == null || mPath.waypoints.Count == 0) return;

        float distanceToWP = Vector3.Distance(transform.position, mPath.waypoints[currentWPindex].position);
        mWPprox = 8f;


        if (distanceToWP < mWPprox)
        {
            AddReward(1.0f);
            currentWPindex++;
            if (currentWPindex >= mPath.waypoints.Count)
            {
                currentWPindex = 0;
            }
        }
    }

    //Below is for training AI
    private void OnTriggerEnter(Collider other)
    {

        switch (other.tag)
        {
            case "Wall":
                AddReward(-2f);
                EndEpisode();
                break;

            case "Grass":
                AddReward(-1.5f);
                EndEpisode();
                break;

            case "Gravel":
                AddReward(-1.8f);
                EndEpisode();
                break;
            case "DirtyTarmac":
                AddReward(-0.5f); 
                break;

            case "Checkpoint":
                AddReward(-0.3f);
                break;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Grass" || other.tag == "Gravel" || other.tag == "DirtyTarmac")
        {
            AddReward(-0.001f); 
        }
    }

    private void CheckStuck()
    {
        if (Speed < 1f)
        {
            stuckTimer += Time.fixedDeltaTime;
        }
        else
        {
            stuckTimer = 0f;
        }

        if (stuckTimer > 5f)
        {
            AddReward(-1f);
            EndEpisode();
        }
    }

    public override void OnEpisodeBegin()
    {

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        transform.position = new Vector3(0, 0, 6.5f);
        transform.rotation = Quaternion.Euler(0, 90, 0);

        mBattery = mMotoSpecCustom.BatteryCapacity;
        currentWPindex = 0;
    }

    public override void CollectObservations(Unity.MLAgents.Sensors.VectorSensor sensor)
    {
        //control stats
        sensor.AddObservation(Speed / 200f);
        sensor.AddObservation(mBattery / 100f);
        sensor.AddObservation(mCurrentSteerAngle / mMaxSteerAngle);
        sensor.AddObservation(transform.forward);
        sensor.AddObservation(rb.linearVelocity.normalized);
        sensor.AddObservation(mMoveInput);

        Vector3 targetWP = mPath.waypoints[currentWPindex].position;
        Vector3 relativeWP = transform.InverseTransformPoint(mPath.waypoints[currentWPindex].position);
        sensor.AddObservation(relativeWP.normalized);

        float distanceToWP = Vector3.Distance(transform.position, targetWP);
        sensor.AddObservation(distanceToWP / 100f);

        Vector3 directionToWP = (targetWP - transform.position).normalized;
        float angleToWP = Vector3.Angle(transform.forward, directionToWP);
        sensor.AddObservation(angleToWP / 180f);
    }

    public override void OnActionReceived(Unity.MLAgents.Actuators.ActionBuffers actions)
    {
        float moveY = Mathf.Clamp(actions.ContinuousActions[0], -1f, 1f); 
        float steerX = Mathf.Clamp(actions.ContinuousActions[1], -1f, 1f);
        float regen = actions.ContinuousActions.Length > 2 ? actions.ContinuousActions[2] : 0;
        var contActions = actions.ContinuousActions;
        //Debug.Log($"AI actions: {contActions[0]}, {contActions[1]}, {(contActions.Length > 2 ? contActions[2] : 0)}");
        //Debug.Log($"AI input moveY: {moveY}, steerX: {steerX}, regen: {regen}");

        mMoveInput = new Vector2(steerX, moveY);
        bRegenBrake = regen > 0.5f;

        Vector3 directionToWP = (mPath.waypoints[currentWPindex].position - transform.position).normalized;
        float alignment = Vector3.Dot(transform.forward, directionToWP);
        AddReward(alignment * 0.005f);

        float forwardSpeed = Vector3.Dot(rb.linearVelocity, transform.forward);
        AddReward(forwardSpeed * 0.001f);


        float steerDirection = Mathf.Sign(Vector3.SignedAngle(transform.forward, directionToWP, Vector3.up));
        float steerInput = mMoveInput.x; 
        float steerAlignment = steerDirection * steerInput; 
        AddReward(Mathf.Clamp(steerAlignment, 0, 1) * 0.01f);


        float speedTowardsWP = Vector3.Dot(rb.linearVelocity, directionToWP);
        AddReward(Mathf.Clamp(speedTowardsWP, 0, 20) * 0.0005f);

        
        if (alignment < 0.5f)
        {
            AddReward(-0.005f);
        }
        if (mBattery <= 0)
        {
            AddReward(-1f); 
            EndEpisode();
        }
        if (bRegenBrake && Speed > 10f)
            AddReward(0.001f);

        if (transform.position.y < -5f)
        {
            AddReward(-1f); 
            EndEpisode();
        }
    }



    //Training by Human

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;

        // Example: use keyboard input to control
        float moveY = 0f;
        float steerX = 0f;

        if (Keyboard.current.wKey.isPressed) moveY = 1f;
        else if (Keyboard.current.sKey.isPressed) moveY = -1f;

        if (Keyboard.current.aKey.isPressed) steerX = -1f;
        else if (Keyboard.current.dKey.isPressed) steerX = 1f;

        continuousActionsOut[0] = moveY;
        continuousActionsOut[1] = steerX;
        continuousActionsOut[2] = 0f;
    }
}

