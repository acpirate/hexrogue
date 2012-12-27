using UnityEngine;
using System.Collections;

public class TutorialCode: MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnGUI () {
		GUI.enabled=true;
		if (GUI.Button (new Rect (10,10,150,100), "This is the tutorial")) {
			print("hit tutorial button");
		}
		if (GUI.Button (new Rect (10,110,150,100), "Back to menu")) {
			print("hit back button");
			Application.LoadLevel ("startmenu"); 
		}		
	}	
}
