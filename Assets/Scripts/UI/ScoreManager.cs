using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class ScoreManager : MonoBehaviour
{
    private static ScoreManager instance;

    [SerializeField] private Score_SO highscore;
    public static ScoreManager Instance { get => instance; }
    public Score_SO Highscore { get => highscore; }

    private void Awake() // Initiate highest score at the beginning = 0
    {
        instance = this;
        highscore.Score = 0;
    }

    public void SetHighestScore()
    {
        if (highscore.Score < highscore.HighestScore) return;
        highscore.HighestScore = highscore.Score;
    }

    public void ShowScore() // For debugging
    {
        Debug.Log($"Score: {highscore.Score}");
    }

    private void OnTriggerEnter(Collider other) // If the car passing other car, point increases
    {
        // if (other.name.Equals("car_hatchback"))
        if (other.CompareTag("CarAI"))
        {
            highscore.Score++;
            ShowScore();
            SetHighestScore();
        }
    }
}
