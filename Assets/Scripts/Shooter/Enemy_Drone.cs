using UnityEngine;
using System.Collections;
using StateFramework;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy_Drone : MonoBehaviour, IDamageable {
	[SerializeField]
	private float _health;

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
	}

	private void Update() {
		if (_health > 0.0f) {
			_fsm.Step();
		}
	}

	public void ReceiveDamage(Vector3 pDirection, Vector3 pPoint, float pDamage) {
		if (_health > 0.0f) {
			_health -= pDamage;

			//For drawing Gizmos
			_lastShootPos = transform.InverseTransformPoint(pPoint);
			_lastFlyDirection = transform.InverseTransformDirection(pDirection);

			if (_health <= 0.0f) {
				_fsm.SetState<DroneDeadState>();
			}

			_fsm.GetState().ReceiveDamage(pDirection, pPoint, pDamage);
		}
	}






	
	private Vector3 _lastShootPos = Vector3.zero;
	private Vector3 _lastFlyDirection = Vector3.zero;

#if UNITY_EDITOR
	private void OnDrawGizmos() {
		if (_visualizeHits) {
			if (_lastShootPos != Vector3.zero) {
				Gizmos.color = Color.red;
				Gizmos.DrawSphere(transform.TransformPoint(_lastShootPos), 0.1f);
			}

			if (_lastFlyDirection != Vector3.zero) {
				Gizmos.color = Color.blue;
				Gizmos.DrawRay(transform.TransformPoint(_lastShootPos), transform.TransformDirection(_lastFlyDirection));
			}
		}

		if (_visualizeView) {

			#if (UNITY_EDITOR)
			Gizmos.color = Color.white;
			UnityEditor.Handles.color = Color.white;

			if (_seeAngle < 360f) {
				//Gizmos.DrawLine(transform.TransformPoint(Quaternion.Euler(0f, _seeAngle / 2f, 0f) * (Vector3.forward * _attackRange)), transform.TransformPoint(Quaternion.Euler(0f, _seeAngle / 2f, 0f) * (Vector3.forward * _seeRange)));
				//Gizmos.DrawLine(transform.TransformPoint(Quaternion.Euler(0f, -(_seeAngle / 2f), 0f) * (Vector3.forward * _attackRange)), transform.TransformPoint(Quaternion.Euler(0f, -(_seeAngle / 2f), 0f) * (Vector3.forward * _seeRange)));
				Gizmos.DrawLine(transform.position, transform.TransformPoint(Quaternion.Euler(0f, _seeAngle / 2f, 0f) * (Vector3.forward * _seeRange)));
				Gizmos.DrawLine(transform.position, transform.TransformPoint(Quaternion.Euler(0f, -(_seeAngle / 2f), 0f) * (Vector3.forward * _seeRange)));
				UnityEditor.Handles.DrawWireArc(transform.position, -transform.up, transform.TransformDirection(Quaternion.Euler(0f, _seeAngle / 2f, 0f) * (Vector3.forward * _seeRange).normalized), _seeAngle, _seeRange);
				UnityEditor.Handles.DrawWireArc(transform.position, -transform.up, transform.TransformDirection(Quaternion.Euler(0f, _seeAngle / 2f, 0f) * (Vector3.forward * _seeRange).normalized), _seeAngle, _attackRange);
			} else {
				UnityEditor.Handles.DrawWireDisc(transform.position, -transform.up, _seeRange);
				UnityEditor.Handles.DrawWireDisc(transform.position, -transform.up, _attackRange);
			}
			#endif
		}
	}
#endif
}
