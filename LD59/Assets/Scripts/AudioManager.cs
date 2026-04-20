using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using FMOD.Studio;
using UnityEngine;
using FMOD;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Music and Ambience")]
    public EventReference musicMenu;
    public EventReference musicTutorial;
    public EventReference computerAmbience;

    [Header("Character")]
    public EventReference steps;
    public EventReference jump;
    public EventReference doubleJump;
    public EventReference dashStrong;
    public EventReference dashWeak;
    public EventReference spawn;
    public EventReference death;
    public EventReference spawnInGame;
    public EventReference signalLost;
    public EventReference clickMail;

    [Header("Scenario")]
    public EventReference movingPlatform;
    public EventReference movingSaw;
    public EventReference cascadingPlatform;
    public EventReference maximizePlatform;
    public EventReference minimizePlatform;
    public EventReference trashCan;
    public EventReference bullet;
    public EventReference pickableWifi;
    public EventReference uiClick;
    public EventReference blueScreen;

    private VCA musicVCA;
    private VCA sfxVCA;

    public EventInstance menuMusicInstance;
    public EventInstance tutorialMusicInstance;
    public EventInstance ambienceInstance;

    public bool isTutorial = false;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        musicVCA = RuntimeManager.GetVCA("vca:/vca_music_fader");
        sfxVCA = RuntimeManager.GetVCA("vca:/vca_sfx_fader");

        StopAll();
        if (!isTutorial) PlayMusicMenu();
        else PlayMusicTutorial();
        PlayAmbience();
    }

    public void StopAll()
    {
        RuntimeManager.GetBus("Bus:/").stopAllEvents(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    #region Music

    public void PlayMusicMenu()
    {
        menuMusicInstance = RuntimeManager.CreateInstance(musicMenu);
        menuMusicInstance.start();
    }

    public void StopMusicMenu()
    {
        menuMusicInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        menuMusicInstance.release();
    }

    public void PlayMusicTutorial()
    {
        tutorialMusicInstance = RuntimeManager.CreateInstance(musicTutorial);
        tutorialMusicInstance.start();
    }

    public void StopMusicTutorial()
    {
        tutorialMusicInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        tutorialMusicInstance.release();
    }

    public void PlayAmbience()
    {
        ambienceInstance = RuntimeManager.CreateInstance(computerAmbience);
        ambienceInstance.start();
    }

    public void StopAmbience()
    {
        ambienceInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        ambienceInstance.release();
    }

    public void SetMusicVolume(float volume)
    {
        musicVCA.setVolume(volume);
    }

    public void SetSFXVolume(float volume)
    {
        sfxVCA.setVolume(volume);
    }

    #endregion

    #region Character

    public void PlayCharacterSteps()
    {
        RuntimeManager.PlayOneShot(steps);
    }

    public void PlayCharacterJump()
    {
        RuntimeManager.PlayOneShot(jump);
    }

    public void PlayCharacterDoubleJump()
    {
        RuntimeManager.PlayOneShot(doubleJump);
    }

    public void PlayCharacterDashStrong()
    {
        RuntimeManager.PlayOneShot(dashStrong);
    }

    public void PlayCharacterDashWeak()
    {
        RuntimeManager.PlayOneShot(dashWeak);
    }

    public void PlayCharacterSpawn()
    {
        RuntimeManager.PlayOneShot(spawn);
    }

    public void PlayCharacterDeath()
    {
        RuntimeManager.PlayOneShot(death);
    }

    public void PlaySpawnInGame()
    {
        RuntimeManager.PlayOneShot(spawnInGame);
    }

    public void PlaySignalLost()
    {
        RuntimeManager.PlayOneShot(signalLost);
    }

    public void PlayClickMail()
    {
        RuntimeManager.PlayOneShot(clickMail);
    }

    #endregion

    #region Scenario

    public void PlayMovingPlatform(GameObject objectToAttach)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(movingPlatform);
        RuntimeManager.AttachInstanceToGameObject(eventInstance, objectToAttach);
        eventInstance.start();
        eventInstance.release();
    }

    public void PlayMovingSaw(GameObject objectToAttach)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(movingSaw);
        RuntimeManager.AttachInstanceToGameObject(eventInstance, objectToAttach);
        eventInstance.start();
        eventInstance.release();
    }

    public void PlayCascadingPlatform(GameObject objectToAttach)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(cascadingPlatform);
        RuntimeManager.AttachInstanceToGameObject(eventInstance, objectToAttach);
        eventInstance.start();
        eventInstance.release();
    }

    public void PlayMaximizePlatform(GameObject objectToAttach)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(maximizePlatform);
        RuntimeManager.AttachInstanceToGameObject(eventInstance, objectToAttach);
        eventInstance.start();
        eventInstance.release();
    }

    public void PlayMinimizePlatform(GameObject objectToAttach)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(minimizePlatform);
        RuntimeManager.AttachInstanceToGameObject(eventInstance, objectToAttach);
        eventInstance.start();
        eventInstance.release();
    }

    public void PlayPickableWifi()
    {
        RuntimeManager.PlayOneShot(pickableWifi);
    }

    public void PlayClickUI()
    {
        RuntimeManager.PlayOneShot(uiClick);
    }

    public void PlayBlueScreen()
    {
        RuntimeManager.PlayOneShot(blueScreen);
    }

    #endregion
}
