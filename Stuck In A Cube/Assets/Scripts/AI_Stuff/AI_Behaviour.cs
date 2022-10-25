using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI_Behaviour : MonoBehaviour
{

    public Transform _playerPosition;

    public Transform _enemyFolder;

    public NavMeshAgent _AI;

    float x;
    float z;

    [SerializeField]
    Vector3 _destination;

    public float _AI_Speed;




    public bool Stuck = false;

    public Transform Rescue_Class;

    private Renderer Rend;

    public Material Stuck_mat;

    private Material unStuck_mat;



    private Vector3 StuckPosition;
    private int Queue;

    public bool _doorsOpen = false;


    public Collider _d1hitbox;
    public Collider _d2hitbox;

    bool _escaped = false;

    public Collider _pointOrb;

    public GameObject _gameManager;

    //------------methods----------------------------------------------


    private void Start()
    {
        _destination = transform.position;//validation so that the update method doesn't break from the start
        Rend = GetComponent<Renderer>();
        unStuck_mat = Rend.material;

        Physics.IgnoreCollision(this.transform.GetComponent<Collider>(), _d1hitbox);//Allows the AI's rigid body to pass through the doors colliders
        Physics.IgnoreCollision(this.transform.GetComponent<Collider>(), _d2hitbox);
        Physics.IgnoreCollision(this.transform.GetComponent<Collider>(), _pointOrb);
    }


    void Update()
    {
        AI_DecisionMaking();        
        UpdateObstacle();
        if (Stuck == true)
        {
            transform.position = StuckPosition;
        }
    }

    void AI_DecisionMaking()
    {
        if (_escaped == false)
        {
            AI_Rescue RescueAccess = Rescue_Class.transform.GetComponent<AI_Rescue>();
            Vector3 currentTarget = transform.position;
            if (RescueAccess.StuckAI == true && Stuck == false)
            {
                currentTarget = RescueAccess.Positions[0];
                if (Vector3.Distance(transform.position, currentTarget) >= Vector3.Distance(transform.position, RescueAccess.Positions[1]))//is pos0 farther away from me than pos1?
                {
                    currentTarget = RescueAccess.Positions[1];//if yes then pos 1 is my new target
                    if (Vector3.Distance(transform.position, currentTarget) >= Vector3.Distance(transform.position, RescueAccess.Positions[2]))//is pos 1 farther away from me than pos2?
                    {
                        currentTarget = RescueAccess.Positions[2];//if yes then pos2 is my new target, otherwise pos1 stays my target.
                    }
                }
                else//if no then is pos0 farther away from me than pos2?
                {
                    if (Vector3.Distance(transform.position, currentTarget) >= Vector3.Distance(transform.position, RescueAccess.Positions[2]))
                    {
                        currentTarget = RescueAccess.Positions[2];//if yes then pos 2 is my new target
                    }
                }//otherwise nothing happens so pos0 stays my target
                if (Vector3.Distance(_playerPosition.position, currentTarget) >= 3f)//Stops AI from going to help their ally if the player is nearby
                {
                    _destination = currentTarget;
                    _AI.SetDestination(currentTarget);
                }
                else
                {
                    AI_Walk_Init();//initiates random walking if the player is too close to the AI's target ally
                }
            }
            else
            {
                AI_Walk_Init();//initiates walking when the AI has no target allies to help
            }
        }
        else
        {
            _AI.SetDestination(new Vector3(45f, 2.3f, 45f));//sends AI to corner of world if they have already escaped outside of the map
        }
    }
    
    void AI_Walk_Init()//manages random movement depending on different factors within the game
    {
        if (_AI.pathPending == true && _doorsOpen == false)
        {
            AI_Walk();//If the AI gets all of their quickest paths to a point blocked, then give up and look for a new point
        }

        else if (_doorsOpen == true)
        {
            _AI.SetDestination(new Vector3(9f, 2.3f, 16f));//If the doors are open, the AI will try to escape instead of running around randomly
        }

        else if (Vector3.Distance(transform.position, _destination) <= 4f || Vector3.Distance(transform.position, _destination) >= 30f || Vector3.Distance(_playerPosition.transform.position, _destination) <= 3.5)
        {
            AI_Walk();//to stop AI getting a new destination every frame and make their destinations not too far, lowering  the chance of their paths being blocked
        }
    }

    void AI_Walk()
    {
        RandomizeDestination();
        _destination = new Vector3(x, 2.3f, z);
        _AI.SetDestination(_destination);
    }

    void RandomizeDestination()//randomizes x and z co-ordinates within map boundaries
    {
        x = Random.Range(-20f, 20f);
        if (x >= 8f)
        {
            z = Random.Range(-15f, 0f);//bottom right
        }
        else if (x >= -9)
        {
            z = Random.Range(-15f, 15f);//middle
        }
        else
        {
            z = Random.Range(-2f, 10f);//top left
        }
    }


    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (Stuck == false)
            {
                _AI.speed = 0f;
                Stuck = true;
                AI_Rescue RescueAccess = Rescue_Class.transform.GetComponent<AI_Rescue>();
                Queue = RescueAccess.Queue;
                StuckPosition = transform.position;
                RescueAccess.UpdatePositions(StuckPosition, true, Queue);
                Rend.material = Stuck_mat;
            }
        }
        else if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Enemy1") || collision.gameObject.CompareTag("Enemy2") || collision.gameObject.CompareTag("Enemy3"))
        {

            if (Stuck == true)
            {
                AI_Rescue RescueAccess = Rescue_Class.transform.GetComponent<AI_Rescue>();
                RescueAccess.UpdatePositions(StuckPosition, false, Queue);
            }
            Stuck = false;
            _AI.speed = _AI_Speed;
            Rend.material = unStuck_mat;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("EscapePlate"))
        {
            _escaped = true;
            WinConditions conditionUpdate = _gameManager.GetComponent<WinConditions>();
            conditionUpdate._conditionCheck += 1;
        }
        if (collision.gameObject.CompareTag("Player") && Stuck == false)
        {
            Abilities _abilities = collision.transform.GetComponent<Abilities>();
            if (_abilities._abilityActive == false)
            {
                _abilities._abilityPoints += 1;//gives player a point towards ability if they catch an AI and their ability isn't already being used
            }
            WinConditions conditionUpdate = _gameManager.GetComponent<WinConditions>();
            conditionUpdate._timeStop = false;
            if (_abilities._playersAbility == "Time Stop")
            {
                _abilities._abilityActive = false;
            }
            conditionUpdate._totalBeansCaught += 1;
        }
    }


    private void UpdateObstacle()
    {
        string _id = gameObject.tag;//I set each Ai there own tag so that I can differentiate between them in code
        float _distanceToPlayer = Vector3.Distance(_playerPosition.transform.position, transform.position);
        PlayerObstacleRadius PlayerObstacle = _enemyFolder.transform.GetComponent<PlayerObstacleRadius>();
        PlayerObstacle.DistanceStore(_distanceToPlayer, _id);
    }

    public void InDangerZone()
    {
        if (Stuck == false)
        {
            _AI.speed = 1f;
        }
    }

    public void OutsideDangerZone()
    {
        if (Stuck == false)
        {
            _AI.speed = _AI_Speed;
        }
    }
}
