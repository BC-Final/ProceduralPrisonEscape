using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecuritystationMapIcon : AbstractMapIcon {

	public static void ProcessPacket(NetworkPacket.Create.SecurityStation pPacket) {
		SecuritystationMapIcon icon = HackerPackageSender.GetNetworkedObject<SecuritystationMapIcon>(pPacket.Id);

		if (icon == null) {
			createInstance(pPacket);
		}
	}

	public static void ProcessPacket(NetworkPacket.Update.SecurityStation pPacket)
	{
		SecuritystationMapIcon icon = HackerPackageSender.GetNetworkedObject<SecuritystationMapIcon>(pPacket.Id);

		if (icon == null)
		{
			icon.ChangeState((SecurityStation.StationState)pPacket.state);
		}
	}

	public void ChangeState(SecurityStation.StationState state)
	{
		switch (state)
		{
			case SecurityStation.StationState.Passive: changeColor(Color.white); break;
			case SecurityStation.StationState.Triggerd: changeColor(Color.red); break;
			case SecurityStation.StationState.HalfDeactivated: changeColor(Color.yellow); break;
			case SecurityStation.StationState.Deactivated: changeColor(Color.green); break;
		}
	}

	private static void createInstance(NetworkPacket.Create.SecurityStation pPacket) {
		SecuritystationMapIcon icon = Instantiate(HackerReferenceManager.Instance.SecurityStationIcon, new Vector3(pPacket.PosX / MinimapManager.scale, pPacket.PosY / MinimapManager.scale, 0), Quaternion.identity).GetComponent<SecuritystationMapIcon>();
		icon.Id = pPacket.Id;
	}



	public void Interaction() {
		HackerPackageSender.SendPackage(new NetworkPacket.Update.SecurityStationHackerInteract(Id));
	}
}
