using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BatteryModule : AbstractModule {
	[SerializeField]
	private Transform _spawnPos;

	[SerializeField]
	private float _minDistance = 1.5f;

	[SerializeField]
	private float _toSpawnTime = 0.5f;

	[SerializeField]
	private float _toCenterTime = 0.5f;


	public override void Interact () {
		base.Interact();

		BatteryObjective battery = _objective as BatteryObjective;

		if (!battery.IsInSlot() && Vector3.Distance(battery.transform.position, _spawnPos.position) < _minDistance) {
			battery.Disable();

			battery.SetVisible(false);

			DOTween.Sequence()
			.Append(battery.transform.DOMove(_spawnPos.position, _toSpawnTime))
			.Join(battery.transform.DORotate(transform.rotation.eulerAngles, _toCenterTime))
			.Append(battery.transform.DOMove(transform.position, _toCenterTime))
			.AppendCallback(() => _objective.SetSolved());
		}
	}
}
