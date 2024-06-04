using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SceneObjectsActivator : MonoBehaviour
{
    private Renderer playerRenderer;
    private Color originalColor;
    public GameObject player; // Player reference
    public TextMeshProUGUI invincibilityCountText; // To display invincibility count
    public TextMeshProUGUI doubleItemCollectionCountText; // To display double item collection count
    public TextMeshProUGUI invincibilityTimerText; // To display remaining invincibility time
    public TextMeshProUGUI doubleItemCollectionTimerText; // To display remaining double item collection time

    private void Start()
    {
        playerRenderer = player.GetComponent<Renderer>();

        if (playerRenderer != null)
        {
            originalColor = playerRenderer.material.color;
        }

        UpdateUI();
    }

    public void ActivateDoubleItemCollection()
    {
        Debug.Log("ActivateDoubleItemCollection button pressed");
        if (PlayerData.doubleItemCollectionCount > 0)
        {
            PlayerData.doubleItemCollectionCount -= 1;
            PlayerData.doubleItemCollectionActive = true;
            StartCoroutine(DoubleItemCollectionRoutine());
            PlayerData.SaveProgress();
        }
    }

    public void ActivateInvincibility()
    {
        Debug.Log("ActivateInvincibility button pressed");
        if (PlayerData.invincibilityCount > 0)
        {
            PlayerData.invincibilityCount -= 1;
            PlayerData.invincibilityActive = true;
            StartCoroutine(InvincibilityRoutine());
            PlayerData.SaveProgress();
        }
    }

    private IEnumerator DoubleItemCollectionRoutine()
    {
        Debug.Log("DoubleItemCollectionRoutine started");
        float duration = 10f; // 10 seconds of double item collection
        float remainingTime = duration;
        
        while (remainingTime > 0)
        {
            doubleItemCollectionTimerText.text = remainingTime.ToString("F1") + "s";
            yield return null;
            remainingTime -= Time.deltaTime;
        }

        PlayerData.doubleItemCollectionActive = false;
        doubleItemCollectionTimerText.text = "";
        Debug.Log("DoubleItemCollectionRoutine ended");
    }

    private IEnumerator InvincibilityRoutine()
    {
        Debug.Log("InvincibilityRoutine started");
        Collider[] obstacleColliders = FindObjectsOfType<Collider>();
        List<Collider> validColliders = new List<Collider>();

        foreach (Collider col in obstacleColliders)
        {
            if (col != null && col.gameObject.CompareTag("Obstacle"))
            {
                validColliders.Add(col);
                col.enabled = false;
            }
        }

        if (playerRenderer != null)
        {
            playerRenderer.material.color = Color.red; // Change player color to indicate invincibility
        }

        float duration = 10f; // 10 seconds of invincibility
        float remainingTime = duration;
        
        while (remainingTime > 0)
        {
            invincibilityTimerText.text = remainingTime.ToString("F1") + "s";
            yield return null;
            remainingTime -= Time.deltaTime;
        }

        foreach (Collider col in validColliders)
        {
            if (col != null)
            {
                col.enabled = true;
            }
        }

        if (playerRenderer != null)
        {
            playerRenderer.material.color = originalColor; // Revert player color
        }

        PlayerData.invincibilityActive = false;
        invincibilityTimerText.text = "";
        Debug.Log("InvincibilityRoutine ended");
    }

    private void UpdateUI()
    {
        if (invincibilityCountText != null)
        {
            invincibilityCountText.text = PlayerData.invincibilityCount.ToString();
        }
        if (doubleItemCollectionCountText != null)
        {
            doubleItemCollectionCountText.text = PlayerData.doubleItemCollectionCount.ToString();
        }
        Debug.Log("UI Updated: Invincibility=" + PlayerData.invincibilityCount + ", Double Coins=" + PlayerData.doubleItemCollectionCount);
    }
}
