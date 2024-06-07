using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScripts : MonoBehaviour
{
    AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private void Start()
    {
        // Play background music
        audioManager.PlaySFX_Loop(audioManager.Menu_Background);
    }

    public void PlayGame()
    {
        if (audioManager != null)
            audioManager.PlaySFX_OneTime(audioManager.MouseClick);
        SceneManager.LoadSceneAsync("Stage");
    }

    public void QuitGame()
    {
        if (audioManager != null)
            audioManager.PlaySFX_OneTime(audioManager.MouseClick);
        #if UNITY_STANDALONE
        Application.Quit();
        #endif  

        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
