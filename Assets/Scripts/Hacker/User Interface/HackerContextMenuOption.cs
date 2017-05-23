using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class HackerContextMenuOption : MonoBehaviour {
	/// <summary>
	/// Action data for this context menu option
	/// </summary>
	private AbstractMapIcon.ActionData _actionData;



	/// <summary>
	/// Reference to own button component
	/// </summary>
	private Button _button;



	/// <summary>
	/// Set up references and add callbacks
	/// </summary>
	private void Start () {
		_button = GetComponent<Button>();
		_button.onClick.AddListener(() => _actionData.Action.Invoke());
		_button.onClick.AddListener(() => HackerResourceManager.Instance.RemoveHackerPoints(_actionData.HackerPointsCost));
	}



	/// <summary>
	/// 
	/// </summary>
	/// <param name="pDisplayName"></param>
	/// <param name="pCallback"></param>
	/// <param name="pSpacing"></param>
	/// <param name="pPos"></param>
	/// <param name="pContentHeight"></param>
	/// <param name="pHackerPointsCost"></param>
	public void Initialize (AbstractMapIcon.ActionData pActionData, int pOrderPos, float pSpacing,  float pContentHeight, float pPanning) {
		_actionData = pActionData;
		//Determine if I need to display hp cost
		//TODO Improve the HP cost display
		string hpCost = "";

		if (pActionData.HackerPointsCost > 0) {
			hpCost = " " + pActionData.HackerPointsCost + " HP";
		}

		GetComponentInChildren<Text>().text = pActionData.DisplayName + hpCost;

		//This was for spacing around button
		//(transform as RectTransform).anchoredPosition3D = new Vector3(pSpacing, -((pPos + 1) * pSpacing + pPos * contentHeight), -5.0f);
		//(transform as RectTransform).sizeDelta = new Vector2(contentHeight, (transform as RectTransform).sizeDelta.y);

		(transform as RectTransform).anchoredPosition3D = new Vector3(pPanning, -(pOrderPos * pContentHeight + pPanning), -5.0f);
		(transform as RectTransform).sizeDelta = new Vector2(GetPreferedWidth() + 2 * pSpacing, pContentHeight);
		(GetComponentInChildren<Text>().transform as RectTransform).sizeDelta = new Vector2(GetPreferedWidth(), pContentHeight - 2 * pSpacing);

		transform.localScale = Vector3.one;
	}



	/// <summary>
	/// Adjusts the width of the context menu option
	/// </summary>
	/// <param name="pWidth">The desired width</param>
	public void AdjustWidth (float pWidth) {
		(transform as RectTransform).sizeDelta = new Vector2(pWidth, (transform as RectTransform).sizeDelta.y);
	}



	/// <summary>
	/// Gets the preffered width of the text component of the context menu option
	/// </summary>
	/// <returns>The preffered width</returns>
	public float GetPreferedWidth () {
		return GetComponentInChildren<Text>().preferredWidth;
	}



	/// <summary>
	/// Determines if a option should be displayed as clickable based on hackerpoints cost
	/// </summary>
	private void OnGUI () {
		if (HackerResourceManager.Instance.HackerPoints < _actionData.HackerPointsCost && _button.interactable) {
			_button.interactable = false;
		} else if (HackerResourceManager.Instance.HackerPoints >= _actionData.HackerPointsCost && !_button.interactable) {
			_button.interactable = true;
		}
	}
}
