using UnityEngine;
using System.Collections.Generic;

public class BoostSignalInteractable : MonoBehaviour
{
    public List<Antenna> antennasToBoost;
    [Tooltip("Must be in order, ex: element 2 on this list will afect antenna 2 on that list")]public List<int> signalIndexsToEnable;
    public ChunkWall wallToEnable;

    private int indexListCounter;
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
            if(wallToEnable != null) wallToEnable.EnableWall();

            indexListCounter = 0;
            foreach (Antenna antenna in antennasToBoost)
            {
                antenna.SignalToEnable(signalIndexsToEnable[indexListCounter]);
                indexListCounter++;
            }
            interactHitbox.enabled = false;
            sr.color = new Color(1, 0, 0, 0.5f);

            hasInteracted = true;
        }
    }
}
