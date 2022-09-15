using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockingController : MonoBehaviour
{
    public List<FlockAgent> flockAgentList = new List<FlockAgent>();
    [SerializeField] FlockAgent flockAgentPrefab = null;
    [SerializeField] GameObject flockAgentGroup = null;

    [Header("Starting settings")]
    [SerializeField] int numberOfFlockAgents = 200;
    [SerializeField] float agentSpawnDensity = 0.3f;
    [SerializeField] float flockingAreaRadius = 35f;

    [Header("FlockAgent properties")]
    [SerializeField] float neighbourRadius = 3f;
    [SerializeField] float avoidanceRadius = 2f;
    [SerializeField] float squareAvoidanceRadius = 4f;
    [SerializeField] float predatorSquareAvoidanceRadius = 20f;
    [SerializeField] float obstacleSquareAvoidanceRadius = 20f;

    [SerializeField] float agentSmoothTime = 0.3f;
    [SerializeField] float agentSpeed = 0.2f;

    [Header("Behaviour priority")]
    [SerializeField] private float cohesionPriority = 1f;
    [SerializeField] private float avoidancePriority = 4f;
    [SerializeField] private float alignmentPriority = 2f;
    [SerializeField] private float steeringPriority = 6f;
    [SerializeField] private float obstaclePriority = 1f;
    [SerializeField] private float predatorPriority = 10f;

    Vector3 currentVelocity;

    // Start is called before the first frame update
    void Start()
    {
        SpawnFlockAgents();
    }

    void SpawnFlockAgents()
    {
        squareAvoidanceRadius = avoidanceRadius * avoidanceRadius;
        for (int i = 0; i < numberOfFlockAgents; i++)
        {
            // Set random position and rotation of flock agents within the circle
            Vector2 randomPosition2D = Random.insideUnitCircle * numberOfFlockAgents * agentSpawnDensity;
            Vector3 randomPosition = new Vector3(transform.position.x + randomPosition2D.x, 0, transform.position.z + randomPosition2D.y);
            Quaternion randomRotation = Quaternion.Euler(0, Random.Range(0.0f, 360.0f), 0);

            FlockAgent unit = Instantiate(flockAgentPrefab, randomPosition, randomRotation, flockAgentGroup.transform);
            if (unit != null)
            {
                // Set references to this flocking controller for every single flock agent
                unit.Initialize(this);
                // Add flock agent to the list of the current flock agents
                flockAgentList.Add(unit);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (FlockAgent agent in flockAgentList)
        {
            List<Transform> obstacleList = new List<Transform>();
            List<Transform> predatorList = new List<Transform>();

            List<Transform> neighbourList = GetNearbyObjects(agent, out obstacleList, out predatorList);

            Vector3 move = CalculateMove(agent, neighbourList, obstacleList, predatorList);
            agent.SetNumberOfNeighbours(neighbourList.Count);

            move *= agentSpeed;

            agent.Move(move);
        }
    }

    // Return all of the neighbours for the current flock
    private List<Transform> GetNearbyObjects(FlockAgent currentAgent, out List<Transform> obstacles, out List<Transform> predators)
    {
        List<Transform> neighbours = new List<Transform>();

        obstacles = new List<Transform>();
        predators = new List<Transform>();

        Collider[] contextColliders = Physics.OverlapSphere(currentAgent.transform.position, neighbourRadius);

        foreach (Collider c in contextColliders)
        {
            // Check if collider is not this same as this object collider
            if (c != currentAgent.GetAgentCollider)
            {
                // Check if the component is FlockAgent
                if (c.gameObject.GetComponent<FlockAgent>())
                {
                    neighbours.Add(c.transform);
                }

                // Check if object has obstacle component
                if (c.gameObject.GetComponent<Obstacle>())
                {
                    obstacles.Add(c.transform);
                }

                // Check if object has predator component
                if (c.gameObject.GetComponent<Predator>())
                {
                    predators.Add(c.transform);
                }
            }

        }
        return neighbours;
    }

    private Vector3 CalculateMove(FlockAgent agent, List<Transform> neighbours, List<Transform> obstacleList, List<Transform> predatorList)
    {
        Vector3 move = Vector3.zero;

        Vector3 alignmentMovement = CalculateAlignment(agent, neighbours);
        Vector3 cohesionMovement = CalculateCohesion(agent, neighbours);
        Vector3 avoidanceMovement = CalculateAvoidance(agent, neighbours, squareAvoidanceRadius);
        Vector3 smoothSteering = CalculateSmoothSteer(agent, neighbours);
        Vector3 stayInRadiusMovement = CalculateStayInRadius(agent, neighbours);
        Vector3 obstacleAvoidanceMove = CalculateAvoidance(agent, obstacleList, obstacleSquareAvoidanceRadius);
        Vector3 predatorAvoidanceMove = CalculateAvoidance(agent, predatorList, predatorSquareAvoidanceRadius);

        cohesionMovement.Normalize();
        avoidanceMovement.Normalize();
        smoothSteering.Normalize();

        move += alignmentMovement * alignmentPriority;
        move += cohesionMovement * cohesionPriority;
        move += avoidanceMovement * avoidancePriority;
        move += obstacleAvoidanceMove * obstaclePriority;
        move += predatorAvoidanceMove * predatorPriority;

        move += stayInRadiusMovement;

        move.Normalize();
        move += smoothSteering * steeringPriority;

        return move;
    }

    // Alignment behaviour, set average direction of movement for the individual flocks
    private Vector3 CalculateAlignment(FlockAgent agent, List<Transform> neighbours)
    {
        // if there is no neighbours, maintain current alignment
        if (neighbours.Count == 0)
            return agent.transform.forward;

        // Finding an average point between all of the boids
        // add all points together and average
        Vector3 alignmentMove = Vector3.zero;
        foreach (Transform neigbour in neighbours)
        {
            alignmentMove += (Vector3)neigbour.transform.forward;
        }
        alignmentMove /= neighbours.Count;

        return alignmentMove;
    }

    private Vector3 CalculateAvoidance(FlockAgent agent, List<Transform> neighbours, float squareAvoidanceRadiusValue)
    {
        // if there is no neighbours, return no adjusment 
        if (neighbours.Count == 0)
            return Vector3.zero;

        // Finding an average point between all of the boids
        // add all points together and average
        Vector3 avoidanceMove = Vector3.zero;
        int nAvoid = 0;
        foreach (Transform neighbour in neighbours)
        {
            if (Vector3.SqrMagnitude(neighbour.position - agent.transform.position) < squareAvoidanceRadiusValue)
            {
                nAvoid++;
                avoidanceMove += (Vector3)(agent.transform.position - neighbour.position);
            }
        }
        if (nAvoid > 0)
        {
            avoidanceMove /= nAvoid;
        }

        return avoidanceMove;
    }

    public Vector3 CalculateCohesion(FlockAgent agent, List<Transform> context)
    {
        // if there is no neighbours, return no adjusment 
        if (context.Count == 0)
            return Vector3.zero;

        // Finding an average point between all of the boids
        // add all points together and average
        Vector3 cohesionMove = Vector3.zero; // declare vector zero
        foreach (Transform item in context)
        {
            cohesionMove += (Vector3)item.position;
        }
        cohesionMove /= context.Count;

        // create offset from agent position
        cohesionMove -= (Vector3)agent.transform.position;
        return cohesionMove;
    }


    public Vector3 CalculateSmoothSteer(FlockAgent agent, List<Transform> context)
    {
        // if there is no neighbours, return no adjusment 
        if (context.Count == 0)
            return Vector3.zero;

        // Finding an average point between all of the boids
        // add all points together and average
        Vector3 cohesionMove = Vector3.zero;
        foreach (Transform item in context)
        {
            cohesionMove += (Vector3)item.position;
        }
        cohesionMove /= context.Count;

        // create offset from agent position
        cohesionMove -= (Vector3)agent.transform.position;
        cohesionMove = Vector3.SmoothDamp(agent.transform.forward, cohesionMove, ref currentVelocity, agentSmoothTime);
        return cohesionMove;
    }

    public Vector3 CalculateStayInRadius(FlockAgent agent, List<Transform> context)
    {
        Vector3 centerOffset = transform.position - (Vector3)agent.transform.position;
        float t = centerOffset.magnitude / flockingAreaRadius;
        if (t < 0.9f)
        {
            return Vector3.zero;
        }
        return centerOffset * t * t;
    }

    public void RemoveAgentFromList(FlockAgent agent)
    {
        flockAgentList.Remove(agent);
    }
}
