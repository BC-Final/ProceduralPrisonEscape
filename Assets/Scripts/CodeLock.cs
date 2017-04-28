using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CodeLock : MonoBehaviour, IShooterNetworked
{
    [SerializeField]
    private string _codeString;
    private ShooterDoor _door;

    public static void ProcessPacket(NetworkPacket.Update.CodeLockCode pPacket)
    {
        ShooterPackageSender.GetNetworkedObject<CodeLock>(pPacket.Id).UseCode(pPacket.codeString);
    }


    private int _id;

    public int Id
    {
        get
        {
            if (_id == 0)
            {
                _id = IdManager.RequestId();
            }

            return _id;
        }
    }
    public void Initialize()
    {
        ShooterPackageSender.SendPackage(new NetworkPacket.Create.CodeLockAddon(Id, GetComponent<ShooterDoor>().Id, _codeString));
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
