using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecuritystationMapIcon : AbstractMapIcon {

	public static void ProcessPacket(NetworkPacket.GameObjects.SecurityStation.Creation pPacket) {
		SecuritystationMapIcon icon = HackerPackageSender.GetNetworkedObject<SecuritystationMapIcon>(pPacket.Id);

		if (icon == null) {
			createInstance(pPacket);
		}
	}

	public static void ProcessPacket(NetworkPacket.GameObjects.SecurityStation.sUpdate pPacket)
	{
		SecuritystationMapIcon icon = HackerPackageSender.GetNetworkedObject<SecuritystationMapIcon>(pPacket.Id);
		if (icon != null)
		{
			icon.ChangeState((ShooterSecurityStation.StationState)pPacket.State);
		}
	}

	public void ChangeState(ShooterSecurityStation.StationState state)
	{
		switch (state)
		{
			case ShooterSecurityStation.StationState.Passive: actions[0].Disabled = true; IsInteractable = false; changeColor(Color.white); break;
			case ShooterSecurityStation.StationState.Triggerd: actions[0].Disabled = true; IsInteractable = false; changeColor(Color.red); break;
			case ShooterSecurityStation.StationState.HalfDeactivated: actions[0].Disabled = false; IsInteractable = true; changeColor(Color.yellow); break;
			case ShooterSecurityStation.StationState.Deactivated: actions[0].Disabled = true; IsInteractable = false; changeColor(Color.green); break;
		}
	}

	private static void createInstance(NetworkPacket.GameObjects.SecurityStation.Creation pPacket) {
		SecuritystationMapIcon icon = Instantiate(HackerReferenceManager.Instance.SecurityStationIcon, new Vector3(pPacket.PosX / MinimapManager.scale, pPacket.PosY / MinimapManager.scale, 0), Quaternion.identity).GetComponent<SecuritystationMapIcon>();
		icon.Id = pPacket.Id;
	}



	public void Interaction() {
		HackerPackageSender.SendPackage(new NetworkPacket.GameObjects.SecurityStation.hUpdate(Id));
	}
}
