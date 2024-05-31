using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
    private bool isMuted = false;
    public GameObject shopCanvas; // Shop canvas'ı referansı
    private bool isShopOpen = false; // Shop'un açık olup olmadığını kontrol eder
    public GameObject playButton; // Play butonu referansı
    public GameObject quitButton; // Quit butonu referansı
    public GameObject soundButton; // Sound butonu referansı

    private void Start()
    {
        Time.timeScale = 1;
        shopCanvas.SetActive(false); // Başlangıçta shop canvas'ını gizle
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("Level");
    }

    public void OpenShop() // Shop canvas'ını açma/kapatma fonksiyonu
    {
        isShopOpen = !isShopOpen;
        shopCanvas.SetActive(isShopOpen);

        // Diğer butonları gizle/göster
        playButton.SetActive(!isShopOpen);
        quitButton.SetActive(!isShopOpen);
        soundButton.SetActive(!isShopOpen);
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