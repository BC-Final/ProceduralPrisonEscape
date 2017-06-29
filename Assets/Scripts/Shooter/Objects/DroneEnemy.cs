using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using StateFramework;
using System.Net.Sockets;
using UnityEngine.AI;
using System.Linq;

[SelectionBase]
[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
public class DroneEnemy : MonoBehaviour, IDamageable, ITargetable, IShooterNetworked {
	[SerializeField]
	private DroneParameters _parameters;
	public DroneParameters Parameters { get { return _parameters; } }


	private PatrolRoute _route;
	public PatrolRoute Route { get { return _route; } set { _route = value; } }

	private DronePoint _startPoint;
	public DronePoint StartPoint { get { return _startPoint; } set { _startPoint = value; } }

	[Header("References")]
	[SerializeField]
	private Transform _viewTransform;
	public Transform ViewTransform { get { return _viewTransform; } }

	[SerializeField]
	private Transform _shotTransform;
	public Transform ShotTransform { get { return _shotTransform; } }

	[SerializeField]
	private Transform _model;
	public Transform Model { get { return _model; } }

	[SerializeField]
	private SphereCollider _viewTrigger;


	[Header("Debug")]
	[SerializeField]
	private bool _visualizeView;

	[SerializeField]
	private bool _visualizeHits;

	public UnityEngine.Events.UnityEvent _onShoot;
	public UnityEngine.Events.UnityEvent _onDie;
	public UnityEngine.Events.UnityEvent _onDamaged;
	public UnityEngine.Events.UnityEvent _onPlayerEnter;
	public UnityEngine.Events.UnityEvent _onPlayerExit;

	private bool _alarmResponder;
	public bool AlarmResponder { get { return _alarmResponder; } set { _alarmResponder = value; } }

	private float _currentHealth;

	private bool _seesTarget;
	public bool SeesTarget { get { return _seesTarget; } set { _seesTarget = value; } }

	private StateFramework.StateMachine<AbstractDroneState> _fsm;

	private Faction _faction = Faction.Prison;
	public Faction Faction { get { return _faction; } }

	private NavMeshAgent _agent;
	public NavMeshAgent Agent { get { return _agent; } }

	private Timer _networkUpdateTimer;

	public void SendUpdates(bool pValue) {
		if (_networkUpdateTimer != null) {
			if (pValue) {
				_networkUpdateTimer.Start();
			} else {
				_networkUpdateTimer.Pause();
			}
		}
	}

	private List<ITargetable> _possibleTargets = new List<ITargetable>();
	public Transform[] PossibleTargets {
		get {
			return _possibleTargets.FindAll(x => x.Faction != _faction).Select(x => x.GameObject.transform).ToArray();
		}
	}



	private System.Action<GameObject> _destroyEvent;

	public void AddToDestroyEvent(System.Action<GameObject> pObject) {
		_destroyEvent += pObject;
	}

	public void RemoveFromDestroyEvent(System.Action<GameObject> pObject) {
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


	public void SetFollowPlayer() {
		_lastTarget = FindObjectOfType<PlayerHealth>().GameObject;

		if (_fsm.GetState() is DroneAlarmSearchState) {
			_fsm.SetState<DroneAlarmFollowState>();
		}
	}

	public void SetSearchPlayer() {
		_fsm.SetState<DroneAlarmSearchState>();
	}



	public void Initialize() {
		sendUpdate();

		_networkUpdateTimer = TimerManager.CreateTimer("Drone Network Update", false).SetDuration(_parameters.NetworkUpdateRate).SetLoops(-1).AddCallback(() => sendUpdate());

		//TODO This is super hacky. When the hacker connects all "network idle" states keep sending because they tried to pause before the hacker joined (maybe create an "idle" state until hacker joins)
		if (_fsm == null || !(_fsm.GetState() is DroneGuardState)) {
			_networkUpdateTimer.Start();
		}
	}



	private void Awake() {
		ShooterPackageSender.RegisterNetworkObject(this);
	}



	private void OnDestroy() {
		if (_networkUpdateTimer != null) {
			_networkUpdateTimer.Stop();
			sendUpdate();
		}

		ShooterPackageSender.UnregisterNetworkedObject(this);
	}



	private void Start() {
		_fsm = new StateMachine<AbstractDroneState>();

		_fsm.AddState(new DroneGuardState(this, _fsm));
		_fsm.AddState(new DronePatrolState(this, _fsm));
		_fsm.AddState(new DroneEngangeState(this, _fsm));
		_fsm.AddState(new DroneDeadState(this, _fsm));
		_fsm.AddState(new DroneFollowState(this, _fsm));
		_fsm.AddState(new DroneSearchState(this, _fsm));
		_fsm.AddState(new DroneReturnState(this, _fsm));
		_fsm.AddState(new DroneAttackState(this, _fsm));
		_fsm.AddState(new DroneStunnedState(this, _fsm));
		_fsm.AddState(new DroneAmbushState(this, _fsm));

		_fsm.AddState(new DroneAlarmFollowState(this, _fsm));
		_fsm.AddState(new DroneAlarmSearchState(this, _fsm));
		_fsm.AddState(new DroneAlarmReturnState(this, _fsm));

		_agent = GetComponent<NavMeshAgent>();

		_currentHealth = _parameters.MaximumHealth;

		_viewTrigger.radius = _parameters.ViewRange;

		if (!_alarmResponder) {
			_fsm.SetState<DroneReturnState>();
		} else {
			_lastTarget = FindObjectOfType<PlayerHealth>().GameObject;
			_fsm.SetState<DroneAlarmFollowState>();
		}

		if (_onPlayerEnter != null) {
			_onPlayerEnter.Invoke();
		}
	}



	private void sendUpdate() {
		EnemyState state = ((_fsm == null || _faction != Faction.Prison) ? EnemyState.Controlled : ((_fsm.GetState() is DroneStunnedState) ? EnemyState.Stunned : ((_seesTarget) ? EnemyState.SeesPlayer : EnemyState.None)));
		
		ShooterPackageSender.SendPackage(new NetworkPacket.GameObjects.Drone.sUpdate(Id, transform.position.x, transform.position.z, transform.rotation.eulerAngles.y, _currentHealth / _parameters.MaximumHealth, state));
	}

	[SerializeField]
	private string CurrentState;

	private void Update() {
		CurrentState = _fsm.GetState().GetType().Name;

		_possibleTargets.OrderBy(x => Vector3.Distance(x.GameObject.transform.position, transform.position));

		_fsm.Step();

		GetComponent<FMODEventCollection>().SetParameter(4, "P_drone_speed", _agent.velocity.magnitude / _agent.speed);
	}



	private void OnTriggerEnter(Collider other) {
		ITargetable d = other.GetComponentInParent<ITargetable>();

		if (d != null && other.gameObject.layer != LayerMask.NameToLayer("RangeTrigger")) {
			if (d.GameObject.tag == "Player") {
				//if (_onPlayerEnter != null) {
				//	_onPlayerEnter.Invoke();
				//}
			}

			d.AddToDestroyEvent(g => onTargetDestroyed(g));
			_possibleTargets.Add(d);
		}
	}



	private void OnTriggerExit(Collider other) {
		ITargetable d = other.GetComponentInParent<ITargetable>();

		if (d != null && other.gameObject.layer != LayerMask.NameToLayer("RangeTrigger")) {
			//if (_onPlayerExit != null) {
			//	_onPlayerExit.Invoke();
			//}

			d.RemoveFromDestroyEvent(g => onTargetDestroyed(g));
			_possibleTargets.RemoveAll(x => x.GameObject == d.GameObject);
		}
	}

	private void onTargetDestroyed(GameObject pObject) {
		if (pObject.layer != LayerMask.NameToLayer("RangeTrigger")) {
			_possibleTargets.RemoveAll(x => x.GameObject == pObject);
		}
	}

	public void StartAmbush() {
		_lastTarget = FindObjectOfType<PlayerHealth>().gameObject;
		_fsm.SetState<DroneAmbushState>();
	}

	public void ReceiveDamage(Transform pSource, Vector3 pHitPoint, float pDamage, float pForce) {
		if (_currentHealth > 0.0f) {
			_currentHealth -= pDamage;

			//_lastTarget = pSender.GameObject;
			if (_onDamaged != null) {
				_onDamaged.Invoke();
			}

			if (_currentHealth <= 0.0f) {
				DestroyEvent();

				_fsm.SetState<DroneDeadState>();
			}

			_fsm.GetState().ReceiveDamage(pSource, pHitPoint, pDamage, pForce);


			//#if UNITY_EDITOR
			//			_hitInfo.Add(new HitInfo(transform.InverseTransformPoint(pPoint), transform.InverseTransformDirection(pDirection)));
			//#endif
		}
	}

	public void DestroyEvent() {
		if (_destroyEvent != null) {
			_destroyEvent.Invoke(gameObject);
		}
	}



	//#if UNITY_EDITOR
	//	private struct HitInfo {
	//		public HitInfo (Vector3 pPoint, Vector3 pDirection) {
	//			Point = pPoint;
	//			Direction = pDirection;
	//		}

	//		public Vector3 Point;
	//		public Vector3 Direction;
	//	}

	//	private List<HitInfo> _hitInfo = new List<HitInfo>();

	//	private void OnDrawGizmos () {
	//		if (_visualizeHits) {
	//			foreach (HitInfo hi in _hitInfo) {
	//				Gizmos.color = Color.red;
	//				Gizmos.DrawSphere(transform.TransformPoint(hi.Point), 0.05f);

	//				Gizmos.color = Color.blue;
	//				Gizmos.DrawRay(transform.TransformPoint(hi.Point), transform.TransformDirection(hi.Direction));
	//			}
	//		}

	//		if (_visualizeView) {
	//			Gizmos.color = Color.white;
	//			UnityEditor.Handles.color = Color.white;

	//			UnityEditor.Handles.DrawWireDisc(_viewTransform.position, -_viewTransform.up, _parameters.AwarenessRange);
	//			UnityEditor.Handles.DrawWireDisc(_viewTransform.position, -_viewTransform.right, _parameters.AwarenessRange);
	//			UnityEditor.Handles.DrawWireDisc(_viewTransform.position, (_viewTransform.right + _viewTransform.up).normalized, _parameters.AwarenessRange);
	//			UnityEditor.Handles.DrawWireDisc(_viewTransform.position, (_viewTransform.right - _viewTransform.up).normalized, _parameters.AwarenessRange);

	//			if (_parameters.ViewAngle < 360f) {
	//				Gizmos.DrawLine(_viewTransform.position, _viewTransform.TransformPoint(Quaternion.Euler(0f, _parameters.ViewAngle / 2f, 0f) * (Vector3.forward * _parameters.ViewRange)));
	//				Gizmos.DrawLine(_viewTransform.position, _viewTransform.TransformPoint(Quaternion.Euler(0f, -(_parameters.ViewAngle / 2f), 0f) * (Vector3.forward * _parameters.ViewRange)));

	//				Gizmos.DrawLine(_viewTransform.position, _viewTransform.TransformPoint(Quaternion.Euler(_parameters.ViewAngle / 2f, 0f, 0f) * (Vector3.forward * _parameters.ViewRange)));
	//				Gizmos.DrawLine(_viewTransform.position, _viewTransform.TransformPoint(Quaternion.Euler(-(_parameters.ViewAngle / 2f), 0f, 0f) * (Vector3.forward * _parameters.ViewRange)));


	//				UnityEditor.Handles.DrawWireArc(_viewTransform.position, -_viewTransform.up, _viewTransform.TransformDirection(Quaternion.Euler(0f, _parameters.ViewAngle / 2f, 0f) * (Vector3.forward * _parameters.ViewRange).normalized), _parameters.ViewAngle, _parameters.ViewRange);
	//				UnityEditor.Handles.DrawWireArc(_viewTransform.position, -_viewTransform.up, _viewTransform.TransformDirection(Quaternion.Euler(0f, _parameters.ViewAngle / 2f, 0f) * (Vector3.forward * _parameters.ViewRange).normalized), _parameters.ViewAngle, _parameters.AttackRange);

	//				UnityEditor.Handles.DrawWireArc(_viewTransform.position, -_viewTransform.right, _viewTransform.TransformDirection(Quaternion.Euler(_parameters.ViewAngle / 2f, 0f, 0f) * (Vector3.forward * _parameters.ViewRange).normalized), _parameters.ViewAngle, _parameters.ViewRange);
	//				UnityEditor.Handles.DrawWireArc(_viewTransform.position, -_viewTransform.right, _viewTransform.TransformDirection(Quaternion.Euler(_parameters.ViewAngle / 2f, 0f, 0f) * (Vector3.forward * _parameters.ViewRange).normalized), _parameters.ViewAngle, _parameters.AttackRange);
	//			} else {
	//				UnityEditor.Handles.DrawWireDisc(_viewTransform.position, -_viewTransform.up, _parameters.ViewRange);
	//				UnityEditor.Handles.DrawWireDisc(_viewTransform.position, -_viewTransform.up, _parameters.AttackRange);
	//			}
	//		}
	//	}
	//#endif
}
