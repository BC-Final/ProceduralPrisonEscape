using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamelogic.Extensions;

public class FirewallMapIcon : AbstractMapIcon {

	private ObservedValue<bool> _active = new ObservedValue<bool>(false);

	public static void ProcessPacket(NetworkPacket.Create.Firewall pPacket)
	{
		FirewallMapIcon icon = HackerPackageSender.GetNetworkedObject<FirewallMapIcon>(pPacket.Id);

		if (icon == null)
		{
			createInstance(pPacket);
		}
		else
		{
			icon.updateInstance(pPacket);
		}
	}

	public static void ProcessPacket(NetworkPacket.Update.Firewall pPacket)
	{
		FirewallMapIcon icon = HackerPackageSender.GetNetworkedObject<FirewallMapIcon>(pPacket.Id);
		Debug.Log("I'm reading the packet!");
		if (icon)
		{
			icon.updateInstance(pPacket);
		}
	}

	private static void createInstance(NetworkPacket.Create.Firewall pPacket)
	{
		FirewallMapIcon icon = Instantiate(HackerReferenceManager.Instance.Firewall, new Vector3(pPacket.PosX / MinimapManager.scale, pPacket.PosY / MinimapManager.scale, 0), 
																								Quaternion.Euler(0, 0, 0)).GetComponent<FirewallMapIcon>();

		icon._active.OnValueChange += icon.changeSprite;

		icon.Id = pPacket.Id;
		icon._active.Value = pPacket.Active;
		icon.changeSprite();
	}

	private void updateInstance(NetworkPacket.Create.Firewall pPacket)
	{
		_active.Value = pPacket.Active;
	}
	private void updateInstance(NetworkPacket.Update.Firewall pPacket)
	{
		_active.Value = pPacket.Active;
	}

	private void changeSprite()
	{
		Debug.Log("changing Sprite");
		if (!_active.Value)
		{
			changeSprite(HackerReferenceManager.Instance.FirewallOffSprie);
		}
	}
}
