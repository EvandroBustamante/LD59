using UnityEngine;

public class Rotate : MonoBehaviour
{

    public float rotationSpeed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }
}
