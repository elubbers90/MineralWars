using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GameLogic : MonoBehaviour {
	public const int MOVE_LIMIT = 10;

	public int Move = 0;
	public Money Backpack;
	public Money Objective;
	public Money CurrentSales;


	// Use this for initialization
	void Start () {
		StartGame ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.R)) {
			StartGame ();
		}
	}

	public void StartGame(){
		Backpack = new Money ();
		Backpack.AddResource (ResourceType.GOLD, UnityEngine.Random.Range (1000, 1500));
		Objective = new Money ();
		ResourceType resourceGoal = (ResourceType)UnityEngine.Random.Range (1, 7);
		int amountGoal;
		switch (resourceGoal) {
		case ResourceType.BLUE:
			amountGoal = UnityEngine.Random.Range (1, 3);
			break;
		case ResourceType.GREEN:
			amountGoal = UnityEngine.Random.Range (1, 3);
			break;
		case ResourceType.ORANGE:
			amountGoal = UnityEngine.Random.Range (1, 4);
			break;
		case ResourceType.PINK:
			amountGoal = UnityEngine.Random.Range (2, 4);
			break;
		case ResourceType.PURPLE:
			amountGoal = UnityEngine.Random.Range (2, 4);
			break;
		case ResourceType.RED:
			amountGoal = UnityEngine.Random.Range (3, 5);
			break;
		default:
			amountGoal = 0;
			break;
		}
		Objective.AddResource (resourceGoal, amountGoal);
		Move = 0;

		ShowObjective ();
		NextTurn ();
	}

	public void ShowObjective(){
		Dictionary<ResourceType,int> objective = Objective.GetResources ();
		string objectiveString = "";
		foreach(var item in objective)
		{
			if (item.Value > 0) {
				objectiveString = String.Format ("{0} {1}: {2}", objectiveString, item.Key.ToString (), item.Value);
			}
		}
		UIManager.SetText ("GoalAmount", objectiveString);
	}

	public void NextTurn(){
		if (CheckObjective()) {
			UIManager.SetText ("GameFinishedText", "Good job!");
			UIManager.ToggleCanvasGroup ("DialogWindows", true);
		} else {
			CurrentSales = new Money ();
			CurrentSales.AddResource (ResourceType.BLUE, UnityEngine.Random.Range (1000, 5000));
			CurrentSales.AddResource (ResourceType.GREEN, UnityEngine.Random.Range (500, 4000));
			CurrentSales.AddResource (ResourceType.ORANGE, UnityEngine.Random.Range (250, 2500));
			CurrentSales.AddResource (ResourceType.PINK, UnityEngine.Random.Range (100, 2000));
			CurrentSales.AddResource (ResourceType.PURPLE, UnityEngine.Random.Range (100, 2000));
			CurrentSales.AddResource (ResourceType.RED, UnityEngine.Random.Range (50, 1500));
			ShowSales ();
			ShowBackpack ();
			ShowMovesLeft ();
			if (CheckBankrupt()) {
				UIManager.SetText ("GameFinishedText", "Bankrupt :(");
				UIManager.ToggleCanvasGroup ("DialogWindows", true);
			} else if (CheckLoss()) {
				UIManager.SetText ("GameFinishedText", "The order was cancelled, you took to long :(");
				UIManager.ToggleCanvasGroup ("DialogWindows", true);
			}
		}
	}

	public bool CheckObjective(){
		Dictionary<ResourceType,int> objective = Objective.GetResources ();
		foreach (var item in objective) {
			if (item.Value > Backpack.GetResource (item.Key)) {
				return false;
			}
		}
		return true;
	}

	public bool CheckLoss(){
		return Move >= MOVE_LIMIT;
	}

	public bool CheckBankrupt(){
		Dictionary<ResourceType,int> sales = CurrentSales.GetResources ();
		foreach (var item in sales) {
			if (item.Key != ResourceType.GOLD) {
				if (Backpack.GetResource (item.Key) > 0 || Backpack.GetResource (ResourceType.GOLD) >= item.Value) {
					return false;
				}
			}
		}
		return true;
	}

	public void ShowSales(){
		UIManager.SetText ("ShopItemBlue/Bubble/Price", CurrentSales.GetResource(ResourceType.BLUE).ToString());
		UIManager.SetText ("ShopItemGreen/Bubble/Price", CurrentSales.GetResource(ResourceType.GREEN).ToString());
		UIManager.SetText ("ShopItemOrange/Bubble/Price", CurrentSales.GetResource(ResourceType.ORANGE).ToString());
		UIManager.SetText ("ShopItemPink/Bubble/Price", CurrentSales.GetResource(ResourceType.PINK).ToString());
		UIManager.SetText ("ShopItemPurple/Bubble/Price", CurrentSales.GetResource(ResourceType.PURPLE).ToString());
		UIManager.SetText ("ShopItemRed/Bubble/Price", CurrentSales.GetResource(ResourceType.RED).ToString());
		UpdateButtons ();
	}

	public void UpdateButtons(){
		UpdateButtons (CurrentSales.GetResource (ResourceType.BLUE), Backpack.GetResource (ResourceType.BLUE), "ShopItemBlue/Bubble/SellButton", "ShopItemBlue/Bubble/BuyButton");
		UpdateButtons (CurrentSales.GetResource (ResourceType.GREEN), Backpack.GetResource (ResourceType.GREEN), "ShopItemGreen/Bubble/SellButton", "ShopItemGreen/Bubble/BuyButton");
		UpdateButtons (CurrentSales.GetResource (ResourceType.ORANGE), Backpack.GetResource (ResourceType.ORANGE), "ShopItemOrange/Bubble/SellButton", "ShopItemOrange/Bubble/BuyButton");
		UpdateButtons (CurrentSales.GetResource (ResourceType.PINK), Backpack.GetResource (ResourceType.PINK), "ShopItemPink/Bubble/SellButton", "ShopItemPink/Bubble/BuyButton");
		UpdateButtons (CurrentSales.GetResource (ResourceType.PURPLE), Backpack.GetResource (ResourceType.PURPLE), "ShopItemPurple/Bubble/SellButton", "ShopItemPurple/Bubble/BuyButton");
		UpdateButtons (CurrentSales.GetResource (ResourceType.RED), Backpack.GetResource (ResourceType.RED), "ShopItemRed/Bubble/SellButton", "ShopItemRed/Bubble/BuyButton");
	}

	public void UpdateButtons(int salePrice, int backpackAmount, string salebutton, string buybutton){
		if (backpackAmount <= 0) {
			UIManager.EnableButton (salebutton, false);
		} else {
			UIManager.EnableButton (salebutton, true);
		}
		if (salePrice > Backpack.GetResource (ResourceType.GOLD)) {
			UIManager.EnableButton (buybutton, false);
		} else {
			UIManager.EnableButton (buybutton, true);
		}
	}

	public void ShowBackpack(){
		UIManager.SetText ("BackpackResourceGold/Amount", Backpack.GetResource(ResourceType.GOLD).ToString());
		UIManager.SetText ("BackpackResourceBlue/Amount", Backpack.GetResource(ResourceType.BLUE).ToString());
		UIManager.SetText ("BackpackResourceGreen/Amount", Backpack.GetResource(ResourceType.GREEN).ToString());
		UIManager.SetText ("BackpackResourceOrange/Amount", Backpack.GetResource(ResourceType.ORANGE).ToString());
		UIManager.SetText ("BackpackResourcePink/Amount", Backpack.GetResource(ResourceType.PINK).ToString());
		UIManager.SetText ("BackpackResourcePurple/Amount", Backpack.GetResource(ResourceType.PURPLE).ToString());
		UIManager.SetText ("BackpackResourceRed/Amount", Backpack.GetResource(ResourceType.RED).ToString());
	}

	public void ShowMovesLeft(){
		UIManager.SetText ("MovesLeft", (MOVE_LIMIT - Move).ToString());
	}

	public void GameButtonClick(string value){
		string[] values = value.Split (' ');
		switch (values [0]) {
		case "Restart":
			UIManager.ToggleCanvasGroup ("DialogWindows", false);
			StartGame ();
			break;
		case "Sell":
			switch (values [1]) {
			case "Blue":
				Backpack.AddResource (ResourceType.BLUE, -1);
				Backpack.AddResource (ResourceType.GOLD, CurrentSales.GetResource (ResourceType.BLUE));
				break;
			case "Green":
				Backpack.AddResource (ResourceType.GREEN, -1);
				Backpack.AddResource (ResourceType.GOLD, CurrentSales.GetResource (ResourceType.GREEN));
				break;
			case "Orange":
				Backpack.AddResource (ResourceType.ORANGE, -1);
				Backpack.AddResource (ResourceType.GOLD, CurrentSales.GetResource (ResourceType.ORANGE));
				break;
			case "Pink":
				Backpack.AddResource (ResourceType.PINK, -1);
				Backpack.AddResource (ResourceType.GOLD, CurrentSales.GetResource (ResourceType.PINK));
				break;
			case "Purple":
				Backpack.AddResource (ResourceType.PURPLE, -1);
				Backpack.AddResource (ResourceType.GOLD, CurrentSales.GetResource (ResourceType.PURPLE));
				break;
			case "Red":
				Backpack.AddResource (ResourceType.RED, -1);
				Backpack.AddResource (ResourceType.GOLD, CurrentSales.GetResource (ResourceType.RED));
				break;
			}
			Move++;
			NextTurn ();
			break;
		case "Buy":
			switch (values [1]) {
			case "Blue":
				Backpack.AddResource (ResourceType.BLUE, 1);
				Backpack.AddResource (ResourceType.GOLD, -CurrentSales.GetResource (ResourceType.BLUE));
				break;
			case "Green":
				Backpack.AddResource (ResourceType.GREEN, 1);
				Backpack.AddResource (ResourceType.GOLD, -CurrentSales.GetResource (ResourceType.GREEN));
				break;
			case "Orange":
				Backpack.AddResource (ResourceType.ORANGE, 1);
				Backpack.AddResource (ResourceType.GOLD, -CurrentSales.GetResource (ResourceType.ORANGE));
				break;
			case "Pink":
				Backpack.AddResource (ResourceType.PINK, 1);
				Backpack.AddResource (ResourceType.GOLD, -CurrentSales.GetResource (ResourceType.PINK));
				break;
			case "Purple":
				Backpack.AddResource (ResourceType.PURPLE, 1);
				Backpack.AddResource (ResourceType.GOLD, -CurrentSales.GetResource (ResourceType.PURPLE));
				break;
			case "Red":
				Backpack.AddResource (ResourceType.RED, 1);
				Backpack.AddResource (ResourceType.GOLD, -CurrentSales.GetResource (ResourceType.RED));
				break;
			}
			Move++;
			NextTurn ();
			break;
		}
	}
}
