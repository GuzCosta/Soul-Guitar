using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ShopState { Menu, Buying, Selling, Busy }

public class ShopController : MonoBehaviour
{
    [SerializeField] InventoryUI inventoryUI;

    public event Action OnStart;
    public event Action OnFinish;
    ShopState state;

    public static ShopController i { get; private set; }

    private void Awake()
    {
        i = this;
    }

    Inventory inventory;

    private void Start()
    {
        inventory = Inventory.GetInventory();
    }

    public IEnumerator StartTrading(Merchant merchant)
    {
        OnStart?.Invoke();
        yield return StartMenuState();
    }

    IEnumerator StartMenuState()
    {
        state = ShopState.Menu;

        int selectedChoice = 0;
        yield return DialogManager.Instance.ShowDialogText("Welcome to Rock Wings Store. How can I help you?",
            waitForInput: false,
            choices: new List<string>() { "Buy", "Sell", "Quit" },
            onChoiceSelected: choiceIndex => selectedChoice = choiceIndex);

        if (selectedChoice == 0)
        {
            //Buy
        }
        else if (selectedChoice == 1)
        {
            //Sell
            state = ShopState.Selling;
            inventoryUI.gameObject.SetActive(true);
        }
        else if (selectedChoice == 2)
        {
            //Quit
            OnFinish?.Invoke();
            yield break;
        }
    }

    public void HandleUpdate()
    {
        if (state == ShopState.Selling)
        {
            inventoryUI.HandleUpdate(OnBackFromSelling, (selectedItem) => StartCoroutine(SellItem(selectedItem)));
        }
    }

    void OnBackFromSelling()
    {
        inventoryUI.gameObject.SetActive(false);
        StartCoroutine(StartMenuState());
    }

    IEnumerator SellItem(ItemBase item)
    {
        state = ShopState.Busy;

        if (!item.IsSellable)
        {
            yield return DialogManager.Instance.ShowDialogText("You can't sell that!");
            state = ShopState.Selling;
            yield break;
        }

        float sellingPrice = Mathf.Round(item.Price / 2);

        int selectedChoice = 0;
        yield return DialogManager.Instance.ShowDialogText($"I can give {sellingPrice} for that. Would you like to sell?",
            waitForInput: false,
            choices: new List<string>() { "Yes", "No" },
            onChoiceSelected: choiceIndex => selectedChoice = choiceIndex);

        if (selectedChoice == 0)
        {
            //Yes
            inventory.RemoveItem(item);
            //dsdas
            yield return DialogManager.Instance.ShowDialogText($"Turned over {item.Name} and received {sellingPrice}!");
        }

        state = ShopState.Selling;
    }
}
