using UnityEngine;
using System.Collections;
using DG.Tweening;
using Gamelogic.Extensions;

public abstract class Weapon : MonoBehaviour {
	[Header("Shoot Settings")]
	[SerializeField]
	protected float _shootDamage;

	[SerializeField]
	private float _shootRange;

	[SerializeField]
	private float _shootDelay;

	[SerializeField]
	private float _shootForce;

	[Header("Spread Settings")]
	[SerializeField]
	private float _spreadConeRadius;

	[SerializeField]
	private float _spreadConeLength;

	[SerializeField]
	private float _aimedSpreadConeRadius;

	[SerializeField]
	private AnimationCurve _velocitySpreadRadiusCurve = new AnimationCurve(new Keyframe(0.0f, 0.0f), new Keyframe(12.0f, 1.75f));

	[Header("Ammo Settings")]
	[SerializeField]
	protected int _magazineCapacity;
	//public int MagazineCapacity { get { return _magazineCapacity; } }

	[SerializeField]
	protected int _maxReserveAmmo;
	//public int MaxReserveAmmo { get { return _maxReserveAmmo; } }

	[Header("References")]
	[SerializeField]
	protected Transform _muzzlePosition;

	[SerializeField]
	private Transform _aimPosition;
	public Transform AimPosition { get { return _aimPosition; } }

	[Header("Timer Settings")]
	[SerializeField]
	private float _reloadTime;

	[SerializeField]
	private float _meleeTime;

	[SerializeField]
	private float _aimTime;
	public float AimTime { get { return _aimTime; } }

	[SerializeField]
	private float _drawTime;

	[Header("Recoil Settings")]
	[SerializeField]
	private Vector2 _cameraRecoilForce;

	[SerializeField]
	private Vector2 _weaponRotationRecoilForce;

	[SerializeField]
	private float _weaponMoveRecoilForce;

	[SerializeField]
	private float _weaponRecoilApplyTime;

	[SerializeField]
	private float _weaponRecoilReturnTime;

	[Header("Reload Settings")]
	[SerializeField]
	private float _reloadDownMovement;

	[SerializeField]
	private float _reloadDownRotation;

	[SerializeField]
	private float _reloadMoveTime;

	[SerializeField]
	private GameObject _model;



	protected int _magazineContent;
	public int MagazineContent { get { return _magazineContent; } }

	protected int _reserveAmmo;
	public int ReserveAmmo { get { return _reserveAmmo; } }



	//protected bool _canShoot = true;
	//public bool CanShoot { get { return _canShoot; } }

	//protected bool _reloading = false;
	//public bool Reloading { get { return _reloading; } }

	//protected bool _moving = false;
	//public bool Moving { set { _moving = value; } }

	//protected bool _aiming = false;
	//public bool Aiming { set { _aiming = value; } }

	//protected bool _active;



	private WeaponHolder _holder;
	private CharacterController _controller;
	private MouseLook _mouseLook;
	private Timer _shootTimer;

	private Tween _reloadSeqence;
	private Tween _recoilSequence;
	private Tween _drawSequence;

	private bool _reloadQueued = false;
	public bool ReloadQueued { get { return _reloadQueued; } }

	[SerializeField]
	protected WeaponState _currentState = WeaponState.Hidden;
	public enum WeaponState {
		Drawing,
		Idle,
		Shooting,
		Reloading,
		Hidden
	}

	//Set base ammo for debug purposes
	protected virtual void Awake () {
		_magazineContent = _magazineCapacity;
		_reserveAmmo = Mathf.Min(_magazineCapacity, _maxReserveAmmo);
	}


	//Set References
	protected virtual void Start () {
		_holder = GetComponentInParent<WeaponHolder>();
		_mouseLook = GetComponentInParent<MouseLook>();
		_controller = GetComponentInParent<CharacterController>();
		_shootTimer = TimerManager.CreateTimer("Weapon Shoot Rate", false).SetDuration(_shootDelay).ResetOnFinish();
	}

	protected virtual void Update () {
		if (_reloadQueued) {
			reload();
		}
	}


	//Hides the weapon
	public void SetActive (bool pActive) {
		_model.SetActive(pActive);

		if (pActive) {
			_currentState = WeaponState.Drawing;

			//TODO This is placeholder
			_model.transform.Rotate(_reloadDownRotation, 0, 0);
			_model.transform.localPosition = _model.transform.localPosition + new Vector3(0.0f, -_reloadDownMovement, 0.0f);

			_drawSequence = DOTween.Sequence()
			 .Append(_model.transform.DOLocalRotate(new Vector3(0.0f, 0.0f, 0.0f), _drawTime))
			 .Join(_model.transform.DOLocalMove(Vector3.zero, _drawTime))
			 .OnComplete(() => _currentState = WeaponState.Idle);
		} else {
			if (_currentState == WeaponState.Reloading) {
				abortReload();
			} else if (_currentState == WeaponState.Drawing) {
				_drawSequence.Kill(true);
			}

			abortShot();

			_currentState = WeaponState.Hidden;
		}
	}

	protected virtual void abortShot() {
		if (_recoilSequence != null) {
			_recoilSequence.Kill(true);
		}
	}

	public bool IsNotInStates (params WeaponState[] pStates) {
		bool result = true;

		foreach (WeaponState state in pStates) {
			result = result && _currentState != state;
		}

		return result;
	}

	public bool IsInState (WeaponState pState) {
		return pState == _currentState;
	}



	protected void shoot (float pDamage, int pNumberOfShots = 1, bool pConsumeAmmo = true) {
		_currentState = WeaponState.Shooting;

		_shootTimer.Start();

		if (pConsumeAmmo) {
			_magazineContent = Mathf.Max(_magazineContent - 1, 0);
		}

		RaycastHit hit;

		for (int i = 0; i < pNumberOfShots; ++i) {
			if (Utilities.Weapons.CastShot(CalcFinalSpreadConeRadius(), _spreadConeLength, Camera.main.transform, out hit)) {
				spawnBullet(hit.point);
				spawnDecal(hit.point, hit.normal, hit.transform);

				if (hit.rigidbody != null && hit.rigidbody.GetComponent<IDamageable>() != null) {
					hit.rigidbody.GetComponent<IDamageable>().ReceiveDamage(GetComponentInParent<PlayerHealth>().transform, hit.point, _shootDamage, _shootForce);
				}
			} else {
				//FIX This is an edge case for when nothin was hit
				//spawnBullet(_muzzlePosition.position + shootDir * _shootRange);
				spawnBullet(_muzzlePosition.position + _muzzlePosition.forward * _shootRange);
			}
		}
		//TODO Make the weapon recoil in the opposite direction of the shot

		_mouseLook.ApplyRecoil(new Vector2(Random.Range(-_cameraRecoilForce.y, _cameraRecoilForce.y), _cameraRecoilForce.x));


		_recoilSequence = DOTween.Sequence()
		.Append(_model.transform.DOLocalRotate(new Vector3(-_weaponRotationRecoilForce.x, Random.Range(-_weaponRotationRecoilForce.y, _weaponRotationRecoilForce.y), 0.0f), _weaponRecoilApplyTime))
		.Join(_model.transform.DOLocalMove(_model.transform.localPosition + new Vector3(0.0f, 0.0f, -_weaponMoveRecoilForce), _weaponRecoilApplyTime))
		.Append(_model.transform.DOLocalRotate(Vector3.zero, _weaponRecoilReturnTime))
		.Join(_model.transform.DOLocalMove(_model.transform.localPosition, _weaponRecoilApplyTime))
		.OnComplete(() => _currentState = WeaponState.Idle);
	}


	protected void reload () {
		if (_currentState == WeaponState.Idle && !_holder.IsAimingOrTransit) {
			_currentState = WeaponState.Reloading;

			_reloadSeqence = DOTween.Sequence()
			.Append(_model.transform.DOLocalRotate(new Vector3(_reloadDownRotation, 0.0f, 0.0f), _reloadMoveTime))
			.Join(_model.transform.DOLocalMove(_model.transform.localPosition + new Vector3(0.0f, -_reloadDownMovement, 0.0f), _reloadMoveTime))
			.AppendInterval(_reloadTime - 2 * _reloadMoveTime)
			.Append(_model.transform.DOLocalRotate(new Vector3(0.0f, 0.0f, 0.0f), _reloadMoveTime))
			.Join(_model.transform.DOLocalMove(_model.transform.localPosition, _reloadMoveTime))
			.OnComplete(() =>  finishReload());

			_reloadQueued = false;
		} else if(_currentState != WeaponState.Reloading) {
			if (_holder.IsAimingOrTransit) {
				_holder.CancelAim();
			}

			_reloadQueued = true;
		}
	}

	private void finishReload () {
		int diff = _magazineCapacity - _magazineContent;
		_magazineContent = (diff > _reserveAmmo) ? _magazineContent + _reserveAmmo : _magazineCapacity;
		_reserveAmmo = Mathf.Max(_reserveAmmo - diff, 0);

		_currentState = WeaponState.Idle;
	}

	private void abortReload () {
		_reloadSeqence.Kill(true);
		_recoilSequence.Kill(true);
	}


	private float CalcFinalSpreadConeRadius () {
		return (_holder.IsAiming ? _aimedSpreadConeRadius : _spreadConeRadius) + _velocitySpreadRadiusCurve.Evaluate(_controller.velocity.magnitude);
	}

	public void AddAmmo (int pAmount) {
		_reserveAmmo = Mathf.Min(_maxReserveAmmo, _reserveAmmo + pAmount);
	}

	protected abstract void spawnBullet(Vector3 pHitPoint);
	protected abstract void spawnDecal(Vector3 pHitPoint, Vector3 pHitNormal, Transform pHitTransform);


	private void OnGUI() {
		if (_currentState != WeaponState.Hidden) {
			FindObjectOfType<CrosshairControllerShooterUI>().SetDistance(CalcFinalSpreadConeRadius(), _spreadConeLength);
		}
	}

#if UNITY_EDITOR
	private void OnDrawGizmos() {
		if (_currentState != WeaponState.Hidden) {
			UnityEditor.Handles.color = Color.white;
			UnityEditor.Handles.DrawWireDisc(Camera.main.transform.position + Camera.main.transform.forward * _spreadConeLength, Camera.main.transform.forward, CalcFinalSpreadConeRadius());
		}
	}
#endif
}
