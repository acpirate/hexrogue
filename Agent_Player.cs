using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Agent_Player : Agent {
	
	List<Item> inventory= new List<Item>();
		
	public Agent_Player(AGENTTYPE inType, string inName, Location inLocation) 
		: base (inType,inName,inLocation) {
	}	
	
	public override void Act() {
		if (getEnergy()>=1000) {
			Debug.Log("subclass player's turn");
			userinterfaceHolder.GetComponent<UserInterfaceCode>().AcceptPlayerCommand();				
		}	
	}
	
	public void addToInventory(Item itemToAdd) {
		inventory.Add(itemToAdd);
		userinterfaceHolder.GetComponent<UserInterfaceCode>().updateInventoryDisplay();
	}
	
	public List<Item> getInventory() {
		return inventory;
	}	
	
}