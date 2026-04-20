using UnityEngine;

public class SetOutline : MonoBehaviour
{
    public Material material;
    CircleCollider2D circleCollider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       material = GetComponent<Renderer>().material;
       circleCollider = GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
        material.SetFloat("_Radius", circleCollider.radius);
    }
}
