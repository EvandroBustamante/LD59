using NUnit.Framework;
using UnityEngine;

public class RoundArea : MonoBehaviour
{
    public LineRenderer circleRenderer;
    public CircleCollider2D circleCollider;
    public float width;
    public int sections;
    public Color color;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        circleRenderer.startWidth = width;
        circleRenderer.endWidth = width;

        circleRenderer.startColor = color;
        circleRenderer.endColor = color;
    }

    // Update is called once per frame
    void Update()
    {
        DrawCircle(sections, circleCollider.radius);
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
}
