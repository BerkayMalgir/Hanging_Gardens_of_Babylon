using UnityEngine;
using TMPro;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance; // Singleton instance
    public GameObject gameOverPanel;
    public GameObject startingText;
    public TextMeshProUGUI coinsText;
    public TextMeshProUGUI invincibilityCountText;
    public TextMeshProUGUI doubleItemCollectionCountText;

    void Start()
    {
        InitializeGame();
    }

    void InitializeGame()
    {
        PlayerData.gameOver = false;
        Time.timeScale = 1;
        PlayerData.isGameStarted = false;

        PlayerData.LoadProgress();
        UpdateUI();
    }

    void Update()
    {
        if (PlayerData.gameOver)
        {
            Time.timeScale = 0;
            if (gameOverPanel != null)
            {
                gameOverPanel.SetActive(true);
            }
        }

        if (SwipeManager.tap && !PlayerData.isGameStarted)
        {
            PlayerData.isGameStarted = true;
            if (startingText != null)
            {
                startingText.SetActive(false); // Disable
            }
        }

        // Update coin text every frame
        if (coinsText != null)
        {
            coinsText.text = "Coins: " + PlayerData.numberOfCoins.ToString();
        }
    }

    void OnApplicationQuit()
    {
        PlayerData.SaveProgress();
    }

    private void UpdateUI()
    {
        if (invincibilityCountText != null)
        {
            invincibilityCountText.text = "Invincibility: " + PlayerData.invincibilityCount.ToString();
        }
        if (doubleItemCollectionCountText != null)
        {
            doubleItemCollectionCountText.text = "Double Items: " + PlayerData.doubleItemCollectionCount.ToString();
        }
        if (coinsText != null)
        {
            coinsText.text = "Coins: " + PlayerData.numberOfCoins.ToString();
        }
        Debug.Log("UI Updated: Invincibility=" + PlayerData.invincibilityCount + ", Double Items=" + PlayerData.doubleItemCollectionCount);
    }
}
