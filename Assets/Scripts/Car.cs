using UnityEngine;

[RequireComponent(typeof(CarChassis))]
public class Car : MonoBehaviour
{
    [SerializeField] private float m_MaxMotorTorque;
    [SerializeField] private float m_MaxSteerAngle;
    [SerializeField] private float m_MaxBrakeTorque;

    private CarChassis m_Chassis;

    public float throttleControl;
    public float steerControl;
    public float brakeControl;

    private void Start()
    {
        m_Chassis = GetComponent<CarChassis>();
    }

    private void Update()
    {
        m_Chassis.motorTorque = m_MaxMotorTorque * throttleControl;
        m_Chassis.steerAngle = m_MaxSteerAngle * steerControl;
        m_Chassis.motorTorque = m_MaxBrakeTorque * brakeControl;
    }
}
