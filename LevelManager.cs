using UnityEngine;
using System.Collections;

public class LevelManager: MonoBehaviour {
	
	private static bool _keyPressed = false;

/*	void Awake () {
	//This is where it all begins.
		print("waking up...");
	}

	// Use this for initialization
	void Start () {
		print("starting...");
	//in order to lock a coroutine down and keep it from running in parallel, we must wrap a coroutine inside of the first one.
		StartCoroutine(FirstFunction());
	}*/

	public IEnumerator FirstFunction()
	{
		//by default in our settings this is the space bar, so we will use this just to make it simple.
		yield return StartCoroutine(WaitForKeyPress("Jump"));
		//this time the coroutine won't fire this print statement off until it completes
		print("Coroutine did finish.");
	}
	
//	private bool _keyPressed = false;

	public IEnumerator WaitForKeyPress(string _button)
	{
		while(!_keyPressed)
		{
			if(Input.GetButtonDown(_button))
			{
				StartGame();
				break;
			}
			print("Awaiting key input.");
			yield return 0;
		}
	}
	
	private void StartGame()
	{
		_keyPressed = true;
		print("Player has entered a key");
	}
}