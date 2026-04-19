using FMODUnity;
using FMOD.Studio;
using UnityEngine;

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

    private VCA musicVCA;
    private VCA sfxVCA;

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

        PlayMusicMenu();
        PlayAmbience();
    }

    #region Music

    public void PlayMusicMenu()
    {
        RuntimeManager.PlayOneShot(musicMenu);
    }

    public void PlayMusicTutorial()
    {
        RuntimeManager.PlayOneShot(musicTutorial);
    }

    public void PlayAmbience()
    {
        RuntimeManager.PlayOneShot(computerAmbience);
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

    #endregion
}
