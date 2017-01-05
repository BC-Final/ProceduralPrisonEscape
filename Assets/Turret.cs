using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateFramework;

public class Turret : MonoBehaviour, IDamageable, INetworked {
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
	private float _maxHealth;
	private float _currentHealth;

	[SerializeField]
	private bool _visualizeView;

	[SerializeField]
	private Transform _shootPos;
	public Transform ShootPos { get { return _shootPos; } }

	[SerializeField]
	private float _maxGunRotation;
	public float MaxGunRotation { get { return _maxGunRotation; } }

	private StateMachine<AbstractTurretState> _fsm;

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
		//TODO Create init package??
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
		_fsm.AddState(new TurretDeadState(this, _fsm));

		_fsm.SetState<TurretIdleState>();

		_currentHealth = _maxHealth;
	}

	private void Update () {
		//HACK Remove this later
		/*
		if (_positionSendTimer - Time.time <= 0.0f) {
			_positionSendTimer = Time.time + _postionUpdateRate;
			ShooterPackageSender.SendPackage(new CustomCommands.Update.EnemyUpdate(Id, (int)(_currentHealth / _maxHealth * 100), transform.position, transform.rotation.eulerAngles.y));
		}
		*/


		_fsm.Step();
	}

	public void ReceiveDamage (Vector3 pDirection, Vector3 pPoint, float pDamage) {
		if (_currentHealth > 0.0f) {
			_currentHealth -= pDamage;


			if (_currentHealth <= 0.0f) {
				OnDestroy();
				_fsm.SetState<TurretDeadState>();
			}

			_fsm.GetState().ReceiveDamage(pDirection, pPoint, pDamage);
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
