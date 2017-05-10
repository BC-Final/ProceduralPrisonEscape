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

	private static void createInstance(NetworkPacket.Create.SecurityStation pPacket) {
		SecuritystationMapIcon icon = Instantiate(HackerReferenceManager.Instance.SecurityStationIcon, new Vector3(pPacket.PosX / MinimapManager.scale, pPacket.PosY / MinimapManager.scale, 0), Quaternion.identity).GetComponent<SecuritystationMapIcon>();
		icon.Id = pPacket.Id;
	}

	public void Interaction() {
		HackerPackageSender.SendPackage(new NetworkPacket.Update.SecurityStation(Id));
	}
}
