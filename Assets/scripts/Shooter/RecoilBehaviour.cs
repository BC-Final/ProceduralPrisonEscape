using UnityEngine;
using System.Collections;
using DG.Tweening;

public class RecoilBehaviour : MonoBehaviour {
	//[SerializeField]
	//private float _maxSideRandom;

	public void ApplyRecoil(float pRecoilForce) {
		//FIX This conflicts with the mouselook
		//float rnd = Random.Range(-_maxSideRandom, _maxSideRandom);
		//Sequence recoil = DOTween.Sequence();
		//recoil.Append(transform.DORotate(transform.rotation.eulerAngles - new Vector3(pRecoilForce, rnd, 0.0f), 0.05f));
		//recoil.Append(transform.DORotate(transform.rotation.eulerAngles, 0.05f));

		transform.DORotate(transform.rotation.eulerAngles - new Vector3(pRecoilForce, 0.0f, 0.0f), 0.05f);

		//transform.Rotate(new Vector3(pRecoilForce, 0.0f, 0.0f));
	}
}
