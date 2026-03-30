using UnityEngine;

public class FullHealPickup : MonoBehaviour
{
   // public int healAmount = 100; // Quantidade de vida a restaurar
    //public AudioClip pickupSound; // Som ao coletar
    
    public GameObject healEffectPrefab; // Efeito  de cura (opcional)

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verifica se o objeto que colidiu é o jogador
        if (collision.CompareTag("Player"))
        {
            Health playerHealth = collision.GetComponent<Health>();
            
            // Se o jogador tiver componente de vida
            if (playerHealth != null)
            {
                // Tenta curar o jogador
               // bool healed = playerHealth.ReceiveHealing(healAmount);
              // bool healed = playerHealth.ReceiveHealing(playerHealth.maximumHealth);
              playerHealth.ReceiveHealing(playerHealth.maximumHealth);
                
                // Se a cura foi bem sucedida (jogador não estava full life)
               // if (healed) 
               //  {
                    // Toca o som de coleta
                   /* if (pickupSound != null)
                    {
                        AudioSource.PlayClipAtPoint(pickupSound, transform.position);
                    }*/
                    
                    // Cria efeito visual se existir
                    if (healEffectPrefab != null)
                    {
                        Instantiate(healEffectPrefab, transform.position, Quaternion.identity);
                    }
                    
                    // Remove o pickup do cenário
                    Destroy(gameObject);
               // }
            }
        }
    }
}
