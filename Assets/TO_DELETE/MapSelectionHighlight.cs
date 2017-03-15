//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using DG.Tweening;
//using UnityEngine.UI;

//public class MapSelectionHighlight : MonoBehaviour {
//	public void IndicateSelection (Vector3 pPos) {
//		transform.position = pPos;
//		Sequence s = DOTween.Sequence();
//		s.Append(GetComponent<SpriteRenderer>().DOFade(1.0f, 0.1f));
//		s.Append(GetComponent<SpriteRenderer>().DOFade(0.5f, 0.1f));
//		s.SetLoops(3);
//		s.AppendCallback(() => Disable());
//	}

//	private void Disable () {
//		GetComponent<SpriteRenderer>().DOFade(0.0f, 0.1f);
//	}
//}
