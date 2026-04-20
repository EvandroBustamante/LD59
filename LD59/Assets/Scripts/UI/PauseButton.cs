using UnityEngine;
using UnityEngine.UI;

public class PauseButton : MonoBehaviour
{
    public Slider musicSlider;
    public Slider sfxSlider;
    public Button quitButton;

    private bool isPaused = false;

    private Button myButton;
    private PlayerCharacter player;

    private void Start()
    {
        myButton = GetComponent<Button>();
        player = FindAnyObjectByType<PlayerCharacter>();

        isPaused = false;

        myButton.onClick.AddListener(OnButtonClicked);
        musicSlider.onValueChanged.AddListener(OnMusicValueChanged);
        sfxSlider.onValueChanged.AddListener(OnSFXValueChanged);
        quitButton.onClick.AddListener(QuitButton);
    }

    private void OnDestroy()
    {
        myButton.onClick.RemoveListener(OnButtonClicked);
        musicSlider.onValueChanged.RemoveListener(OnMusicValueChanged);
        sfxSlider.onValueChanged.RemoveListener(OnSFXValueChanged);
        quitButton.onClick.RemoveListener(QuitButton);
    }

    private void OnButtonClicked()
    {
        if (!isPaused)
        {
            player.DisablePlayerControls();
            isPaused = true;
        }
        else if (isPaused)
        {
            player.EnablePlayerControls();
            isPaused = false;
        }
    }

    private void OnMusicValueChanged(float value)
    {
        AudioManager.Instance.SetMusicVolume(value);
    }

    private void OnSFXValueChanged(float value)
    {
        AudioManager.Instance.SetSFXVolume(value);
    }

    private void QuitButton()
    {
        Debug.Log("Quit game");
        Application.Quit();
    }
}
