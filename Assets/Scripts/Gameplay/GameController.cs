using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState { FreeRoam, Battle, Dialog, Menu, Case, Cutscene, Paused, Shop }

public class GameController : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    [SerializeField] Camera worldCamera;
    [SerializeField] InventoryUI inventoryUI;

    GameState state;
    GameState prevState;

    MenuController menuController;

    public static GameController Instance { get; private set; }
    private void Awake()
    {

        Instance = this;

        menuController = GetComponent<MenuController>();

        ItemDB.Init();
        QuestDB.Init();

        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
    }

    private void Start()
    {
        DialogManager.Instance.OnShowDialog += () =>
        {
            prevState = state;
            state = GameState.Dialog;
        };

        DialogManager.Instance.OnDialogFinished += () =>
        {
            if (state == GameState.Dialog)
                state = prevState;
        };

        menuController.onBack += () =>
        {
            state = GameState.FreeRoam;
        };

        menuController.onMenuSelected += OnMenuSelected;

        ShopController.i.OnStart += () => state = GameState.Shop;
        ShopController.i.OnFinish += () => state = GameState.FreeRoam;
    }

    public void PauseGame(bool pause)
    {
        if (pause)
        {
            prevState = state;
            state = GameState.Paused;
        }
        else
        {
            state = prevState;
        }
    }

    GuitarPlayerNPCController guitarPlayerNPC;

    public void StartGuitarPlayerNPCBattle(GuitarPlayerNPCController guitarPlayerNPCController)
    {
        state = GameState.Battle;
        PlayerPrefs.SetInt("Score", 0);
        SceneManager.LoadScene("BattleScene");
        MusicControl.instance.PlayMusic(guitarPlayerNPCController.Song);
    }

    public void OnEnterGuitarPlayerNPCView(GuitarPlayerNPCController guitarPlayerNPC)
    {
        state = GameState.Cutscene;
        StartCoroutine(guitarPlayerNPC.TriggerGuitarPlayerNPCBattle(playerController));
    }

    void EndGuitarPlayerNPCBattle(bool won)
    {
        if (guitarPlayerNPC != null && won == true)
        {
            guitarPlayerNPC.BattleLost();
            guitarPlayerNPC = null;
        }
        state = GameState.FreeRoam;
        worldCamera.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (state == GameState.FreeRoam)
        {
            playerController.HandleUpdate();

            if (Input.GetKeyDown(KeyCode.Return))
            {
                playerController.Character.Animator.IsMoving = false;
                menuController.OpenMenu();
                state = GameState.Menu;
            }
        }
        else if (state == GameState.Dialog)
        {
            DialogManager.Instance.HandleUpdate();
        }
        else if (state == GameState.Menu)
        {
            menuController.HandleUpdate();
        }
        else if (state == GameState.Case)
        {
            Action onBack = () =>
            {
                inventoryUI.gameObject.SetActive(false);
                state = GameState.FreeRoam;
            };

            inventoryUI.HandleUpdate(onBack);
        }
        else if (state == GameState.Shop)
        {
            ShopController.i.HandleUpdate();
        }
    }

    void OnMenuSelected(int selectedItem)
    {
        if (selectedItem == 0)
        {
            //Case
            inventoryUI.gameObject.SetActive(true);
            state = GameState.Case;
            return;
        }
        else if (selectedItem == 1)
        {
            //Guitars

        }
        else if (selectedItem == 2)
        {
            //Save
            SavingSystem.i.Save("saveSlot1");
            state = GameState.FreeRoam;
        }
        else if (selectedItem == 3)
        {
            //Load
            SavingSystem.i.Load("saveSlot1");
            state = GameState.FreeRoam;
        }
    }

    public GameState State => state;
}
