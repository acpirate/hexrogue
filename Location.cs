using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Location {
	TILETYPE tile;
	Feature feature=null;
	Vector2 coords;
	List<Item> items=new List<Item>();
	Level level;
	
	
	GameObject locationDisplayObject;
	static GameObject displayHolder;
	
	List<Agent> occupants=new List<Agent>();
	
	public Location(Vector2 inCoords, Level inLevel) {
		tile=generateTile();
		coords=inCoords;
		level=inLevel;
		
		if (!(displayHolder)) displayHolder=GameObject.Find("Display");
	}
	
	public void addItem(Item itemToAdd) {
		items.Add(itemToAdd);
		displayHolder.GetComponent<DisplayCode>().DrawItemsOnFloor(MainGameCode.getLevel(),coords);
	}	
	
	public List<Item> getItems() {
		return items;	
	}	
	
	public void addFeature(FEATURETYPE featureToAdd) {
		switch (featureToAdd) {
		
			case FEATURETYPE.STAIRSDOWN:
				feature=new Feature(featureToAdd,
									"stairsdown",
									this);
			break;
			case FEATURETYPE.STAIRSUP:
				feature=new Feature(featureToAdd,
									"stairsup",
									this);
			break;			
			
		}	
	}
	
	public void addOccupant(Agent occupantToAdd) {
		occupants.Add(occupantToAdd);
	}
	
	public void removeOccupant(Agent occupantToRemove) {
		occupants.Remove(occupantToRemove);
	}	
	
	public List<Agent> getOccupants() {
		return occupants;	
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

	public List<Location> getAdjacentLocations() {
		
		List<Location> adjacentLocations=new List<Location>();
		Location tempNeighbor=null;
		
		tempNeighbor=level.getNeighborLocation(this,DIRECTION.UPLEFT);
		if (tempNeighbor!=null) {
			adjacentLocations.Add(tempNeighbor);
			tempNeighbor=null;
		}
		tempNeighbor=level.getNeighborLocation(this,DIRECTION.UP);
		if (tempNeighbor!=null) {
			adjacentLocations.Add(tempNeighbor);
			tempNeighbor=null;
		}		
		tempNeighbor=level.getNeighborLocation(this,DIRECTION.UPRIGHT);
		if (tempNeighbor!=null) {
			adjacentLocations.Add(tempNeighbor);
			tempNeighbor=null;
		}
		tempNeighbor=level.getNeighborLocation(this,DIRECTION.DOWNLEFT);
		if (tempNeighbor!=null) {
			adjacentLocations.Add(tempNeighbor);
			tempNeighbor=null;
		}
		tempNeighbor=level.getNeighborLocation(this,DIRECTION.DOWN);
		if (tempNeighbor!=null) {
			adjacentLocations.Add(tempNeighbor);
			tempNeighbor=null;
		}
		tempNeighbor=level.getNeighborLocation(this,DIRECTION.DOWNRIGHT);
		if (tempNeighbor!=null) {
			adjacentLocations.Add(tempNeighbor);
		}
		
		return adjacentLocations;
		
	}
	
	public bool availableForMove() {
		bool availableForMove=false;
		
		if (tile==TILETYPE.SPACE && occupants.Count==0) availableForMove=true;
		
		return availableForMove;
	}
	
	public bool hasPlayer() {
		bool hasPlayer=false;
			
			foreach(Agent agent in occupants) {
				if (agent.getAgentType()==AGENTTYPE.PLAYER) {
					hasPlayer=true;
					break;
				}
			}
		
		return hasPlayer;
	}	
	
}
