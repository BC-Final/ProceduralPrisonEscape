using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CodeLock : MonoBehaviour, IShooterNetworked
{
    [SerializeField]
    private string _codeString;
    private ShooterDoor _door;

    public static void ProcessPacket(NetworkPacket.GameObjects.Door.Addons.hCodeLockUpdate pPacket)
    {
        ShooterPackageSender.GetNetworkedObject<CodeLock>(pPacket.Id).UseCode(pPacket.codeString);
    }


	private ShooterNetworkId _id = new ShooterNetworkId();
	public ShooterNetworkId Id { get { return _id; } }

	public void Initialize()
    {
        ShooterPackageSender.SendPackage(new NetworkPacket.GameObjects.Door.Addons.AddCodeLockAddon(Id, GetComponent<ShooterDoor>().Id, _codeString));
    }
    
    public void UseCode(string codeString)
    {
        Debug.Log("Code: " + codeString + "/" + "Solution: " + _codeString);
        if (codeString.ToUpper() == _codeString.ToUpper())
        {
            _door.ForceOpen();
        }
    }

    private void Awake()
    {
        _door = GetComponent<ShooterDoor>();
        ShooterPackageSender.RegisterNetworkObject(this);
    }
}
