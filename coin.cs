using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private int coinValue = 1; // Valor da moeda
    [SerializeField] private AudioClip collectSound; // Som de coleta
    [SerializeField] private GameObject collectEffect; // Efeito visual ao coletar
    

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Notifica o GameManager para adicionar pontos
            GameManager.instance.AddCoins(coinValue);
            
            // Toca som de coleta (se existir)
            if (collectSound != null)
                AudioSource.PlayClipAtPoint(collectSound, transform.position);
            
            // Instancia efeito visual (se existir)
            if (collectEffect != null)
                Instantiate(collectEffect, transform.position, Quaternion.identity);
            
            // Destroi a moeda
            Destroy(gameObject);
        }
    }
}