using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BatteryCubeSlot : AbstractModule {
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

		foreach (Objective o in _objectives) {
			if (!(o as BatteryCube).IsInSlot() && Vector3.Distance((o as BatteryCube).transform.position, _spawnPos.position) < _minDistance) {
				(o as BatteryCube).Disable();

				(o as BatteryCube).SetVisible(false);

				DOTween.Sequence()
				.Append((o as BatteryCube).transform.DOMove(_spawnPos.position, _toSpawnTime))
				.Join((o as BatteryCube).transform.DORotate(transform.rotation.eulerAngles, _toCenterTime))
				.Append((o as BatteryCube).transform.DOMove(transform.position, _toCenterTime))
				.AppendCallback(() => o.Solved.Value = true);
			}
		}
	}
}
