using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{
    [Header("Main Menu UI")]
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private Button playButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button quitButton;

    [Header("Settings UI")]
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private TMP_Text volumeValueText;
    [SerializeField] private Slider brightnessSlider;
    [SerializeField] private TMP_Text brightnessValueText;
    [SerializeField] private Button backButton;

    [Header("Settings")]
    [SerializeField] private string gameSceneName = "GameScene";
    [SerializeField] private float titleAnimationAmplitude = 0.5f;
    [SerializeField] private float titleAnimationFrequency = 1f;

    private Vector3 titleOriginalPosition;
    private const string VolumePrefKey = "GameVolume";
    private const string BrightnessPrefKey = "GameBrightness";

    private void Start()
    {
        // Title animation setup
        titleOriginalPosition = titleText.transform.position;

        // Load saved settings or use defaults
        volumeSlider.value = PlayerPrefs.GetFloat(VolumePrefKey, 0.8f);
        brightnessSlider.value = PlayerPrefs.GetFloat(BrightnessPrefKey, 0.8f);
        UpdateSettingsDisplay();

        // Button listeners
        playButton.onClick.AddListener(OnPlayButtonClicked);
        settingsButton.onClick.AddListener(OnSettingsButtonClicked);
        quitButton.onClick.AddListener(OnQuitButtonClicked);
        backButton.onClick.AddListener(OnBackButtonClicked);

        // Slider listeners
        volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
        brightnessSlider.onValueChanged.AddListener(OnBrightnessChanged);

        // Show main menu by default
        ShowMainMenu();
    }

    private void Update()
    {
        // Animate title text
        float yOffset = Mathf.Sin(Time.time * titleAnimationFrequency) * titleAnimationAmplitude;
        titleText.transform.position = titleOriginalPosition + new Vector3(0, yOffset, 0);
    }

    private void ShowMainMenu()
    {
        mainMenuPanel.SetActive(true);
        settingsPanel.SetActive(false);
    }

    private void ShowSettings()
    {
        mainMenuPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    private void UpdateSettingsDisplay()
    {
        volumeValueText.text = Mathf.RoundToInt(volumeSlider.value * 100) + "%";
        brightnessValueText.text = Mathf.RoundToInt(brightnessSlider.value * 100) + "%";

        // Apply brightness (this is a simple implementation - you might need a more sophisticated approach)
        Screen.brightness = brightnessSlider.value;
        
        // Apply volume (assuming you have an AudioMixer set up)
        AudioListener.volume = volumeSlider.value;
    }

    private void OnVolumeChanged(float value)
    {
        PlayerPrefs.SetFloat(VolumePrefKey, value);
        UpdateSettingsDisplay();
    }

    private void OnBrightnessChanged(float value)
    {
        PlayerPrefs.SetFloat(BrightnessPrefKey, value);
        UpdateSettingsDisplay();
    }

    public void OnPlayButtonClicked()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    public void OnSettingsButtonClicked()
    {
        ShowSettings();
    }

    public void OnBackButtonClicked()
    {
        ShowMainMenu();
    }

    public void OnQuitButtonClicked()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}