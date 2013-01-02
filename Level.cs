using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Level {
	public static int levelSizeX=30;
	public static int levelSizeY=20;
	
	static GameObject displayHolder=null;
	
	int levelNumber=0;
	
	Location[,] Locations= new Location[levelSizeX,levelSizeY];
	List<Agent> Agents = new List<Agent>();


	public List<Agent> getAgents() {
		return Agents;	
	}

	public Level(int levelToMake) {
		
		if (!(displayHolder)) displayHolder=GameObject.Find("Display");
				
		
		for (int counterX=0; counterX<levelSizeX; counterX++) {
			for (int counterY=0;counterY<levelSizeY; counterY++) {
				Locations[counterX,counterY]= new Location(new Vector2((float) counterX, (float) counterY));
			}
		}
		//add end of queue to agent list
		Agents.Add(new Agent(AGENTTYPE.ENDOFQUEUE,"endofqueue"));
		
		levelNumber=levelToMake;
		
		MakeStairs();
		
		
		
	}
	
	void MakeStairs() {
		//0 index arrays, bah
		// make down stairs, 1 per level except for the last level
		Location downStairsLocation=new Location(new Vector2(-1,-1));
		
		if (levelNumber!=DungeonCode.numLevels-1) {
			downStairsLocation=getEmptyLocation();
			downStairsLocation.addFeature(FEATURETYPE.STAIRSDOWN);	
		}	
		
		// make up stairs, 1 per level
		Location upStairsLocation=getEmptyLocation();
		
		while (upStairsLocation==downStairsLocation) upStairsLocation=getEmptyLocation();		
		
		upStairsLocation.addFeature(FEATURETYPE.STAIRSUP);
		
		
	}	
	
	public Location getLocation(Vector2 coords) {
		return Locations[(int) coords.x, (int) coords.y]; 	
	}	
	
	public Location getEmptyLocation() {
		int candidateX=0;
		int candidateY=0;
		bool emptyFound=false;
		
		while (!(emptyFound)) {
			candidateX=Random.Range(0,levelSizeX);
			candidateY=Random.Range(0,levelSizeY);
			
			if (isSpace(Locations[candidateX,candidateY])) emptyFound=true;
		}
		
		
		return Locations[candidateX,candidateY];
	}		
		
	
	public Location getUnoccupiedLocation() {
		int candidateX=0;
		int candidateY=0;
		bool unOccupiedFound=false;	
		
		while (!(unOccupiedFound)) {
			candidateX=Random.Range(0,levelSizeX);
			candidateY=Random.Range(0,levelSizeY);
			
			if (isSpace(Locations[candidateX,candidateY])) {
				unOccupiedFound=isUnoccupied(Locations[candidateX,candidateY]);
			}	
			
		}
		
		
		return Locations[candidateX,candidateY];		
		
	}
	
	public TILETYPE getTileAtLocation(Vector2 coords) {
		
		return Locations[(int) coords.x,(int) coords.y].getTile();
		
	}
	
	public Feature getFeatureAtLocation(Location featureLocation) {		
		return featureLocation.getFeature();
		
	}	
	
	public List<Item> getItemsAtLocation(Vector2 coords) {
		return Locations[(int) coords.x,(int) coords.y].getItems();	
	}	
		
	public void Spawn(AGENTTYPE agentToSpawn, Location locationToSpawn) {
		switch (agentToSpawn) {
		
			case AGENTTYPE.PLAYER:
				//add the player to the front of the heartbeat queue
				Agents.Insert(0,new Agent_Player(agentToSpawn,
									 "player",
									 locationToSpawn));
			break;
			case AGENTTYPE.MONSTER:
				//add the player to the front of the heartbeat queue
				Agents.Insert(0,new Agent_Monster(agentToSpawn,
									 "monster",
									 locationToSpawn));
			break;			
			
		}		
	}	
	
	public Agent passFirstAgent() {
		Agent tempAgent = Agents[0];
		//moves the agent from the front of the queue to the back of the queue
		Agents.Remove(tempAgent);
		Agents.Add(tempAgent);
		return tempAgent;
	}
		
	public bool checkNeighbor(Location location, DIRECTION direction) {
		bool validMove=false;
		
			Location neighborLocation = getNeighborLocation(location,direction);
			if (neighborLocation!=null) {
				if (isSpace(neighborLocation) && isUnoccupied(neighborLocation)) validMove=true;
			}
		
		return validMove;
	}
	

	public Location getNeighborLocation(Location startingLocation, DIRECTION direction) {

		int deltaX=0;
		int deltaY=0;
		
		Location requestedLocation=null;
		
		switch (direction) {
		case DIRECTION.UPLEFT :
			deltaX=-1;
			deltaY=1;
			//requestedLocation=Locations[(int)startingLocation.getCoords().x-1,(int)startingLocation.getCoords().y+1];
			break;
		case DIRECTION.UP :
			deltaX=0;
			deltaY=1;
			//requestedLocation=Locations[(int)startingLocation.getCoords().x,(int)startingLocation.getCoords().y+1];
			break;
		case DIRECTION.UPRIGHT :
			deltaX=1;
			deltaY=0;
			//requestedLocation=Locations[(int)startingLocation.getCoords().x+1,(int)startingLocation.getCoords().y];			
			break;
		case DIRECTION.DOWNLEFT :
			deltaX=-1;
			deltaY=0;
			//requestedLocation=Locations[(int)startingLocation.getCoords().x-1,(int)startingLocation.getCoords().y];				
			break;
		case DIRECTION.DOWN :
			deltaX=0;
			deltaY=-1;
			//requestedLocation=Locations[(int)startingLocation.getCoords().x,(int)startingLocation.getCoords().y-1];
			break;
		case DIRECTION.DOWNRIGHT :
			deltaX=1;
			deltaY=-1;
			//requestedLocation=Locations[(int)startingLocation.getCoords().x+1,(int)startingLocation.getCoords().y-1];
			break;			
		}		
		//checks to see if the neighbor location is out of range
		if ((startingLocation.getCoords().x+deltaX<levelSizeX && startingLocation.getCoords().x+deltaX>-1) 
			&& (startingLocation.getCoords().y+deltaY<levelSizeY) && (startingLocation.getCoords().y+deltaY>-1)) {
			requestedLocation=Locations[(int)startingLocation.getCoords().x+deltaX,(int)startingLocation.getCoords().y+deltaY];
		}
		
		return requestedLocation;
	}		
	
	
	public bool isUnoccupied(Location locationToCheck) {
		bool unoccupied=true;
		
			foreach (Agent agent in Agents) {
				Location tempLocation=agent.getLocation();
					
				if (tempLocation==locationToCheck) unoccupied=false;		
			}
		return unoccupied;
	}	
	
	public bool isSpace(Location locationToCheck) {
		bool isSpace=false;
		
		//if ((locationToCheck.getCoords().x<levelSizeX && locationToCheck.getCoords().x>-1) && (locationToCheck.getCoords().y<levelSizeY) && (locationToCheck.getCoords().y>-1)) {
			if (locationToCheck.getTile()==TILETYPE.SPACE) isSpace=true;	
		//}
		
		return  isSpace;	
	}
	
	
	public void moveActiveAgent(DIRECTION direction) {
		Agent activeAgent = Agents[Agents.Count-1];
		activeAgent.MoveTo(getNeighborLocation(activeAgent.getLocation(), direction));
	}
	
	public GameObject getDisplayObjectAtLocation(Vector2 coords) {
			return Locations[(int) coords.x,(int) coords.y].getLocationDisplayObject();
	}
	
	public void setDisplayObjectAtLocation(GameObject inDisplayObject, Vector2 coords) {
		Locations[(int) coords.x, (int) coords.y].setLocationDisplayObject(inDisplayObject);
	}	
	
	public void spawnMonsters() {
		int numberOfMonsters=Random.Range(1,4);
		
		for (int counter=0;counter<numberOfMonsters;counter++) {
			Spawn(AGENTTYPE.MONSTER,getUnoccupiedLocation());	
		}
	}	
	
	
	public void makeItem(ITEMTYPE itemToMake) {
		
		Location itemLocation=getEmptyLocation();
		
		switch (itemToMake) {
			
			case ITEMTYPE.FOOD :
				itemLocation.addItem(new Item(ITEMTYPE.FOOD,"food"));
			break;
			
		}
	}
	
	public void spawnItems() {
		int numberOfItems=Random.Range(5,10);
			
		for (int counter=0;counter<numberOfItems;counter++) {
			makeItem(ITEMTYPE.FOOD);	
		}		
	}	

}


