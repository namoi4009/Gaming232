using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public void RestartGame() // Handle Restart Button
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("Stage");
    }
    
    public void BackToMenu() // Handle Menu Button
    {
        SceneManager.LoadScene("Main Menu");
    }
}
