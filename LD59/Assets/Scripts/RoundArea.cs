using System.Collections;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using UnityEngine;

public class RoundArea : MonoBehaviour
{
    public LineRenderer circleRenderer;
    public CircleCollider2D circleCollider;
    public Transform circleTransform;
    public float width;
    public int sections;
    public Color color;
    public float rotationSpeed;

    public Color32[] colors;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Rainbow();
        
    }

    // Update is called once per frame
    void Update()
    {
        circleRenderer.material.color = color;
        DrawCircle(sections, circleCollider.radius);

        circleTransform.Rotate(0, 0, rotationSpeed * Time.deltaTime);

        circleRenderer.startWidth = width;
        circleRenderer.endWidth = width;
    }

    void DrawCircle (int steps, float radius)
    {
        circleRenderer.positionCount = steps;

        for (int currentStep = 0; currentStep < steps; currentStep++)
        {
            float circumferenceProgress = (float)currentStep/(steps-1);

            float currentRadian = circumferenceProgress * 2 * Mathf.PI;

            float xScaled = Mathf.Cos(currentRadian);
            float yScaled = Mathf.Sin(currentRadian);

            float x = xScaled * radius;
            float y = yScaled * radius;

            Vector3 currentPosition = new Vector3(x, y, 0);

            circleRenderer.SetPosition(currentStep, currentPosition);
        }
  
    }

    void Rainbow()
    {
        colors = new Color32[7]
        {
            new Color32(255,0,0,255),
            new Color32(255,165,0,255),
            new Color32(255,255,0,255),
            new Color32(0,255,0,255),
            new Color32(0,0,255,255),
            new Color32(75,0,130,255),
            new Color32(238,130,238,255),
        };
        StartCoroutine(Cycle());
    }

    public IEnumerator Cycle()
    {
        int startColor = 0;
        int endColor = 0;
        startColor = Random.Range(0, colors.Length);
        endColor = Random.Range(0, colors.Length);

        while (true)
        {
            for(float interpolant = 0f; interpolant <1f; interpolant+= 0.01f)
            {
                circleRenderer.material.color = Color.Lerp(colors[startColor], colors[endColor], interpolant);
                yield return null;
            }
            startColor = endColor;
            endColor = Random.Range(0, colors.Length);
        }
    }

}
