using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // Singleton
    
    [Header("Coin System")]
    [SerializeField] private TMP_Text coinText; // Texto da UI
    private int coinsCollected = 0; // Contador interno

    private void Awake()
    {
        // Configura o singleton
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddCoins(int amount)
    {
        coinsCollected += amount;
        UpdateCoinUI();
    }

    private void UpdateCoinUI()
    {
        if (coinText != null)
            coinText.text = coinsCollected.ToString();
    }
}