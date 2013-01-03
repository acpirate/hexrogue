using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum COMMAND { UPLEFT, UP, UPRIGHT, DOWNLEFT, DOWN, DOWNRIGHT, WAIT, DESCENDSTAIRS, ASCENDSTAIRS, PICKUP, DROP };

public class UserInterfaceCode : MonoBehaviour {
	
	public GameObject dungeon;
	public GameObject messageLine;
	public GameObject main;
	
	int screenHeight=Screen.height;
	int screenWidth=Screen.width;
	
	bool isPlayerTurn=false;
	
	string inventoryDisplay="";
	
	int duplicateMessageCounter=1;
	
	// Use this for initialization
	
	void Awake() {
		
		
	}	
	
    void Start () {
		setMessageLine("Hello, "+UserInfo.getPlayerName()+"! Good luck on your adventure!");
    }
	
	void OnGUI () {
		
		GUI.BeginGroup (new Rect (0,0,screenWidth*.2f, (float) screenHeight));
		GUI.Box (new Rect (0,0,(float) screenWidth*.2f,(float) screenHeight),UserInfo.getPlayerName());
		GUI.Label(new Rect (10,20,500,20),"Dungeon Level: "+MainGameCode.getLevel());
		GUI.Label(new Rect (10,50,500,500),"Inventory:\n"+inventoryDisplay);
		GUI.EndGroup ();
	}
	
	
	public void updateInventoryDisplay() {
		
		inventoryDisplay="";
		List<Item> tempInventory = dungeon.GetComponent<DungeonCode>().getPlayerInventory(MainGameCode.getLevel());
		
		Dictionary<string,int> inventoryCount = new Dictionary<string,int>();

		
		
		foreach (Item item in tempInventory) {
			if (!(inventoryCount.ContainsKey(item.getName()))) {
				inventoryCount[item.getName()]=1;
			}
			else {
				inventoryCount[item.getName()]=(int) inventoryCount[item.getName()]+1;
			}	
			
		}
		foreach (KeyValuePair<string,int> countedItem in inventoryCount) {
			
			inventoryDisplay+=countedItem.Value+" "+countedItem.Key+"\n";
			
		}	
			
	}	
	
   	void Update () {
		
		//get keystrokes, only does anything if it is the player's turn
		if (isPlayerTurn) {
			
			if (Input.GetKeyDown(KeyCode.RightBracket)																			) SendCommand(COMMAND.DESCENDSTAIRS);
			if (Input.GetKeyDown(KeyCode.LeftBracket) 																			) SendCommand(COMMAND.ASCENDSTAIRS);			
			if (Input.GetKeyDown(KeyCode.Keypad7) || Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.Home)      		) SendCommand(COMMAND.UPLEFT); 
			if (Input.GetKeyDown(KeyCode.Keypad8) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)   		) SendCommand(COMMAND.UP); 
			if (Input.GetKeyDown(KeyCode.Keypad9) || Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.PageUp)    		) SendCommand(COMMAND.UPRIGHT);
			if (Input.GetKeyDown(KeyCode.Keypad1) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.End) 				) SendCommand(COMMAND.DOWNLEFT);
			if (Input.GetKeyDown(KeyCode.Keypad2) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)         ) SendCommand(COMMAND.DOWN);
			if (Input.GetKeyDown(KeyCode.Keypad3) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.PageDown)	    	) SendCommand(COMMAND.DOWNRIGHT);
			if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.F9) || Input.GetKeyDown(KeyCode.KeypadPeriod) ||
				Input.GetKeyDown(KeyCode.Delete)																				) SendCommand(COMMAND.WAIT);
			if (Input.GetKeyDown(KeyCode.Comma)																					) SendCommand(COMMAND.PICKUP);
			if (Input.GetKeyDown(KeyCode.P)																						) SendCommand(COMMAND.DROP);
		}
		
	}
	
	public void setMessageLine(string messageText) {
		string currentMessage=messageLine.GetComponent<GUIText>().text;
		
		if (duplicateMessageCounter>1) {
			currentMessage=currentMessage.Split('(')[0];
			currentMessage=currentMessage.Trim();
		}	
		
		if (messageText==currentMessage) {
			duplicateMessageCounter++;
		}	
		else duplicateMessageCounter=1;
		
		if (duplicateMessageCounter>1) messageText+=" (x"+duplicateMessageCounter+")";
		messageLine.GetComponent<GUIText>().text=messageText;
	}	
	
	void SendCommand(COMMAND inputCommand) {
		
		//first thing we do is turn off the player's ability to enter another command while we are processing the current one
		isPlayerTurn=false;
		
		dungeon.GetComponent<DungeonCode>().ProcessCommand(inputCommand);		
	}
	
	public bool PlayerTurnCheck() {
			return isPlayerTurn;
	}
	
	public void AcceptPlayerCommand() {
		isPlayerTurn=true;	
	}
	

	

}
