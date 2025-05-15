using UnityEngine;
using System.Collections;

public class MoveIconBehaviour : MonoBehaviour
{
    [Header("References")]
    public Material[] materials = new Material[2];  // materials[0] = default, materials[1] = alternative
    public GameObject[] gameObjects;
    public AnimationClip animationClip;

    [Header("Settings")]
    public float delay = 1f; // Delay after animation before applying material

    public Animator animator;

    void Awake()
    {
        // Optional: Try to get Animator if exists on this object
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        SetGameObjectMaterial(1, true);
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
        yield return new WaitForSeconds(0.1f);

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
        yield return new WaitForSeconds(0.1f);

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

    public void PlayAnimation()
    {
        animator.ResetTrigger("GO"); // Clear any previous accidental triggers
        animator.SetTrigger("GO");   // Fire fresh
    }
}
