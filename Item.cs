using UnityEngine;
using System.Collections;

public enum ITEMTYPE { WEAPON, ARMOR, FOOD, IDCARD };

public class Item  {
	
	string name;
	ITEMTYPE itemType;
	GameObject itemDisplayObject;
	
	public Item(ITEMTYPE inItemType, string inName) {
		name=inName;
		itemType=inItemType;
	}	
	
	public void setItemDisplayObject(GameObject inItemDisplayObject) {
		itemDisplayObject=inItemDisplayObject;	
	}	
	
	public GameObject getItemDisplayObject() {
		return itemDisplayObject;
	}	
	
	public ITEMTYPE getItemType() {
		return itemType;	
	}
	
	public string getName() {
		return name;	
	}	
	
}
