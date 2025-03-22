using System;
using UnityEngine;

[Serializable]
public class WheelAxle
{
    [SerializeField] private WheelCollider m_LeftWheelCollider;
    [SerializeField] private WheelCollider m_RightWheelCollider;

    [SerializeField] private Transform m_LeftWheelMesh;
    [SerializeField] private Transform m_RighttWheelMesh;

    [SerializeField] private bool isMotor;
    [SerializeField] private bool isSteer;

    [SerializeField] private float wheelWidth;

    [SerializeField] private float antiRollForce;

    [SerializeField] private float additionalWheelDownForce;

    [SerializeField] private float baseForwardStiffnes = 1.5f;
    [SerializeField] private float stabilityForwardFactor = 1.0f;

    [SerializeField] private float baseSidewaysStiffnes = 2.0f;
    [SerializeField] private float stabilitySidewaysFactor = 1.0f;

    private WheelHit leftWheelHit;
    private WheelHit rightWheelHit;

    public bool IsMotor => isMotor;
    public bool IsSteer => isSteer;

    public void Update()
    {
        UpdateWheelHits();

        ApplyAntiRoll();
        ApplyDownForce();
        CorrectStiffness();

        SyncMeshTransform();
    }

    private void UpdateWheelHits()
    {
        m_LeftWheelCollider.GetGroundHit(out leftWheelHit);
        m_RightWheelCollider.GetGroundHit(out rightWheelHit);
    }

    private void CorrectStiffness()
    {
        WheelFrictionCurve leftForward = m_LeftWheelCollider.forwardFriction;
        WheelFrictionCurve rightForward = m_RightWheelCollider.forwardFriction;

        WheelFrictionCurve leftSideways = m_LeftWheelCollider.sidewaysFriction;
        WheelFrictionCurve rightSideways = m_RightWheelCollider.sidewaysFriction;

        leftForward.stiffness = baseForwardStiffnes + Mathf.Abs(leftWheelHit.forwardSlip) * stabilityForwardFactor;
        rightForward.stiffness = baseForwardStiffnes + Mathf.Abs(rightWheelHit.forwardSlip) * stabilityForwardFactor;

        leftSideways.stiffness = baseSidewaysStiffnes + Mathf.Abs(leftWheelHit.sidewaysSlip) * stabilitySidewaysFactor;
        rightSideways.stiffness = baseSidewaysStiffnes + Mathf.Abs(rightWheelHit.sidewaysSlip) * stabilitySidewaysFactor;

        m_LeftWheelCollider.forwardFriction = leftForward;
        m_RightWheelCollider.forwardFriction = rightForward;

        m_LeftWheelCollider.sidewaysFriction = leftSideways;
        m_RightWheelCollider.sidewaysFriction = rightSideways;
    }

    private void ApplyDownForce()
    {
        if (m_LeftWheelCollider.isGrounded == true)
        {
            m_LeftWheelCollider.attachedRigidbody.AddForceAtPosition(leftWheelHit.normal * -additionalWheelDownForce *
                m_LeftWheelCollider.attachedRigidbody.velocity.magnitude, m_LeftWheelCollider.transform.position);
        }

        if (m_RightWheelCollider.isGrounded == true)
        {
            m_RightWheelCollider.attachedRigidbody.AddForceAtPosition(rightWheelHit.normal * -additionalWheelDownForce *
                m_RightWheelCollider.attachedRigidbody.velocity.magnitude, m_RightWheelCollider.transform.position);
        }
    }

    private void ApplyAntiRoll()
    {
        float travelL = 1.0f;
        float travelR = 1.0f;

        if (m_LeftWheelCollider.isGrounded == true)
        {
            travelL = (-m_LeftWheelCollider.transform.InverseTransformPoint(leftWheelHit.point).y - m_LeftWheelCollider.radius) / m_LeftWheelCollider.suspensionDistance;
        }

        if (m_RightWheelCollider.isGrounded == true)
        {
            travelR = (-m_RightWheelCollider.transform.InverseTransformPoint(rightWheelHit.point).y - m_RightWheelCollider.radius) / m_RightWheelCollider.suspensionDistance;
        }

        float forceDir = (travelL - travelR);

        if (m_LeftWheelCollider.isGrounded == true)
        {
            m_LeftWheelCollider.attachedRigidbody.AddForceAtPosition(m_LeftWheelCollider.transform.up * -forceDir * antiRollForce, m_LeftWheelCollider.transform.position);
        }

        if (m_RightWheelCollider.isGrounded == true)
        {
            m_RightWheelCollider.attachedRigidbody.AddForceAtPosition(m_RightWheelCollider.transform.up * forceDir * antiRollForce, m_RightWheelCollider.transform.position);
        }
    }

    public void ApplySteerAngle(float steerAngle, float wheelBaseLenght)
    {
        if (isSteer == false)
        {
            return;
        }

        float radius = Mathf.Abs(wheelBaseLenght * Mathf.Tan(Mathf.Deg2Rad * (90 - Mathf.Abs(steerAngle))));
        float angleSing = Mathf.Sign(steerAngle);

        if (steerAngle > 0)
        {
            m_LeftWheelCollider.steerAngle = Mathf.Rad2Deg * Mathf.Atan(wheelBaseLenght / (radius + (wheelWidth * 0.5f))) * angleSing;
            m_RightWheelCollider.steerAngle = Mathf.Rad2Deg * Mathf.Atan(wheelBaseLenght / (radius - (wheelWidth * 0.5f))) * angleSing;
        }
        else if (steerAngle < 0)
        {
            m_LeftWheelCollider.steerAngle = Mathf.Rad2Deg * Mathf.Atan(wheelBaseLenght / (radius - (wheelWidth * 0.5f))) * angleSing;
            m_RightWheelCollider.steerAngle = Mathf.Rad2Deg * Mathf.Atan(wheelBaseLenght / (radius + (wheelWidth * 0.5f))) * angleSing;
        }
        else
        {
            m_LeftWheelCollider.steerAngle = 0;
            m_RightWheelCollider.steerAngle = 0;
        }
    }

    public void ApplyMotorTorque(float motorTorque)
    {
        if (isMotor == false)
        {
            return;
        }

        m_LeftWheelCollider.motorTorque = motorTorque;
        m_RightWheelCollider.motorTorque = motorTorque;
    }

    public void ApplyBreakTorque(float brakeTorque)
    {
        m_LeftWheelCollider.brakeTorque = brakeTorque;
        m_RightWheelCollider.brakeTorque = brakeTorque;
    }

    public float GetAvarageRpm()
    {
        return (m_LeftWheelCollider.rpm + m_RightWheelCollider.rpm) * 0.5f;
    }

    public float GetRadius()
    {
        return m_LeftWheelCollider.radius;
    }

    private void SyncMeshTransform()
    {
        UpdateWheelTransform(m_LeftWheelCollider, m_LeftWheelMesh);
        UpdateWheelTransform(m_RightWheelCollider, m_RighttWheelMesh);
    }

    private void UpdateWheelTransform(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 position;
        Quaternion rotation;
        wheelCollider.GetWorldPose(out position, out rotation);

        wheelTransform.position = position;
        wheelTransform.rotation = rotation;
    }
}
