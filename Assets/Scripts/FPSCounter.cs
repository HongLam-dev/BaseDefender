
using TMPro;
using UnityEngine;

public class FPSCounter : MonoBehaviour
{
    [SerializeField] private TMP_Text fpsText;
    [SerializeField] private float updateInterval = 0.5f;

    private float elapsedTime;
    private int frameCount;

    private void Start()
    {
        Application.targetFrameRate = 60;
    }

    private void Update()
    {
        elapsedTime += Time.unscaledDeltaTime;
        frameCount++;

        if (elapsedTime >= updateInterval)
        {
            float fps = frameCount / elapsedTime;

            fpsText.text = $"FPS: {fps:F0}";

            elapsedTime = 0f;
            frameCount = 0;
        }
    }
}
