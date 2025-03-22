using System;
using UnityEngine;

public class CarInputControl : MonoBehaviour
{
    [SerializeField] private Car m_Car;
    [SerializeField] private AnimationCurve m_BreakCurve;
    [SerializeField] private AnimationCurve m_SteerCurve;

    [SerializeField] [Range(0.0f, 1.0f)] private float m_AutoBreakStrength = 0.5f;

    private float wheelSpeed;
    private float verticalAxis;
    private float horizontalAxis;
    private float handbreakAxis;

    private void Update()
    {
        wheelSpeed = m_Car.WheelSpeed;

        UpdateAxis();

        UpdateThrottleAndBreak();
        UpdateSteer();

        UpdateAutoBreak();
    }

    private void UpdateSteer()
    {
        m_Car.steerControl = m_SteerCurve.Evaluate(m_Car.WheelSpeed / m_Car.MaxSpeed) * horizontalAxis;
    }

    private void UpdateThrottleAndBreak()
    {
        if (Mathf.Sign(verticalAxis) == Mathf.Sign(wheelSpeed) || Mathf.Abs(wheelSpeed) < 0.5f)
        {
            m_Car.throttleControl = verticalAxis;
            m_Car.brakeControl = 0;
        }
        else
        {
            m_Car.throttleControl = 0;
            m_Car.brakeControl = m_BreakCurve.Evaluate(wheelSpeed / m_Car.MaxSpeed);
        }
    }

    private void UpdateAutoBreak()
    {
        if (verticalAxis == 0)
        {
            m_Car.brakeControl = m_BreakCurve.Evaluate(wheelSpeed / m_Car.MaxSpeed) * m_AutoBreakStrength;
        }
    }

    private void UpdateAxis()
    {
        verticalAxis = Input.GetAxis("Vertical");
        horizontalAxis = Input.GetAxis("Horizontal");
        handbreakAxis = Input.GetAxis("Jump");
    }
}
