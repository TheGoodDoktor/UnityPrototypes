using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockController : MonoBehaviour
{
    // Public Parameters
    public float m_MinSpeed = 5.0f;
    public float m_MaxSpeed = 20.0f;
    public float m_Randomness = 1.0f;
    public int m_FlockSize = 20;
    public GameObject m_FlockEntityPrefab;
    public GameObject m_FollowTarget;

    private Vector3 m_FlockCentre;
    private Vector3 m_FlockVelocity;
    private FlockEntity[] m_FlockEntities;

    // public properties
    public Vector3 FlockCentre {get { return m_FlockCentre; } }
    public Vector3 FlockVelocity { get { return m_FlockVelocity; } }

    // Use this for initialization
    void Start ()
    {
        m_FlockEntities = new FlockEntity[m_FlockSize];
        Collider collider = GetComponent<Collider>();
        for(int i=0;i<m_FlockSize;i++)
        {
            Vector3 position = new Vector3(
                Random.value * collider.bounds.size.x,
                Random.value * collider.bounds.size.y,
                Random.value * collider.bounds.size.z
                ) - collider.bounds.extents;

            GameObject flockEntity = Instantiate(m_FlockEntityPrefab, transform.position, transform.rotation) as GameObject;
            flockEntity.transform.parent = transform;
            flockEntity.transform.localPosition = position;
            flockEntity.GetComponent<FlockEntity>().SetController(gameObject);
            m_FlockEntities[i] = flockEntity.GetComponent<FlockEntity>();
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
        Vector3 centre = Vector3.zero;
        Vector3 velocity = Vector3.zero;
		
        foreach(FlockEntity flockEntity in m_FlockEntities)
        {
            centre += flockEntity.transform.localPosition;
            velocity += flockEntity.Velocity;
        }

        m_FlockCentre = centre / m_FlockSize;
        m_FlockVelocity = velocity / m_FlockSize;
	}
}
