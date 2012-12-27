using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DisplayCode : MonoBehaviour {
	
	public GameObject spacetile;
	public GameObject walltile;
	public float multy;
	public float multx;
	public GameObject dungeon;
	public GameObject player;	
	public GameObject gameCamera;
	public GameObject stairsdown;
	public GameObject monster;
	       
	
	void Awake () {
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}		
	
	public void CreateDisplayObjects(int levelToDraw) {
		GameObject tempTile=null;
		GameObject tileObject=null;
		
		//height offset is used to make sure the wall tiles are put in the correct position relative to the space tiles
		float heightoffset=0;
		
		int levelSizeX=Level.levelSizeX;
		int levelSizeY=Level.levelSizeY;
		//iterate over level and draw each tile and features
		for (int counterX=0;counterX<levelSizeX;counterX++) {
			for (int counterY=0;counterY<levelSizeY;counterY++) {
				tempTile=null;
				//draw level tiles
				switch (dungeon.GetComponent<DungeonCode>().getTileAtLocation(levelToDraw,new Vector2((float) counterX, (float) counterY))) {
					
					case TILETYPE.SPACE:
						tileObject=spacetile;
						heightoffset=0;
						break;
					case TILETYPE.WALL:
						tileObject=walltile;
						heightoffset=.5f;
						break;
				}	
					
				
				tempTile=(GameObject) Instantiate(tileObject,new Vector3(0,0,0),transform.rotation);
				
				
				tempTile.transform.Rotate(new Vector3(90,0,90));
				Vector2 tileScreenLocation = translateMapPositionToScreenCoords(new Vector2(counterX,counterY));
				tempTile.transform.position+=new Vector3(tileScreenLocation.x,heightoffset,tileScreenLocation.y);
				
				dungeon.GetComponent<DungeonCode>().setDisplayObjectAtLocation(levelToDraw,new Vector2(counterX,counterY),tempTile);
				
				Feature tempFeature=dungeon.GetComponent<DungeonCode>().getFeatureAtLocation(levelToDraw,new Vector2((float) counterX, (float) counterY));
				
				if (tempFeature!=null) {			
					switch (tempFeature.getFeatureType()) {
				
						case FEATURETYPE.STAIRSDOWN:
							tempFeature.setFeatureDisplayObject((GameObject) Instantiate(stairsdown,new Vector3(tileScreenLocation.x,.3f,tileScreenLocation.y),transform.rotation));
						break;
					}
				}
				
				
			}	
			
		}
	}	
	
	public void DrawAgents(int level) {
		List<Agent> Agents = dungeon.GetComponent<DungeonCode>().getAgents(level);
				

		foreach (Agent agentitem in Agents) {
			
			if (agentitem.getDisplayObject()!=null) {
				Vector2 agentScreenPosition = translateMapPositionToScreenCoords(agentitem.getLocation());
				agentitem.getDisplayObject().transform.position=new Vector3(agentScreenPosition.x,1,agentScreenPosition.y);
			}	
		}	
	}	
	
	
	public GameObject getDisplayObject(TILETYPE tile) {
		
		GameObject tempGameObject=null;
		
		switch (tile) {
		
		case TILETYPE.SPACE:
			tempGameObject=(GameObject) Instantiate(spacetile);
			break;
		
		case TILETYPE.WALL:
			tempGameObject=(GameObject) Instantiate(walltile);
			break;
		}
		
		return tempGameObject;
		
	}	

	public GameObject getDisplayObject(AGENTTYPE agent) {
		
		GameObject tempGameObject=null;
		
		switch (agent) {
		
		case AGENTTYPE.PLAYER:
			tempGameObject=(GameObject) Instantiate(player);
			break;
		case AGENTTYPE.MONSTER:
			tempGameObject=(GameObject) Instantiate(monster);
			break;
		}		
		
		return tempGameObject;
		
	}
	
	public GameObject getDisplayObject(FEATURETYPE feature) {
		
		GameObject tempGameObject=null;
		
		switch (feature) {
		
		case FEATURETYPE.STAIRSDOWN:
			tempGameObject=(GameObject) Instantiate(stairsdown);
			break;
		}
		
		return tempGameObject;
		
	}	
	
	public void centerCameraAbovePlayer() {
		List<Agent> Agents = dungeon.GetComponent<DungeonCode>().getAgents(MainGameCode.getLevel());
		
		foreach (Agent agentitem in Agents) {
			if (agentitem.getName()=="player") {
				Vector2 translatedLocation = translateMapPositionToScreenCoords(agentitem.getLocation());
				gameCamera.transform.position=new Vector3(translatedLocation.x,gameCamera.transform.position.y,translatedLocation.y);	
			}
			
		}
		
		
			
	}	
	
	public void DestroyLevelDisplay(int currentLevel) {
		List<Agent> Agents = dungeon.GetComponent<DungeonCode>().getAgents(currentLevel);
		
		print ("destroying level "+currentLevel);	
		
		foreach (Agent agent in Agents ) {
			if (!(agent.getDisplayObject()==null)) Destroy(agent.getDisplayObject());	
		}
		
		int levelSizeX=Level.levelSizeX;
		int levelSizeY=Level.levelSizeY;
		//iterate over level and draw each tile and features
		for (int counterX=0;counterX<levelSizeX;counterX++) {
			for (int counterY=0;counterY<levelSizeY;counterY++) {	
				Destroy(dungeon.GetComponent<DungeonCode>().getDisplayObjectAtLoction(currentLevel,new Vector2(counterX,counterY)));
				
				Feature tempFeature=dungeon.GetComponent<DungeonCode>().getFeatureAtLocation(currentLevel,new Vector2(counterX,counterY));
				
				if (!(tempFeature==null)) Destroy(tempFeature.getDisplayObject());
				
			}
		}	
		
	}	
	
	// returns the vector 2 of the screen position of a tile
	//changing calculation to use tilted y axis
	Vector2 translateMapPositionToScreenCoords(Vector2 coords) {
		
		float tempX =  (float)coords.x*multx;
		float tempY =  (float)coords.y*2*multy + coords.x*multy;
	//	if (coords.x%2==0) tempY+=	(float).5*multy;
		
		return new Vector2(tempX, tempY);
		
	}		
	
}
