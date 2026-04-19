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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(this.transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }
}
