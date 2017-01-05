using UnityEngine;
using System.Collections;
using DG.Tweening;

public abstract class Weapon : MonoBehaviour {
	[Header("Shoot Settings")]
	[SerializeField]
	private float _shootDamage;

	[SerializeField]
	private float _shootRange;

	[SerializeField]
	private float _shootDelay;

	[Header("Spread Settings")]
	[SerializeField]
	private float _spreadConeRadius;

	[SerializeField]
	private float _spreadConeLength;

	[Header("Ammo Settings")]
	[SerializeField]
	protected int _magazineCapacity;
	public int MagazineCapacity { get { return _magazineCapacity; } }

	[SerializeField]
	protected int _maxReserveAmmo;
	public int MaxReserveAmmo { get { return _maxReserveAmmo; } }

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

	protected bool _canShoot = true;
	public bool CanShoot { get { return _canShoot; } }

	protected bool _reloading = false;
	public bool Reloading { get { return _reloading; } }

	protected bool _moving = false;
	public bool Moving { set { _moving = value; } }

	protected bool _aiming = false;
	public bool Aiming { set { _aiming = value; } }

	protected bool _active;

	private MouseLook _mouseLook;

	private Timers.Timer _shootTimer;

	protected virtual void Awake() {
		_magazineContent = _magazineCapacity;
		_reserveAmmo = _magazineCapacity;
	}

	protected virtual void Start() {
		_mouseLook = FindObjectOfType<MouseLook>();
		_shootTimer = Timers.CreateTimer().SetTime(_shootDelay).SetCallback(() => _canShoot = true).ResetOnFinish();
	}

	public void AddAmmo(int pAmount) {
		_reserveAmmo = Mathf.Min(_maxReserveAmmo, _reserveAmmo + pAmount);
	}

	protected abstract void spawnBullet(Vector3 pHitPoint);
	protected abstract void spawnDecal(Vector3 pHitPoint, Vector3 pHitNormal, Transform pHitTransform);

	protected void reload () {
		_reloading = true;

		Sequence reloadStartSequence = DOTween.Sequence();
		reloadStartSequence.Append(transform.DOLocalRotate(new Vector3(_reloadDownRotation, 0.0f, 0.0f), _reloadMoveTime));
		reloadStartSequence.Join(transform.DOLocalMove(transform.localPosition + new Vector3(0.0f, -_reloadDownMovement, 0.0f), _reloadMoveTime));
		reloadStartSequence.AppendInterval(_reloadTime - 2 * _reloadMoveTime);
		reloadStartSequence.Append(transform.DOLocalRotate(new Vector3(0.0f, 0.0f, 0.0f), _reloadMoveTime));
		reloadStartSequence.Join(transform.DOLocalMove(transform.localPosition, _reloadMoveTime));
		reloadStartSequence.AppendCallback(() => finishReload());
	}

	private void finishReload () {
		int diff = _magazineCapacity - _magazineContent;
		_magazineContent = (diff > _reserveAmmo) ? _magazineContent + _reserveAmmo : _magazineCapacity;
		_reserveAmmo = Mathf.Max(_reserveAmmo - diff, 0);

		_reloading = false;
	}

	protected void shoot () {
		_shootTimer.Start();

		_canShoot = false;

		_magazineContent = Mathf.Max(_magazineContent - 1, 0);

		RaycastHit hit;

		if (Physics.Raycast(Camera.main.transform.position, calulateShootDirection(), out hit, _shootRange, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore)) {
			spawnBullet(hit.point);
			spawnDecal(hit.point, hit.normal, hit.transform);
			ShooterPackageSender.SendPackage(new CustomCommands.Creation.Shots.LaserShotCreation(Camera.main.transform.position, hit.point));
			if (hit.rigidbody != null && hit.rigidbody.GetComponent<IDamageable>() != null) {
				hit.rigidbody.GetComponent<IDamageable>().ReceiveDamage(Camera.main.transform.forward, hit.point, _shootDamage);
			}
		} else {
			spawnBullet(_muzzlePosition.position + _muzzlePosition.forward * _shootRange);
			ShooterPackageSender.SendPackage(new CustomCommands.Creation.Shots.LaserShotCreation(Camera.main.transform.position, _muzzlePosition.position + _muzzlePosition.forward * _shootRange));
		}

		_mouseLook.ApplyRecoil(new Vector2(Random.Range(-_cameraRecoilForce.y, _cameraRecoilForce.y), _cameraRecoilForce.x));

		//FIX This is very costly I think
		Sequence recoilSequence = DOTween.Sequence();
		recoilSequence.Append(transform.DOLocalRotate(new Vector3(-_weaponRotationRecoilForce.x, Random.Range(-_weaponRotationRecoilForce.y, _weaponRotationRecoilForce.y), 0.0f), _weaponRecoilApplyTime));
		recoilSequence.Join(transform.DOLocalMove(transform.localPosition + new Vector3(0.0f, 0.0f, -_weaponMoveRecoilForce), _weaponRecoilApplyTime));
		recoilSequence.Append(transform.DOLocalRotate(Vector3.zero, _weaponRecoilReturnTime));
		recoilSequence.Join(transform.DOLocalMove(transform.localPosition, _weaponRecoilApplyTime));
	}


	private Vector3 calulateShootDirection() {
		//TODO Modify with walkspeed, aim, and length of fire
		float randomRadius = UnityEngine.Random.Range(0, _spreadConeRadius);
		float randomAngle = UnityEngine.Random.Range(0, 2 * Mathf.PI);

		Vector3 rayDir = new Vector3(
			randomRadius * Mathf.Cos(randomAngle),
			randomRadius * Mathf.Sin(randomAngle),
			_spreadConeLength
			);

		return Camera.main.transform.TransformDirection(rayDir.normalized);
	}

	public void SetActive (bool pActive) {
		_active = pActive;
		_model.SetActive(pActive);
	}

	private void OnGUI() {
		if (_active) {
			FindObjectOfType<CrosshairDistance>().SetDistance(_spreadConeRadius, _spreadConeLength);
		}
	}

#if UNITY_EDITOR
	private void OnDrawGizmos() {
		if (_active) {
			UnityEditor.Handles.color = Color.white;
			UnityEditor.Handles.DrawWireDisc(Camera.main.transform.position + Camera.main.transform.forward * _spreadConeLength, Camera.main.transform.forward, _spreadConeRadius);
		}
	}
#endif
}
