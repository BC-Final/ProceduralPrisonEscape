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

	public static void ProcessPacket(NetworkPacket.Create.Fusebox pPacket) {
		createInstance(pPacket);
	}

	public static void ProcessPacket(NetworkPacket.Update.Fusebox pPacket) {
		FuseboxMapIcon icon = HackerPackageSender.GetNetworkedObject<FuseboxMapIcon>(pPacket.Id);

		if (icon == null) {
			Debug.LogError("No Fusebox with id " + pPacket.Id + " found!");
		} else {
			icon.updateInstance(pPacket);
		}
	}

	private static void createInstance(NetworkPacket.Create.Fusebox pPacket) {
		FuseboxMapIcon icon = Instantiate(HackerReferenceManager.Instance.FuseBoxIcon, new Vector3(pPacket.PosX, pPacket.PosY) / MinimapManager.scale, Quaternion.identity).GetComponent<FuseboxMapIcon>();

		icon.Id = pPacket.Id;
		icon._used = pPacket.Used;
		icon.determineSprite(pPacket.Charged);
	}

	private void updateInstance(NetworkPacket.Update.Fusebox pPacket) {
		_used = pPacket.Charged;
		determineSprite(false);
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
		HackerPackageSender.SendPackage(new NetworkPacket.Update.Fusebox(Id, pCharged));
	}
}
