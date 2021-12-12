using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* This is the CONTEXT, where instances of concrete states are handled. It passes data to the currently active concrete state or states in the hierarchical machine.
  Some data that may pass to the concrete states from here, will also determine when a concrete state should switch to another. It is also the main body of our player object */
public class PlayerStateMachine : MonoBehaviour
{
    [SerializeField] Transform _orientation;
    [SerializeField] GameObject _hitBlood;
    [SerializeField] GameObject _hpBar;
    [SerializeField] GameObject _playerGO;
    [SerializeField] GameObject _playerWeapon;
    GameObject _activeWeapon;

    Animator _weaponAnimator;

    BoxCollider _weaponCollider;

    RaycastHit _lean;

    Rigidbody _playerRB;

    Collider _weapon;

    Vector3 _moveDir;
    Vector3 _playerGravity;

    [Header("Keybinds")]
    [SerializeField] KeyCode _jumpKey = KeyCode.Space;
    [SerializeField] KeyCode _reveseKey = KeyCode.S;
    [SerializeField] KeyCode _sprintKey = KeyCode.LeftShift;
    [SerializeField] KeyCode _attackKey = KeyCode.Mouse0;

    [Header("Ground Detection")]
    [SerializeField] LayerMask _groundLayer; // *IMPORTANT* don't forget to add layer to objects where player can walk on
    [SerializeField] Transform _groundPos; // use groundPos object to help check if player touches the ground
    private float _groundDist = 0.1f;
    bool _onGround;
    bool _onLean;

    [Header("Player Stats")]
    public static int _health;
    public static int _moneyCollected = 0;
    public static int _winningStarCollected;
    float _playerHeight = 2f;

    [Header("Movement")]
    public float _moveSpeed;
    public float _normalSpeed = 5f;
    public float _moveSpeedAdjuster;
    public float _airSpeedAdjuster = 0.3f;
    public float _reverseMoveSpeedAdjuster = 2f;
    public float _attackedMoveSpeedAdjuster = 5f;
    public float _normalMoveSpeedAdjuster = 10f;
    public float _sprintSpeed = 8f;
    public float _accelerateAdjuster = 5f;
    float _horizontalMove;
    float _verticalMove;
    bool _doubleJump;
    bool _hasJumped;


    [Header("Drag")]
    public float _normalDrag = 6f;
    public float _airMoveDrag = 2f;

    [Header("Jumping")]
    public float _jumpPower = 15f;

    // Battle Mode
    bool _isAttacked = false;
    bool _getHit = false;
    float _timeNotAttacked = 0;

    [Header("Audio")]
    [SerializeField] AudioSource getHitSound;
    [SerializeField] AudioSource swingSound;
    [SerializeField] AudioSource deathSound;
    [SerializeField] AudioSource walkSound;
    [SerializeField] AudioSource runSound;

    // state variables
    PlayerBaseState _currentState;
    PlayerStateFactory _states;

    // getters and setters 
    public PlayerBaseState CurrentState { get { return _currentState; } set { _currentState = value; } }
    public Rigidbody PlayerRB { get { return _playerRB; } }
    public GameObject PlayerGO { get { return _playerGO; } }
    public GameObject HpBar { get { return _hpBar; } }
    public GameObject HitBlood { get { return _hitBlood; } }
    public GameObject ActiveWeapon { get { return _activeWeapon; } set { _activeWeapon = value; } }
    public Vector3 MoveDirection { get { return _moveDir; } }
    public Vector3 PlayerGravity { get { return _playerGravity; } }
    public Collider Weapon { get { return _weapon; } }
    public Animator WeaponAnimator { get { return _weaponAnimator; } }
    public float JumpPower { get { return _jumpPower; } }
    public float MoveSpeed { get { return _moveSpeed; } set { _moveSpeed = value; } }
    public float MoveSpeedAdjuster { get { return _moveSpeedAdjuster; } set { _moveSpeedAdjuster = value; } }
    public float AirSpeedAdjuster { get { return _airSpeedAdjuster; } }
    public float AttackedSpeedAdjuster { get { return _attackedMoveSpeedAdjuster; } }
    public float NormalSpeedAdjuster { get { return _normalMoveSpeedAdjuster; } }
    public float AirMoveDrag { get { return _airMoveDrag; } }
    public float NormalSpeed { get { return _normalSpeed; } }
    public float SprintSpeed { get { return _sprintSpeed; } }
    public float AccelerateAdjuster { get { return _accelerateAdjuster; } }
    public float NormalDrag { get { return _normalDrag; } }
    public float TimeNotAttacked { get { return _timeNotAttacked; } set { _timeNotAttacked = value; } }
    public int Health { get { return _health; } set { _health = value; } }
    public bool DoubleJump { get { return _doubleJump; } set { _doubleJump = value; } }
    public bool HasJumped { get { return _hasJumped; } set { _hasJumped = value; } }
    public bool PlayerMoves { get { return (_verticalMove != 0 || _horizontalMove != 0); } }
    public bool PlayerSprint { get { return Input.GetKey(_sprintKey); } }
    public bool PlayerReverse { get { return Input.GetKey(_reveseKey); } }
    public bool Attack { get { return Input.GetKey(_attackKey); } }
    public bool OnLean { get { return _onLean; } }
    public bool OnGround { get { return _onGround; } }
    public bool IsJumpPressed { get { return Input.GetKeyDown(_jumpKey); } }
    public bool IsAttacked { get { return _isAttacked; } set { _isAttacked = value; } }
    public bool GetHit { get { return _getHit; } set { _getHit = value; } }
    public AudioSource WalkSound { get { return walkSound; } }
    public AudioSource RunSound { get { return runSound; } }


    void Awake()
    {
        _playerRB = GetComponent<Rigidbody>(); // get player's rigidbody

        _playerGravity = -29 * Vector3.up; // gravity for player when in the air

        _activeWeapon = _playerWeapon.transform.GetChild(0).gameObject; // get player's weapon
        _weaponAnimator = _activeWeapon.GetComponent<Animator>(); // get weapon's animator
        _weaponCollider = _activeWeapon.GetComponent<BoxCollider>(); // get weapon's collider
        _weaponCollider.enabled = false; // desable weapon's animator

        // setup state
        _states = new PlayerStateFactory(this);
        _currentState = _states.Grounded(); // start on grounded state
        _currentState.EnterState(); // enter state
    }

    void Start()
    {
        _health = Levels.maxHealth;
        _hpBar.GetComponent<HPBar>().MaxHealth(_health); // set hp bar to playres health
    }

    void Update()
    {
        // if attack key is pressed and game is not paused player attacks
        if (Input.GetKeyDown(_attackKey) && !Levels.GamePaused)
        {
            swingSound.Play();
            _weaponAnimator.SetTrigger("Attack");
        }

        _verticalMove = Input.GetAxisRaw("Vertical"); // returns -1 when press "S" or "down-arrow" keys and returns 1 when press "W" or "up-arrow" key
        _horizontalMove = Input.GetAxisRaw("Horizontal"); // returns -1 when press "A" or "left-arrow" keys and returns 1 when press "D" or "up-arrow" key

        // returns true if there are any collidrers overlapping the sphere defined by groundPos's position, radius equal to groundDistance and using as layermask groundObj
        _onGround = Physics.CheckSphere(_groundPos.position, _groundDist, _groundLayer); // check if player walks on the ground or is in the air

        /* | Player Walk on Leaned Ground | */
        if (Physics.Raycast(transform.position, Vector3.down, out _lean, _playerHeight / 2))  // if player hits object with position equal to player's position, Vector3.down for the direction (pointing on ground) and max distance of half playerHeight
        {
            // if the normal of an object is not pointing straight up, then it leans in a direction
            if (_lean.normal != Vector3.up)
            {
                _onLean = true; // so return true that it is leaning
            }
            else
            {
                _onLean = false; // else return false that the object is not leaning
            }
        }

        if (_onLean && _onGround)
        {
            _moveDir = Vector3.ProjectOnPlane(_moveDir, _lean.normal); // projects a Vector3 defined by a normal that is perpendicular to a surface
        }
        else
        {
            _moveDir = _orientation.forward * _verticalMove + _orientation.right * _horizontalMove; // direction relative to where the player's object looking
        }


        // if player is not touching the ground, has jumped
        if (!_onGround)
        {
            _hasJumped = true;
        }

        // if player's goign backwards, lower move speed adjuster so it moves slower
        if (Input.GetKey(_reveseKey))
        {
            _moveSpeedAdjuster = _reverseMoveSpeedAdjuster;
        }

        _currentState.UpdateStates(); // call current's states update (keep this in the last line of Update() to make sure it gets all the variables that are being initialized inside here)

        //if player falls from stage
        if (transform.position.y < -20)
        {
            _health = 0;
        }

        if(_health <= 0)
        {
            deathSound.Play();
        }
    }

    void FixedUpdate()
    {
        _currentState.FixedUpdateStates(); // call current's states fixed update (keep this in the last line of FixedUpdate() to make sure it gets all the variables that are being initialized inside here)
    }

    IEnumerator OnCollisionEnter(Collision collision)
    {
        // if enemy collides with sword
        if (collision.gameObject.CompareTag("Enemy Weapon"))
        {
            getHitSound.Play();
            _weapon = collision.collider.GetComponent<Collider>(); // get collider object

            _isAttacked = true;
            _getHit = true;

            // blood hit effect when player is hit
            _hitBlood.SetActive(true);
            yield return new WaitForSeconds(0.2f);
            _hitBlood.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // if player tirggers heart, get 20 hp, but make sure it doesn't exceed player's max health
        if (other.gameObject.CompareTag("Heart"))
        {
            if( _health + 20 > Levels.maxHealth)
            {
                _health = Levels.maxHealth;
            }
            else
            {
                _health += 20;
            }
            _hpBar.GetComponent<HPBar>().AdjustHealth(_health); // adjust hp on hp bar
        }
        else if (other.gameObject.CompareTag("Diamond"))
        {
            _moneyCollected += 10;
        }
        else if (other.gameObject.CompareTag("Star"))
        {
            _winningStarCollected += 1;
        }
    }
}
