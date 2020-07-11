using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreHandler : MonoBehaviour
{
    private int _score;

    // Start is called before the first frame update
    void Start()
    {
        _score = 0;
    }

    public void addScore(int amount)
    {
        _score += amount;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
