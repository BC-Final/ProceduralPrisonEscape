using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamelogic.Extensions;

public class DoorMapIcon : AbstractMapIcon
{

    public static void ProcessPacket(NetworkPacket.Update.Door pPacket)
    {
        DoorMapIcon icon = HackerPackageSender.GetNetworkedObject<DoorMapIcon>(pPacket.Id);

        if (icon == null)
        {
            createInstance(pPacket);
        }
        else
        {
            icon.updateInstance(pPacket);
        }
    }

    private static void createInstance(NetworkPacket.Update.Door pPacket)
    {
        DoorMapIcon icon = Instantiate(HackerReferenceManager.Instance.DoorIcon, new Vector3(pPacket.PosX / MinimapManager.scale, pPacket.PosY / MinimapManager.scale, 0), Quaternion.Euler(0, 0, -pPacket.Rot)).GetComponent<DoorMapIcon>();

        icon.Id = pPacket.Id;
        icon._open.Value = pPacket.Open;
        icon._locked.Value = pPacket.Locked;

        //TODO Research why onValueChanged is not called
        icon.stateChanged();
    }

    private void updateInstance(NetworkPacket.Update.Door pPacket)
    {
        _open.Value = pPacket.Open;
        _locked.Value = pPacket.Locked;
    }

    private void Awake()
    {
        _open.OnValueChange += stateChanged;
        _locked.OnValueChange += stateChanged;
    }

    private bool _keycardLocked = false;
    private Color _keyColor;
    private ObservedValue<bool> _open = new ObservedValue<bool>(false);
    private ObservedValue<bool> _locked = new ObservedValue<bool>(false);
    [SerializeField]
    private Color _lockColor;

    #region Sprites
    [Header("Sprites")]
    [SerializeField]
    private Sprite _openSprite;
    [SerializeField]
    private Sprite _closedSprite;
    //[SerializeField]
    //private Sprite _lockedOpenSprite;
    //[SerializeField]
    //private Sprite _lockedClosedSprite;
    #endregion

    public void Toggle()
    {
        //TODO Send state update but only change when receiving (if it causes problems)
        _open.Value = !_open.Value;

        sendUpdate();
    }

    public void Lock()
    {
        _locked.Value = true;

        sendUpdate();
    }

    public void InputCode()
    {

    }

    public void SetKeyColor(Color nColor)
    {
        _keycardLocked = true;
        _keyColor = nColor;
        stateChanged();
    }

    private void sendUpdate()
    {
        HackerPackageSender.SendPackage(new NetworkPacket.Update.Door(Id, _open.Value, _locked.Value));
    }

    private void stateChanged()
    {
        if (_open.Value)
        {
            changeSprite(_openSprite);
            if (_locked.Value)
            {
                changeColor(Color.red);
            }
            else if (_keycardLocked)
            {
                changeColor(_keyColor);
            }
        }
        else
        {
            changeSprite(_closedSprite);
            if (_locked.Value)
            {
                changeColor(Color.red);
            }
            else if (_keycardLocked)
            {
                changeColor(_keyColor);
            }
        }
    }
}
