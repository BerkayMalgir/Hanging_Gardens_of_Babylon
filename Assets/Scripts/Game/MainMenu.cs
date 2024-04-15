using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

public class MainMenu : MonoBehaviour
{
    private bool isMuted = false;
    private void Start()
    {
        Time.timeScale = 1;
    }
    public void PlayGame()
    {
        SceneManager.LoadScene("Level");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    public void ToggleSound()
    {
        isMuted = !isMuted;
        AudioListener.volume = isMuted ? 0 : 1; 
        Debug.Log("Sound is " + (isMuted ? "muted" : "unmuted"));
    }
}