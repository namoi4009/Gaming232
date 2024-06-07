using System.Collections;
using System.Collections.Generic;
using Unity.Profiling;
using Unity.VisualScripting;
using UnityEngine;

public class PauseGame : MonoBehaviour
{
    [SerializeField] CarHandler carHandler;

    private bool isPaused = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyUp(KeyCode.P))
        {
            if (isPaused)
            {
                isPaused = false;
                Resume();
            }
            else
            {
                isPaused = true;
                Pause();
            }
        }

        if (Input.GetKeyDown(KeyCode.Q) && isPaused)
        {
            GoToEnding();
        }
    }

    public void Pause()
    {
        isPaused = true;
        ScoreUI.Instance.ViewPausingPanel();
    }

    public void Resume()
    {
        isPaused = false;
        ScoreUI.Instance.ExitPausingPanel();
    }

    public void GoToEnding()
    {
        Resume();
        carHandler.SetForceToExplode();
    }
}
