using UnityEngine;

public class BubbleSpawner : MonoBehaviour
{
    [Header("Bubble Settings")]
    public GameObject bubblePrefab;
    public Vector2 scaleRange = new Vector2(0.3f, 1.0f); // a to b
    public Vector2 upwardSpeedRange = new Vector2(0.5f, 2.0f);
    public Vector2 chaosForceXZ = new Vector2(-0.5f, 0.5f); // chaotic lateral drift

    [Header("Breath Cycle")]
    public float exhaleDuration = 2.0f;  // How long to spawn
    public float restDuration = 2.0f;    // How long to pause
    public float spawnInterval = 0.5f;

    private bool isExhaling = true;
    private float timer = 0f;

    private void Update()
    {
        timer += Time.deltaTime;

        if (isExhaling)
        {
            // During Exhale, spawn periodically
            if (timer >= spawnInterval)
            {
                SpawnBubble();
                timer = 0f; // reset for next spawn
            }

            // Switch to Rest after exhaleDuration
            if (Time.timeSinceLevelLoad % (exhaleDuration + restDuration) >= exhaleDuration)
            {
                isExhaling = false;
                timer = 0f;
            }
        }
        else
        {
            // During Rest, do nothing until it's over
            if (Time.timeSinceLevelLoad % (exhaleDuration + restDuration) < exhaleDuration)
            {
                isExhaling = true;
                timer = 0f;
            }
        }
    }

    void SpawnBubble()
    {
        GameObject bubble = Instantiate(bubblePrefab, transform.position, Quaternion.identity);

        // Random Scale
        float randomScale = Random.Range(scaleRange.x, scaleRange.y);
        bubble.transform.localScale = Vector3.one * randomScale;

        // Add Velocity
        Rigidbody rb = bubble.GetComponent<Rigidbody>();
        if (rb != null)
        {
            float upSpeed = Random.Range(upwardSpeedRange.x, upwardSpeedRange.y);
            float xDrift = Random.Range(chaosForceXZ.x, chaosForceXZ.y);
            float zDrift = Random.Range(chaosForceXZ.x, chaosForceXZ.y);

            rb.linearVelocity = new Vector3(xDrift, upSpeed, zDrift);  // ✅ Correct setter
        }
    }
}
