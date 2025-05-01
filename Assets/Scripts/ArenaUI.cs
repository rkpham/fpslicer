using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class ArenaUI : MonoBehaviour
{
    public Entity PlayerEntity;
    UIDocument document;
    Label healthLabel;

    void Start()
    {
        UIDocument document = GetComponent<UIDocument>();
        healthLabel = document.rootVisualElement.Q<Label>("HealthLabel");

        if (PlayerEntity != null)
        {
            PlayerEntity.onDamaged += OnPlayerDamaged;
        }
    }

    void OnPlayerDamaged(DamageInstance damageInstance)
    {
        healthLabel.text = string.Format("Health: {0}", PlayerEntity.Health);
    }
}
