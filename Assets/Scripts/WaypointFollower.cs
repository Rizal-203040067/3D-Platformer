using UnityEngine;

public class WaypointFollower : MonoBehaviour
{
    [SerializeField] GameObject[] waypoints;
    int currentWaypointIndex = 0;

    [SerializeField] float speed = 5f;

    [SerializeField] Animator animator; // optional

    void Update()
    {
        Vector3 target = waypoints[currentWaypointIndex].transform.position;

        transform.position = Vector3.MoveTowards(
            transform.position,
            target,
            speed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, target) < 0.1f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }

        //  SAFE animation call
        if (animator != null)
        {
            animator.SetFloat("Speed", speed);
        }
    }
}
