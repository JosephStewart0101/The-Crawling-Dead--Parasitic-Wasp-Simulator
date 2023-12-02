using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ScoreTextUpdater : MonoBehaviour
{
    public ScoreInt score;

    [SerializeField]
    private GameObject scoreTextObject;
    [SerializeField]
    private TextMeshProUGUI scoreText;

    public Action OnScoreUpdate;

    void Start()
    {
        score.score = 0;    // Reset the score -- TO-DO: Migrate into CaterpillarGameManager
        OnScoreUpdate += UpdateScore;
        //scoreText = scoreTextObject.GetComponent<TextMeshProUGUI>();
    }

    public void UpdateScore()
    {
        //Debug.LogError("Updating score text");
        scoreText.text = "Score: " + score.score.ToString();
    }
}
