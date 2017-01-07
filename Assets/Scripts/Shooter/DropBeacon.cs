using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DropBeacon : MonoBehaviour, INetworked {
	[SerializeField]
	public List<ShooterDoor> _doors;
    [SerializeField]
    ColorEnum.KeyColor keyColor;

    private Timers.Timer _sendTimer;


    

	private void Start() {
		foreach (ShooterDoor d in _doors) {
			d.SetRequireKeyCard();
		}

		_sendTimer = Timers.CreateTimer()
			.SetTime(0.5f)
			.SetCallback(() => ShooterPackageSender.SendPackage(new CustomCommands.Update.Items.ItemUpdate(Id, false, transform.position)))
			.SetLoop(-1);
	}

	private int _id;
	public int Id {
		get {
			if (_id == 0) {
				_id = IdManager.RequestId();
			}

			return _id;
		}
	}

	public void Initialize () {
        int[] intArray;
        List<int> tempArray = new List<int>();
        foreach(ShooterDoor door in _doors)
        {
            tempArray.Add(door.Id);
        }
        intArray = tempArray.ToArray();
		ShooterPackageSender.SendPackage(new CustomCommands.Creation.Items.KeyCardCreation(Id, intArray, transform.position.x, transform.position.z, (int)keyColor));

		_sendTimer.Start();
	}

	private void Awake () {
		ShooterPackageSender.RegisterNetworkObject(this);
	}

	private void OnDestroy () {
		ShooterPackageSender.UnregisterNetworkedObject(this);
	}

	public void Drop() {
		if (_doors != null && _doors.Count > 0) {
			GameObject go = Instantiate(ShooterReferenceManager.Instance.DroneBeacon, transform.position, Quaternion.identity) as GameObject;
			go.GetComponent<KeyCard>().SetDoors(_doors);
			_sendTimer.Stop();
			ShooterPackageSender.SendPackage(new CustomCommands.Update.Items.ItemUpdate(Id, true, transform.position));
		}
	}
}
