using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public void RestartGame()
    {
        SceneManager.LoadScene("Stage");
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
