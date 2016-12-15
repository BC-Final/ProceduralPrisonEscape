using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackageControl : MonoBehaviour {

    private FeedbackPacket _storedPackage;

    private bool _copyModeActivated;
    private bool _blockModeActivated;

    public bool CopyModeActivated
    {
        get { return _copyModeActivated; }
        set { _copyModeActivated = value; UpdateColor(); }
    }
    public bool BlockModeActivated
    {
        get { return _blockModeActivated; }
        set { _blockModeActivated = value; UpdateSprite(); }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Q)) { CopyModeActivated = !CopyModeActivated; }
        if (Input.GetKeyDown(KeyCode.E)) { BlockModeActivated = !BlockModeActivated; }
    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Triggered");
        FeedbackPacket packet = other.GetComponent<FeedbackPacket>();
        if (packet != null)
        {
            if (CopyModeActivated) { _storedPackage = packet; }
            if (BlockModeActivated) { Destroy(packet.gameObject); }
        }
    }

    private void UpdateSprite()
    {
    }
    private void UpdateColor()
    {
    } 
}
