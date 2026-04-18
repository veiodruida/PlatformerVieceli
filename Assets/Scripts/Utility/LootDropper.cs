using UnityEngine;

/// <summary>
/// This component can be attached to any GameObject. 
/// Its main purpose is to drop a specific prefab when a UnityEvent (like onDie) is triggered.
/// </summary>
public class LootDropper : MonoBehaviour
{
    [Tooltip("The Prefab (like ScorePickup) to drop when this is called.")]
    public GameObject lootPrefab;

    /// <summary>
    /// Spawns the lootPrefab exactly at the position of this GameObject.
    /// This method is meant to be called by a UnityEvent (like from Health.cs).
    /// </summary>
    public void DropItem()
    {
        if (lootPrefab != null)
        {
            Instantiate(lootPrefab, transform.position, transform.rotation);
        }
        else
        {
            Debug.LogWarning("LootDropper was asked to drop an item, but the Loot Prefab is not assigned in the inspector!", this.gameObject);
        }
    }
}
