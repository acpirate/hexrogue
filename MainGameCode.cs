using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MainGameCode : MonoBehaviour {
	
	public GameObject display;
	public GameObject dungeon;
	public GameObject mainCamera;
	public GameObject userinterface;
	static int currentLevel=1;
	
	// Use this for initialization
	void Start () {
		
		DrawMap();
		AddPlayerToDungeon();
		spawnMonsters(currentLevel);
		//DrawPlayer();
				
		
	}
	
	void DrawMap() {
		display.GetComponent<DisplayCode>().CreateDisplayObjects(currentLevel);
	}	
	
	void AddPlayerToDungeon() {	
		dungeon.GetComponent<DungeonCode>().Spawn(currentLevel, AGENTTYPE.PLAYER, dungeon.GetComponent<DungeonCode>().getEmptyLocation(currentLevel));	

	}
	
	void spawnMonsters(int level) {
			dungeon.GetComponent<DungeonCode>().spawnMonsters(level);
	}	
			
	
	void MoveCameraAbovePlayer() {
		display.GetComponent<DisplayCode>().centerCameraAbovePlayer();
	}		
	
	//remove the first agent from the queue, process ask him to act, return him to the end of the queue
	void AgentAct (int Level) {
		Agent actingAgent = dungeon.GetComponent<DungeonCode>().getFrontAgent(currentLevel);
		actingAgent.Act();
	}
	
	
	//This is the main game loop, if it isn't the players turn walk through the agent queue of the current level
	void Update () {
	
		while (!(userinterface.GetComponent<UserInterfaceCode>().PlayerTurnCheck())) {
			display.GetComponent<DisplayCode>().DrawAgents(currentLevel);
			MoveCameraAbovePlayer();
			AgentAct (currentLevel);
			
		}	
	}
	
	public void GoDownStairs() {
		//increment level counter
		//draw new level
		//move player to new level
		
		display.GetComponent<DisplayCode>().DestroyLevelDisplay(currentLevel);
		currentLevel++;
		print ("the current level is "+currentLevel);
		display.GetComponent<DisplayCode>().CreateDisplayObjects(currentLevel);
		dungeon.GetComponent<DungeonCode>().Spawn(currentLevel, AGENTTYPE.PLAYER, dungeon.GetComponent<DungeonCode>().getEmptyLocation(currentLevel));	
		dungeon.GetComponent<DungeonCode>().spawnMonsters(currentLevel);

	}	
	
	public void WinGame() {
		Application.LoadLevel ("win"); 	
	}	
	
	public static int getLevel() {
		return currentLevel;	
	}	

		
	
}
