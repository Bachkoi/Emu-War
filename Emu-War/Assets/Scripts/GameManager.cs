using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    #region Fields
    [SerializeField] private GameObject _playerObject;
    private Player _player;
    private int _amountOfWheat;
    #endregion

    #region Methods
    private void Start()
    {
        // TODO: Create LevelManager to handle wheat in the level/win conditions
        _amountOfWheat = 13; // temporary assignment for CollisionTest scene

        _player = _playerObject.GetComponent<Player>();
    }

    private void Update()
    {
        if (_player.health <= 0)
        {
            SceneManager.LoadScene("GameOver");
        }

        if (_player.wheat >= _amountOfWheat)
        {
            SceneManager.LoadScene("GameOver");
        }
    }
    #endregion
}
