using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

//Author Mattias Tronslien, 2018
//mntronslien@gmail.com

[RequireComponent(typeof(AudioSource))]

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

    //Sounds
    [Header("Sounds")]
    public AudioClip[] _punches;
    public AudioClip[] _jumping;


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
    public float _moveDuration = 1;


    //Setting up component references, Awake() is called before Start()
    private void Awake()
    {
        if (_playerNumber == 0)
        {
            Debug.LogError("A script with playerBehaviour attached has not been assigned a player number or the player number is 0!");
        }
        Debug.Log("<b>Controlls:</b> left ctrl (p1) & right ctrl (p2)");
    }

    // Update is called once per frame
    void Update()
    {
        //todo: Remove this debug
        //if (Input.GetButtonDown("Jump"))
        //{
        //    StartCoroutine(Moving(Direction.Backward));
        //}


        gameObject.name = "Player " + _state; //For debugging

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
                //set animation idle
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
                }
                else if (_charge < 1.5)
                {
                    _state = State.Punching;
                    Punch(Attack.Light);
                }
                else if (_charge < 3.5)
                {
                    _state = State.Moving;
                    Moving(Direction.Forward);
                }
                else if (_charge >= 3.5)
                {
                    _state = State.Punching;
                    Punch(Attack.Heavy);
                }

            }
            //TODO determine action based on charge

        }

        _isEnteringState = false; //reset entering state to false

    } //End Update

    private IEnumerator Punch(Attack light)
    {
        throw new NotImplementedException();
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
            _charge += Time.deltaTime;
        }

        //charge reset on state exit and not here
    }


    public State GetState() { return _state; }
}
