using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CameraFollow : MonoBehaviour
{
    public Transform followTarget;
    public float followSpeed;
    public Transform cameraBoundUp;
    public Transform cameraBoundDown;
    public Transform cameraBoundLeft;
    public Transform cameraBoundRight;

    private Vector3 followTrasform;
    private float cameraHeight;
    private float cameraWidth;

    private void Start()
    {
        cameraHeight = 2 * GetComponent<Camera>().orthographicSize;
        cameraWidth = cameraHeight * GetComponent<Camera>().aspect;
    }

    private void LateUpdate()
    {
        followTrasform = followTarget.position;
        if (followTrasform.x - cameraWidth / 2 < cameraBoundLeft.transform.position.x)
            followTrasform.x = cameraBoundLeft.transform.position.x + cameraWidth / 2;
        if (followTrasform.x + cameraWidth / 2 > cameraBoundRight.transform.position.x)
            followTrasform.x = cameraBoundRight.transform.position.x - cameraWidth / 2;
        if (followTrasform.y - cameraHeight / 2 < cameraBoundDown.transform.position.y)
            followTrasform.y = cameraBoundDown.transform.position.y + cameraHeight / 2;
        if (followTrasform.y + cameraHeight / 2 > cameraBoundUp.transform.position.y)
            followTrasform.y = cameraBoundUp.transform.position.y - cameraHeight / 2;

        transform.position = new Vector3(Mathf.Lerp(transform.position.x, followTrasform.x, followSpeed * Time.deltaTime), Mathf.Lerp(transform.position.y, followTrasform.y, followSpeed * Time.deltaTime), transform.position.z);
    }
}
