using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerManager : MonoBehaviour
{
    public static bool gameOver;
    public GameObject gameOverPanel;
    public static bool isGameStarted;
    public GameObject startingText;

    public static int numberOfCoins;
    public Text coinsText;

    

  

  

    void Start()
    {
        gameOver = false;
        Time.timeScale = 1;
        isGameStarted = false;


    }

    void Update()
    {
        if (gameOver)
        {
            Time.timeScale = 0;
            gameOverPanel.SetActive(true);
        }

        coinsText.text = "" + numberOfCoins;
        if (SwipeManager.tap  && !isGameStarted)
        {
            isGameStarted = true;
            Destroy(startingText);
        }
        
        
        
        
        
    }


}