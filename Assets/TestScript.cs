using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour {

	[Header("HUD Keycard holder testing")]

	HUDCardHolder cardHolder;
	[SerializeField]
	Color keycardColor;

	[ContextMenu("AddKeycardToHUD")]
	void AddKeycardToHUD()
	{
		if (!cardHolder)
		{
			cardHolder = GameObject.FindObjectOfType<HUDCardHolder>();
		}
		cardHolder.AddCard(keycardColor);
	}
}
