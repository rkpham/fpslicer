using UnityEngine;
using System.Collections;

public class MoveIconBehaviour : MonoBehaviour
{
    [Header("References")]
    public Material[] materials = new Material[2];  // materials[0] = default, materials[1] = alternative
    public GameObject[] gameObjects = new GameObject[4];
    public AnimationClip animationClip;

    [Header("Settings")]
    public float delay = 1f; // Delay after animation before applying material

    public Animator animator;

    void Awake()
    {
        // Optional: Try to get Animator if exists on this object
        animator = GetComponent<Animator>();
    }

    public void SetGameObjectMaterial(int index, bool useSecondMaterial)
    {
        if (index < 0 || index >= gameObjects.Length)
        {
            Debug.LogWarning("MaterialChanger: Index out of bounds.");
            return;
        }

        StartCoroutine(SetMaterialRoutine(index, useSecondMaterial));
    }

    public void ResetAllGameObjectMaterials()
    {
        StartCoroutine(ResetAllMaterialsRoutine());
    }

    private IEnumerator SetMaterialRoutine(int index, bool useSecondMaterial)
    {
        PlayAnimation();

        yield return new WaitForSeconds(delay);

        Renderer renderer = gameObjects[index].GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material = useSecondMaterial ? materials[1] : materials[0];
            gameObjects[0].GetComponent<Renderer>().material = materials[1];
        }
        else
        {
            Debug.LogWarning("MaterialChanger: No Renderer found on GameObject at index " + index);
        }
    }

    private IEnumerator ResetAllMaterialsRoutine()
    {
        PlayAnimation();

        yield return new WaitForSeconds(delay);

        foreach (GameObject obj in gameObjects)
        {
            if (obj != null)
            {
                Renderer renderer = obj.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.material = materials[0];
                }
                else
                {
                    Debug.LogWarning("MaterialChanger: No Renderer found on GameObject " + obj.name);
                }
            }
        }
    }

    private void PlayAnimation()
    {
        animator.ResetTrigger("GO"); // Clear any previous accidental triggers
        animator.SetTrigger("GO");   // Fire fresh
    }
}
