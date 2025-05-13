using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    [Header("Coin System")]
    [SerializeField] private TMP_Text coinText;
    private int coinsCollected = 0;
    
    [Header("Health System")]
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private Image[] heartImages;
    [SerializeField] private Sprite fullHeart;
    [SerializeField] private Sprite emptyHeart;
    private int currentHealth;

    [Header("Audio System")]
    public AudioClip backgroundMusic;
    [Range(0, 1)] public float musicVolume = 0.5f; // Volume control slider
    private AudioSource musicPlayer;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            musicPlayer = gameObject.AddComponent<AudioSource>();
            musicPlayer.playOnAwake = false;
            musicPlayer.loop = true;
            musicPlayer.volume = musicVolume; // Set initial volume
            
            InitializeAudio();
        }
        else
        {
            Destroy(gameObject);
        }
        
        InitializeHealth();
    }

    private void InitializeAudio()
    {
        if (backgroundMusic != null)
        {
            musicPlayer.clip = backgroundMusic;
            musicPlayer.Play();
        }
    }

    // Call this whenever you want to change volume
    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume); // Keeps value between 0 and 1
        if (musicPlayer != null)
        {
            musicPlayer.volume = musicVolume;
        }
    }

    private void InitializeHealth()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    public void AddCoins(int amount)
    {
        coinsCollected += amount;
        coinText.text = $"{coinsCollected}";
    }

    public void TakeDamage()
    {
        if (currentHealth <= 0) return;
        
        currentHealth--;
        UpdateHealthUI();
        
        if (currentHealth <= 0)
        {
            PlayerDeath();
        }
    }

    private void UpdateHealthUI()
    {
        for (int i = 0; i < heartImages.Length; i++)
        {
            heartImages[i].sprite = (i < currentHealth) ? fullHeart : emptyHeart;
            heartImages[i].gameObject.SetActive(i < maxHealth);
        }
    }

    private void PlayerDeath()
    {
        Debug.Log("Player Died! Game Over!");
        if (musicPlayer != null)
        {
            musicPlayer.Stop();
        }
    }

    public void RestartGame()
    {
        InitializeHealth();
        if (musicPlayer != null && backgroundMusic != null)
        {
            musicPlayer.Play();
        }
    }
}