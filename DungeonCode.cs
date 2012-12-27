using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum TILETYPE {SPACE, WALL};
public enum AGENTTYPE {PLAYER, ENDOFQUEUE, MONSTER};
public enum FEATURETYPE {STAIRSDOWN, STAIRSUP, NOFEATURE};
public enum DIRECTION {UPLEFT, UP, UPRIGHT, DOWNLEFT, DOWN, DOWNRIGHT};

public class DungeonCode: MonoBehaviour {
	
	static int numLevels=11; 	//initially set to 11 so we have 10 levels and array index
	public GameObject display;
	public GameObject userinterface;
	public GameObject main;
	
	Level[] Levels= new Level[numLevels];
	
	void Awake() {
		for (int i=0;i<numLevels;i++) {
			Levels[i]=new Level();
		}		
		
	}	
		
	public List<Agent> getAgents(int level) {
			return Levels[level].getAgents();
	}	
		
	public void Spawn (int level, AGENTTYPE agentToSpawn, Vector2 coords) {
			Levels[level].Spawn(agentToSpawn,coords);
	}	
	
	public Vector2 getEmptyLocation(int level) {
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
			break;
		case COMMAND.DESCENDSTAIRS:	
			print ("descend selected");
			tryDescend();
			break;
		}		
		
	}
	
	
	void tryDescend() {
		int currentLevel=MainGameCode.getLevel();
		
		List<Agent> agents = Levels[currentLevel].getAgents();
		Feature tempFeature=Levels[currentLevel].getFeatureAtLocation(agents[agents.Count-1].getLocation());
		
		bool noDownStairs=false;
		if(!(tempFeature==null)) {
			if(tempFeature.getFeatureType()==FEATURETYPE.STAIRSDOWN) {
				
				if (currentLevel==10) {
					main.GetComponent<MainGameCode>().WinGame();
				}
				else {
					main.GetComponent<MainGameCode>().GoDownStairs();
				}	
			}	
			else {
				noDownStairs=true;	
			}
		}
		else {
			noDownStairs=true;	
		}	
		if (noDownStairs) print ("You can't go down here.");
		
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
		else print("can't move there");
	}
	
	
	public Feature getFeatureAtLocation(int level, Vector2 coords) {
		return Levels[level].getFeatureAtLocation(coords);
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
	
	public void spawnMonsters(int level) {
		Levels[level].spawnMonsters();	
	}	
	
}


public class Level {
	public static int levelSizeX=30;
	public static int levelSizeY=20;
	
	static GameObject displayHolder=null;
	
	Location[,] Locations= new Location[levelSizeX,levelSizeY];
	List<Agent> Agents = new List<Agent>();


	public List<Agent> getAgents() {
		return Agents;	
	}

	public Level() {
		
		if (!(displayHolder)) displayHolder=GameObject.Find("Display");
				
		
		for (int counterX=0; counterX<levelSizeX; counterX++) {
			for (int counterY=0;counterY<levelSizeY; counterY++) {
				Locations[counterX,counterY]= new Location(new Vector2((float) counterX, (float) counterY));
			}
		}
		//add end of queue to agent list
		Agents.Add(new Agent(AGENTTYPE.ENDOFQUEUE,"endofqueue"));
		
		MakeStairs();
		
	}
	
	void MakeStairs() {
		Vector2 stairsLocation=getEmptyLocation();
		Locations[(int)stairsLocation.x,(int)stairsLocation.y].addFeature(FEATURETYPE.STAIRSDOWN);	
	}	
		
	public Vector2 getEmptyLocation() {
		int candidateX=0;
		int candidateY=0;
		bool emptyFound=false;
		
		while (!(emptyFound)) {
			candidateX=Random.Range(0,levelSizeX);
			candidateY=Random.Range(0,levelSizeY);
			
			if (isSpace(new Vector2((float)candidateX,(float) candidateY))) emptyFound=true;
		}
		
		
		return new Vector2 ((float)candidateX,(float)candidateY);
	}		
		
	
	public Vector2 getUnoccupiedLocation() {
		int candidateX=0;
		int candidateY=0;
		bool unOccupiedFound=false;	
		
		while (!(unOccupiedFound)) {
			candidateX=Random.Range(0,levelSizeX);
			candidateY=Random.Range(0,levelSizeY);
			
			if (isSpace(new Vector2((float)candidateX,(float) candidateY))) {
				unOccupiedFound=isUnoccupied(new Vector2((float) candidateX,(float) candidateY));
			}	
			
		}
		
		
		return new Vector2 ((float)candidateX,(float)candidateY);		
		
	}
	
	public TILETYPE getTileAtLocation(Vector2 coords) {
		
		return Locations[(int) coords.x,(int) coords.y].getTile();
		
	}
	
	public Feature getFeatureAtLocation(Vector2 coords) {		
		return Locations[(int) coords.x,(int) coords.y].getFeature();
		
	}	
		
	public void Spawn(AGENTTYPE agentToSpawn, Vector2 locationToSpawn) {
		switch (agentToSpawn) {
		
			case AGENTTYPE.PLAYER:
				//add the player to the front of the heartbeat queue
				Agents.Insert(0,new Agent(agentToSpawn,
									 "player",
									 locationToSpawn,
									 displayHolder.GetComponent<DisplayCode>().getDisplayObject(agentToSpawn))
									);
			break;
			case AGENTTYPE.MONSTER:
				//add the player to the front of the heartbeat queue
				Agents.Insert(0,new Agent(agentToSpawn,
									 "monster",
									 locationToSpawn,
									 displayHolder.GetComponent<DisplayCode>().getDisplayObject(agentToSpawn))
									);
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
		
	public bool checkNeighbor(Vector2 location, DIRECTION direction) {
		bool validMove=false;
		
		Vector2 neighborLocation = getNeighborCoords(location,direction);
			
		if (isSpace(neighborLocation) && isUnoccupied(neighborLocation)) validMove=true;
		
		return validMove;
	}
	
	
	public bool isUnoccupied(Vector2 location) {
		bool unoccupied=true;
		
			foreach (Agent agent in Agents) {
				Vector2 tempLocation=agent.getLocation();
					
				if (tempLocation==location) unoccupied=false;		
			}
		return unoccupied;
	}	
	
	public bool isSpace(Vector2 location) {
		bool isSpace=false;
		if (!((location.x>levelSizeX-1) || (location.x<-1) || (location.y>levelSizeY-1) || (location.y<-1))) {
			if (Locations[(int) location.x, (int) location.y].getTile()==TILETYPE.SPACE) isSpace=true;	
		}
		
		return  isSpace;	
	}
	
	public Vector2 getNeighborCoords(Vector2 startingCoords, DIRECTION direction) {

		Vector2 requestedLocation=startingCoords;
		
		switch (direction) {
		case DIRECTION.UPLEFT :
			requestedLocation+=new Vector2(-1,1);
			break;
		case DIRECTION.UP :
			requestedLocation+=new Vector2(0,1);
			break;
		case DIRECTION.UPRIGHT :
			requestedLocation+=new Vector2(1,0);
			break;
		case DIRECTION.DOWNLEFT :
			requestedLocation+=new Vector2(-1,0);
			break;
		case DIRECTION.DOWN :
			requestedLocation+=new Vector2(0,-1);
			break;
		case DIRECTION.DOWNRIGHT :
			requestedLocation+=new Vector2(1,-1);
			break;			
		}		
		
		return requestedLocation;
	}	
	
	public void moveActiveAgent(DIRECTION direction) {
		Agent activeAgent = Agents[Agents.Count-1];
		activeAgent.MoveTo(getNeighborCoords(activeAgent.getLocation(), direction));
	}
	
	public GameObject getDisplayObjectAtLocation(Vector2 coords) {
			return Locations[(int) coords.x,(int) coords.y].getLocationDisplayObject();
	}
	
	public void setDisplayObjectAtLocation(GameObject inDisplayObject, Vector2 coords) {
		Locations[(int) coords.x, (int) coords.y].setLocationDisplayObject(inDisplayObject);
	}	
	
	public void spawnMonsters() {
		int numberOfMonsters=Random.Range(1,10);
		
		for (int counter=0;counter<numberOfMonsters;counter++) {
			Spawn(AGENTTYPE.MONSTER,getUnoccupiedLocation());	
		}
	}	
	
}


	
public class Agent {
	AGENTTYPE agenttype;
	string agentName="";
	Vector2 agentLocation=new Vector2(-1,-1);
	GameObject agentDisplayObject=null;
	int energy=1000;
	static GameObject userinterfaceHolder=null;
	//bool occupiesSpace=true;
	
	
	public Agent(AGENTTYPE inType, string inName, Vector2 inLocation, GameObject inAgentDisplayObject) {
		agenttype=inType;
		agentName=inName;
		agentLocation=inLocation;
		agentDisplayObject=inAgentDisplayObject;
		
		if (!(userinterfaceHolder)) userinterfaceHolder=GameObject.Find("UserInterface");
	}

	public Agent(AGENTTYPE inType, string inName) {
		agenttype=inType;
		agentName=inName;
	}

	public Vector2 getLocation() {
		return agentLocation;
	}

	public GameObject getDisplayObject() {
		return agentDisplayObject;	
	}

	public string getName() {
		return agentName;
	}

	public int getEnergy() {
		return energy;
	}
	
	public AGENTTYPE getAgentType() {
		return agenttype;
	}
	
	public void Act() {
		if (energy>=1000) {
				
			switch (agenttype) {
			case AGENTTYPE.PLAYER :
				Debug.Log("player's turn");
				userinterfaceHolder.GetComponent<UserInterfaceCode>().AcceptPlayerCommand();
				//userinterface.GetComponent<UserInterfaceCode>().AcceptPlayerCommand();
				break;
			case AGENTTYPE.MONSTER :
				Debug.Log("monster's turn");
				break;				
					
			case AGENTTYPE.ENDOFQUEUE :
				Debug.Log("end of queue reached");
				break;
			
			}
		}
	}
	
	public void MoveTo(Vector2 moveLocation) {
			agentLocation=moveLocation;
		
	}
	
	
}


public class Location {
	TILETYPE tile;
	Feature feature=null;
	Vector2 coords;

	GameObject locationDisplayObject;
	static GameObject displayHolder;
	
	
	List<Agent> occupants=new List<Agent>();
	
	public Location(Vector2 inCoords) {
		tile=generateTile();
		coords=inCoords;
		
		if (!(displayHolder)) displayHolder=GameObject.Find("Display");
	}
	
	public void addFeature(FEATURETYPE featureToAdd) {
		switch (featureToAdd) {
		
			case FEATURETYPE.STAIRSDOWN:
				//add the player to the front of the heartbeat queue
				feature=new Feature(featureToAdd,
									"stairsdown",
									this);
			break;
			
		}	
	}
	
	public void addOccupant(Agent occupantToAdd) {
		occupants.Add(occupantToAdd);
	}	
	
	public TILETYPE getTile() {
		return tile;
	}
	
	static TILETYPE generateTile() {
		return (Random.Range(1,5)>3) ? TILETYPE.WALL : TILETYPE.SPACE;		
	}	
	
	public Feature getFeature() {
		
		Feature tempFeature=null;
		
		if (!(feature==null)) tempFeature=feature;
		
		return tempFeature;		
	}
	
	public Vector2 getCoords() {
	 	return coords;	
	}
	
	public void setLocationDisplayObject(GameObject inputDisplayObject) {
		locationDisplayObject=inputDisplayObject;	
	}
	
	public GameObject getLocationDisplayObject() {
		return locationDisplayObject;	
	}	
	
}

public class Feature {
	FEATURETYPE featureType;
	string featureName="";
	GameObject featureDisplayObject=null;
	Location myLocation;
	

	public Feature(FEATURETYPE inType, string inName, Location inLocation) {
		featureType=inType;
		featureName=inName;
		myLocation=inLocation;
		
		//if (!(userinterfaceHolder)) userinterfaceHolder=GameObject.Find("UserInterface");
	}	
	
	public Vector2 getLocation() {
		return myLocation.getCoords();
	}

	public GameObject getDisplayObject() {
		return featureDisplayObject;	
	}

	public string getName() {
		return featureName;
		
	}
	
	public void setFeatureDisplayObject(GameObject inDisplayObject) {
		featureDisplayObject=inDisplayObject;	
	}
	
	public FEATURETYPE getFeatureType() {
		return featureType;	
	}
}	