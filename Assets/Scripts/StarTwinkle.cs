using UnityEngine;
using UnityEngine.UI;

public class StarTwinkle : MonoBehaviour
{

    [Range(0f, 1f)] public float maxOpacity = 1.0f;
    [Range(0f, 1f)] public float minOpacity = 0.0f;
    private const float frequency = 3f;
    private Image image;

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        Color color = image.color;
        float alpha = minOpacity + ((maxOpacity - minOpacity) * (Mathf.Sin(Time.time * frequency) + 1) * 0.5f); // +1 and *0.5 to oscillate between 0 and 1
        image.color = new Color(color.r, color.g, color.b, alpha);
    }
}
