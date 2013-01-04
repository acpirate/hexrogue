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
		GUI.Label(new Rect(200,200,1000,1000),
			
			"Your goal is to retrieve the ID card from the last level and escape the facility\n\n"+"Controls:\n"+
			"Move: QWEASD or 789123(numberpad)\n" +
			"Pickup: , (comma)\n" +
			"Drop: p\n" +
			"Ascend Stairs: [\n" +
			"Descend Stairs: ]\n" +
			"Wait: (spacebar)");
	}	
}
