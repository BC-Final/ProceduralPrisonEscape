using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamelogic.Extensions;

public class ShooterLasergate : MonoBehaviour, IShooterNetworked
{

	private ShooterNetworkId _id = new ShooterNetworkId();
	public ShooterNetworkId Id { get { return _id; } }

	public void Initialize()
	{
		ShooterPackageSender.SendPackage(new NetworkPacket.GameObjects.Lasergate.Creation(Id, this.transform.position.x, this.transform.position.z, transform.rotation.eulerAngles.y, _isActive.Value));
	}

	private void Awake()
	{
		ShooterPackageSender.RegisterNetworkObject(this);
	}

	public static void ProcessPacket(NetworkPacket.GameObjects.Lasergate.hUpdate pPacket)
	{
		ShooterLasergate gate = ShooterPackageSender.GetNetworkedObject<ShooterLasergate>(pPacket.Id);
	
		if (gate != null)
		{
			gate._isActive.Value = pPacket.Active;
		}
		else
		{
			Debug.LogError("Trying to access non existent networked object with id " + pPacket.Id);
		}
	}

	private List<LineRenderer> _lasers;
	private Light _light;
	private ObservedValue<bool> _isActive = new ObservedValue<bool>(true);
	private Timer _timer;

	void Start()
	{
		_timer = TimerManager.CreateTimer("LaserResetTimer", false);
		_timer.SetDuration(0.5f);
		_lasers = new List<LineRenderer>();
		foreach (LineRenderer lr in GetComponentsInChildren<LineRenderer>())
		{
			_lasers.Add(lr);
		}
		_light = GetComponentInChildren<Light>();
		_isActive.OnValueChange += OnIsActiveChange;
		SetLight(_isActive.Value);
		_timer.AddCallback(() => ApplyChange(), true);
	}

	private void OnTriggerStay(Collider other)
	{
		IDamageable dam = other.GetComponentInParent<IDamageable>();
		if (dam != null && _isActive.Value)
		{
			dam.ReceiveDamage(this.transform, this.transform.position, 150 * Time.deltaTime, 0);
		}
	}

	private void OnIsActiveChange()
	{
		if (_isActive.Value)
		{
			//turn on is delayed
			_timer.Reset();
			_timer.Start();
		}
		else
		{
			//turn off is instant
			ApplyChange();
		}
	}

	private void ApplyChange()
	{
		SetLight(_isActive.Value);
		ShooterPackageSender.SendPackage(new NetworkPacket.GameObjects.Lasergate.sUpdate(Id, _isActive.Value));
	}

	private void SetLight(bool state)
	{
		foreach(LineRenderer lr in _lasers)
		{
			lr.enabled = state;
		}
		_light.enabled = state;
	}
}
