using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class LevelEvents : MonoBehaviour
{
    
    public void ReplayGame()
    {
        SceneManager.LoadScene("Level");
    }
    
    public void QuitGame()
    {
        SceneManager.LoadScene("Menu");
    }
    
}