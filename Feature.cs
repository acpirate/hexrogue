using UnityEngine;
using System.Collections;

public class Feature {
	FEATURETYPE featureType;
	string featureName="";
	GameObject featureDisplayObject=null;
	Location myLocation; //make into location object
	

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
