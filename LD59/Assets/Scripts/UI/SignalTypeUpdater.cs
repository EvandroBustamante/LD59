using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SignalTypeUpdater : MonoBehaviour
{
    [Header("Buttons")]
    public Sprite availableButton;
    public Sprite unavailableButton;
    public Image doubleJumpButton;
    public Image dashButton;

    [Header("DoubleJump")]
    public Sprite doubleJumpAvailable;
    public Sprite doubleJumpUnavailable;
    public Image doubleJumpIcon;

    [Header("Dashed")]
    public Sprite noDash;
    public Sprite weakDash;
    public Sprite strongDash;
    public Image dashIcon;
    public TextMeshProUGUI dashText;

    [Header("Signal Feedback")]
    public Sprite noSignal;
    public Sprite weakSignal;
    public Sprite strongSignal;
    public Image feedbackImage;

    [Header("Time Text")]
    public TextMeshProUGUI myText;

    private PlayerCharacter player;

    private void Start()
    {
        player = FindAnyObjectByType<PlayerCharacter>();
    }

    private void Update()
    {
        switch (player.currentSignal)
        {
            case SignalType.NoSignal:
                UpdateNoSignal();
                break;
            case SignalType.WeakSignal:
                UpdateWeakSignal();
                break;
            case SignalType.StrongSignal:
                UpdateStrongSignal();
                break;
        }

        myText.text = System.DateTime.Now.ToString("hh:mm");
    }

    private void UpdateNoSignal()
    {
        doubleJumpButton.sprite = unavailableButton;
        doubleJumpIcon.sprite = doubleJumpUnavailable;

        dashButton.sprite = unavailableButton;
        dashIcon.sprite = noDash;
        dashText.text = "Dash";

        feedbackImage.sprite = noSignal;
    }

    private void UpdateWeakSignal()
    {
        doubleJumpButton.sprite = unavailableButton;
        doubleJumpIcon.sprite = doubleJumpUnavailable;

        dashButton.sprite = availableButton;
        dashIcon.sprite = weakDash;
        dashText.text = "Dash";

        feedbackImage.sprite = weakSignal;
    }

    private void UpdateStrongSignal()
    {
        doubleJumpButton.sprite = availableButton;
        doubleJumpIcon.sprite = doubleJumpAvailable;

        dashButton.sprite = availableButton;
        dashIcon.sprite = strongDash;
        dashText.text = "Super dash!";

        feedbackImage.sprite = strongSignal;
    }
}
