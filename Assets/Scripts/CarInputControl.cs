using UnityEngine;

public class CarInputControl : MonoBehaviour
{
    [SerializeField] private Car m_Car;

    private void Update()
    {
        m_Car.throttleControl = Input.GetAxis("Vertical");
        m_Car.brakeControl = Input.GetAxis("Jump");
        m_Car.steerControl = Input.GetAxis("Horizontal");
    }
}
