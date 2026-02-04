using UnityEngine;
using UnityEngine.AI;

public class FlightEnemyMoveTo : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float offset = 3f;
    [SerializeField] private float heightFollowSpeed = 3f;

    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updatePosition = false; // 👈 critical
        agent.updateUpAxis = false;
    }

    void Update()
    {
        // Let NavMesh calculate X/Z path only
        Vector3 destination = target.position;
        destination.y = agent.nextPosition.y;
        agent.SetDestination(destination);

        // Smooth vertical movement
        float desiredHeight = target.position.y + offset;

        Vector3 newPosition = agent.nextPosition;
        newPosition.y = Mathf.Lerp(
            transform.position.y,
            desiredHeight,
            Time.deltaTime * heightFollowSpeed
        );

        // Apply final position
        transform.position = newPosition;

        // Keep agent synced with transform
        agent.nextPosition = transform.position;
    }
}