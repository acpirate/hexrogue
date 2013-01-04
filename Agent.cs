using UnityEngine;
using System.Collections;

public class Agent {
	AGENTTYPE agenttype;
	string agentName="";
	Location agentLocation=null; //make into location object
	GameObject agentDisplayObject=null;
	int energy=1000;
	public static GameObject userinterfaceHolder=null;
	//bool occupiesSpace=true;
	
	
	public Agent(AGENTTYPE inType, string inName, Location inLocation) {
		agenttype=inType;
		agentName=inName;
		agentLocation=inLocation;
		
		inLocation.addOccupant(this);
		
		if (!(userinterfaceHolder)) userinterfaceHolder=GameObject.Find("UserInterface");
	}

	public Agent(AGENTTYPE inType, string inName) {
		agenttype=inType;
		agentName=inName;
	}

	public Location getLocation() {
		return agentLocation;
	}

	public GameObject getDisplayObject() {
		return agentDisplayObject;	
	}
	
	public void setDisplayObject(GameObject inDisplayObject) {
		agentDisplayObject=inDisplayObject;	
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
	
	public virtual void Act() {
		if (energy>=1000) {
				
			switch (agenttype) {
			case AGENTTYPE.PLAYER :
				Debug.Log("x player's turn");
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
	
	public void MoveTo(Location moveLocation) {
			agentLocation.removeOccupant(this);
			agentLocation=moveLocation;
			moveLocation.addOccupant(this);
		
	}
	
	
}


