using System;
using UnityEngine;

public class CarChassis : MonoBehaviour
{
    [SerializeField] private WheelAxle[] m_WheelAxles;

    public float motorTorque;
    public float brakeTorque;
    public float steerAngle;

    private void FixedUpdate()
    {
        UpdateWheelAxles();
    }

    private void UpdateWheelAxles()
    {
        for (int i = 0; i < m_WheelAxles.Length; i++)
        {
            m_WheelAxles[i].Update();

            m_WheelAxles[i].ApplyMotorTorque(motorTorque);
            m_WheelAxles[i].ApplySteerAngle(steerAngle);
            m_WheelAxles[i].ApplyBreakTorque(brakeTorque);
        }
    }
}
