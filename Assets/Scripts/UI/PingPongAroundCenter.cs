
using UnityEngine;

public class PingPongAroundCenter : MonoBehaviour
{
    [SerializeField] private float amplitude = 0.15f;
    [SerializeField] private float frequency = 2f;
    [SerializeField] private Vector3 direction;

    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.localPosition;
    }

    public void SetPosition(Vector3 position)
    {
        startPosition = position;
        transform.position = position;
    }

    private void Update()
    {
        float offset = Mathf.Sin(Time.time * frequency * Mathf.PI * 2f)
                       * amplitude;

        transform.localPosition = startPosition + direction * offset;
    }
}
