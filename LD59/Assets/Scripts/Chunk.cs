using UnityEngine;

public class Chunk : MonoBehaviour
{
    public Transform respawnPoint;

    [HideInInspector] public BoxCollider2D chunkBound;

    private void Start()
    {
        chunkBound = GetComponent<BoxCollider2D>();

        GetComponent<SpriteRenderer>().enabled = false;
    }
}
