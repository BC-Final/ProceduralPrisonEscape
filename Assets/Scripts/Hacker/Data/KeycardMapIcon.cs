using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeycardMapIcon : AbstractMapIcon {

    private bool _used;
    private Color _keyColor;
    private List<DoorMapIcon> _doorList;

    public static void ProcessPacket(NetworkPacket.Create.KeyCard pPacket)
    {
        KeycardMapIcon icon = HackerPackageSender.GetNetworkedObject<KeycardMapIcon>(pPacket.Id);

        if (icon == null)
        {
            createInstance(pPacket);
        }
    }
    public static void ProcessPacket(NetworkPacket.Update.Icon pPacket)
    {
        KeycardMapIcon icon = HackerPackageSender.GetNetworkedObject<KeycardMapIcon>(pPacket.Id);

        if (icon != null)
        {
            icon.updateInstance(pPacket);
        }
    }

    private static void createInstance(NetworkPacket.Create.KeyCard pPacket)
    {
        KeycardMapIcon icon = Instantiate(HackerReferenceManager.Instance.KeyCardIcon, new Vector3(pPacket.posX / MinimapManager.scale, pPacket.posY / MinimapManager.scale, 0), Quaternion.Euler(0, 0, 0)).GetComponent<KeycardMapIcon>();

        icon.Id = pPacket.Id;
        icon.SetKeyColor(new Color(pPacket.colorR, pPacket.colorG, pPacket.colorB));
        icon.SetDoorArray(pPacket.intArray);

        //TODO Research why onValueChanged is not called
        //icon.stateChanged();
    }

    private void SetKeyColor(Color nKeyColor)
    {
        _keyColor = nKeyColor;
    }

    private void SetDoorArray(int[] nArray)
    {
        _doorList = new List<DoorMapIcon>();
        for(int i = 0; i<nArray.Length; i++)
        {
            _doorList.Add(HackerPackageSender.GetNetworkedObject<DoorMapIcon>(nArray[Id]));
        }

        foreach(DoorMapIcon d in _doorList)
        {
            d.SetKeyColor(_keyColor);
        }
    }

    private void updateInstance(NetworkPacket.Update.Icon pPacket)
    {
        _used = pPacket.used;
    }
}
