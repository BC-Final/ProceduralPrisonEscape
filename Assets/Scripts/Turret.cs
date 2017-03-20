using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateFramework;
using System.Linq;

public class Turret : MonoBehaviour, IShooterNetworked {
	//TODO Put into scriptable object

	[SerializeField]
	private Transform _rotaryBase;
	public Transform RotaryBase { get { return _rotaryBase; } }

	[SerializeField]
	private Transform _gun;
	public Transform Gun { get { return _gun; } }

	[SerializeField]
	private float _seeRange;
	public float SeeRange { get { return _seeRange; } }

	[SerializeField]
	private float _attackRange;
	public float AttackRange { get { return _attackRange; } }

	[SerializeField]
	private float _attackAngle;
	public float AttackAngle { get { return _attackAngle; } }

	[SerializeField]
	private float _fireRate;
	public float FireRate { get { return _fireRate; } }

	[SerializeField]
	private float _damage;
	public float Damage { get { return _damage; } }

	[SerializeField]
	private float _deployTime;
	public float DeployTime { get { return _deployTime; } }

	[SerializeField]
	private float _scanTime;
	public float ScanTime { get { return _scanTime; } }

	[SerializeField]
	private float _spreadConeLength;
	public float SpreadConeLength { get { return _spreadConeLength; } }

	[SerializeField]
	private float _spreadConeRadius;
	public float SpreadConeRadius { get { return _spreadConeRadius; } }

	[SerializeField]
	private float _deployedReactionTime;
	public float DeployedReactionTime { get { return _deployedReactionTime; } }

	[SerializeField]
	private float _horizontalRotationSpeed;
	public float HorizontalRotationSpeed { get { return _horizontalRotationSpeed; } }

	[SerializeField]
	private float _verticalRotationSpeed;
	public float VerticalRotationSpeed { get { return _verticalRotationSpeed; } }

	[SerializeField]
	private float _quitIdleRange;
	public float QuitIdleRange { get { return _quitIdleRange; } }

	[SerializeField]
	private float _scanRotationSpeed;
	public float ScanRotaionSpeed { get { return _scanRotationSpeed; } }

	[SerializeField]
	private float _scanRotationAngle;
	public float ScanRotationAngle { get { return _scanRotationAngle; } }


	[SerializeField]
	private bool _visualizeView;

	[SerializeField]
	private float _disabledTime = 5.0f;
	public float DisabledTime { get { return _disabledTime; } }

	[SerializeField]
	private Transform _shootPos;
	public Transform ShootPos { get { return _shootPos; } }

	[SerializeField]
	private float _maxGunRotation;
	public float MaxGunRotation { get { return _maxGunRotation; } }

	[SerializeField]
	private float _positionSendRate = 0.5f;
	private float _positionSendTimer = 0.0f;

	private StateMachine<AbstractTurretState> _fsm;

	[SerializeField]
	private float _controllTime = 10.0f;

	private bool _controlled = false;
	public bool Controlled { get { return _controlled; } }

	private bool _seesTarget = false;
	public bool SeesTarget { set { _seesTarget = value; } }

	private List<GameObject> _targets = new List<GameObject>();
	public GameObject[] Targets {
		get { return _targets.ToArray(); }
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

	public static void ProcessPacket (NetworkPacket.Update.Turret pPacket) {
		Turret tur = ShooterPackageSender.GetNetworkedObject<Turret>(pPacket.Id);

		if (tur != null) {
			switch (pPacket.State) {
				case EnemyState.Stunned:
					tur.disable();
					break;
				case EnemyState.Controlled:
					tur.control();
					break;
			}
		} else {
			Debug.LogError("Trying to access non existent networked object with id " + pPacket.Id);
		}
	}

	public void Initialize () {
		sendUpdate();
	}

	private void Awake () {
		ShooterPackageSender.RegisterNetworkObject(this);
	}

	private void OnDestroy () {
		ShooterPackageSender.UnregisterNetworkedObject(this);
	}


	private void Start () {
		_fsm = new StateMachine<AbstractTurretState>();

		_fsm.AddState(new TurretIdleState(this, _fsm));
		_fsm.AddState(new TurretGuardState(this, _fsm));
		_fsm.AddState(new TurretDeployState(this, _fsm));
		_fsm.AddState(new TurretEngageState(this, _fsm));
		_fsm.AddState(new TurretAttackState(this, _fsm));
		_fsm.AddState(new TurretScanState(this, _fsm));
		_fsm.AddState(new TurretHideState(this, _fsm));
		_fsm.AddState(new TurretDisabledState(this, _fsm));
		//TODO Add Controlled state

		_fsm.SetState<TurretIdleState>();

		SphereCollider[] sc = GetComponentsInChildren<SphereCollider>();

		foreach (SphereCollider s in sc) {
			if (s.gameObject.layer == LayerMask.NameToLayer("InteractionTrigger")) {
				s.radius = _seeRange;
			}
		}
	}

	private void sendUpdate () {
		//TODO Make the state check easier!!!
		EnemyState currState = _fsm.GetState() is TurretDisabledState ? EnemyState.Stunned : (_controlled ? EnemyState.Controlled : (_seesTarget ? EnemyState.SeesPlayer : EnemyState.None)); 
		ShooterPackageSender.SendPackage(new NetworkPacket.Update.Turret(Id, transform.position.x, transform.position.z, _rotaryBase.rotation.eulerAngles.y, currState));
	}

	private void Update () {
		//HACK Remove this later
		if (_positionSendTimer - Time.time <= 0.0f) {
			_positionSendTimer = Time.time + _positionSendRate;
			sendUpdate();
		}

		if (_controlled) {
			_targets = _targets.OrderBy(x => Vector3.Distance(transform.position, x.transform.position)).ToList();
		}

		_fsm.Step();
	}

	private void control () {
		_controlled = true;
		//Timers.CreateTimer("Turret Controll").SetTime(_controllTime).SetCallback(() => _controlled = false).Start();
		//TODO Switch to controlled state
	}

	private void disable () {
		_fsm.SetState<TurretDisabledState>();
	}

	private void OnTriggerEnter (Collider other) {
		if (other.GetComponentInParent<IDamageable>() != null && other.GetComponentInParent<PlayerHealth>() == null) {
			Debug.Log("Added");
			//HACK
			_targets.Add(other.GetComponentInParent<DroneEnemy>().gameObject);
		}
	}

	private void OnTriggerExit (Collider other) {
		if (other.GetComponentInParent<IDamageable>() != null && other.GetComponentInParent<PlayerHealth>() == null) {
			Debug.Log("Removed");
			_targets.RemoveAll(x => x == other.GetComponentInParent<DroneEnemy>().gameObject);
		}
	}

#if UNITY_EDITOR
	private void OnDrawGizmos () {
		if (_visualizeView) {
			Gizmos.color = Color.white;
			UnityEditor.Handles.color = Color.white;

			UnityEditor.Handles.DrawWireDisc(transform.position, -transform.up, _seeRange);

			if (_attackAngle < 360.0f) {
				Gizmos.DrawLine(_shootPos.position, _shootPos.position + Quaternion.Euler(_shootPos.TransformDirection(0f, -(_attackAngle / 2f), 0f)) * (_shootPos.forward * _attackRange));
				Gizmos.DrawLine(_shootPos.position, _shootPos.position + Quaternion.Euler(_shootPos.TransformDirection(0f, (_attackAngle / 2f), 0f)) * (_shootPos.forward * _attackRange));
				UnityEditor.Handles.DrawWireArc(_shootPos.position, -_shootPos.up, Quaternion.Euler(_shootPos.TransformDirection(0f, _attackAngle / 2f, 0f)) * (_shootPos.forward * _seeRange), _attackAngle, _attackRange);
			} else {
				UnityEditor.Handles.DrawWireDisc(_shootPos.position, -_shootPos.up, _attackRange);
			}
		}
	}
#endif
}
