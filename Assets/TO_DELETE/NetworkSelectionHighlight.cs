//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using DG.Tweening;
//using UnityEngine.UI;

//public class NetworkSelectionHighlight : MonoBehaviour {
//	public void IndicateSelection (Vector2 pPos) {
//		GetComponent<RectTransform>().anchoredPosition = pPos;
//		Sequence s = DOTween.Sequence();
//		s.Append(GetComponent<Image>().DOFade(1.0f, 0.1f));
//		s.Append(GetComponent<Image>().DOFade(0.5f, 0.1f));
//		s.SetLoops(3);
//		s.AppendCallback(() => Disable());
//	}

//	private void Disable () {
//		GetComponent<Image>().DOFade(0.0f, 0.1f);
//	}
//}
