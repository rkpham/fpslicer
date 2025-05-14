using UnityEngine;

public class moveicontesterinator : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public MoveIconBehaviour moveIconBehaviour;
    void Start()
    {
        moveIconBehaviour = GetComponent<MoveIconBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha0))
            {
            moveIconBehaviour.ResetAllGameObjectMaterials();
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
            {
            moveIconBehaviour.SetGameObjectMaterial(1, true);
        }
        if(Input.GetKeyDown(KeyCode.Alpha2))
            {
            moveIconBehaviour.SetGameObjectMaterial(2, true);
        }
        if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            moveIconBehaviour.SetGameObjectMaterial(3, true);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4)) {
            moveIconBehaviour.SetGameObjectMaterial(4, true);
        }
    }
}
