using UnityEngine;

[RequireComponent(typeof(CarChassis))]
public class Car : MonoBehaviour
{    
    [SerializeField] private float m_MaxSteerAngle;
    [SerializeField] private float m_MaxBrakeTorque;

    [SerializeField] private AnimationCurve m_EngineTorqueCurve;
    [SerializeField] private float m_EngineMaxTorque;
    [SerializeField] private float m_EngineTorque;
    [SerializeField] private float m_EngineRpm;
    [SerializeField] private float m_EngineMinRpm;
    [SerializeField] private float m_EngineMaxRpm;

    [SerializeField] private float maxSpeed;

    public float LinearVelocity => chassis.LinearVelocity;
    public float WheelSpeed => chassis.GetWheelSpeed();
    public float MaxSpeed => maxSpeed;

    private CarChassis chassis;

    [SerializeField] private float linearVelocity;
    public float throttleControl;
    public float steerControl;
    public float brakeControl;

    private void Start()
    {
        chassis = GetComponent<CarChassis>();
    }

    private void Update()
    {
        linearVelocity = LinearVelocity;

        UpdateEngineTorque();

        float engineTorque = m_EngineTorqueCurve.Evaluate(LinearVelocity / maxSpeed) * m_EngineMaxTorque;

        if (LinearVelocity >= maxSpeed)
        {
            engineTorque = 0;
        }

        chassis.motorTorque = engineTorque * throttleControl;
        chassis.steerAngle = m_MaxSteerAngle * steerControl;
        chassis.brakeTorque = m_MaxBrakeTorque * brakeControl;
    }

    private void UpdateEngineTorque()
    {
        m_EngineRpm = m_EngineMinRpm + Mathf.Abs(chassis.GetAverageRpm() * 3.7f);
        m_EngineRpm = Mathf.Clamp(m_EngineRpm, m_EngineMinRpm, m_EngineMaxRpm);

        m_EngineTorque = m_EngineTorqueCurve.Evaluate(m_EngineRpm / m_EngineMaxRpm) * m_EngineMaxTorque;
    }
}
