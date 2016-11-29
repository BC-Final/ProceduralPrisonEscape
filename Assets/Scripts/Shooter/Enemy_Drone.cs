using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using StateFramework;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy_Drone : MonoBehaviour, IDamageable, INetworked {
	private static List<Enemy_Drone> _drones = new List<Enemy_Drone>();
	public static List<Enemy_Drone> GetEnemyList () { return _drones; }

	[SerializeField]
	private float _maxHealth;
	private float _currentHealth;

	[SerializeField]
	private float _attackDamage;
	public float AttackDamage { get { return _attackDamage; } }

	[SerializeField]
	private float _attackRange;
	public float AttackRange { get { return _attackRange; } }

	[SerializeField]
	private float _attackRate;
	public float AttackRate { get { return _attackRate; } }

	[SerializeField]
	private float _seeRange;
	public float SeeRange { get { return _seeRange; } }

	[SerializeField]
	private float _seeAngle;
	public float SeeAngle { get { return _seeAngle; } }

	[SerializeField]
	private float _quitIdleRange;
	public float QuitIdleRange { get { return _quitIdleRange; } }

	[SerializeField]
	private float _pathTickRate;
	public float PathTickRate { get { return _pathTickRate; } }

	[SerializeField]
	private bool _visualizeHits;

	[SerializeField]
	private bool _visualizeView;

	[SerializeField]
	private int _searchCount;
	public int SearchCount { get { return _searchCount; } }

	[SerializeField]
	private float _searchRadius;
	public float SearchRadius { get { return _searchRadius; } }

	[SerializeField]
	private float _rotationSpeed;
	public float RotationSpeed { get { return _rotationSpeed; } }

	[SerializeField]
	private float _quitHearRadius;

	[SerializeField]
	private float _loadHearRadius;

	[SerializeField]
	private float _postionUpdateRate = 0.5f;
	private float _positionSendTimer;

	[SerializeField]
	private eAttackType _attackType;
	public eAttackType AttackType { get { return _attackType; } }
	public enum eAttackType {
		Ranged,
		Melee
	}

	[SerializeField]
	private float _spreadConeLength;
	public float SpreadConeLength { get { return _spreadConeLength; } }

	[SerializeField]
	private float _spreadConeRadius;
	public float SpreadConeRadius { get { return _spreadConeRadius; } }

	private StateMachine<AbstractDroneState> _fsm;

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
		
	}

	private void Awake () {
		_drones.Add(this);
	}

	private void OnDestroy () {
		_drones.Remove(this);
	}


	private void Start() {
		_fsm = new StateMachine<AbstractDroneState>();

		_fsm.AddState(new DroneIdleState(this, _fsm));
		_fsm.AddState(new DroneGuardState(this, _fsm));
		_fsm.AddState(new DroneEngangeState(this, _fsm));
		_fsm.AddState(new DroneDeadState(this, _fsm));
		_fsm.AddState(new DroneFollowState(this, _fsm));
		_fsm.AddState(new DroneSearchState(this, _fsm));
		_fsm.AddState(new DroneReturnState(this, _fsm));
		_fsm.AddState(new DroneAttackState(this, _fsm));

		_fsm.SetState<DroneIdleState>();

		_currentHealth = _maxHealth;
	}

	private void Update() {
		//HACK Remove this later
		if (_positionSendTimer - Time.time <= 0.0f) {
			_positionSendTimer = Time.time + _postionUpdateRate;
			ShooterPackageSender.SendPackage(new CustomCommands.Update.EnemyUpdate(_id, (int)(_currentHealth / _maxHealth * 100), transform.position, transform.rotation.eulerAngles.y));
		}
		

		_fsm.Step();
	}

	public void ReceiveDamage(Vector3 pDirection, Vector3 pPoint, float pDamage) {
		if (_currentHealth > 0.0f) {
			_currentHealth -= pDamage;

#if UNITY_EDITOR
			_hitInfo.Add(new HitInfo(transform.InverseTransformPoint(pPoint), transform.InverseTransformDirection(pDirection)));
#endif

			if (_currentHealth <= 0.0f) {
				_drones.Remove(this);
				_fsm.SetState<DroneDeadState>();
			}

			_fsm.GetState().ReceiveDamage(pDirection, pPoint, pDamage);
		}
	}

	private struct HitInfo {
		public HitInfo(Vector3 pPoint, Vector3 pDirection) {
			Point = pPoint;
			Direction = pDirection;
		}

		public Vector3 Point;
		public Vector3 Direction;
	}

#if UNITY_EDITOR
	private List<HitInfo> _hitInfo = new List<HitInfo>();

	private void OnDrawGizmos() {
		if (_visualizeHits) {
			foreach (HitInfo hi in _hitInfo) {
				Gizmos.color = Color.red;
				Gizmos.DrawSphere(transform.TransformPoint(hi.Point), 0.05f);

				Gizmos.color = Color.blue;
				Gizmos.DrawRay(transform.TransformPoint(hi.Point), transform.TransformDirection(hi.Direction));
			}
		}

		if (_visualizeView) {
			Gizmos.color = Color.white;
			UnityEditor.Handles.color = Color.white;

			if (_seeAngle < 360f) {
				Gizmos.DrawLine(transform.position, transform.TransformPoint(Quaternion.Euler(0f, _seeAngle / 2f, 0f) * (Vector3.forward * _seeRange)));
				Gizmos.DrawLine(transform.position, transform.TransformPoint(Quaternion.Euler(0f, -(_seeAngle / 2f), 0f) * (Vector3.forward * _seeRange)));
				UnityEditor.Handles.DrawWireArc(transform.position, -transform.up, transform.TransformDirection(Quaternion.Euler(0f, _seeAngle / 2f, 0f) * (Vector3.forward * _seeRange).normalized), _seeAngle, _seeRange);
				UnityEditor.Handles.DrawWireArc(transform.position, -transform.up, transform.TransformDirection(Quaternion.Euler(0f, _seeAngle / 2f, 0f) * (Vector3.forward * _seeRange).normalized), _seeAngle, _attackRange);
			} else {
				UnityEditor.Handles.DrawWireDisc(transform.position, -transform.up, _seeRange);
				UnityEditor.Handles.DrawWireDisc(transform.position, -transform.up, _attackRange);
			}
		}
	}
#endif
}
