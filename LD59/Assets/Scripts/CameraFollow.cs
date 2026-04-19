using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CameraFollow : MonoBehaviour
{
    public float followSpeed;

    [HideInInspector] public Transform followTarget;
    [HideInInspector] public BoxCollider2D chunkBounds;
    private Vector3 followTrasform;
    private float cameraHeight;
    private float cameraWidth;

    private void Start()
    {
        cameraHeight = 2 * GetComponent<Camera>().orthographicSize;
        cameraWidth = cameraHeight * GetComponent<Camera>().aspect;

        //chunkBounds.GetComponent<SpriteRenderer>().enabled = false;
    }

    private void FixedUpdate()
    {
        followTrasform = followTarget.position;

        if (followTrasform.x - cameraWidth / 2 < chunkBounds.transform.position.x - (chunkBounds.size.x * chunkBounds.transform.localScale.x) / 2)
            followTrasform.x = chunkBounds.transform.position.x - (chunkBounds.size.x * chunkBounds.transform.localScale.x) / 2 + cameraWidth / 2;
        if (followTrasform.x + cameraWidth / 2 > chunkBounds.transform.position.x + (chunkBounds.size.x * chunkBounds.transform.localScale.x) / 2)
            followTrasform.x = chunkBounds.transform.position.x + (chunkBounds.size.x * chunkBounds.transform.localScale.x) / 2 - cameraWidth / 2;
        if (followTrasform.y - cameraHeight / 2 < chunkBounds.transform.position.y - (chunkBounds.size.y * chunkBounds.transform.localScale.y) / 2)
            followTrasform.y = chunkBounds.transform.position.y - (chunkBounds.size.y * chunkBounds.transform.localScale.y) / 2 + cameraHeight / 2;
        if (followTrasform.y + cameraHeight / 2 > chunkBounds.transform.position.y + (chunkBounds.size.y * chunkBounds.transform.localScale.y) / 2)
            followTrasform.y = chunkBounds.transform.position.y + (chunkBounds.size.y * chunkBounds.transform.localScale.y) / 2 - cameraHeight / 2;

        transform.position = new Vector3(Mathf.Lerp(transform.position.x, followTrasform.x, followSpeed * Time.deltaTime), Mathf.Lerp(transform.position.y, followTrasform.y, followSpeed * Time.deltaTime), transform.position.z);
    }
}
