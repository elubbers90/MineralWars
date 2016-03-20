using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager
{
	private static Dictionary<string,GameObject> GameObjects = new Dictionary<string, GameObject>();

	public UIManager()
	{
	}

	public static GameObject Find(string name){
		GameObject result;
		if (GameObjects.TryGetValue (name, out result)) {
		} else {
			result = GameObject.Find (name);
			GameObjects.Add (name, result);
		}
		return result;
	}

	public static void SetText(string name, string text){
		SetText (Find (name), text);
	}

	public static void SetText(GameObject go, string text){
		if (go != null) {
			Text textComponent = go.GetComponent<Text>();
			if (textComponent != null) {
				textComponent.text = text;
			}
		}
	}

	public static void EnableButton(string name, bool enable){
		EnableButton(Find(name),enable);
	}

	public static void EnableButton(GameObject go, bool enable){
		if (go != null) {
			Button button = go.GetComponent<Button> ();
			if (button != null) {
				button.enabled = enable;
			}
		}
	}

	public static void ToggleCanvasGroup(string name, bool enable){
		ToggleCanvasGroup (Find (name), enable);
	}

	public static void ToggleCanvasGroup(GameObject go, bool enable){
		if (go != null) {
			CanvasGroup group = go.GetComponent<CanvasGroup> ();
			if (group != null) {
				if (enable) {
					group.alpha = 1;
					group.interactable = true;
					group.blocksRaycasts = true;
				} else {
					group.alpha = 0;
					group.interactable = false;
					group.blocksRaycasts = false;
				}
			}
		}
	}
}