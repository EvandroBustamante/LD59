using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenuLogic : MonoBehaviour
{
    public int sceneToLoad;
    public Button muteButton;

    private Button myButton;
    private bool isMuted = false;

    private void Start()
    {
        myButton = GetComponent<Button>();

        myButton.onClick.AddListener(StartGame);
        muteButton.onClick.AddListener(MuteGame);

        StartCoroutine(StartDelay());
    }

    private void OnDestroy()
    {
        myButton.onClick.RemoveListener(StartGame);
        muteButton.onClick.RemoveListener(MuteGame);
    }

    private IEnumerator StartDelay()
    {
        yield return new WaitForEndOfFrame();

        AudioManager.Instance.StopMusicMenu();
        AudioManager.Instance.StopMusicTutorial();
        AudioManager.Instance.PlayAmbience();
    }

    private void StartGame()
    {
        SceneManager.LoadScene(sceneToLoad);
    }

    private void MuteGame()
    {
        if (!isMuted)
        {
            AudioManager.Instance.SetMusicVolume(0);
            AudioManager.Instance.SetSFXVolume(0);
            isMuted = true;
        }
        else if (isMuted)
        {
            AudioManager.Instance.SetMusicVolume(1);
            AudioManager.Instance.SetSFXVolume(1);
            isMuted = true;
        }
    }
}
