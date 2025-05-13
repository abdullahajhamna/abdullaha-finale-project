using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private int value = 1; // How much this coin is worth
    [SerializeField] private float rotationSpeed = 100f; // Visual rotation effect
    [SerializeField] private AudioClip collectSound;
    
    private void Update()
    {
        // Rotate the coin for visual effect
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Notify the GameManager to add coins
            GameManager.Instance.AddCoins(value);
            
            // Play sound if available
            if (collectSound != null)
            {
                AudioSource.PlayClipAtPoint(collectSound, transform.position);
            }
            
            // Destroy the coin
            Destroy(gameObject);
        }
    }
}