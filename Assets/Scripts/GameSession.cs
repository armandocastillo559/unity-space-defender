using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSession : MonoBehaviour {

    int score = 0;
    int multiplier = 1;

    // Use this for initialization

    private void Awake()
    {
        SetUpSingleton();
    }

    private void SetUpSingleton()
    {
        int numberGameSessions = FindObjectsOfType<GameSession>().Length;
        if (numberGameSessions > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    public int GetScore()
    {
        return score;
    }

    public int GetMultiplier()
    {
        return multiplier;
    }

    public void SetMultiplier(int multiplierValue)
    {
        multiplier = multiplier * multiplierValue;
    }

    public void AddToScore(int scoreValue)
    {
        score += scoreValue * multiplier;
    }

    public void ResetGame()
    {
        Destroy(gameObject);
    }
}

