using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerminalMapIcon : AbstractMapIcon {

	public static void ProcessPacket(NetworkPacket.GameObjects.Terminal.Creation pPacket)
	{
		CameraMapIcon icon = HackerPackageSender.GetNetworkedObject<CameraMapIcon>(pPacket.Id);

		if (icon == null)
		{
			createInstance(pPacket);
		}
		else
		{
			Debug.Log("TERMINAL ALREADY EXISTS!!! ID: " + pPacket.Id);
		}
	}
	private static void createInstance(NetworkPacket.GameObjects.Terminal.Creation pPacket)
	{
		TerminalMapIcon icon = Instantiate(HackerReferenceManager.Instance.TerminalIcon, new Vector3(pPacket.PosX / MinimapManager.scale, pPacket.PosY / MinimapManager.scale, 0), Quaternion.Euler(0, 0, -0)).GetComponent<TerminalMapIcon>();

		icon.Id = pPacket.Id;
	}
}
