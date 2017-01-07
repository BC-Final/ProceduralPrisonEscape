using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemDisplayIcon {

	private static List<ItemDisplayIcon> _items = new List<ItemDisplayIcon>();

	private int _id;

	private MinimapPickupIcon _icon;

	public void ChangeState(bool collected)
	{
		_icon.ChangeState(collected);
	}

	//Static Method
	public static void UpdateItem(CustomCommands.Update.Items.ItemUpdate package)
	{
		ItemDisplayIcon item = GetItemByID(package.ID);
		item.ChangeState(package.collected);
        item._icon.transform.position = new Vector3(package.x/MinimapManager.GetInstance().scale, 2, package.z / MinimapManager.GetInstance().scale);
	}

	public static void CreateItem(CustomCommands.Creation.Items.KeyCardCreation package)
	{
		if(GetItemByID(package.ID) == null)
		{
			ItemDisplayIcon item = new ItemDisplayIcon();
			item._id = package.ID;
            _items.Add(item);

			item._icon = MinimapManager.GetInstance().CreateMinimapIcon(new Vector3(package.x, 2, package.z));
			Sprite sprite = HackerReferenceManager.Instance.KeycardIcon;

            Color color = ColorEnum.EnumToColor((ColorEnum.KeyColor)package.color);


            item._icon.SetSprite(sprite);
            item._icon.SetColor(color);
			item._icon.ChangeState(false);

            for(int i = 0; i < package.doorArray.Length; i++)
            {
                Debug.Log("door " + package.doorArray[i] + " setup");
                HackerDoor door = HackerDoor.GetDoorByID(package.doorArray[i]);
                door.MapDoor.SetKeyColor(color);
            }
		}
	}

	public static void CreateItem(CustomCommands.Creation.Items.HealthKitCreation package)
	{
		if (GetItemByID(package.ID) == null)
		{
			ItemDisplayIcon item = new ItemDisplayIcon();
			item._id = package.ID;
			_items.Add(item);

			item._icon = MinimapManager.GetInstance().CreateMinimapIcon(new Vector3(package.x, 2, package.z));
			Sprite sprite = HackerReferenceManager.Instance.HealthpackIcon;
			item._icon.SetSprite(sprite);
			item._icon.ChangeState(package.collected);
		}
	}

	public static void CreateItem(CustomCommands.Creation.Items.AmmoPackCreation package)
	{
		if (GetItemByID(package.ID) == null)
		{
			ItemDisplayIcon item = new ItemDisplayIcon();
			item._id = package.ID;
			_items.Add(item);

			item._icon = MinimapManager.GetInstance().CreateMinimapIcon(new Vector3(package.x, 2, package.z));
			Sprite sprite = HackerReferenceManager.Instance.AmmobagIcon;
			item._icon.SetSprite(sprite);
			item._icon.ChangeState(package.collected);
		}
	}


	private static ItemDisplayIcon GetItemByID(int id)
	{
		foreach (ItemDisplayIcon i in _items)
		{
			if (i._id == id)
			{
				return i;
			}
		}
		return null;
	}
	
}
