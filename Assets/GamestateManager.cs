using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamestateManager : MonoBehaviour
{

    public int _player1Health = 0;
    public int _player2SHealth = 0;

    Vector3 _p1StartPos;
    Vector3 _p2StartPos;

    GameObject _p1;
    GameObject _p2;

    GameObject _healthTray;
    public GameObject _healthTrayPrefab;
    public GameObject _healthDots;
    public int _maxHealth;

    

    public enum Player
    {
        player1, player2
    }



    // Use this for initialization
    void Start()
    {
        _p1 = GameObject.Find("Player 1");
        _p2 = GameObject.Find("Player 2");

        _p1StartPos = _p1.transform.position;
        _p2StartPos = _p2.transform.position;


        InitializeGame();


    }

    // Update is called once per frame
    void Update()
    {
        //TODO: check for end game
    }

    private void InitializeGame()
    {
        //Generate Health dots

        //place players at location
        _p1.transform.position = _p1StartPos;
        _p2.transform.position = _p2StartPos;

        //Initialize health dots
        if (_healthTray != null)
        {
            Destroy(_healthTray);
        }
        _healthTray = GameObject.Instantiate(_healthTrayPrefab, GameObject.Find("Spacer").transform);
        foreach (Transform child in _healthTray.transform)
        {
            for (int i = 0; i < _maxHealth; i++)
            {
                GameObject thing = GameObject.Instantiate(_healthDots, child);
            }
        }
        _player1Health = _maxHealth;
        _player2SHealth = _maxHealth;
    }
    /// <summary>
    /// Does not support anything else that -1 as a damage value at the moment
    /// </summary>
    /// <param name="player">What player needs theier health adjusted?</param>
    /// <param name="amount">Amount is relative to current health</param>
    public void UpdateHealth(Player player, int amount)
    {
        if (player == Player.player1)
        {
            _player1Health += amount;
            Destroy(_healthTray.transform.GetChild(0).GetChild(0).gameObject);
        }
        else
        {
            _player2SHealth += amount;
            Destroy(_healthTray.transform.GetChild(1).GetChild(0).gameObject);
        }
    }
}
