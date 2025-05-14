using UnityEngine;

public class BubbleButtbehavior : MonoBehaviour
{
    public float noiseStrength = 0.3f;
    public float frequency = 1f;

    private Vector3 originalVelocity;
    private Rigidbody rb;
    private float seed;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (rb == null)
        {
            Debug.LogError("No Rigidbody found on " + gameObject.name);
            return;
        }

        originalVelocity = rb.linearVelocity;  // ? Use .velocity, not linearVelocity (read-only)
        seed = Random.value * 100f;
    }

    void FixedUpdate()
    {
        if (rb == null) return;

        Vector3 offset = new Vector3(
            Mathf.PerlinNoise(seed, Time.time * frequency) - 0.5f,
            0f,
            Mathf.PerlinNoise(seed + 1f, Time.time * frequency) - 0.5f
        ) * noiseStrength;

        rb.linearVelocity = originalVelocity + offset;  // ? Only velocity is assignable
    }

    public void killThyself()
    {
        Destroy(gameObject);
    }
}
