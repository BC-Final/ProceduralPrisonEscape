﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushButtonMapIcon : AbstractMapIcon
{
    public static void ProcessPacket(NetworkPacket.Create.PushButton pPacket)
    {
        createInstance(pPacket);
    }

    public static void ProcessPacket(NetworkPacket.Update.ButtonFeedback pPacket)
    {
        PushButtonMapIcon icon = HackerPackageSender.GetNetworkedObject<PushButtonMapIcon>(pPacket.Id);
        icon.changeColor(new Color(pPacket.colorR, pPacket.colorG, pPacket.colorB));
    }

    private static void createInstance(NetworkPacket.Create.PushButton pPacket)
    {
        PushButtonMapIcon icon = Instantiate(HackerReferenceManager.Instance.PushButton, new Vector3(pPacket.posX / MinimapManager.scale, pPacket.posY / MinimapManager.scale, 0), Quaternion.Euler(0, 0, 0)).GetComponent<PushButtonMapIcon>();

        icon.Id = pPacket.Id;
    }

    public void Push()
    {
        HackerPackageSender.SendPackage(new NetworkPacket.Update.ButtonPush(Id));
    }
}
