using UnityEngine;

public class BackgroundFadeLoop : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    [Range(0f, 1f)] public float minAlpha = 0.3f;
    [Range(0f, 1f)] public float maxAlpha = 0.8f;
    public float fadeSpeed = 1f; 

    private Color originalColor;

    void Start()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        originalColor = spriteRenderer.color;
    }

    void Update()
    {
        float alpha = Mathf.Lerp(minAlpha, maxAlpha,
                     (Mathf.Sin(Time.time * fadeSpeed) + 1f) / 2f);

        spriteRenderer.color = new Color(
            originalColor.r,
            originalColor.g,
            originalColor.b,
            alpha
        );
    }
}
