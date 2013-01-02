using UnityEngine;
using System.Collections;

public class CharacterCreationCode : MonoBehaviour {
	
	string playername = UserInfo.getPlayerName();
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnGUI () {
		GUI.enabled=true;
		//has to be before the text field or else the text field will eat the enter key
		if (Event.current.type == EventType.KeyDown && (Event.current.keyCode == KeyCode.Return || Event.current.keyCode==KeyCode.KeypadEnter) && playername.Length>0) {
			startGame();
		}			
		if (GUI.Button (new Rect (10,10,150,100), "Character Creation")) {
			print("hit tutorial button");
		}
		GUI.Label(new Rect (10,120,100,20),"Enter your name");
		
		playername = GUI.TextField (new Rect (10, 160, 100, 30), playername);
				
		if (GUI.Button (new Rect (10,210,150,100), "Start Game")) {
			startGame();
		}		
		if (GUI.Button (new Rect (10,310,150,100), "Back to menu")) {
			print("hit back button");
			Application.LoadLevel ("startmenu"); 
		}		
	}	
	
	void startGame() {
		print("player name is "+playername+" starting game");
		UserInfo.setPlayerName(playername);
		Application.LoadLevel ("main"); 
	}	
}
