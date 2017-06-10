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


	public static void ProcessPacket (NetworkPacket.GameObjects.Module.sUpdate pPacket) {
		ModuleMapIcon icon = HackerPackageSender.GetNetworkedObject<ModuleMapIcon>(pPacket.Id);

		if (icon != null) {
			icon.updateInstance(pPacket);
		} else {
			Debug.LogError("Module with Id " + pPacket.Id + " does not exist");
		}
	}

	public static void ProcessPacket(NetworkPacket.GameObjects.Module.Creation pPacket) {
		ModuleMapIcon icon = HackerPackageSender.GetNetworkedObject<ModuleMapIcon>(pPacket.Id);

		if (icon == null) {
			createInstance(pPacket);
		} else {
			Debug.LogError("Module with Id " + pPacket.Id + " already exists.");
		}
	}

	private static void createInstance (NetworkPacket.GameObjects.Module.Creation pPacket) {
		ModuleMapIcon icon = Instantiate(HackerReferenceManager.Instance.ModuleIcon, new Vector3(pPacket.PosX / MinimapManager.scale, pPacket.PosY / MinimapManager.scale, 0), Quaternion.Euler(0, 0, -pPacket.Rot)).GetComponent<ModuleMapIcon>();
		icon.Id = pPacket.Id;
		icon.determineSprite(pPacket.Solved);
	}

	private void updateInstance (NetworkPacket.GameObjects.Module.sUpdate pPacket) {
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