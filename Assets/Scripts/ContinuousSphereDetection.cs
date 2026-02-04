using UnityEngine;

public class ContinuousSphereDetection : MonoBehaviour
{
    [SerializeField] private string targetTag = "Player";  // The tag we are looking for
    [SerializeField] private float detectionRadius = 5f;    // The radius of the sphere
    [SerializeField] private float attackInterval = 1f;     // Time between each attack
    private float lastAttackTime = 0f;

    [SerializeField] private GameObject attackSphere;      // Reference to the attack sphere
    private Vector3 attackTargetPosition;                   // Store the position where the player was when attack started
    private bool isAttacking = false;                       // Flag to check if the attack is happening

    private void Start()
    {
        // Initially disable the attack sphere (hidden inside the enemy model)
        attackSphere.SetActive(false);
    }

    void Update()
    {
        // If attack is not triggered, follow the enemy's position
        if (!isAttacking)
        {
            // Update the attack sphere's position to match the enemy's position
            attackSphere.transform.position = transform.position; 
        }
        else
        {
            // Once the attack is triggered, move the sphere towards the player's position
            MoveAttackSphere();
        }
    }

    // This method will be called when the player enters the collider
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(targetTag))
        {
            // If the player is inside the sphere, trigger the attack
            if (Time.time - lastAttackTime >= attackInterval)
            {
                // Perform your attack action here (start the attack)
                Attack(other.transform);
                lastAttackTime = Time.time;
            }
        }
    }

    // Trigger attack (start moving the sphere toward the player's position)
    public void Attack(Transform player)
    {
        // Record the player's position when the attack starts
        attackTargetPosition = player.position;

        // Activate the sphere and start the attack movement
        attackSphere.SetActive(true);
        isAttacking = true;

        Debug.Log("Attack triggered! Moving sphere toward player...");
    }

    // Move the attack sphere towards the player's recorded position
    private void MoveAttackSphere()
    {
        // Move the attack sphere toward the player's position using world space
        attackSphere.transform.position = Vector3.MoveTowards(
            attackSphere.transform.position,
            attackTargetPosition,
            5f * Time.deltaTime  // Adjust speed as needed
        );

        // Optionally, you can add some logic to check when the attack sphere reaches the target
        if (Vector3.Distance(attackSphere.transform.position, attackTargetPosition) < 0.1f)
        {
            // Stop the attack or deal damage here
            isAttacking = false;
            Debug.Log("Attack finished!");
            attackSphere.SetActive(false);  // Hide the sphere after the attack
        }
    }

    // If the player is still inside the sphere, you can also check every frame (optional)
    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(targetTag))
        {
            // You can add additional logic here if you want to keep tracking the player inside the sphere
        }
    }
}
