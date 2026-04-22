using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseButton : MonoBehaviour
{
    public Slider musicSlider;
    public Slider sfxSlider;
    public Button quitButton;
    public Button restartButton;
    public GameObject timer;
    private InputManager inputManager;
   

    private bool isPaused = false;

    private Button myButton;
    private PlayerCharacter player;

    private void Start()
    {
        myButton = GetComponent<Button>();
        player = FindAnyObjectByType<PlayerCharacter>();
        inputManager = player.GetComponent<InputManager>();

        isPaused = false;

        myButton.onClick.AddListener(OnButtonClicked);
        musicSlider.onValueChanged.AddListener(OnMusicValueChanged);
        sfxSlider.onValueChanged.AddListener(OnSFXValueChanged);
        quitButton.onClick.AddListener(QuitButton);
        restartButton.onClick.AddListener(RestartGame);
    }

    private void OnDestroy()
    {
        myButton.onClick.RemoveListener(OnButtonClicked);
        musicSlider.onValueChanged.RemoveListener(OnMusicValueChanged);
        sfxSlider.onValueChanged.RemoveListener(OnSFXValueChanged);
        quitButton.onClick.RemoveListener(QuitButton);
        restartButton.onClick.RemoveListener(RestartGame);
    }

    private void Update()
    {
        if(inputManager.isRestarting)
        {
            inputManager.isRestarting = false;
            RestartGame();
        }
    }

    private void OnButtonClicked()
    {
        if (!isPaused)
        {
            player.DisablePlayerControls();
            isPaused = true;
            if (timer) timer.GetComponent<Timer>().pauseTime = true;
        }
        else if (isPaused)
        {
            player.EnablePlayerControls();
            isPaused = false;
            if (timer) timer.GetComponent<Timer>().pauseTime = false;
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

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Debug.Log("Restarting Game");
    }
}
