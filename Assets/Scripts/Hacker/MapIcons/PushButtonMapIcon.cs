using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushButtonMapIcon : AbstractMapIcon
{
	[SerializeField]
	SpriteRenderer _hackerButton;
	[SerializeField]
	SpriteRenderer _shooterButton;

	public static void ProcessPacket(NetworkPacket.GameObjects.Button.Creation pPacket)
    {
        createInstance(pPacket);
    }

    public static void ProcessPacket(NetworkPacket.GameObjects.Button.sUpdate pPacket)
    {
		PushButtonMapIcon icon = HackerPackageSender.GetNetworkedObject<PushButtonMapIcon>(pPacket.Id);
		SpriteRenderer button = null;
		if(pPacket.ButtonNumber == 0)
		{
			button = icon._hackerButton;
		}
		else
		{
			button = icon._shooterButton;
		}
        button.color = (new Color(pPacket.ColorR, pPacket.ColorG, pPacket.ColorB));
    }

    private static void createInstance(NetworkPacket.GameObjects.Button.Creation pPacket)
    {
        PushButtonMapIcon icon = Instantiate(HackerReferenceManager.Instance.PushButton, new Vector3(pPacket.PosX / MinimapManager.scale, pPacket.PosY / MinimapManager.scale, 0), Quaternion.Euler(0, 0, 0)).GetComponent<PushButtonMapIcon>();

        icon.Id = pPacket.Id;
    }

    public void Push()
    {
        HackerPackageSender.SendPackage(new NetworkPacket.GameObjects.Button.hUpdate(Id));
    }
}
