using UnityEngine;

public class BobbingTitle : MonoBehaviour
{
    public float amplitude = 10f;  // How far it moves up/down
    public float frequency = 2f;   // How fast it bobs

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.localPosition;
    }

    void Update()
    {
        float newY = startPos.y + Mathf.Sin(Time.time * frequency) * amplitude;
        transform.localPosition = new Vector3(startPos.x, newY, startPos.z);
    }
}
