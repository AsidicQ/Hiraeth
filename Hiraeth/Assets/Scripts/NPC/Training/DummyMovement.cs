using UnityEngine;
using System.Collections;

public class DummyMovement : MonoBehaviour
{
    public Transform[] waypoints;
    public float dummySpeed = 2f;
    public float stoppingDistance = 0.2f;

    public bool stopAtWaypoint = false;
    public float pauseTime = 1f;

    private int currentWaypointIndex = 0;
    private bool isPaused = false;

    void Update()
    {
        if (isPaused || waypoints.Length == 0)
            return;

        MoveToWaypoint();
    }

    void MoveToWaypoint()
    {
        Transform targetWaypoint = waypoints[currentWaypointIndex];

        transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, dummySpeed * Time.deltaTime);

        Vector3 direction = (targetWaypoint.position - transform.position).normalized;
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * dummySpeed);
        }

        if (Vector3.Distance(transform.position, targetWaypoint.position) < stoppingDistance)
        {
            if (stopAtWaypoint)
            {
                StartCoroutine(WaitAtWaypoint());
            }
            else
            {
                SetNextRandomWaypoint();
            }
        }
    }

    void SetNextRandomWaypoint()
    {
        if (waypoints.Length > 1)
        {
            int newWaypointIndex = currentWaypointIndex;
            while (newWaypointIndex == currentWaypointIndex)
            {
                newWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
            }
            currentWaypointIndex = newWaypointIndex;
        }
        else
        {
            currentWaypointIndex = 0;
        }
    }


    IEnumerator WaitAtWaypoint()
    {
        isPaused = true;
        yield return new WaitForSeconds(pauseTime);
        SetNextRandomWaypoint();
        isPaused = false;
    }
}
