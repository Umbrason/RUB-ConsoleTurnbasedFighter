
using UnityEngine;

public class SinAnimation : MonoBehaviour
{
    public float frequency = 1f;
    private Vector3 startPosition;
    public float min, max;

    void Start() => startPosition = transform.position;

    void Update()
    {
        var h = ((Mathf.Sin(Mathf.PI * Time.time * frequency) + 1) / 2f) * (max - min) + min;
        transform.position = startPosition + Vector3.up * h;
    }
}
