using UnityEngine;

public class BackgroundColorCycle : MonoBehaviour
{
    public Camera cam;
    public Color[] gradientColors;
    public float cycleDuration = 4f;

    private int currentIndex = 0;
    private int nextIndex = 1;
    private float t = 0f;

    void Start()
    {
        if (cam == null)
            cam = Camera.main;
    }

    void Update()
    {
        if (gradientColors.Length < 2) return;

        t += Time.deltaTime / cycleDuration;
        cam.backgroundColor = Color.Lerp(
            gradientColors[currentIndex],
            gradientColors[nextIndex],
            t
        );

        if (t >= 1f)
        {
            t = 0f;
            currentIndex = nextIndex;
            nextIndex = (nextIndex + 1) % gradientColors.Length;
        }
    }
}
