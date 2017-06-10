using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuseboxMapIcon : AbstractMapIcon {
	[SerializeField]
	private Sprite _neutralSprite;

	[SerializeField]
	private Sprite _usedSprite;

	[SerializeField]
	private Sprite _primedSprite;

	private bool _used;

	public static void ProcessPacket(NetworkPacket.GameObjects.Fusebox.Creation pPacket) {
		FuseboxMapIcon icon = HackerPackageSender.GetNetworkedObject<FuseboxMapIcon>(pPacket.Id);

		if (icon == null) {
			createInstance(pPacket);
		} else {
			Debug.LogError("Fusebox with Id " + pPacket.Id + " already exists.");
		}

		
	}

	public static void ProcessPacket(NetworkPacket.GameObjects.Fusebox.sUpdate pPacket) {
		FuseboxMapIcon icon = HackerPackageSender.GetNetworkedObject<FuseboxMapIcon>(pPacket.Id);

		if (icon != null) {
			icon.updateInstance(pPacket);
		} else {
			Debug.LogError("Fusebox with Id " + pPacket.Id + " does not exist");
		}
	}

	private static void createInstance(NetworkPacket.GameObjects.Fusebox.Creation pPacket) {
		FuseboxMapIcon icon = Instantiate(HackerReferenceManager.Instance.FuseBoxIcon, new Vector3(pPacket.PosX, pPacket.PosY) / MinimapManager.scale, Quaternion.identity).GetComponent<FuseboxMapIcon>();

		icon.Id = pPacket.Id;
		icon._used = pPacket.Used;
		icon.determineSprite(pPacket.Charged);
	}

	private void updateInstance(NetworkPacket.GameObjects.Fusebox.sUpdate pPacket) {
		_used = pPacket.Used;
		//TODO Display effect effect with Target ID
		determineSprite(!_used);
	}

	private void determineSprite(bool pPrimed) {
		if (_used) {
			changeSprite(_usedSprite);
		} else {
			if (pPrimed) {
				changeSprite(_primedSprite);
			} else {
				changeSprite(_neutralSprite);
			}
		}
	}

	public void Prime() {
		sendUpdate(false);
		determineSprite(true);
	}

	public void PrimeCharged() {
		sendUpdate(true);
		determineSprite(true);
	}

	private void sendUpdate(bool pCharged) {
		HackerPackageSender.SendPackage(new NetworkPacket.GameObjects.Fusebox.hUpdate(Id, pCharged));
	}
}
