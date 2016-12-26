using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockEntity : MonoBehaviour
{
    private GameObject m_FlockController;
    private bool m_Initialised = false;
    private float m_MinSpeed;
    private float m_MaxSpeed;
    private float m_Randomness;
    private GameObject m_FollowTarget;
    private Vector3 m_Velocity;

    // public properties
    public Vector3 Velocity { get { return m_Velocity; } }

    // Use this for initialization
    void Start ()
    {
        StartCoroutine("FlockSteering");
    }

    IEnumerator FlockSteering()
    {
        while (true)
        {
            if (m_Initialised)
            {
                m_Velocity = m_Velocity + CalcAcceleration() * Time.deltaTime;

                // enforce minimum and maximum speeds for the boids
                float speed = m_Velocity.magnitude;
                if (speed > m_MaxSpeed)
                {
                    m_Velocity = m_Velocity.normalized * m_MaxSpeed;
                }
                else if (speed < m_MinSpeed)
                {
                    m_Velocity = m_Velocity.normalized * m_MinSpeed;
                }
            }

            float waitTime = Random.Range(0.3f, 0.5f);
            yield return new WaitForSeconds(waitTime);
        }
    }

    private Vector3 CalcAcceleration()
    {
        Vector3 randomize = new Vector3((Random.value * 2) - 1, (Random.value * 2) - 1, (Random.value * 2) - 1);

        randomize.Normalize();
        FlockController boidController = m_FlockController.GetComponent<FlockController>();
        Vector3 flockCenter = boidController.FlockCentre;
        Vector3 flockVelocity = boidController.FlockVelocity;
        Vector3 follow = m_FollowTarget.transform.localPosition;

        flockCenter = flockCenter - transform.localPosition;
        flockVelocity = flockVelocity - m_Velocity;
        follow = follow - transform.localPosition;

        return (flockCenter + flockVelocity + follow * 2 + randomize * m_Randomness);
    }

    // Update is called once per frame
    void Update ()
    {
        transform.position += m_Velocity * Time.deltaTime;
        // TODO: Apply rotation to point in vel dir
    }

    public void SetController(GameObject controller)
    {
        m_FlockController = controller;
        FlockController flockController = controller.GetComponent<FlockController>();
        m_MinSpeed = flockController.m_MinSpeed;
        m_MaxSpeed = flockController.m_MaxSpeed;
        m_Randomness = flockController.m_Randomness;
        m_FollowTarget = flockController.m_FollowTarget;
        m_Initialised = true;
    }
}
