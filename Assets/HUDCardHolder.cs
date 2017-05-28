using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDCardHolder : MonoBehaviour {
	[SerializeField]
	bool ShooterHUD = true;
	public void AddCard(Color color)
	{
		if (ShooterHUD)
		{
			GameObject card = Instantiate(ShooterReferenceManager.Instance.KeycardHudIcon, transform).gameObject;
			card.GetComponent<Image>().color = color;
		}
		else
		{
			GameObject card = Instantiate(HackerReferenceManager.Instance.KeycardHolderKeyIcon, transform).gameObject;
			card.GetComponent<Image>().color = color;
		}
	}
}
