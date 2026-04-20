using UnityEngine;

public class BatteryAudio : MonoBehaviour
{
    public void LostSignalAudio()
    {
        AudioManager.Instance.PlaySignalLost();
    }
}
