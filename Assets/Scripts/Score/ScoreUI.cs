using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreSceneView;
    [SerializeField] private TextMeshProUGUI scorePauseView;
    [SerializeField] private TextMeshProUGUI highscore;
    [SerializeField] private GameObject panel;

    private static ScoreUI instance;

    public static ScoreUI Instance { get => instance;  }
    private void Awake()
    {
        instance = this;
    }

    void SetScoreSceneView()
    {
        scoreSceneView.text = ScoreManager.Instance.Highscore.Score.ToString();
    }

    private void Update()
    {
        SetScoreSceneView();
    }

    public void ViewPanel()
    {
        panel.SetActive(true);
        scorePauseView.text = scoreSceneView.text;
        highscore.text = ScoreManager.Instance.Highscore.HighestScore.ToString();
    }
}
