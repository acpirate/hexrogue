using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Location {
	TILETYPE tile;
	Feature feature=null;
	Vector2 coords;
	List<Item> items=new List<Item>();
	
	GameObject locationDisplayObject;
	static GameObject displayHolder;
	
	List<Agent> occupants=new List<Agent>();
	
	public Location(Vector2 inCoords) {
		tile=generateTile();
		coords=inCoords;
		
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
	
}
