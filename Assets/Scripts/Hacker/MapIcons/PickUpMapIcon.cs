using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamelogic.Extensions;

public class PickUpMapIcon : AbstractMapIcon
{

    private ObservedValue<bool> _used = new ObservedValue<bool>(false);

    public static void ProcessPacket(NetworkPacket.AbstractPacket pPacket)
    {
        if (pPacket is NetworkPacket.GameObjects.PickUpIcon.PhaserAmmoCreation) { createInstance(pPacket as NetworkPacket.GameObjects.PickUpIcon.PhaserAmmoCreation); }
        if (pPacket is NetworkPacket.GameObjects.PickUpIcon.MachineGunAmmoCreation) { createInstance(pPacket as NetworkPacket.GameObjects.PickUpIcon.MachineGunAmmoCreation); }
        if (pPacket is NetworkPacket.GameObjects.PickUpIcon.ShotgunAmmoCreation) { createInstance(pPacket as NetworkPacket.GameObjects.PickUpIcon.ShotgunAmmoCreation); }
        if (pPacket is NetworkPacket.GameObjects.PickUpIcon.HealthKitCreation) { createInstance(pPacket as NetworkPacket.GameObjects.PickUpIcon.HealthKitCreation); }
    }

    private static void createInstance(NetworkPacket.GameObjects.PickUpIcon.PhaserAmmoCreation pPacket)
    {
        PickUpMapIcon icon = Instantiate(HackerReferenceManager.Instance.AmmoPhaser, new Vector3(pPacket.posX / MinimapManager.scale, pPacket.posY / MinimapManager.scale, 0), Quaternion.Euler(0, 0, 0)).GetComponent<PickUpMapIcon>();
        icon.Id = pPacket.Id;
    }
    private static void createInstance(NetworkPacket.GameObjects.PickUpIcon.MachineGunAmmoCreation pPacket)
    {
        PickUpMapIcon icon = Instantiate(HackerReferenceManager.Instance.AmmoMachineGun, new Vector3(pPacket.posX / MinimapManager.scale, pPacket.posY / MinimapManager.scale, 0), Quaternion.Euler(0, 0, 0)).GetComponent<PickUpMapIcon>();
        icon.Id = pPacket.Id;
    }
    private static void createInstance(NetworkPacket.GameObjects.PickUpIcon.ShotgunAmmoCreation pPacket)
    {
        PickUpMapIcon icon = Instantiate(HackerReferenceManager.Instance.AmmoShotgun, new Vector3(pPacket.posX / MinimapManager.scale, pPacket.posY / MinimapManager.scale, 0), Quaternion.Euler(0, 0, 0)).GetComponent<PickUpMapIcon>();
        icon.Id = pPacket.Id;
    }
    private static void createInstance(NetworkPacket.GameObjects.PickUpIcon.HealthKitCreation pPacket)
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
