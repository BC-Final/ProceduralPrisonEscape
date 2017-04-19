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

		foreach (AbstractObjective o in _objectives) {
			if (!(o as BatteryObjective).IsInSlot() && Vector3.Distance((o as BatteryObjective).transform.position, _spawnPos.position) < _minDistance) {
				(o as BatteryObjective).Disable();

				(o as BatteryObjective).SetVisible(false);

				//TODO Reimplement
				//DOTween.Sequence()
				//.Append((o as BatteryObjective).transform.DOMove(_spawnPos.position, _toSpawnTime))
				//.Join((o as BatteryObjective).transform.DORotate(transform.rotation.eulerAngles, _toCenterTime))
				//.Append((o as BatteryObjective).transform.DOMove(transform.position, _toCenterTime))
				//.AppendCallback(() => o.Solved.Value = true);
			}
		}
	}
}
