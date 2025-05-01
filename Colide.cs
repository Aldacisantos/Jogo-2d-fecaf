using UnityEngine;

public class TeleportPlayerOnTouch : MonoBehaviour
{
    [Header("Target Position")]
    [SerializeField] private Vector2 targetPosition = new Vector2(22.05f, 2.2f);
    
    [Header("Effects")]
    [SerializeField] private ParticleSystem teleportEffect;
    [SerializeField] private AudioClip teleportSound;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            TeleportPlayer(other.gameObject);
        }
    }

    private void TeleportPlayer(GameObject player)
    {
        // Toca efeito sonoro
        if (teleportSound != null)
        {
            AudioSource.PlayClipAtPoint(teleportSound, transform.position);
        }

        // Cria efeito visual
        if (teleportEffect != null)
        {
            Instantiate(teleportEffect, player.transform.position, Quaternion.identity);
        }

        // Teleporta o jogador
        player.transform.position = targetPosition;

        // Cria efeito no destino
        if (teleportEffect != null)
        {
            Instantiate(teleportEffect, targetPosition, Quaternion.identity);
        }
    }
}
