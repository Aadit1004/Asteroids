using UnityEngine;


public class LightFlash : MonoBehaviour
{
    private UnityEngine.Rendering.Universal.Light2D light2D;
    public float maxIntensity = 1.7f;
    public float speed = 1.0f;

    private void Start()
    {
        light2D = GetComponent<UnityEngine.Rendering.Universal.Light2D>();
    }

    private void Update()
    {
        // Oscillate the intensity using a sine wave
        float sineValue = Mathf.Sin(Time.time * speed) * 0.5f + 0.5f; // This will give a value between 0 and 1
        light2D.intensity = sineValue * maxIntensity;
    }
}