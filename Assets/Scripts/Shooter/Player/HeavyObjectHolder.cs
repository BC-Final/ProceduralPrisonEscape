using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[SelectionBase]
public class HeavyObjectHolder : MonoBehaviour {
	[SerializeField]
	private float _pickUpTime;

	[SerializeField]
	private float _putDownTime;

	private BatteryObjective _cube;

	private bool _carrying;

	public void Pickup (BatteryObjective pCube) {
		pCube.transform.parent = this.transform;

		GetComponent<WeaponHolder>().DisableWeapons();
		GetComponentInParent<PlayerInteract>().SetActive(false);
		GetComponentInParent<PlayerMotor>().SetCanMove(false);
		//TODO Play animation

		DOTween.Sequence()
		.Append(pCube.transform.DOLocalMove(Vector3.zero, _pickUpTime))
		.Join(pCube.transform.DOLocalRotate(Vector3.zero, _pickUpTime))
		.AppendCallback(() => finishedPickup());

		_cube = pCube;
	}

	private void finishedPickup () {
		GetComponentInParent<PlayerMotor>().SetCanMove(true);
		GetComponentInParent<PlayerMotor>().SetSlowMove(true);
		_carrying = true;

		foreach (Transform t in _cube.GetComponentsInChildren<Transform>()) {
			t.gameObject.layer = LayerMask.NameToLayer("Weapons");
		}
	}

	private void finishedDrop () {
		_cube.transform.parent = null;
		_cube.Drop();
		

		GetComponentInParent<PlayerMotor>().SetCanMove(true);
		GetComponentInParent<PlayerMotor>().SetSlowMove(false);
		GetComponent<WeaponHolder>().EnableWeapons();
		FindObjectOfType<PlayerInteract>().SetActive(true);

		foreach (Transform t in _cube.GetComponentsInChildren<Transform>()) {
			t.gameObject.layer = LayerMask.NameToLayer("Default");
		}

		_cube = null;
	}

	public void Update () {
		if (_carrying && Input.GetKeyDown(KeyCode.E)) {
			GetComponentInParent<PlayerMotor>().SetCanMove(false);
			//TODO Play animation
			_carrying = false;

			_cube.transform.DOLocalMove(new Vector3(0, -0.75f, 0), _putDownTime).OnComplete(() => finishedDrop());
		}
	}
}
