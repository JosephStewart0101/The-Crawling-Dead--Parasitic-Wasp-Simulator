using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GhostState
{
    Search,
    Chase,
    Flee
}

public enum MovementDirection
{
    Up,
    Down,
    Left,
    Right
}

public class GhostBehavior : MonoBehaviour
{
    public float baseSpeed = 2.0f;
    public float chaseDuration = 3.0f;

    [SerializeField]
    private GhostState currentState = GhostState.Search;
    [SerializeField]
    private MovementDirection movementDirection = MovementDirection.Up;
    private MovementDirection lastDirection = MovementDirection.Up;

    [SerializeField]
    private Transform playerTransform;
    [SerializeField]
    private Transform ghostTransform;
    
    public bool ghostIsFrozen = true;   // This tells us whether the ghost should be moving right now

    private Vector2 fleeTarget;
    private float fleeSpeedMultiplier = 1.5f;
    private float chaseTimer = 0.0f;

    private bool isTryingToEndChase = false;

    private bool isInRangeOfPlayer = false;

    public bool debugging = false;

    public CaterpillarGameManager caterpillarGameManager;

    [HideInInspector] public GameObject ghostInstance;

    void Start()
    {
        caterpillarGameManager = CaterpillarGameManager.Instance;

        // Subscribe Shutdown to OnCatMiniStateChanged event from CaterpillarGameManager
        caterpillarGameManager.OnCatMiniStateChanged += OnStateChanged;

        // Subscribe FleeManicPlayer to manic/chase event from CaterpillarGameManager
        caterpillarGameManager.OnFrenzyStarted += FleeManicPlayer;
        caterpillarGameManager.OnFrenzyEnded += EndManicEpisode;

        StartCoroutine(Setup());
    }

    private void FixedUpdate()
    {
        if (caterpillarGameManager.isPlayerManic && currentState != GhostState.Flee)
        {
            currentState = GhostState.Flee;
        }
    }

    /// <summary>
    /// Initial setup for each of the ghosts
    /// </summary>
    private IEnumerator Setup()
    {
        yield return new WaitForSeconds(3f);    // TO-DO: Make this variable for difficulty?

        ghostIsFrozen = false;                  // The ghost shouldn't be frozen after countdown

        playerTransform = CaterpillarGameManager.Instance.Player.transform; // Get player transform

        baseSpeed = caterpillarGameManager.ghostSpeed;

        while (playerTransform == null)
        {
            playerTransform = CaterpillarGameManager.Instance.Player.transform;
            Debug.Log("bruh");
            yield return null;
        }
    }

    /// <summary>
    /// Manage ghost reactions to state changes
    /// </summary>
    public void OnStateChanged(object sender, EventArgs e)
    {
        switch (caterpillarGameManager.miniState)
        {
            case CaterpillarGameManager.MiniState.playing:
                ghostIsFrozen = false; 
                break;
            case CaterpillarGameManager.MiniState.paused:
                ghostIsFrozen = true;
                break;
            case CaterpillarGameManager.MiniState.ending:
                ghostIsFrozen = false;
                gameObject.SetActive(false);
                Destroy(gameObject);
                break;
        }
    }

    void Update()
    {
        // Check if the ghost should be frozen
        if (!ghostIsFrozen)
        {
            switch (movementDirection)
            {
                case MovementDirection.Up:
                    transform.Translate(Vector2.up * baseSpeed * Time.deltaTime);
                    break;

                case MovementDirection.Down:
                    transform.Translate(Vector2.down * baseSpeed * Time.deltaTime);
                    break;

                case MovementDirection.Left:
                    transform.Translate(Vector2.left * baseSpeed * Time.deltaTime);
                    break;

                case MovementDirection.Right:
                    transform.Translate(Vector2.right * baseSpeed * Time.deltaTime);
                    break;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //if (debugging)
        //    Debug.LogError("OnTriggerEnter2D :: other == " + other.gameObject);

        // If ghost ran into a node, check to see if it's a decision node
        if (other.gameObject.name.StartsWith("NodeCenter"))
        {
            //if (debugging)
            //    Debug.LogError("GhostBehavior::OnTriggerEnter2D: StartsWith Node");
            NodeState nodeState = other.gameObject.GetComponentInParent<NodeState>();

            if (nodeState.isDecisionNode)
            {
                if (debugging)
                    Debug.LogError("GhostBehavior::OnTriggerEnter2D: isDecisionNode");

                ChangeGhostDirection(nodeState, other);
            }
        }
        else if (other.gameObject.name.StartsWith("GhostBait"))
        {
            if (debugging)
                Debug.LogError("GhostBehavior::OnTriggerEnter2D: Is the player! Chase them!!!");

            isInRangeOfPlayer = true;
            if (!caterpillarGameManager.isPlayerManic)  // verify player isn't manic
                currentState = GhostState.Chase;
        }
        else if (other.gameObject.name.StartsWith("PlayerHitbox"))
        {
            if (!caterpillarGameManager.isPlayerManic)
            {
                if (debugging)
                    Debug.LogError("GhostBehavior::OnTriggerEnter2D: Player needs to die");
                caterpillarGameManager.SetPlayerWinLose(false);
                caterpillarGameManager.RaisePlayerDeathEvent();
            }
            else
            {
                caterpillarGameManager.SpawnNewGhost();
                caterpillarGameManager.GiveGhostKillPoints();
                Destroy(gameObject);
                //RespawnGhost();
            }
        }
    }

    private void RespawnGhost()
    {
        ghostTransform.position = caterpillarGameManager.ghostSpawn.position;
        movementDirection = MovementDirection.Up;
        StartCoroutine(Setup());
        caterpillarGameManager.GiveGhostKillPoints();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.name.StartsWith("GhostBait") && !isTryingToEndChase)
        {
            isInRangeOfPlayer = false;
            StartCoroutine(EndChase());
        }
    }

    IEnumerator EndChase()
    {
        float timer = 5f;

        while (timer > 0)
        {
            if (!isInRangeOfPlayer)
            {
                timer--;
            }
            else
            {
                timer = 5;
            }
            yield return new WaitForSeconds(1f);
        }

        // Chase should now end
        currentState = GhostState.Search;
    }

    /// <summary>
    /// Used to change the ghost's direction when at a decision node
    /// Decides direction based on ghost's current state
    /// </summary>
    public void ChangeGhostDirection(NodeState nodeState, Collider2D other)
    {
        List<MovementDirection> nodesAvailable = new List<MovementDirection>();

        // Check each node in NodeState (TO-DO: Perhaps refine this?)
        // Exclude the previous node, so AI can't do a 180 degree turn
        if (nodeState.nodeUp != null && lastDirection != MovementDirection.Down)
            nodesAvailable.Add(MovementDirection.Up);
        if (nodeState.nodeDown != null && lastDirection != MovementDirection.Up)
            nodesAvailable.Add(MovementDirection.Down);
        if (nodeState.nodeLeft != null && lastDirection != MovementDirection.Right)
            nodesAvailable.Add(MovementDirection.Left);
        if (nodeState.nodeRight != null && lastDirection != MovementDirection.Left)
            nodesAvailable.Add(MovementDirection.Right);

        // Check for ghost state
        switch (currentState)
        {
            // Move in a random direction
            case GhostState.Search:
                movementDirection = RandomNode(nodeState, nodesAvailable);
                break;
            case GhostState.Chase:
                movementDirection = NodeCloserToPlayer(nodeState, nodesAvailable);
                break;
            case GhostState.Flee:
                movementDirection = NodeFarthestFromPlayer(nodeState, nodesAvailable);
                break;
        }


        lastDirection = movementDirection;

        // Decide the direction based on state
        //movementDirection = MovementDirection.Up;

        // Set the location of the ghost to the center of the node, done for housekeeping
        if (playerTransform != null && other != null)
        {
            ghostTransform.position = other.gameObject.transform.position;
        }
    }

    /// <summary>
    /// Calculates and returns the most optimal node for the ghost to chase with
    /// </summary>
    private MovementDirection NodeCloserToPlayer(NodeState nodeState, List<MovementDirection> nodesAvailable)
    {
        if (playerTransform == null)
            return MovementDirection.Up;

        Vector2 playerPosition = playerTransform.position;

        float shortestDistance = float.MaxValue;

        MovementDirection closestDirection = nodesAvailable[0];

        foreach (MovementDirection direction in nodesAvailable)
        {
            // Calculate position of this directional node
            Vector2 nodePosition = CalculateNodePosition(nodeState, direction);

            // Calculate distance between player and node
            float distanceToPlayer = Vector2.Distance(playerPosition, nodePosition);

            if (distanceToPlayer < shortestDistance)
            {
                shortestDistance = distanceToPlayer;
                closestDirection = direction;
            }
        }

        return closestDirection;
    }

    /// <summary>
    /// Calculates and returns the most optimal node for the ghost to flee with
    /// </summary>
    private MovementDirection NodeFarthestFromPlayer(NodeState nodeState, List<MovementDirection> nodesAvailable)
    {
        if (playerTransform == null)
            return MovementDirection.Up;

        Vector2 playerPosition = playerTransform.position;

        float longestDistance = float.MinValue;

        MovementDirection farthestDirection = MovementDirection.Down;
        Debug.LogError("lastDirection = " + lastDirection);

        foreach (MovementDirection direction in nodesAvailable)
        {
            // Calculate position of this directional node
            Vector2 nodePosition = CalculateNodePosition(nodeState, direction);

            // Calculate distance between player and node
            float distanceToPlayer = Vector2.Distance(playerPosition, nodePosition);

            if (distanceToPlayer > longestDistance && direction != lastDirection)
            {
                longestDistance = distanceToPlayer;
                farthestDirection = direction;
            }
        }
        Debug.LogError("NodeFarthestFromPlayer: " + farthestDirection);
        return farthestDirection;
    }

    private Vector2 CalculateNodePosition(NodeState nodeState, MovementDirection direction)
    {
        Vector2 nodePosition = ghostTransform.position; // Start from the ghost's position

        // Calculate the new position based on the chosen direction
        switch (direction)
        {
            case MovementDirection.Up:
                nodePosition += Vector2.up;
                break;
            case MovementDirection.Down:
                nodePosition += Vector2.down;
                break;
            case MovementDirection.Left:
                nodePosition += Vector2.left;
                break;
            case MovementDirection.Right:
                nodePosition += Vector2.right;
                break;
                // Add cases for other directions if needed
        }

        return nodePosition;
    }

    /// <summary>
    /// Used for Search ghostState - selects random available node, excluding previous path
    /// </summary>
    private MovementDirection RandomNode(NodeState nodeState, List<MovementDirection> nodesAvailable)
    {
        int randomIndex = UnityEngine.Random.Range(0,nodesAvailable.Count);

        //Debug.LogError("randomIndex = " + randomIndex);

        return nodesAvailable[randomIndex];
    }

    private void MoveToFleeTarget()
    {
        // Flee from the player by moving towards the 'fleeTarget' (opposite direction).
        Vector2 fleeDirection = ((Vector2)transform.position - fleeTarget).normalized;
        GetComponent<Rigidbody2D>().velocity = fleeDirection * baseSpeed * fleeSpeedMultiplier;
    }

    public void SetState(GhostState newState)
    {
        currentState = newState;

        switch (currentState)
        {
            case GhostState.Search:
                // Optionally reset any specific state setup.
                break;

            case GhostState.Chase:
                chaseTimer = 0.0f;
                break;

            case GhostState.Flee:
                break;
        }
    }

    public void ChangeStateExternally(GhostState newState)
    {
        SetState(newState);
    }

    /// <summary>
    /// Player is manic and we need to avoid them.
    /// </summary>
    private void FleeManicPlayer(object sender, EventArgs e)
    {
        // Verify we should be fleeing just to be sure
        if (!caterpillarGameManager.isPlayerManic)
            return;
        Debug.Log("FleeManicPlayer:Caterpillar is fleeing");
        currentState = GhostState.Flee;
    }

    private void EndManicEpisode(object sender, EventArgs e)
    {
        if (caterpillarGameManager.isPlayerManic)   // Verify second manic hasn't happened
            return;
        Debug.Log("EndManicEpisode:Caterpillar not fleeing");
        // Start a chase but put the timer on
        currentState = GhostState.Chase;
        StartCoroutine(EndChase());
    }
}
