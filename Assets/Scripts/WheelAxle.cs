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

    public void Update()
    {
        SyncMeshTransform();
    }

    public void ApplySteerAngle(float steerAngle)
    {
        if (isSteer == false)
        {
            return;
        }

        m_LeftWheelCollider.steerAngle = steerAngle;
        m_RightWheelCollider.steerAngle = steerAngle;
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
