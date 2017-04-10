using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamelogic.Extensions;

public class PickUpMapIcon : AbstractMapIcon
{

    private ObservedValue<bool> _used = new ObservedValue<bool>(false);

    public static void ProcessPacket(NetworkPacket.AbstractPacket pPacket)
    {
        if (pPacket is NetworkPacket.Create.PhaserAmmoIcon) { createInstance(pPacket as NetworkPacket.Create.PhaserAmmoIcon); }
        if (pPacket is NetworkPacket.Create.MachineGunAmmoIcon) { createInstance(pPacket as NetworkPacket.Create.MachineGunAmmoIcon); }
        if (pPacket is NetworkPacket.Create.ShotgunAmmoIcon) { createInstance(pPacket as NetworkPacket.Create.ShotgunAmmoIcon); }
        if (pPacket is NetworkPacket.Create.HealthKitIcon) { createInstance(pPacket as NetworkPacket.Create.HealthKitIcon); }
    }

    private static void createInstance(NetworkPacket.Create.PhaserAmmoIcon pPacket)
    {
        PickUpMapIcon icon = Instantiate(HackerReferenceManager.Instance.AmmoPhaser, new Vector3(pPacket.posX / MinimapManager.scale, pPacket.posY / MinimapManager.scale, 0), Quaternion.Euler(0, 0, 0)).GetComponent<PickUpMapIcon>();
        icon.Id = pPacket.Id;
    }
    private static void createInstance(NetworkPacket.Create.MachineGunAmmoIcon pPacket)
    {
        PickUpMapIcon icon = Instantiate(HackerReferenceManager.Instance.AmmoMachineGun, new Vector3(pPacket.posX / MinimapManager.scale, pPacket.posY / MinimapManager.scale, 0), Quaternion.Euler(0, 0, 0)).GetComponent<PickUpMapIcon>();
        icon.Id = pPacket.Id;
    }
    private static void createInstance(NetworkPacket.Create.ShotgunAmmoIcon pPacket)
    {
        PickUpMapIcon icon = Instantiate(HackerReferenceManager.Instance.AmmoShotgun, new Vector3(pPacket.posX / MinimapManager.scale, pPacket.posY / MinimapManager.scale, 0), Quaternion.Euler(0, 0, 0)).GetComponent<PickUpMapIcon>();
        icon.Id = pPacket.Id;
    }
    private static void createInstance(NetworkPacket.Create.HealthKitIcon pPacket)
    {
        PickUpMapIcon icon = Instantiate(HackerReferenceManager.Instance.HealthKit, new Vector3(pPacket.posX / MinimapManager.scale, pPacket.posY / MinimapManager.scale, 0), Quaternion.Euler(0, 0, 0)).GetComponent<PickUpMapIcon>();
        icon.Id = pPacket.Id;
    }

    private void Awake()
    {
        _used = new ObservedValue<bool>(false);
        _used.OnValueChange += StateChanged;
    }

    private void StateChanged()
    {
        Debug.Log("changed");
        gameObject.SetActive(!_used.Value);
    }
}
