using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModuleMapIcon : AbstractMapIcon {
	[SerializeField]
	private Sprite _neutralSprite;

	[SerializeField]
	private Sprite _solvedSprite;

	[SerializeField]
	private Sprite _activeSprite;


	public static void ProcessPacket (NetworkPacket.Update.Module pPacket) {
		ModuleMapIcon icon = HackerPackageSender.GetNetworkedObject<ModuleMapIcon>(pPacket.Id);

		if (icon == null) {
			createInstance(pPacket);
		} else {
			icon.updateInstance(pPacket);
		}
	}

	private static void createInstance (NetworkPacket.Update.Module pPacket) {
		ModuleMapIcon icon = Instantiate(HackerReferenceManager.Instance.ModuleIcon, new Vector3(pPacket.PosX / MinimapManager.scale, pPacket.PosY / MinimapManager.scale, 0), Quaternion.Euler(0, 0, -pPacket.Rot)).GetComponent<ModuleMapIcon>();
		icon.Id = pPacket.Id;
		icon.determineSprite(pPacket.Solved);
	}

	private void updateInstance (NetworkPacket.Update.Module pPacket) {
		//TODO Add Sctive State
		determineSprite(pPacket.Solved);
	}

	private void determineSprite (bool pSolved) {
		if (pSolved) {
			changeSprite(_solvedSprite);
		} else {
			changeSprite(_neutralSprite);
		}
	}
}