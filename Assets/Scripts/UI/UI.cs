using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
//using UnityEditor.Rendering;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreSceneView;
    [SerializeField] private TextMeshProUGUI scorePauseView;
    [SerializeField] private TextMeshProUGUI scoreEndView;
    [SerializeField] private TextMeshProUGUI highscore;
    [SerializeField] private GameObject endPanel;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject PauseButton;
    [SerializeField] private GameObject newHighScorePanel;
    [SerializeField] private GameObject scoreBox;

    private static ScoreUI instance;

    public static ScoreUI Instance {
        get
        {
            if (instance == null)
                instance = new ScoreUI();
            return instance;
        }
    }
    private void Awake()
    {
        instance = this;
    }

    void SetScoreSceneView()
    {
        scoreSceneView.text = "Score " + ScoreManager.Instance.Highscore.Score.ToString();
    }

    private void Update()
    {
        SetScoreSceneView();
    }

    public void ViewEndingPanel()
    {
        newHighScorePanel.SetActive(false);
        scoreBox.SetActive(false);
        PauseButton.SetActive(false);
        endPanel.SetActive(true);
        scoreEndView.text = scoreSceneView.text;
        highscore.text = ScoreManager.Instance.Highscore.HighestScore.ToString();
    }

    public void ViewPausingPanel()
    {
        pausePanel.SetActive(true);
        scorePauseView.text = scoreSceneView.text;
        Time.timeScale = 0f;
    }

    public void ExitPausingPanel()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1.0f;
    }

    public void ViewHigherScorePanel() // when player crash car at a highscore
    {
        newHighScorePanel.SetActive(true);
    }
}
