using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HackerKeyCard : MonoBehaviour {

	private static List<HackerKeyCard> _items = new List<HackerKeyCard>();

	private int _id;
	private MinimapPickupIcon _icon;

	public void ChangeState(bool collected)
	{
		_icon.ChangeState(collected);
	}

	//Static Method
	public static void UpdateKeyCard(CustomCommands.Update.KeyCardUpdate package)
	{
		HackerKeyCard item = GetItemByID(package.ID);
		item.ChangeState(package.collected);
	}

	public static void CreateKeyCard(CustomCommands.Creation.KeyCardCreation package)
	{
		if(GetItemByID(package.ID) == null)
		{
			HackerKeyCard item = new HackerKeyCard();
			item._id = package.ID;
            _items.Add(item);

			item._icon = MinimapManager.GetInstance().CreateMinimapIcon(new Vector3(package.x, 1, package.z));
			//Sprite sprite = Resources.Load<Sprite>("Sprites/spr_keycard.png");
            //item._icon.SetSprite(sprite);
			item._icon.ChangeState(package.collected);
		}
	}

	private static HackerKeyCard GetItemByID(int id)
	{
		foreach (HackerKeyCard i in _items)
		{
			if (i._id == id)
			{
				return i;
			}
		}
		return null;
	}
	
}
