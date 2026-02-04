using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class AgentMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 20f;
    public float gravity = -9.81f;

    private NavMeshAgent agent;
    private float verticalVelocity;
    private bool isGrounded;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updatePosition = false;
        agent.updateRotation = true;
    }

    void Update()
    {
        Keyboard kb = Keyboard.current;

        float x = 0f;
        float z = 0f;

        if (kb.wKey.isPressed) z += 1;
        if (kb.sKey.isPressed) z -= 1;
        if (kb.dKey.isPressed) x += 1;
        if (kb.aKey.isPressed) x -= 1;

        Vector3 input = new Vector3(x, 0, z).normalized;
        Vector3 move = transform.TransformDirection(input) * moveSpeed;

        agent.Move(move * Time.deltaTime);

        isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.1f);

        if (isGrounded && verticalVelocity < 0)
            verticalVelocity = -2f;

        if (kb.spaceKey.wasPressedThisFrame && isGrounded)
            verticalVelocity = jumpForce;

        verticalVelocity += gravity * Time.deltaTime;

        Vector3 pos = agent.nextPosition;
        pos.y += verticalVelocity * Time.deltaTime;

        transform.position = pos;
        agent.nextPosition = transform.position;
    }
}