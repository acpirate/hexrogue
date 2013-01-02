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
	public GameObject food;
	public GameObject stairsup;
	//magic number to adjust camera so player stays centered in screen with gui
	public int cameraOffset;
	       
	
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
		
		//iterate over level and draw each tile and features
		
		
		List<Location> levelLocations=dungeon.GetComponent<DungeonCode>().getAllLevelLocations(levelToDraw);
		
		
		foreach(Location location in levelLocations) {
			
			tempTile=null;
			//draw level tiles
			switch (location.getTile()) {
				
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
			Vector2 tileScreenLocation = translateMapPositionToScreenCoords(location.getCoords());
			tempTile.transform.position+=new Vector3(tileScreenLocation.x,heightoffset,tileScreenLocation.y);
			
			location.setLocationDisplayObject(tempTile);
			//draw features
			Feature tempFeature=location.getFeature();
			
			if (tempFeature!=null) {			
				switch (tempFeature.getFeatureType()) {
			
					case FEATURETYPE.STAIRSDOWN:
						tempFeature.setFeatureDisplayObject((GameObject) Instantiate(stairsdown,new Vector3(tileScreenLocation.x,.3f,tileScreenLocation.y),transform.rotation));
					break;
					case FEATURETYPE.STAIRSUP:
						tempFeature.setFeatureDisplayObject((GameObject) Instantiate(stairsup,new Vector3(tileScreenLocation.x,.5f,tileScreenLocation.y),transform.rotation));
						tempFeature.getDisplayObject().transform.Rotate(new Vector3(-90,0,0));
					break;					
				}
			}
			//draw items
			DrawItemsOnFloor(MainGameCode.getLevel(),new Vector2(location.getCoords().x,location.getCoords().y));
			
		}	
		
	}
	
	public void DrawAgents(int level) {
		List<Agent> Agents = dungeon.GetComponent<DungeonCode>().getAgents(level);
				

		foreach (Agent agentitem in Agents) {
			if (agentitem.getDisplayObject()==null) {
				agentitem.setDisplayObject(getDisplayObject(agentitem.getAgentType()));
			}	
			if (agentitem.getDisplayObject()!=null) {
				Vector2 agentScreenPosition = translateMapPositionToScreenCoords(agentitem.getLocation().getCoords());
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
		case FEATURETYPE.STAIRSUP:
			tempGameObject=(GameObject) Instantiate(stairsup);
			break;
		}
		
		
		return tempGameObject;
		
	}	
	
	public GameObject getDisplayObject(ITEMTYPE item) {
		GameObject tempGameObject=null;
		
		switch (item) {
			case ITEMTYPE.FOOD:
			
			tempGameObject=(GameObject) Instantiate(food);
			//adjust the y position so the object is drawn above the floor
			tempGameObject.transform.position=new Vector3(0,.5f,0);
			break;
		}
		
		return tempGameObject;
	}	
	
	
	public void centerCameraAbovePlayer() {
		List<Agent> Agents = dungeon.GetComponent<DungeonCode>().getAgents(MainGameCode.getLevel());
		
		foreach (Agent agentitem in Agents) {
			if (agentitem.getName()=="player") {
				Vector2 translatedLocation = translateMapPositionToScreenCoords(agentitem.getLocation().getCoords());
				gameCamera.transform.position=new Vector3(translatedLocation.x-cameraOffset,gameCamera.transform.position.y,translatedLocation.y);	
			}
			
		}
		
		
			
	}
	
	public void DrawItemsOnFloor(int level, Vector2 coords) {
		List<Item> itemsToDraw=dungeon.GetComponent<DungeonCode>().getItemsAtLocation(level,coords);
		
		foreach(Item item in itemsToDraw) {
			if (item.getItemDisplayObject()==null) {
				GameObject tempDisplayObject=getDisplayObject(item.getItemType());
				Vector2 itemCoords=translateMapPositionToScreenCoords(coords);
				tempDisplayObject.transform.position+=new Vector3(itemCoords.x,0,itemCoords.y);
				item.setItemDisplayObject(tempDisplayObject);
			}	
			
			
		}	
		
	}	
	
	public void DestroyLevelDisplay(int currentLevel) {
		List<Agent> Agents = dungeon.GetComponent<DungeonCode>().getAgents(currentLevel);
		
		print ("destroying level "+currentLevel);	
		
		foreach (Agent agent in Agents ) {
			if (agent.getDisplayObject()!=null && agent.getAgentType()!=AGENTTYPE.PLAYER) Destroy(agent.getDisplayObject());	
		}

		List<Location> levelLocations=dungeon.GetComponent<DungeonCode>().getAllLevelLocations(currentLevel);
		
		foreach(Location location in levelLocations) {
			Destroy(location.getLocationDisplayObject());	
			
			Feature tempFeature=location.getFeature();
			
			if (!(tempFeature==null)) Destroy(tempFeature.getDisplayObject());
				
			List<Item> tempitems=location.getItems();
			
			foreach(Item item in tempitems) {
				Destroy(item.getItemDisplayObject());
							
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
