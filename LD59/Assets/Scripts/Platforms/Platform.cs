using System.Collections;
using UnityEngine;

public class Platform : MonoBehaviour
{

    public float worldDelay;

    [HideInInspector] public bool canWork = false;

    private void Start()
    {
        StartCoroutine(WaitWorldDelay());
    }

    private IEnumerator WaitWorldDelay()
    {
        yield return new WaitForSeconds(worldDelay);
        canWork = true;
    }
}
