using UnityEngine;
using System.Collections;

public class Agent_Monster : Agent {
		
	public Agent_Monster(AGENTTYPE inType, string inName, Location inLocation) 
		: base (inType,inName,inLocation) {
	}	
	
	public override void Act() {
		if (getEnergy()>=1000) {
			Debug.Log("subclass monster's turn");
			//userinterfaceHolder.GetComponent<UserInterfaceCode>().AcceptPlayerCommand();				
		}	
	}
	
}