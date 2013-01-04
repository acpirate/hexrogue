using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Agent_Monster : Agent {
		
	public Agent_Monster(AGENTTYPE inType, string inName, Location inLocation) 
		: base (inType,inName,inLocation) {
	}	
	
	public override void Act() {
		if (getEnergy()>=1000) {
			bool acted=false;
			
			
			//check surroundings
			//if any empty spaces are found move to one
			Debug.Log("subclass monster's turn");
			acted=tryAttack();
			if (!(acted)) tryMove();
			//userinterfaceHolder.GetComponent<UserInterfaceCode>().AcceptPlayerCommand();				
		}	
	}
	
	
	bool tryAttack() {
		bool attacked=false;	
		
		List<Location> adjacentLocations=getLocation().getAdjacentLocations();
		
		foreach(Location tempLocation in adjacentLocations) {
			if (tempLocation.hasPlayer()) {
				foreach(Agent tempAgent in tempLocation.getOccupants()) {
					if (tempAgent.getAgentType()==AGENTTYPE.PLAYER) {
						Agent_Player tempPlayer=(Agent_Player) tempAgent;
						tempPlayer.takeDamage();
						userinterfaceHolder.GetComponent<UserInterfaceCode>().setMessageLine(getName()+" hit you!");
					}	
				}				
				attacked=true;
				break;
			}	
		}	
			
		
		return attacked;
	}	
	
	bool tryMove() {
		bool triedMove=true;
		
		List<Location> adjacentLocations=getLocation().getAdjacentLocations();
		
		List<Location> availableLocations=new List<Location>();
		
		foreach (Location tempLocaton in adjacentLocations) {
			if(tempLocaton.availableForMove()) availableLocations.Add(tempLocaton);
		}	
		
		if (availableLocations.Count>0) {
			base.MoveTo(availableLocations[Random.Range(0,availableLocations.Count)]);	
		}
		else Debug.Log("no available moves for "+getName()+".");
		
		return triedMove;
	}
	
	
}