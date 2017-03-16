using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class HackerContextMenu : MonoBehaviour, IPointerExitHandler {
	[SerializeField]
	[Tooltip("Spacing between context menu option its and text")]
	private float _spacing = 2.5f;



	[SerializeField]
	[Tooltip("Height of each context menu option")]
	private float _contentHeight = 20.0f;



	/// <summary>
	/// Displays the ContextMenu at the curser.
	/// </summary>
	/// <param name="pActionData">The data for clickable options</param>
	public static void Display (AbstractMapIcon.ActionData[] pActionData) {
		HackerContextMenu hcm = GameObject.Instantiate(HackerReferenceManager.Instance.ContextMenu, FindObjectOfType<Canvas>().transform).GetComponent<HackerContextMenu>();

		hcm.display(pActionData);
	}



	/// <summary>
	/// Current instance of the context menu
	/// </summary>
	private static HackerContextMenu _instance;



	/// <summary>
	/// Awake checks if there already is an instance and destroys it.
	/// </summary>
	private void Awake () {
		if (_instance != null) {
			Destroy(_instance.gameObject);
		}

		_instance = this;
	}



	/// <summary>
	/// Hides the context window when the cursor moves away
	/// </summary>
	/// <param name="pPointerEventData">Data of the pointer event</param>
	public void OnPointerExit (PointerEventData pPointerEventData) {
		hide();
	}


	/// <summary>
	/// Destroys the current Conect Mneu instance
	/// </summary>
	private void hide () {
		Destroy(this.gameObject);
	}



	/// <summary>
	/// Sets up the context menu and the options
	/// </summary>
	/// <param name="pActionData">The data for clickable options</param>
	private void display (AbstractMapIcon.ActionData[] pActionData) {
		//Find the mouse position on canvas and place context menu at it
		Vector2 clickPosOnCanvas;
		Canvas canvas = GetComponentInParent<Canvas>();
		RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, canvas.worldCamera, out clickPosOnCanvas);
		(transform as RectTransform).anchoredPosition = clickPosOnCanvas;

		//Storing the biggest content width
		float maxContentWidth = 0.0f;

		//Counter for the foreach loop
		int counter = 0;

		//List of all created context menu options
		List<HackerContextMenuOption> options = new List<HackerContextMenuOption>();

		//Loop for creating each context menu option
		foreach (AbstractMapIcon.ActionData data in pActionData) {
			//Instantiating option and adding it to list
			HackerContextMenuOption option = Instantiate(HackerReferenceManager.Instance.ContextMenuOption, transform).GetComponent<HackerContextMenuOption>();
			options.Add(option);
			
			//Initialize the context menu option
			option.Initialize(data, counter, _spacing, _contentHeight);

			//Add listener for when a context menu option is clicked
			option.GetComponent<Button>().onClick.AddListener(() => hide());

			//Determine if this context menu option is bigger than the current max content with
			maxContentWidth = (maxContentWidth < (option.GetPreferedWidth() + 2 * _spacing)) ? option.GetPreferedWidth() + 2 * _spacing : maxContentWidth;

			//Increase loop counter
			counter++;
		}

		//Adjust the width of every context menu option to fit the max content width
		foreach (HackerContextMenuOption opt in options) {
			opt.AdjustWidth(maxContentWidth);
		}

		//Change the size of the context menu to fit the context menu options
		(transform as RectTransform).sizeDelta = new Vector2(maxContentWidth, _contentHeight * pActionData.Length);

		//Display the context menu
		this.gameObject.SetActive(true);
	}
}
