using System.Collections.Generic;
using UnityEngine;

public class ChunkWall : MonoBehaviour
{
    public GameObject wallToToggle;
    public Chunk chunkToToggle;
    public List<Antenna> antennasToDisable;

    private Collider2D trigger;
    private SpriteRenderer sr;

    private void Start()
    {
        trigger = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();

        trigger.enabled = false;
        sr.enabled = false;

        wallToToggle.GetComponent<SpriteRenderer>().enabled = false;
        wallToToggle.SetActive(false);
    }

    public void EnableWall()
    {
        trigger.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            wallToToggle.SetActive(true);
            chunkToToggle.DisableChunk();
            foreach (Antenna antenna in antennasToDisable)
            {
                antenna.DisableAllSignals();
            }
            trigger.enabled = false;
            this.enabled = false;
        }
    }
}
