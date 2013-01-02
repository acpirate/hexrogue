using UnityEngine;
using System.Collections;

public class StartMenuCode : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnGUI () {
		GUI.enabled=false;
		if (GUI.Button (new Rect (10,10,150,100), "Continue")) {
			print ("load most recent game");
		}	
		GUI.enabled=true;
		if (GUI.Button (new Rect (10,110,150,100), "Tutorial")) {
			print ("go to tutorial screen");
			Application.LoadLevel ("tutorial"); 
		}		
		if (GUI.Button (new Rect (10,210,150,100), "New Game")) {
			print ("start a new game");
			Application.LoadLevel ("charactercreation"); 
		}
		if (GUI.Button (new Rect (10,310,150,100), "Load Game")) {
			print ("go to load game screen");
			Application.LoadLevel ("load"); 
		}		
		if (GUI.Button (new Rect (10,410,150,100), "Options")) {
			print ("go to config screen");
			Application.LoadLevel ("options"); 
		}	
		if (GUI.Button (new Rect (10,510,150,100), "About")) {
			print ("go to credits/about screen");
			Application.LoadLevel ("about"); 
		}			
	}	
}

public class UserInfo {
	static string playerName="Adventuro";
	
	public static void setPlayerName(string nameToSet) {
		playerName=nameToSet;	
	}
	
	public static string getPlayerName() {
		return playerName;	
	}	
}	