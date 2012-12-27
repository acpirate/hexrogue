using UnityEngine;
using System.Collections;

public class TileCode : MonoBehaviour {
	
	int x;
	int y;
	

	// Use this for initialization
	void Start () {
	
	}
	
	public void setCoords(int inputX, int inputY) {
	
		x=inputX;
		y=inputY;
		
	}	
	
	public Vector2 getCoords() {
		return new Vector2(x,y);	
	}	
	
	
	
	// Update is called once per frame
	void Update () {
	
	}
}
