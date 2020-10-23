using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    [Header("Asset References: ")]
    [SerializeField] private FlockAgent agentPrefab;
    [SerializeField] private FlockBehaviour behaviour;

    [Header("Main: ")]
    [SerializeField] private int startingCount = 250;
    [SerializeField] private const float agentDensity = 0.08f; //Wrm dit?

    [Header("Starting Position: ")]
    [SerializeField] private bool lockStartX;
    [SerializeField] private bool lockStartY;
    [SerializeField] private bool lockStartZ;
    [SerializeField] private float startX;
    [SerializeField] private float startY;
    [SerializeField] private float startZ;

    [Header("Speed: ")]
    [SerializeField, Range(1f, 100f)] private float driveFactor = 10f;
    [SerializeField, Range(1f, 100f)] private float maxSpeed = 5f;

    [Header("Radii: ")]
    [SerializeField] private float neighborRadius = 1.5f;
    [SerializeField, Range(0f, 1f)] private float avoidanceRadiusMultiplier = 0.5f;

    private float squareMaxSpeed;
    private float squareNeighborRadius;
    public float SquareAvoidanceRadius { get; private set; }

    private List<FlockAgent> agents = new List<FlockAgent>();

    private void Start()
    {
        squareMaxSpeed = maxSpeed * maxSpeed;
        squareNeighborRadius = neighborRadius * neighborRadius;
        SquareAvoidanceRadius = squareNeighborRadius * avoidanceRadiusMultiplier * avoidanceRadiusMultiplier;

        for (int i = 0; i < startingCount; i++)
        {
            //Determine starting position
            Vector3 _startPos = new Vector3(startX, startY, startZ);
            _startPos = _startPos + Random.insideUnitSphere * startingCount * agentDensity;
            if (lockStartX) { _startPos.x = startX; }
            if (lockStartY) { _startPos.y = startY; }
            if (lockStartZ) { _startPos.z = startZ; }

            //Instantiate agent
            FlockAgent _newAgent = Instantiate(
                agentPrefab,
                transform.position + _startPos,
                Quaternion.Euler(Vector3Extension.Vector3Random(Vector3.zero, Vector3.one * 360f)),
                transform
                );

            _newAgent.name = $"Agent {i}";
            _newAgent.Initialize(this);
            agents.Add(_newAgent);
        }
    }

    private void Update()
    {
        foreach (FlockAgent _agent in agents)
        {
            List<Transform> _context = GetNearbyObjects(_agent);
            Vector3 _move = behaviour.CalculateMove(_agent, _context, this);

            _move *= driveFactor;
            if (_move.sqrMagnitude > squareMaxSpeed) { _move = _move.normalized * maxSpeed; } //Clamp to max speed

            _agent.Move(_move);

            //Color debug
            //_agent.GetComponentInChildren<Renderer>().material.SetColor("_BaseColor", Color.Lerp(Color.white, Color.red, _context.Count / 6f));
        }
    }

    private List<Transform> GetNearbyObjects(FlockAgent _agent)
    {
        List<Transform> _context = new List<Transform>();
        Collider[] _contextColliders = Physics.OverlapSphere(_agent.transform.position, neighborRadius);

        foreach (Collider _c in _contextColliders)
        {
            if (_c != _agent.AgentCollider) { _context.Add(_c.transform); }
        }

        return _context;
    }
}
