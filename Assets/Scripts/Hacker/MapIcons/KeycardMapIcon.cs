using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamelogic.Extensions;

public class KeycardMapIcon : AbstractMapIcon {

    private ObservedValue<bool> _used = new ObservedValue<bool>(false);
    private Color _keyColor;
    private List<DoorMapIcon> _doorList;

    public static void ProcessPacket(NetworkPacket.GameObjects.Keycard.Creation pPacket)
    {
        KeycardMapIcon icon = HackerPackageSender.GetNetworkedObject<KeycardMapIcon>(pPacket.Id);

        if (icon == null)
        {
            createInstance(pPacket);
        }
    }

    private static void createInstance(NetworkPacket.GameObjects.Keycard.Creation pPacket)
    {
        KeycardMapIcon icon = Instantiate(HackerReferenceManager.Instance.KeyCardIcon, new Vector3(pPacket.posX / MinimapManager.scale, pPacket.posY / MinimapManager.scale, 0), Quaternion.Euler(0, 0, 0)).GetComponent<KeycardMapIcon>();

        icon.Id = pPacket.Id;
        icon.SetKeyColor(new Color(pPacket.colorR, pPacket.colorG, pPacket.colorB));
        //icon.SetDoorArray(pPacket.intArray);

        //TODO Research why onValueChanged is not called
        //icon.stateChanged();
    }

    private void Awake()
    {
        _used.OnValueChange += StateChanged;
    }

    private void SetKeyColor(Color nKeyColor)
    {
        _keyColor = nKeyColor;
        changeColor(nKeyColor);
    }

    //private void SetDoorArray(int[] nArray)
    //{
    //    _doorList = new List<DoorMapIcon>();
    //    for(int i = 0; i<nArray.Length; i++)
    //    {
    //        _doorList.Add(HackerPackageSender.GetNetworkedObject<DoorMapIcon>(nArray[i]));
    //    }
    //
    //    foreach(DoorMapIcon d in _doorList)
    //    {
    //        d.SetKeyColor(_keyColor);
    //    }
    //}


    private void StateChanged()
    {
        gameObject.SetActive(!_used.Value);
    }
}
