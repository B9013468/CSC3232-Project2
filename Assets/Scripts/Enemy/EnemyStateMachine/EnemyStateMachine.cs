using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/* This is the CONTEXT, where instances of concrete states are handled. It passes data to the currently active concrete state or states in the hierarchical machine.
  Some data that may pass to the concrete states from here, will also determine when a concrete state should switch to another. It is also the main body of our player object */
public class EnemyStateMachine : MonoBehaviour
{
    Rigidbody _enemy;

    Material _defaultMaterial;

    Vector3 _distanceLeft;
    Vector3 _directionOfHit;
    Vector3 _finalDirectionPoint;

    Collider _playerWeapon;

    [SerializeField] Transform _weapon;

    [SerializeField] GameObject _hpBar;
    [SerializeField] GameObject _enemyGO;
    [SerializeField] GameObject _lootItems;
    [SerializeField] GameObject _lootSpawnPosition;
    GameObject _instancedObj;
    Vector3 _randomPosition;

    [Header("Enemy Stats")]
    public int _health = 100;

    [Header("State Layers and Objects")]
    public LayerMask _playerLayer; // set it to Player's layer in unity
    public LayerMask _obstructionLayers; // set it to all layers except Player's layer in unity
    public Transform _player;
    [SerializeField] NavMeshAgent _agent;

    [Header("Battle Mode")]
    public float _attackSpeed = 2f; // time passed for next attack
    public float _attackRange = 1f;
    public float _chaseDistance = 70f;
    public float _chaseSpeed = 6;
    bool _attackMode;
    bool _attackPlayer;
    bool _isHit;
    bool _knockBack;
    Vector3 _positionWhereHit;
    float _attackTimer;
    public static bool _inBattle = false;

    [Header("Loot")]
    public int _numberOfLoot;
    public Dictionary<int, int> _lootPropability = new Dictionary<int, int>() // *IMPORTANT* (When creating the propability dictionary, for the method to work, always put lower propabilities first)
        {
            {99, 3}, // if the random generated number is higher or equal to 99, enemy will drop 3 diamonds
            {50, 2}, // if the random generated number is higher or equal to 50, enemy will drop 2 diamonds
            {5, 1} // if the random generated number is higher or equal to 5, enemy will drop 1 diamonds
        };

    [Header("Walk Mode")]
    public float _destinationPointRange = 10f;
    bool _destinationPointStated = false;
    public float _walkSpeed = 1;
    public string _pathTagName;

    [Header("Enemy View")]
    public float _viewRange = 15f;
    [Range(0,360)]
    public float _viewAngle;
    bool _playerInSpottedRange;
    public bool _canSeePlayer;

    // state variables
    EnemyBaseState _currentState;
    EnemyStateFactory _states;

    [Header("Audio")]
    [SerializeField] AudioSource swingSound;
    [SerializeField] AudioSource getHitSound;
    [SerializeField] AudioSource deathSound;

    // getters and setters 
    public EnemyBaseState CurrentState { get { return _currentState; } set { _currentState = value; } }
    public Vector3 DistanceLeft { get { return _distanceLeft; } set { _distanceLeft = value; } }
    public Vector3 DirectionOfHit { get { return _directionOfHit; } set { _directionOfHit = value; } }
    public Vector3 FinalDirectionPoint { get { return _finalDirectionPoint; } set { _finalDirectionPoint = value; } }
    public Vector3 PositionWhereHit { get { return _positionWhereHit; } set { _positionWhereHit = value; } }
    public Rigidbody Enemy { get { return _enemy; } }
    public Transform Weapon { get { return _weapon; } }
    public Transform Player { get { return _player; } }
    public GameObject HpBar { get { return _hpBar; } }
    public GameObject InstancedObj { get { return _instancedObj; } }
    public GameObject EnemyGO { get { return _enemyGO; } }
    public GameObject LootItems { get { return _lootItems; } }
    public Collider PlayerWeapon { get { return _playerWeapon; } }
    public LayerMask PlayerLayer { get { return _playerLayer; } }
    public LayerMask ObstructionLayers { get { return _obstructionLayers; } }
    public NavMeshAgent Agent { get { return _agent; } }
    public Material DefaultMaterial { get { return _defaultMaterial; } }
    public int Health { get { return _health; } set { _health = value; } }
    public float AttackSpeed { get { return _attackSpeed; } }
    public float AttackRange { get { return _attackRange; } }
    public float ViewRange { get { return _viewRange; } }
    public float ViewAngle { get { return _viewAngle; } }
    public float WalkSpeed { get { return _walkSpeed; } set { _walkSpeed = value; } }
    public float ChaseSpeed { get { return _chaseSpeed; } set { _chaseSpeed = value; } }
    public float DestinationPointRange { get { return _destinationPointRange; } }
    public float ChaseDistance { get { return _chaseDistance; } }
    public bool DestinationPointStated { get { return _destinationPointStated; } set { _destinationPointStated = value; } }
    public bool AttackMode { get { return _attackMode; } set { _attackMode = value; } }
    public bool AttackPlayer { get { return _attackPlayer; } }
    public bool PlayerInSpottedRange { get { return _playerInSpottedRange; } set { _playerInSpottedRange = value; } }
    public bool CanSeePlayer { get { return _canSeePlayer; } set { _canSeePlayer = value; } }
    public bool IsHit { get { return _isHit; } set { _isHit = value; } }
    public bool KnockBack { get { return _knockBack; } set { _knockBack = value; } }
    public bool InBattle { get { return _inBattle; } set { _inBattle = value; } }
    public AudioSource SwingSound { get { return swingSound; } }
    public AudioSource DeathSound { get { return deathSound; } }

    void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _destinationPointStated = false;
        _enemy = gameObject.GetComponent<Rigidbody>();

        // setup state
        _states = new EnemyStateFactory(this);
        _currentState = _states.OutOfBattle(); // start on grounded state
        _currentState.EnterState(); // enter state
    }

    void Start()
    {
        _defaultMaterial = GetComponent<Renderer>().material;
        _defaultMaterial.color = Color.black;
        _hpBar.GetComponent<HPBar>().MaxHealth(_health); // set health to hp bar
    }

    void Update()
    {   
       _playerInSpottedRange = Physics.CheckSphere(transform.position, _viewRange, _playerLayer); // Check if player is on spotted range
        _attackPlayer = Physics.CheckSphere(transform.position, _attackRange, _playerLayer); // Check if player is on attack range

        _distanceLeft = _enemy.transform.position - _finalDirectionPoint; // distance between agent and last destination point

        // time how frequently the attacks are going to occur
        if (!_attackMode && _attackPlayer)
        {
            _attackTimer += Time.deltaTime;

            //if timer is higher than the attack speed enable attack mode and reset timer
            if (_attackTimer > _attackSpeed)
            {
                _attackMode = true;
                _attackTimer = 0;
            }
        }

        _currentState.UpdateStates(); // call current's states update
    }

    void FixedUpdate()
    {
        _currentState.FixedUpdateStates(); // call current state's fixed update
    }

    IEnumerator OnCollisionEnter(Collision collision)
    {
        // if enemy collides with player's sword
        if (collision.gameObject.CompareTag("Player Weapon"))
        {
            getHitSound.Play();
            _isHit = true;
            _knockBack = true;
            _playerWeapon = collision.collider.GetComponent<Collider>(); // get weapon collider

            _defaultMaterial.color = Color.red; // change enemy's material color to red
            yield return new WaitForSeconds(0.2f); // after 0.2 secs change color back to black
            _defaultMaterial.color = Color.black;

            if(_health <= 0)
            {
                _numberOfLoot = GetPropability();
                SpawnLoot(_numberOfLoot);
                yield return new WaitForSeconds(0.4f);
                gameObject.SetActive(false);
            }
        }
    }

    // Returns a random position for loot to spawn, near last enemy's position
    Vector3 RandomPosition()
    {
        _randomPosition = new Vector3 (_lootSpawnPosition.transform.position.x + Random.Range(-1.0f, 1.0f), _lootSpawnPosition.transform.position.y, _lootSpawnPosition.transform.position.z + Random.Range(-1.0f, 1.0f));
        return _randomPosition;
    }

    // Spawns loot to lootNo times 
    void SpawnLoot(int lootNo)
    {
        for(int i =0; i < lootNo; i++)
        {
            Instantiate(_lootItems, RandomPosition(), _lootItems.transform.rotation);
        }
    }

    // Handles the propability of how much loot the enemy will drop and returns the number of it
    int GetPropability()
    {
        int randomNo = Random.Range(0, 100); // generate a random number between 0 and 100

        // for each keyvalue in dictionary, if the random number is higher than the propability key, return its value
        foreach (KeyValuePair<int, int> propability in _lootPropability)
        {
            if(propability.Key < randomNo)
            {
                return propability.Value;
            }
        }
        return 0; // if there was no number to return, then return 0
    }
}