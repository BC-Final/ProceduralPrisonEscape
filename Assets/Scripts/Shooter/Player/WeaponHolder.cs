using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.UI;
using Gamelogic.Extensions;

public class WeaponHolder : Singleton<WeaponHolder> {
	[SerializeField]
	private Transform _aimPosition;

	[SerializeField]
	private float _weaponSwitchSpeed;

	private List<Weapon> _weapons;
	private int _currentWeapon;

	private Tweener _mainFovTween;
	private Tweener _wpnFovTween;
	private Tweener _moveTween;
	private Tweener _crosshairTween;

	private CrosshairDistance _crosshair;
	private Camera _mainCamera;
	private Camera _weaponCamera;

	private ObservedValue<AimState> _currentAimState = new ObservedValue<AimState>(AimState.Hip);
	private enum AimState {
		Aiming,
		Hip,
		TransitToAim,
		TransitToHip,
		ForcedTransitToHip
	}

	public bool IsAiming {
		get { return _currentAimState.Value == AimState.Aiming; }
	}

	public bool IsAimingOrTransit {
		get { return _currentAimState.Value != AimState.Hip; }
	}

	private void Awake () {
		_currentAimState.OnValueChange += aimStateChanged;
	}

	private void Start() {
		_crosshair = FindObjectOfType<CrosshairDistance>();
		_mainCamera = Camera.main;
		_weaponCamera = GameObject.FindGameObjectWithTag("WeaponCamera").GetComponent<Camera>();

		_weapons = new List<Weapon>();

		_weapons.Add((Instantiate(ShooterReferenceManager.Instance.Phaser, transform.position, transform.rotation, transform) as GameObject).GetComponent<Weapon>());
		_weapons.Add((Instantiate(ShooterReferenceManager.Instance.Machinegun, transform.position, transform.rotation, transform) as GameObject).GetComponent<Weapon>());
		_weapons.Add((Instantiate(ShooterReferenceManager.Instance.Mininglaser, transform.position, transform.rotation, transform) as GameObject).GetComponent<Weapon>());

		_currentWeapon = 0;

		foreach (Weapon w in _weapons) {
			w.SetActive(false);
		}

		_weapons[0].SetActive(true);
	}

	private void Update() {
		if (_canUseWeapons) {
			int nextWeapon = Utilities.Math.Modulas(_currentWeapon + (int)Input.mouseScrollDelta.y, _weapons.Count);

			if (Input.GetKey(KeyCode.Alpha1)) {
				nextWeapon = 0;
			} else if (Input.GetKey(KeyCode.Alpha2)) {
				nextWeapon = 1;
			} else if (Input.GetKey(KeyCode.Alpha3)) {
				nextWeapon = 2;
			}

			if (nextWeapon != _currentWeapon) {
				abortAim();

				_weapons[_currentWeapon].SetActive(false);
				_weapons[nextWeapon].SetActive(true);
				_currentWeapon = nextWeapon;
			}




			if (_weapons[_currentWeapon].IsNotInStates(Weapon.WeaponState.Drawing, Weapon.WeaponState.Reloading) && _currentAimState.Value != AimState.ForcedTransitToHip && !_weapons[_currentWeapon].ReloadQueued) {
				if (Input.GetMouseButton(1)) {
					if (_currentAimState.Value == AimState.Hip || _currentAimState.Value == AimState.TransitToHip) {
						_currentAimState.Value = AimState.TransitToAim;
					}
				} else {
					if (_currentAimState.Value == AimState.Aiming || _currentAimState.Value == AimState.TransitToAim) {
						_currentAimState.Value = AimState.TransitToHip;
					}
				}
			}
		}
	}

	public void AddAmmo<T> (int pAmount) {
		_weapons.Find(x => x is T).AddAmmo(pAmount);
	}

	private void aimStateChanged () {
		switch (_currentAimState.Value) {
			case AimState.TransitToAim:
				transitToAim();
				break;
			case AimState.TransitToHip:
				transitToHip();
				break;
			case AimState.ForcedTransitToHip:
				transitToHip();
				break;
		}
	}

	private bool _canUseWeapons = true;
	public void DisableWeapons () {
		_weapons.ForEach(x => x.SetActive(false));
		_canUseWeapons = false;
		abortAim();
		_crosshair.Disable(0.2f);
	}

	public void EnableWeapons () {
		_weapons[_currentWeapon].SetActive(true);
		_canUseWeapons = true;
		_crosshair.Enable(0.2f);
	}



	private void transitToAim () {
		_moveTween.Kill();
		_mainFovTween.Kill();
		_wpnFovTween.Kill();


		float distFromHipToAim = Vector3.Distance(transform.InverseTransformPoint(_aimPosition.position) - _weapons[_currentWeapon].AimPosition.localPosition, Vector3.zero);
		float distFromWpnToAim = Vector3.Distance(transform.InverseTransformPoint(_aimPosition.position) - _weapons[_currentWeapon].AimPosition.localPosition, _weapons[_currentWeapon].transform.localPosition);
		float calcAimTime = distFromWpnToAim / distFromHipToAim * _weapons[_currentWeapon].AimTime;

		_moveTween = _weapons[_currentWeapon].transform.DOLocalMove(transform.InverseTransformPoint(_aimPosition.position) - _weapons[_currentWeapon].AimPosition.localPosition, calcAimTime).OnComplete(() => _currentAimState.Value = AimState.Aiming);

		_crosshair.Disable(calcAimTime);
		//TODO Get FOV from options and get substracted FOV from weapon

		_mainFovTween = _mainCamera.DOFieldOfView(45, _weapons[_currentWeapon].AimTime);
		_wpnFovTween = _weaponCamera.DOFieldOfView(45, _weapons[_currentWeapon].AimTime);
	}

	private void transitToHip () {
		_moveTween.Kill();
		_mainFovTween.Kill();
		_wpnFovTween.Kill();

		float distFromAimToHip = Vector3.Distance(transform.InverseTransformPoint(_aimPosition.position) - _weapons[_currentWeapon].AimPosition.localPosition, Vector3.zero);
		float distFromWpnToHip = Vector3.Distance(Vector3.zero, _weapons[_currentWeapon].transform.localPosition);
		float calcAimTime = distFromWpnToHip / distFromAimToHip * _weapons[_currentWeapon].AimTime;

		_moveTween = _weapons[_currentWeapon].transform.DOLocalMove(Vector3.zero, calcAimTime).OnComplete(() => _currentAimState.Value = AimState.Hip);

		_crosshair.Enable(calcAimTime);
		//TODO Get FOV from options

		_mainFovTween = _mainCamera.DOFieldOfView(60, _weapons[_currentWeapon].AimTime);
		_wpnFovTween = _weaponCamera.DOFieldOfView(60, _weapons[_currentWeapon].AimTime);
	}


	public void CancelAim () {
		_currentAimState.Value = AimState.ForcedTransitToHip;
	}

	private void abortAim () {
		if (_currentAimState.Value != AimState.Hip) {

			_moveTween.Kill();
			_mainFovTween.Kill();
			_wpnFovTween.Kill();

			_weapons[_currentWeapon].transform.localPosition = Vector3.zero;
			_mainCamera.fieldOfView = 60f;
			_weaponCamera.fieldOfView = 60f;

			_currentAimState.SetSilently(AimState.Hip);
		}
	}

	private void OnGUI() {
		AmmoBar.Instance.SetValues(_weapons[_currentWeapon].MagazineContent, _weapons[_currentWeapon].ReserveAmmo);
	}
}
