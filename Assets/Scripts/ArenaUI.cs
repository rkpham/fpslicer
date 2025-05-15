using NUnit.Framework.Interfaces;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class ArenaUI : MonoBehaviour
{
    public Player PlayerEntity;
    public VisualTreeAsset MoveListInstance;
    public GameObject UI3D;
    Transform HealthBarTransform;
    UIDocument document;
    Label moveDescription;
    Dictionary<string, ActionData> actionList = new Dictionary<string, ActionData>();
    VisualElement pauseMenu;
    VisualElement moveList;

    InputSystemActions inputSystemActions;
    InputAction pauseInput;

    bool allowUnpause = true;
    bool paused;
    bool Paused
    {
        set
        {
            if (value == false && !allowUnpause)
                return;

            paused = value;
            if (value)
            {
                Time.timeScale = 0f;
                pauseMenu.visible = true;
                UnityEngine.Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Time.timeScale = 1f;
                pauseMenu.visible = false;
                UnityEngine.Cursor.lockState = CursorLockMode.Locked;
            }
        }
        get
        {
            return paused;
        }
    }

    private void OnEnable()
    {
        pauseInput.Enable();
    }

    private void OnDisable()
    {
        pauseInput.Enable();
    }

    private void Awake()
    {
        inputSystemActions = new InputSystemActions();

        pauseInput = inputSystemActions.Player.Pause;
        pauseInput.performed += PerformedPauseInput;
        pauseInput.canceled += CanceledPauseInput;
    }

    void Start()
    {
        document = GetComponent<UIDocument>();
        moveDescription = document.rootVisualElement.Q<Label>("MoveListInfoLabel");
        pauseMenu = document.rootVisualElement.Q("PauseMenu");
        moveList = document.rootVisualElement.Q("MoveList");
        HealthBarTransform = GameObject.Find("Health").transform;

        if (PlayerEntity != null)
        {
            PlayerEntity.onDamaged += OnPlayerDamaged;
            PlayerEntity.onActionPerformed += OnPlayerActionPerformed;
        }

        string[] actionDataGUIDs = AssetDatabase.FindAssets("t:ActionData");
        foreach (string actionGUID in actionDataGUIDs)
        {
            ActionData loadedActionData = AssetDatabase.LoadAssetAtPath<ActionData>(AssetDatabase.GUIDToAssetPath(actionGUID));
            AddMoveListInstance(loadedActionData);
        }

        document.rootVisualElement.Q("ContinueButton").RegisterCallback<ClickEvent>(OnPauseContinueClicked);
        document.rootVisualElement.Q("MoveListButton").RegisterCallback<ClickEvent>(OnPauseMoveListClicked);
        document.rootVisualElement.Q("ExitGameButton").RegisterCallback<ClickEvent>(OnPauseExitClicked);
        document.rootVisualElement.Q("MoveListClose").RegisterCallback<ClickEvent>(OnMoveListCloseClicked);
    }

    void PerformedPauseInput(InputAction.CallbackContext ctx)
    {
        Paused = !Paused;
    }

    void CanceledPauseInput(InputAction.CallbackContext ctx) {}

    void OnPlayerDamaged(DamageInstance damageInstance)
    {
        HealthBarTransform.localScale = new Vector3(PlayerEntity.Health / PlayerEntity.MaxHealth, 1f, 1f);
    }

    void OnPlayerActionPerformed(ActionData actionData)
    {
        if (!actionList.ContainsKey(actionData.ActionName))
        {
            actionList.Add(actionData.ActionName, actionData);
        }
    }

    void AddMoveListInstance(ActionData actionData)
    {
        TemplateContainer newMoveElement = MoveListInstance.Instantiate();
        document.rootVisualElement.Q("MoveListElements").Add(newMoveElement);
        newMoveElement.RegisterCallback<ClickEvent, ActionData>(OnMoveClicked, actionData);
    }

    void OnMoveClicked(ClickEvent clickEvent, ActionData actionData)
    {
        moveDescription.text = actionData.ActionDescription;
    }

    void OnPauseContinueClicked(ClickEvent clickEvent)
    {
        Paused = false;
    }

    void OnPauseMoveListClicked(ClickEvent clickEvent)
    {
        moveList.visible = true;
        pauseMenu.visible = false;
        allowUnpause = false;
    }
    void OnPauseExitClicked(ClickEvent clickEvent)
    {
        Application.Quit();
    }
    void OnMoveListCloseClicked(ClickEvent clickEvent)
    {
        moveList.visible = false;
        pauseMenu.visible = true;
        allowUnpause = true;
    }
}

struct MoveData
{
    public bool Unlocked;
    public ActionData ActionData;
}