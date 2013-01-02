using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum TILETYPE {SPACE, WALL};
public enum AGENTTYPE {PLAYER, ENDOFQUEUE, MONSTER};
public enum FEATURETYPE {STAIRSDOWN, STAIRSUP, NOFEATURE};
public enum DIRECTION {UPLEFT, UP, UPRIGHT, DOWNLEFT, DOWN, DOWNRIGHT};

public class DungeonCode: MonoBehaviour {
	
	public static int numLevels=11; 	//initially set to 11 so we have 10 levels and array index
	public GameObject display;
	public GameObject userinterface;
	public GameObject main;
	
	Level[] Levels= new Level[numLevels];
	
	void Awake() {
		for (int i=1;i<numLevels;i++) {
			Levels[i]=new Level(i);
			spawnMonsters(i);
			spawnItems(i);
		}		
		
	}	
	
	public List<Location> getAllLevelLocations(int level) {
		
		List<Location> allLevelLocations = new List<Location>();
		
		for (int xCounter=0;xCounter<Level.levelSizeX;xCounter++) {
			for (int yCounter=0;yCounter<Level.levelSizeY;yCounter++) {
				allLevelLocations.Add(Levels[level].getLocation(new Vector2(xCounter,yCounter)));
			}
			
		}
		return allLevelLocations;
	}	
	
	public void spawnItems(int level) {
		Levels[level].spawnItems();	
	}	

	public void spawnMonsters(int level) {
		Levels[level].spawnMonsters();	
	}	
	
	
	public List<Agent> getAgents(int level) {
			return Levels[level].getAgents();
	}	
		
	public void Spawn (int level, AGENTTYPE agentToSpawn, Location spawnLocation) {
			Levels[level].Spawn(agentToSpawn,spawnLocation);
	}	
	
	public Location getEmptyLocation(int level) {
		return Levels[level].getEmptyLocation();
	}	
	
	public Agent getFrontAgent(int level) {
		//Agent firstAgent=Levels[level]
		return Levels[level].passFirstAgent();
	}	
	
	public void ProcessCommand(COMMAND inputCommand) {
		
		switch (inputCommand) {
		
		case COMMAND.UPLEFT :
			
			print ("upleft selected");
			tryMove(DIRECTION.UPLEFT);
			break;
		case COMMAND.UP :
			print ("up selected");
			tryMove(DIRECTION.UP);
			break;
		case COMMAND.UPRIGHT :
			print ("upright selected");
			tryMove(DIRECTION.UPRIGHT);
			break;
		case COMMAND.DOWNLEFT :
			print ("downleft selected");
			tryMove(DIRECTION.DOWNLEFT);
			break;
		case COMMAND.DOWN :
			print ("down selected");
			tryMove(DIRECTION.DOWN);
			break;
		case COMMAND.DOWNRIGHT :
			print ("downright selected");
			tryMove(DIRECTION.DOWNRIGHT);
			break;
		case COMMAND.WAIT :
			print ("wait selected");
			break;
		case COMMAND.ASCENDSTAIRS:
			print ("ascend selected");
			tryAscend();
			break;
		case COMMAND.DESCENDSTAIRS:	
			print ("descend selected");
			tryDescend();
			break;
		case COMMAND.PICKUP:	
			print ("pickup selected");
			tryPickup();
			break;
		case COMMAND.DROP:	
			print ("drop selected");
			tryDrop();
			break;			
		}		
		
	}
		
	void tryDrop() {
		int currentLevel=MainGameCode.getLevel();
		List<Agent> agents = Levels[currentLevel].getAgents();
		Agent_Player workingAgent = (Agent_Player) agents[agents.Count-1];
		Location agentLocation=workingAgent.getLocation();
		
		List<Item> inventory=workingAgent.getInventory();
		
		if (inventory.Count>0) {
			Item tempItem = inventory[0];
			userinterface.GetComponent<UserInterfaceCode>().setMessageLine("You drop the "+tempItem.getName()+"."); 
			inventory.RemoveAt(0);
			agentLocation.addItem(tempItem);
		}
		else {
			userinterface.GetComponent<UserInterfaceCode>().setMessageLine("You don't have anything to drop!");	
		}	
		
	}	
	
	void tryDescend() {
		int currentLevel=MainGameCode.getLevel();
		
		List<Agent> agents = Levels[currentLevel].getAgents();
		Feature tempFeature=Levels[currentLevel].getFeatureAtLocation(agents[agents.Count-1].getLocation());
		
		bool noDownStairs=false;
		if(!(tempFeature==null)) {
			if(tempFeature.getFeatureType()==FEATURETYPE.STAIRSDOWN) {
					userinterface.GetComponent<UserInterfaceCode>().setMessageLine("You go down the stairs.");
					main.GetComponent<MainGameCode>().GoDownStairs();	
			}	
			else noDownStairs=true;
		}
		else noDownStairs=true;	
		
		if (noDownStairs) userinterface.GetComponent<UserInterfaceCode>().setMessageLine("You can't go down here.");
		
	}
	
	void tryAscend() {
		int currentLevel=MainGameCode.getLevel();
		
		List<Agent>agents = Levels[currentLevel].getAgents();
		Feature tempFeature=Levels[currentLevel].getFeatureAtLocation(agents[agents.Count-1].getLocation());
		
		bool noUpStairs=false;
		if(!(tempFeature==null)) {
			if(tempFeature.getFeatureType()==FEATURETYPE.STAIRSUP) {
				
				if (MainGameCode.getLevel()==1) {
					main.GetComponent<MainGameCode>().WinGame();	
				}	
				else {
				userinterface.GetComponent<UserInterfaceCode>().setMessageLine("You go up the stairs.");
				main.GetComponent<MainGameCode>().GoUpStairs();
				}	
			}
			else noUpStairs=true;
				
		}
		else noUpStairs=true;
		
		if (noUpStairs) userinterface.GetComponent<UserInterfaceCode>().setMessageLine("You can't go up here.");
			
	}	
	
	public Location getLocationOfFeature(int level, FEATURETYPE featureLocationToGet) {
			List<Location> locations=getAllLevelLocations(level);
			
			Location tempLocation=null;
			
		
			foreach(Location location in locations) {
				if (location.getFeature()!=null && location.getFeature().getFeatureType()==featureLocationToGet) {
					tempLocation=location;
					break;
				}
			}
		
			return tempLocation;
	}	
	
	void tryPickup() {
		int currentLevel=MainGameCode.getLevel();
		List<Agent> agents = Levels[currentLevel].getAgents();
		Agent_Player workingAgent = (Agent_Player) agents[agents.Count-1];
		Location agentLocation=workingAgent.getLocation();
		
		List<Item> tempItems=agentLocation.getItems();
		
		if (tempItems.Count>0) {
			userinterface.GetComponent<UserInterfaceCode>().setMessageLine("You pick up the "+tempItems[0].getName()+".");
			workingAgent.addToInventory(tempItems[0]);			
			Destroy(tempItems[0].getItemDisplayObject());
			tempItems.RemoveAt(0);
			
		}	
		else {
			userinterface.GetComponent<UserInterfaceCode>().setMessageLine("Nothing to pick up.");
		}
		
	}		
	
	bool validMove(DIRECTION direction) {
		
		int currentLevel=MainGameCode.getLevel();
		List<Agent> agents = Levels[currentLevel].getAgents();
		
		return Levels[currentLevel].checkNeighbor(agents[agents.Count-1].getLocation(),direction);
	}
	
	void tryMove(DIRECTION direction) {

		int currentLevel=MainGameCode.getLevel();
		//List<Agent> agents = Levels[currentLevel].getAgents();		
		
		if (validMove(direction)) {
			Levels[currentLevel].moveActiveAgent(direction);
		}
		else userinterface.GetComponent<UserInterfaceCode>().setMessageLine("You can't move there.");
	}
	
	public Feature getFeatureAtLocation(Location location) {
		return location.getFeature();
	}
	
	public List<Item> getItemsAtLocation(int level, Vector2 coords)  {
		return Levels[level].getItemsAtLocation(coords);
	}	
	
	public TILETYPE getTileAtLocation(int level, Vector2 coords) {
		
		return Levels[level].getTileAtLocation(coords);
		
	}		
	
	public GameObject getDisplayObjectAtLoction(int level, Vector2 coords) {
		return Levels[level].getDisplayObjectAtLocation(coords);	
	}
	
	public void setDisplayObjectAtLocation(int level, Vector2 coords, GameObject inDisplayObject) {
		Levels[level].setDisplayObjectAtLocation(inDisplayObject,coords);
	}
		
	public List<Item> getPlayerInventory(int level) {
		List<Agent> agents = Levels[level].getAgents();
		List<Item> inventory = null;
			
		foreach(Agent agent in agents) {
			if (agent.getAgentType() == AGENTTYPE.PLAYER) {
				Agent_Player workingAgent= (Agent_Player) agent;
				inventory=workingAgent.getInventory();
			}	
		}
		
		return inventory;
	}	
	
}