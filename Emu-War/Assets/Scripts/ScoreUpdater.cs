using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ScoreUpdater : MonoBehaviour
{
    private GameObject _scoreTracker;
    [SerializeReference] private TextMeshProUGUI _scoreText;

    // Start is called before the first frame update
    void Start()
    {
        _scoreTracker = GameObject.FindGameObjectWithTag("Score");
        _scoreText.text = $"SCORE: {_scoreTracker.GetComponent<ScoreTracker>().score}";
    }
}
