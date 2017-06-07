using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamelogic.Extensions;

public class LasergateMapIcon : AbstractMapIcon {

	[SerializeField]
	Sprite ActiveSprite;
	[SerializeField]
	Sprite inactiveSprite;

	private ObservedValue<bool> _isActive = new ObservedValue<bool>(false);

	public static void ProcessPacket(NetworkPacket.GameObjects.Lasergate.Creation pPacket)
	{
		LasergateMapIcon icon = HackerPackageSender.GetNetworkedObject<LasergateMapIcon>(pPacket.Id);

		if (icon == null)
		{
			createInstance(pPacket);
		}
		else
		{
			Debug.Log("ICON ALREADY EXISTS!!!");
		}
	}

	public static void ProcessPacket(NetworkPacket.GameObjects.Lasergate.sUpdate pPacket)
	{
		LasergateMapIcon icon = HackerPackageSender.GetNetworkedObject<LasergateMapIcon>(pPacket.Id);

		if (icon != null)
		{
			icon.updateInstance(pPacket);
		}
		else
		{
			Debug.Log("ICON NOT FOUND!!!");
		}
	}

	private static void createInstance(NetworkPacket.GameObjects.Lasergate.Creation pPacket)
	{
		LasergateMapIcon icon = Instantiate(HackerReferenceManager.Instance.Lasergate, new Vector3(pPacket.PosX / MinimapManager.scale, pPacket.PosY / MinimapManager.scale, 0),
																								Quaternion.Euler(0, 0, -pPacket.Rot)).GetComponent<LasergateMapIcon>();

		icon._isActive.OnValueChange += icon.OnIsActiveChange;

		icon.Id = pPacket.Id;
		icon._isActive.Value = pPacket.Active;
		icon.OnIsActiveChange();
	}

	private void updateInstance(NetworkPacket.GameObjects.Lasergate.sUpdate pPacket)
	{
		_isActive.Value = pPacket.Active;
	}

	private void OnIsActiveChange()
	{
		Debug.Log("Im being called");
		if (_isActive.Value)
		{
			changeSprite(ActiveSprite);
			changeColor(Color.white);
		}
		else
		{
			changeSprite(inactiveSprite);
			changeColor(Color.grey);
		}
	}

	override public void OnMouseEnter()
	{
		HackerPackageSender.SendPackage(new NetworkPacket.GameObjects.Lasergate.hUpdate(Id, false));
		//base.OnMouseEnter();
	}

	override public void OnMouseExit()
	{
		HackerPackageSender.SendPackage(new NetworkPacket.GameObjects.Lasergate.hUpdate(Id, true));
		//base.OnMouseExit();
	}
}
