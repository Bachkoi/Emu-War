using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreTracker : MonoBehaviour
{
    public float score;

    private void Start()
    {
        score = 0;
    }

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }
}
