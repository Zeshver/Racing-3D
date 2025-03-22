using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CarChassis : MonoBehaviour
{
    [SerializeField] private WheelAxle[] m_WheelAxles;
    [SerializeField] private float m_WheelBaseLenght;

    [SerializeField] private Transform m_CenterOfMass;

    [Header("DownForce")]
    [SerializeField] private float m_DownForceMin;
    [SerializeField] private float m_DownForceMax;
    [SerializeField] private float m_DownForceFactor;

    [Header("AngularDrag")]
    [SerializeField] private float m_AngularDragMin;
    [SerializeField] private float m_AngularDragMax;
    [SerializeField] private float m_AngularDragFactor;

    public float motorTorque;
    public float brakeTorque;
    public float steerAngle;

    public float LinearVelocity => rigidbody.velocity.magnitude * 3.6f;

    private new Rigidbody rigidbody;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();

        if (m_CenterOfMass != null)
        {
            rigidbody.centerOfMass = m_CenterOfMass.localPosition;
        }
    }

    private void FixedUpdate()
    {
        UpdateAngularDrag();
        UpdateDownForce();

        UpdateWheelAxles();
    }

    public float GetAverageRpm()
    {
        float sum = 0;

        for (int i = 0; i < m_WheelAxles.Length; i++)
        {
            sum += m_WheelAxles[i].GetAvarageRpm();
        }

        return sum / m_WheelAxles.Length;
    }

    public float GetWheelSpeed()
    {
        return GetAverageRpm() * m_WheelAxles[0].GetRadius() * 2 * 0.1885f;
    }

    private void UpdateAngularDrag()
    {
        rigidbody.angularDrag = Mathf.Clamp(m_AngularDragFactor * LinearVelocity, m_AngularDragMin, m_AngularDragMax);
    }

    private void UpdateDownForce()
    {
        float downForce = Mathf.Clamp(m_DownForceFactor * LinearVelocity, m_DownForceMin, m_DownForceMax);
        rigidbody.AddForce(-transform.up * downForce);
    }

    private void UpdateWheelAxles()
    {
        int amountMotorWheel = 0;

        for (int i = 0; i < m_WheelAxles.Length; i++)
        {
            if (m_WheelAxles[i].IsMotor == true)
            {
                amountMotorWheel += 2;
            }
        }

        for (int i = 0; i < m_WheelAxles.Length; i++)
        {
            m_WheelAxles[i].Update();

            m_WheelAxles[i].ApplyMotorTorque(motorTorque);
            m_WheelAxles[i].ApplySteerAngle(steerAngle, m_WheelBaseLenght);
            m_WheelAxles[i].ApplyBreakTorque(brakeTorque);
        }
    }
}
