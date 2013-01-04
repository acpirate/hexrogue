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
		
		AddPlayerToDungeon();	
		
		//drawing the map happens last
		DrawMap();
				
		
	}
	
	void spawnItems(int level) {
		dungeon.GetComponent<DungeonCode>().spawnItems(currentLevel);
		
	}	
	
	void DrawMap() {
		display.GetComponent<DisplayCode>().CreateDisplayObjects(currentLevel);
	}	
	
	void AddPlayerToDungeon() {	
		dungeon.GetComponent<DungeonCode>().Spawn(currentLevel, AGENTTYPE.PLAYER, dungeon.GetComponent<DungeonCode>().getLocationOfFeature(currentLevel, FEATURETYPE.STAIRSUP));	

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
		if (dungeon.GetComponent<DungeonCode>().getPlayerHealth()<1) LoseGame();
		
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

		transferPlayerBetweenLevels(currentLevel-1,currentLevel);
		//important that this happens last
		display.GetComponent<DisplayCode>().CreateDisplayObjects(currentLevel);		

	}
	
	public void GoUpStairs() {
		//decrement level counter
		//draw new level
		//move player to new level
		
		display.GetComponent<DisplayCode>().DestroyLevelDisplay(currentLevel);
		currentLevel--;
		print ("the current level is "+currentLevel);

		//dungeon.GetComponent<DungeonCode>().Spawn(currentLevel, AGENTTYPE.PLAYER, dungeon.GetComponent<DungeonCode>().getEmptyLocation(currentLevel));	
		transferPlayerBetweenLevels(currentLevel+1,currentLevel);
		
		//important that this happens last
		display.GetComponent<DisplayCode>().CreateDisplayObjects(currentLevel);		

	}
	
	
	void transferPlayerBetweenLevels(int oldLevel, int newLevel) {
		List<Agent> oldAgents = dungeon.GetComponent<DungeonCode>().getAgents(oldLevel);
		List<Agent> newAgents = dungeon.GetComponent<DungeonCode>().getAgents(newLevel);
		
		Agent playerHolder = null;
		
		foreach(Agent agent in oldAgents) {
			if (agent.getAgentType()==AGENTTYPE.PLAYER) {
				playerHolder=agent;
				break;
			}
		}	
		
		oldAgents.Remove(playerHolder);
		newAgents.Add(playerHolder);
		
		FEATURETYPE whichStairs = FEATURETYPE.NOFEATURE;
		
		if (oldLevel>newLevel) whichStairs=FEATURETYPE.STAIRSDOWN;
		if (oldLevel<newLevel) whichStairs=FEATURETYPE.STAIRSUP;
		
		playerHolder.MoveTo(dungeon.GetComponent<DungeonCode>().getLocationOfFeature(newLevel, whichStairs));
		
	}	
	
	public void WinGame() {
		currentLevel=1;
		Application.LoadLevel ("win"); 	
	}	
	
	public void LoseGame() {
		currentLevel=1;
		Application.LoadLevel ("lose"); 	
	}	
	
	public static int getLevel() {
		return currentLevel;	
	}	

		
	
}
