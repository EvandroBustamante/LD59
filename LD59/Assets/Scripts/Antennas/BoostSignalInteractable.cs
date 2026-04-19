using UnityEngine;

public class BoostSignalInteractable : MonoBehaviour
{
    public Antenna antennaToBoost;
    public int signalIndexToEnable;

    private bool hasInteracted = false;
    private Collider2D interactHitbox;
    private SpriteRenderer sr;

    private void Start()
    {
        interactHitbox = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    public void Interact()
    {
        if (!hasInteracted)
        {
            antennaToBoost.SignalToEnable(signalIndexToEnable);
            interactHitbox.enabled = false;
            sr.color = new Color(1, 0, 0, 0.5f);
            hasInteracted = true;
        }
    }
}
