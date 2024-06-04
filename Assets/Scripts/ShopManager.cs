using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public Button invincibilityButton;
    public Button doubleItemCollectionButton;
    public TextMeshProUGUI coinsText;
    public TextMeshProUGUI invincibilityCountText;
    public TextMeshProUGUI doubleItemCollectionCountText;

    void Start()
    {
        invincibilityButton.onClick.AddListener(BuyInvincibility);
        doubleItemCollectionButton.onClick.AddListener(BuyDoubleItemCollection);
        UpdateUI();
    }

    void BuyInvincibility()
    {
        if (PlayerData.numberOfCoins >= 50)
        {
            PlayerData.numberOfCoins -= 50;
            PlayerData.invincibilityCount += 1; // Invincibility purchased
            PlayerData.SaveProgress();
            UpdateUI();
        }
    }

    void BuyDoubleItemCollection()
    {
        if (PlayerData.numberOfCoins >= 30)
        {
            PlayerData.numberOfCoins -= 30;
            PlayerData.doubleItemCollectionCount += 1; // Double item collection purchased
            PlayerData.SaveProgress();
            UpdateUI();
        }
    }

    void UpdateUI()
    {
        coinsText.text = "Coins: " + PlayerData.numberOfCoins.ToString();
        invincibilityCountText.text = "Invincibility: " + PlayerData.invincibilityCount.ToString();
        doubleItemCollectionCountText.text = "Double Coins: " + PlayerData.doubleItemCollectionCount.ToString();
    }
}