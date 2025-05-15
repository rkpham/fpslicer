using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class DeathBehavior : MonoBehaviour
{
    public Image image;               // Image component for the black screen
    public AudioSource audioSourceA;
    public AudioSource audioSourceB;
    public float fadeDuration = 2f;
    public string sceneToLoad;        // Set this in the Inspector
    public Player player;

    void Start()
    {
        image = GetComponentInChildren<Image>();

        if (image != null)
        {
            Color color = image.color;
            color.a = 0f;
            image.color = color;
            image.gameObject.SetActive(false);
        }
        player = GameObject.Find("Player").GetComponent<Player>();
        player.onDied += DIE;
    }

    public void DIE()
    {
        StartCoroutine(FadeIn(image, fadeDuration));
        audioSourceA.Play();
        Invoke(nameof(PlayDelayedAudio), fadeDuration / 2f);
        Invoke(nameof(SwitchScene), fadeDuration * 2f);
    }

    void PlayDelayedAudio()
    {
        audioSourceB.Play();
    }

    void SwitchScene()
    {
        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            Debug.LogWarning("No scene name set for DeathBehavior.");
        }
    }

    IEnumerator FadeIn(Image img, float duration)
    {
        img.gameObject.SetActive(true);
        float time = 0f;
        Color color = img.color;

        while (time < duration)
        {
            float t = time / duration;
            color.a = Mathf.Lerp(0f, 1f, t);
            img.color = color;
            time += Time.deltaTime;
            yield return null;
        }

        color.a = 1f;
        img.color = color;
    }

    private void Update()
    {
        // FOR TESTING
        //if (Input.GetKeyUp(KeyCode.Space))
        //{
           // DIE();
        //}
    }
}
