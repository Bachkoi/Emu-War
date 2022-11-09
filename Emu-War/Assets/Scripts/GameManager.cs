using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;


public class GameManager : MonoBehaviour
{
    #region Fields
    [SerializeField] private GameObject _playerObject;
    private Player _player;
    public Player player;
    public GameObject playerObject;
    private int _amountOfWheat;
    #endregion

    #region Methods
    private void Start()
    {
        // TODO: Create LevelManager to handle wheat in the level/win conditions
        _amountOfWheat = 2; // temporary assignment for CollisionTest scene

        _player = _playerObject.GetComponent<Player>();
        player = playerObject.GetComponent<Player>();
        player.score = 0.0f;
        player.dTime = 0.0f;
        _player.dTime = 0.0f;
    }

    private void Update()
    {
        player.dTime += Time.deltaTime;
        _player.dTime += Time.deltaTime;
        //Console.WriteLine(player.dTime);
        if (_player.health <= 0)
        {
            _player.isDead = true;
            player.isDead = true;
            _player.score = _player.PlayerScore();
            player.score = player.PlayerScore();
            SceneManager.LoadScene("GameOver");
        }

        if (_player.wheat >= _amountOfWheat)
        {
            _player.isDead = false;
            player.isDead = false;
            _player.score = _player.PlayerScore();
            player.score = player.PlayerScore();
            Console.WriteLine(player.score);
            //SceneManager.LoadScene("GameOver");

            //player.dTime += Time.deltaTime;
            //_player.dTime += Time.deltaTime;
        }
    }
    #endregion
}
