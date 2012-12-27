using UnityEngine;
using System.Collections;

public enum COMMAND { UPLEFT, UP, UPRIGHT, DOWNLEFT, DOWN, DOWNRIGHT, WAIT, DESCENDSTAIRS, ASCENDSTAIRS };

public class UserInterfaceCode : MonoBehaviour {
	
	public GameObject dungeon;
	
	
	bool isPlayerTurn=false;
	
    // Use this for initialization

    void Start () {

     //   this.testStartRoutine();   

    }
	
	void OnGUI () {
		
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

		}
		
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
