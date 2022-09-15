using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockAgent : MonoBehaviour
{
    FlockingController flockingController;
    public FlockingController GetFlockingController { get { return flockingController; } }

    Collider agentCollider;
    public Collider GetAgentCollider { get { return agentCollider; } }

    public float numberOfNeigbours;
    public float numberOfPredators;

    // Start is called before the first frame update
    void Start()
    {
        agentCollider = GetComponent<Collider>();
        flockingController = FindObjectOfType<FlockingController>();
    }

    public void Initialize(FlockingController flockController)
    {
        flockingController = flockController;
    }

    public void Move(Vector3 velocity)
    {
        transform.forward = velocity;
        transform.position += (Vector3)velocity * Time.deltaTime;
    }

    /*
        void OnDrawGizmos()
        {
            // Draw a yellow sphere at the transform's position
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, flockingController.neighbourRadius);

            // Draw a yellow sphere at the transform's position
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, Mathf.Sqrt(flockingController.squareAvoidanceRadius));
        }
    */

    public void SetNumberOfNeighbours(float number)
    {
        numberOfNeigbours = number;
    }
}