using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamelogic.Extensions;
using System;

public class GaspipeMapIcon : AbstractMapIcon {
	#region Sprites
	[Header("Sprites")]

	[SerializeField]
	private Sprite _normalSprite;

	[SerializeField]
	private Sprite _explodedSprite;
	#endregion

	public static void ProcessPacket(NetworkPacket.GameObjects.Gaspipe.Creation pPacket) {
		GaspipeMapIcon icon = HackerPackageSender.GetNetworkedObject<GaspipeMapIcon>(pPacket.Id);

		if (icon == null) {
			createInstance(pPacket);
		} else {
			Debug.LogError("Module with Id " + pPacket.Id + " already exists.");
		}
	}

	public static void ProcessPacket (NetworkPacket.GameObjects.Gaspipe.sUpdate pPacket) {
		GaspipeMapIcon icon = HackerPackageSender.GetNetworkedObject<GaspipeMapIcon>(pPacket.Id);

		if (icon != null) {
			icon.updateInstance(pPacket);
		} else {
			Debug.LogError("Module with Id " + pPacket.Id + " does not exist");
		}
	}

	private static void createInstance (NetworkPacket.GameObjects.Gaspipe.Creation pPacket) {
		GaspipeMapIcon icon = Instantiate(HackerReferenceManager.Instance.GasPipeIcon, new Vector3(pPacket.PosX / MinimapManager.scale, pPacket.PosY / MinimapManager.scale, 0), Quaternion.Euler(0, 0, -pPacket.Rot)).GetComponent<GaspipeMapIcon>();

		icon._used.OnValueChange += icon.changedState;

		icon.Id = pPacket.Id;
		icon._used.Value = pPacket.Exploded;
	}

	private void updateInstance (NetworkPacket.GameObjects.Gaspipe.sUpdate pPacket) {
		_used.Value = pPacket.Exploded;
        GaspipeMapIcon icon = HackerPackageSender.GetNetworkedObject<GaspipeMapIcon>(pPacket.Id);
        Array.Clear(icon.actions, 0, icon.actions.Length);
    }

	private ObservedValue<bool> _used = new ObservedValue<bool>(false);

	public void NormalExplosion () {
        if (!_used.Value)
        {          
            sendUpdate(false);
        }      
	}

	public void BigExplosion () {
		//TODO Start ticking animation
		if (!_used.Value) {
            sendUpdate(true);
		}
	}

	private void sendUpdate (bool pChargedExplosion) {
		HackerPackageSender.SendPackage(new NetworkPacket.GameObjects.Gaspipe.hUpdate(Id, pChargedExplosion));
	}

	private void changedState () {
		if (!_used.Value) {
			changeSprite(_normalSprite);
		} else {
            Instantiate(HackerReferenceManager.Instance.Explosion, this.transform.position, Quaternion.identity);
            changeSprite(_explodedSprite);
        }
	}
}
