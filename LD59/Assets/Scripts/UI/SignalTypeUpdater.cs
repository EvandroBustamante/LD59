using TMPro;
using UnityEngine;

public class SignalTypeUpdater : MonoBehaviour
{
    public PlayerCharacter player;

    private TextMeshProUGUI myText;

    private void Start()
    {
        myText = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        switch (player.currentSignal)
        {
            case SignalType.NoSignal:
                myText.text = "NO SIGNAL! -> " + player.dieTimer;
                myText.color = Color.red;
                break;
            case SignalType.WeakSignal:
                myText.text = "WEAK SIGNAL";
                myText.color = Color.yellow;
                break;
            case SignalType.StrongSignal:
                myText.text = "STRONG SIGNAL";
                myText.color = Color.green;
                break;
        }
    }
}
