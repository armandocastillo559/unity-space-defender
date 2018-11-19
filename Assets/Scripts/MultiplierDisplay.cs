using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MultiplierDisplay : MonoBehaviour {

    Text multiplierText;
    GameSession gameSession;

	// Use this for initialization
	void Start ()
    {
        multiplierText = GetComponent<Text>();
        gameSession = FindObjectOfType<GameSession>();
    }
	
	// Update is called once per frame
	void Update () {
        multiplierText.text = gameSession.GetMultiplier().ToString() + "x";	
	}
}
