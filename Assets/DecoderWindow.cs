using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DecoderWindow : MonoBehaviour {

    private DoorMapIcon _door;
    [SerializeField]
    private Text _text;

    public string Solution;

    private void Awake()
    {
        foreach(DecoderNumber num in GetComponentsInChildren<DecoderNumber>())
        {
            num.Window = this;
        }
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (_door)
            {
                _door.showWindow = false;
            }
            GameObject.Destroy(this.gameObject);
        }
    }

    public void PuzzleSolved()
    {
        _text.text = Solution;
    }

    public void SetDoor(DoorMapIcon door)
    {
        _door = door;
    }
}
