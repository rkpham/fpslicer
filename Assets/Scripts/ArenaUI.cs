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
    Dictionary<string, ActionData> performedActionList = new Dictionary<string, ActionData>();
    VisualElement pauseMenu;
    VisualElement moveList;
    MoveIconBehaviour moveIconBehaviour;

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

        moveIconBehaviour = GameObject.Find("MoveIcon").GetComponent<MoveIconBehaviour>();
    }

    void Start()
    {
        document = GetComponent<UIDocument>();
        moveDescription = document.rootVisualElement.Q<Label>("MoveListInfoLabel");
        pauseMenu = document.rootVisualElement.Q("PauseMenu");
        moveList = document.rootVisualElement.Q("MoveList");
        HealthBarTransform = GameObject.Find("Health").transform;

        moveDescription.text = "";

        if (PlayerEntity != null)
        {
            PlayerEntity.onDamaged += OnPlayerDamaged;
            PlayerEntity.onActionPerformed += OnPlayerActionPerformed;
        }

        ActionData[] actionDatas = Resources.FindObjectsOfTypeAll<ActionData>();
        foreach (ActionData actionData in actionDatas)
        {
            AddMoveListInstance(actionData);
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
        if (HealthBarTransform.localScale.x < 0f)
            HealthBarTransform.localScale = new Vector3(0f, 1f, 1f);
    }

    void AddMoveListInstance(ActionData actionData)
    {
        TemplateContainer newMoveElement = MoveListInstance.Instantiate();
        document.rootVisualElement.Q("MoveListElements").Add(newMoveElement);
        newMoveElement.Q<Button>("MoveButton").text = "???";
        newMoveElement.Q<Button>("MoveButton").name = actionData.ActionName;
        newMoveElement.RegisterCallback<ClickEvent, ActionData>(OnMoveClicked, actionData);
    }

    void OnPlayerActionPerformed(ActionData actionData)
    {
        if (!performedActionList.ContainsKey(actionData.ActionName))
        {
            performedActionList.Add(actionData.ActionName, actionData);
            document.rootVisualElement.Q<Button>(actionData.ActionName).text = actionData.ActionNameReadable;
            moveIconBehaviour.PlayAnimation();
        }
    }

    void OnMoveClicked(ClickEvent clickEvent, ActionData actionData)
    {
        if (performedActionList.ContainsKey(actionData.ActionName))
        {
            moveDescription.text = actionData.ActionDescription;
        }
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