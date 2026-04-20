using FMODUnity;
using UnityEngine;

public class DisappearingPlatform : Platform
{
    public float timeAlive;
    public float timeDead;

    private SpriteRenderer sr;
    private Collider2D col;
    private float timer;
    private bool isAlive = true;

    private StudioEventEmitter emitterMin;
    private StudioEventEmitter emitterMax;

    private void Awake()
    {
        transform.localScale = new Vector3(1f, 1f, 1f);
        emitterMin = GetComponent<StudioEventEmitter>();
        emitterMax = GetComponent<StudioEventEmitter>();
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();

        timer = timeAlive;
    }

    private void Update()
    {
        if (canWork)
        {
            timer -= Time.deltaTime;

            if (isAlive && timer < 0)
            {
                emitterMax.Stop();
                emitterMin.EventReference = AudioManager.Instance.minimizePlatform;
                emitterMin.Play();
                /*AudioManager.Instance.PlayMinimizePlatform(gameObject);*/
                sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0.2f);
                col.enabled = false;
                timer = timeDead;
                isAlive = false;
            }
            else if (!isAlive && timer < 0)
            {
                emitterMin.Stop();
                emitterMax.EventReference = AudioManager.Instance.maximizePlatform;
                emitterMax.Play();
                /*AudioManager.Instance.PlayMaximizePlatform(gameObject);*/
                sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1f);
                col.enabled = true;
                timer = timeAlive;
                isAlive = true;
            }
        }
    }
}
