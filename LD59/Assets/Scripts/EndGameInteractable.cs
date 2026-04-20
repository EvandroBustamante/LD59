using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndGameInteractable : MonoBehaviour
{
    public GameObject blackScreen;
    public GameObject blueScreen;
    public TextMeshProUGUI messageText;

    private PlayerCharacter player;

    private void Start()
    {
        player = FindAnyObjectByType<PlayerCharacter>();
    }

    public void TriggerEnd()
    {
        StartCoroutine(EndScreen());
    }

    public IEnumerator EndScreen()
    {
        player.DisablePlayerControls();

        blackScreen.SetActive(true);
        messageText.text = "Delivering message in 5...";

        yield return new WaitForSeconds(1f);

        messageText.text = "Delivering message in 4...";

        yield return new WaitForSeconds(1f);

        messageText.text = "Delivering message in 3...";

        yield return new WaitForSeconds(1f);

        messageText.text = "Delivering message in 2...";

        yield return new WaitForSeconds(1f);

        messageText.text = "Delivering message in 1...";

        yield return new WaitForSeconds(0.3f);

        blueScreen.SetActive(true);
        AudioManager.Instance.StopAll();
        AudioManager.Instance.PlayBlueScreen();

        yield return new WaitForSeconds(3f);

        Debug.Log("GAME QUIT");
        Application.Quit();
    }
}
