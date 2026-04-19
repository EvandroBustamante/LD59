using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ShadowTrail : MonoBehaviour
{

    List<GameObject> trailParts = new List<GameObject>();
    public float lifetime = 0.1f;
    public Color _color;
    private bool isActive = false;

    void Start()
    {
       
    }

    private void Update()
    {
        if (isActive)
        {
            SpawnTrailPart();
        }
    }

    public void Activate(bool shouldActivate)
    {
        isActive = shouldActivate;
    }

    void SpawnTrailPart()
    {
        GameObject trailPart = new GameObject();
        SpriteRenderer trailPartRenderer = trailPart.AddComponent<SpriteRenderer>();
        trailPartRenderer.flipX = GetComponent<SpriteRenderer>().flipX;
        trailPartRenderer.sprite = GetComponent<SpriteRenderer>().sprite;
        trailPartRenderer.sortingOrder = 1;
        trailPart.transform.position = transform.position;
        trailPart.transform.localScale = transform.localScale; 
        trailParts.Add(trailPart);

        StartCoroutine(FadeTrailPart(trailPartRenderer));
    }

    IEnumerator FadeTrailPart(SpriteRenderer trailPartRenderer)
    {
        Color color = trailPartRenderer.color;
        color.a -= 0.5f;
        trailPartRenderer.color = _color;

        yield return new WaitForEndOfFrame();

        Destroy(trailPartRenderer.gameObject, lifetime);
    }
}