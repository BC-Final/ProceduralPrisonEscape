using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateFramework;
using System.Linq;

//TODO Remove IDamageable
[SelectionBase]
public class ShooterTurret : MonoBehaviour, IShooterNetworked, IDamageable {
	[SerializeField]
	private TurretParameters _parameters;
	public TurretParameters Parameters { get { return _parameters; } }


	[Header("References")]
	[SerializeField]
	private Transform _rotaryBase;
	public Transform RotaryBase { get { return _rotaryBase; } }

	[SerializeField]
	private Transform _gun;
	public Transform Gun { get { return _gun; } }

	[SerializeField]
	private Transform _shootPos;
	public Transform ShootPos { get { return _shootPos; } }

	[SerializeField]
	private SphereCollider _viewTrigger;



	[Header("Debug")]
	[SerializeField]
	private bool _visualizeView;



	private StateMachine<AbstractTurretState> _fsm;

	private Timer _networkUpdateTimer;

	[SerializeField]
	private Faction _faction = Faction.Prison;
	public Faction Faction { get { return _faction; } }

	private bool _seesTarget = false;
	public bool SeesTarget { set { _seesTarget = value; } }

	private List<ITargetable> _possibleTargets = new List<ITargetable>();
	public Transform[] PossibleTargets {
		get {
			return _possibleTargets.FindAll(x => x.Faction != _faction).Select(x => x.GameObject.transform).ToArray();
		}
	}


	private System.Action<GameObject> _destroyEvent;

	public void AddToDestroyEvent (System.Action<GameObject> pObject) {
		_destroyEvent += pObject;
	}

	public void RemoveFromDestroyEvent (System.Action<GameObject> pObject) {
		_destroyEvent -= pObject;
	}

	public GameObject GameObject { get { return gameObject; } }

	[SerializeField]
	private GameObject _lastTarget;
	public GameObject LastTarget {
		get { return _lastTarget; }
		set { _lastTarget = value; }
	}

	private ShooterNetworkId _id = new ShooterNetworkId();
	public ShooterNetworkId Id { get { return _id; } }

	public static void ProcessPacket (NetworkPacket.GameObjects.Turret.hUpdate pPacket) {
		ShooterTurret tur = ShooterPackageSender.GetNetworkedObject<ShooterTurret>(pPacket.Id);

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
			Debug.LogError("Turret with Id " + pPacket.Id + " does not exist");
		}
	}

	public void Initialize () {
		_networkUpdateTimer = TimerManager.CreateTimer("Turret Network Update", false).SetDuration(_parameters.NetworkUpdateRate).SetLoops(-1).AddCallback(() => sendUpdate()).Start();
	}

	private void Awake () {
		ShooterPackageSender.RegisterNetworkObject(this);
	}

	private void OnDestroy () {
		if (_networkUpdateTimer != null) {
			_networkUpdateTimer.Stop();
			sendUpdate();
		}

		ShooterPackageSender.UnregisterNetworkedObject(this);
	}

	public void ReceiveDamage (Transform pSource, Vector3 pHitPoint, float pDamage, float pForce) {
		//if (_currentHealth > 0.0f) {
		//	_currentHealth -= pDamage;

			//if (_currentHealth <= 0.0f) {
			//	if (_destroyEvent != null) {
			//		_destroyEvent.Invoke(gameObject);
			//	}
			//}

			_fsm.GetState().ReceiveDamage (pSource, pHitPoint, pDamage, pForce);
		//}
	}


	private void Start () {
		_fsm = new StateMachine<AbstractTurretState>();

		_fsm.AddState(new TurretGuardState(this, _fsm));
		_fsm.AddState(new TurretDeployState(this, _fsm));
		_fsm.AddState(new TurretEngageState(this, _fsm));
		_fsm.AddState(new TurretAttackState(this, _fsm));
		_fsm.AddState(new TurretScanState(this, _fsm));
		_fsm.AddState(new TurretHideState(this, _fsm));
		_fsm.AddState(new TurretDisabledState(this, _fsm));

		_fsm.SetState<TurretGuardState>();

		_viewTrigger.radius = _parameters.ViewRange;
	}

	private void sendUpdate () {
		EnemyState state = _faction != Faction.Prison ? EnemyState.Controlled : _fsm.GetState() is TurretDisabledState ? EnemyState.Stunned : (_seesTarget ? EnemyState.SeesPlayer : EnemyState.None);

		ShooterPackageSender.SendPackage(new NetworkPacket.Update.Turret(Id, transform.position.x, transform.position.z, _rotaryBase.rotation.eulerAngles.y, state));
	}

	[SerializeField]
	private string CurrentState;

	private void Update () {
		CurrentState = _fsm.GetState().GetType().Name;
		//if (_controlled) {
		//	_targets = _targets.OrderBy(x => Vector3.Distance(transform.position, x.transform.position)).ToList();
		//}

		_fsm.Step();
	}

	private void control () {
		_faction = Faction.Player;
		TimerManager.CreateTimer("Turret Controlled", true).SetDuration(_parameters.ControllDuration).AddCallback(() => _faction = Faction.Prison).Start();
	}

	private void disable () {
		_fsm.SetState<TurretDisabledState>();
	}

	private void OnTriggerEnter (Collider other) {
		ITargetable d = other.GetComponentInParent<ITargetable>();

		if (d != null && other.gameObject.layer != LayerMask.NameToLayer("RangeTrigger")) {
			d.AddToDestroyEvent(g => onTargetDestroyed(g));
			_possibleTargets.Add(d);
		}
	}

	private void OnTriggerExit (Collider other) {
		ITargetable d = other.GetComponentInParent<ITargetable>();

		if (d != null && other.gameObject.layer != LayerMask.NameToLayer("RangeTrigger")) {
			d.RemoveFromDestroyEvent(g => onTargetDestroyed(g));
			_possibleTargets.RemoveAll(x => x.GameObject == d.GameObject);
		}
	}

	private void onTargetDestroyed (GameObject pObject) {
		if (pObject.layer != LayerMask.NameToLayer("RangeTrigger")) {
			_possibleTargets.RemoveAll(x => x.GameObject == pObject);
		}
	}

#if UNITY_EDITOR
	private void OnDrawGizmos () {
		if (_visualizeView) {
			Gizmos.color = Color.white;
			UnityEditor.Handles.color = Color.white;

			UnityEditor.Handles.DrawWireDisc(transform.position, -transform.up, _parameters.ViewRange);

			if (_parameters.AttackAngle < 360.0f) {
				Gizmos.DrawLine(_shootPos.position, _shootPos.position + Quaternion.Euler(_shootPos.TransformDirection(0f, -(_parameters.AttackAngle / 2f), 0f)) * (_shootPos.forward * _parameters.AttackRange));
				Gizmos.DrawLine(_shootPos.position, _shootPos.position + Quaternion.Euler(_shootPos.TransformDirection(0f, (_parameters.AttackAngle / 2f), 0f)) * (_shootPos.forward * _parameters.AttackRange));
				UnityEditor.Handles.DrawWireArc(_shootPos.position, -_shootPos.up, Quaternion.Euler(_shootPos.TransformDirection(0f, _parameters.AttackAngle / 2f, 0f)) * (_shootPos.forward * _parameters.ViewRange), _parameters.AttackAngle, _parameters.AttackRange);
			} else {
				UnityEditor.Handles.DrawWireDisc(_shootPos.position, -_shootPos.up, _parameters.AttackRange);
			}
		}
	}
#endif
}
