using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

//Author Mattias Tronslien, 2018
//mntronslien@gmail.com

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Animator))]

public class PlayerBehaviour : MonoBehaviour
{
    //state machine variables
    public enum State
    {
        Counter, Punching, Moving, Idle, Dead
    }
    private State _state = State.Idle;
    private State _lastState = State.Idle;
    private bool _isEnteringState = false;

    //component refs
    private AudioSource _Speaker;
    private Animator _animator;

    //Sounds
    [Header("Sounds")]
    public AudioClip[] _punches;
    public AudioClip _powerPunch;
    public AudioClip _forwardSound;
    public AudioClip _backwardSound;

    [Header("Movement Tweaking")]
    [Range(0.1f, 0.5f)] public float _idleApproachSpeed = 0.2f;
    [Range(1f, 10f)] public float _chargeMultiplier = 1;


    //other global variables
    public int _playerNumber;
    public float _charge = 0;

    private enum Direction
    {
        Forward, Backward
    }
    private enum Attack
    {
        Light, Heavy //If we get more attacks in the future, like projectile. add them as options under this enum
    }
    public float _moveDuration = 1;         //base value of 1 second
    public float _lightPunchDuration = 1;   //base value of 1 second
    public float _heavyPunchDuration = 1;   //base value of 1 second
    private GameObject _otherPlayer;
    public float _maximumCloseness = 1;



    //Setting up component references, Awake() is called before Start()
    private void Awake()
    {
        if (_playerNumber == 0)
        {
            Debug.LogError("A script with playerBehaviour attached has not been assigned a player number or the player number is 0!");
        }
        if (_otherPlayer == null)
        {
            GameObject.Find("Player " + _playerNumber);
        }
        gameObject.name = "Player " + _playerNumber;

        _animator = GetComponent<Animator>();


        Debug.Log("<b>Controlls:</b> left ctrl (p1) & right ctrl (p2)");
    }

    private void Start()
    {
        if (_playerNumber == 1)
        {
            _otherPlayer = GameObject.Find("Player 2");
        }
        else _otherPlayer = GameObject.Find("Player 1");

        _Speaker = GetComponent<AudioSource>();


}

    // Update is called once per frame
    void Update()
    {


        if (_state != _lastState) //detect state transition
        {
            _isEnteringState = true;
            _lastState = _state;
        }

        //State Machine -------------------------------------------------------------------------------------------
        //If you do not know what a state machine is, the basic concept is 
        //that you organize the behaviour into certain states that decribe what the player (or enmy or whatever) may do.
        //The clue here is that each state is *inclusive* and not exlusive in the options of behaviour.
        //An example of this is that "jumping" is possible both while idle and moving. 
        //Instead of making specialized bahaviour for jumping and idle, we just allow both states to call the same method and transition into jumping state.

        //Idle state
        if (_state == State.Idle)
        {
            //Use _isEnteringState to declare bahaviour that should only be run the first frame while in this state
            //Usefull for for setting variables that is needed for this state, but only needs to be run once
            if (_isEnteringState)
            {
                _charge = 0;
            }
            Charge(); //This metod handles charging attacks, wich the player can do while in idle.
            if (Input.GetButtonUp("Fire" + _playerNumber))
            {
                if (_charge < 0.5)
                {
                    _state = State.Counter;
                }
                else if (_charge < 1)
                {
                    _state = State.Moving;
                    StartCoroutine(Moving(Direction.Backward));

                    _Speaker.pitch = Random.Range(0.75f, 1.2f);
                    _Speaker.PlayOneShot(_backwardSound);
                }
                else if (_charge < 1.5)
                {
                    _state = State.Punching;
                    Punch(Attack.Light);

                    _Speaker.pitch = Random.Range(0.9f, 1.1f);
                    _Speaker.PlayOneShot(_punches[Random.Range(0,_punches.Length)]);
                    //animasjon spilles av her og har en eventcall i animasjon step som resetter til idle state
                }
                else if (_charge < 3.5)
                {
                    _state = State.Moving;
                    Moving(Direction.Forward);
                    _Speaker.pitch = Random.Range(0.75f, 1.2f);
                    _Speaker.PlayOneShot(_forwardSound);
                }
                else if (_charge >= 3.5)
                {
                    _state = State.Punching;
                    Punch(Attack.Heavy);
                    _Speaker.pitch = Random.Range(0.9f, 1.1f);
                    _Speaker.PlayOneShot(_powerPunch);

                    //TODO:PlayPuch animation
                }

            }
            //TODO determine action based on charge

        }

        _isEnteringState = false; //reset entering state to false

    } //End Update

    private void Approach(Direction dir)
    {
        if (_state == State.Idle && Vector3.Distance(transform.position, _otherPlayer.transform.position) > _maximumCloseness)
        {
            if (_playerNumber == 1)
            {
                transform.position = transform.position + (Vector3.right * _idleApproachSpeed);
            }
            else
            {
                transform.position = transform.position + (Vector3.left * _idleApproachSpeed);
            }
        }
    }

    private void Punch(Attack att)
    {
        //play punch animation

        if (att == Attack.Light)
        {
            //TODO: play light attack animation
            _animator.SetTrigger("Punch");
            if (Vector3.Distance(transform.position, _otherPlayer.transform.position) < _maximumCloseness + 1)
            {
                _otherPlayer.GetComponent<PlayerBehaviour>().TakeDamage(); //if we are close enought for the opuch to connect, we tell the other player to take damage.
            }
        }
       
    }

    private IEnumerator Moving(Direction dir)
    {
        Debug.Log("Moving of player " + _playerNumber);
        Vector3 startPos = transform.position;
        if (_playerNumber == 2)
        {   //account for player two facing the other way
            if (dir == Direction.Forward) dir = Direction.Backward;
            else dir = Direction.Forward;
        }
        Vector3 endPos = startPos + (dir == Direction.Forward ? -Vector3.left : Vector3.left);
        for (float i = 0; i < _moveDuration; i += Time.deltaTime) //this should mach the timing of the animation, for now i use a second.
        {
            transform.position = Vector3.Lerp(startPos, endPos, i / _moveDuration);
            yield return null;
        }
        transform.position = endPos;
        _state = State.Idle;

    }


    /// <summary>
    /// Listening for charging and setting the current chage level
    /// </summary>
    private void Charge()
    {
        //detect button down
        //if button down increase charge
        if (Input.GetButton("Fire" + _playerNumber))
        {
            _charge += Time.deltaTime * _chargeMultiplier;
        }

        //charge reset on state exit and not here
    }
    public void TakeDamage()
    {
        _animator.SetTrigger("BeingHit");
    }

    public void SetState(State newstate)
    {
        _state = newstate;
    }

    public State GetState() { return _state; }
}
