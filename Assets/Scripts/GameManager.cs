﻿using UnityEngine;
using System.Collections;

public class GameManager : Singleton<GameManager> 
{
	public Transform[] playerPrefabs;
	public Transform[] spawnpoints;
	public AudioClip eatSushi;
	public bool GameIsPaused
	{
		get 
		{
			return (state != State.Playing);
		}
	}
	
	private Rect windowSize = new Rect();
	private GUISkin menuSkin;
	
	
	private enum State
	{
		PreGame,
		Paused,
		Playing,
		GameOver
	}
	
	private State state = State.PreGame;
	
	private enum GUIState
	{
		NoWindows,
		MainMenu,
		GameOver,
		HowToPlay,
		Options,
		Credits
	}
	
	private GUIState gui = GUIState.MainMenu;

	void StartGame()
	{
		/*
		Transform player1 = playerPrefabs[CharacterSelection.Instance.p1Choice];
		Transform player2 = playerPrefabs[CharacterSelection.Instance.p2Choice];
		int p1spawn = Random.Range(0, spawnpoints.Length-1);
		int p2spawn = Random.Range(0, spawnpoints.Length-1);
		if (p1spawn == p2spawn) p1spawn++;
		if (p1spawn > spawnpoints.Length) p1spawn = 0;
		player1 = Instantiate(player1, spawnpoints[p1spawn].position, Quaternion.identity) as Transform;
		player1.GetComponent<PlatformerCharacter2D>().p = 1;
		player2 = Instantiate(player2, spawnpoints[p1spawn].position, Quaternion.identity) as Transform;
		player2.GetComponent<PlatformerCharacter2D>().p = 2;
		SushiCamera.Instance.Initialise(player1, player2);
		*/
		Application.LoadLevel("newscene");
		MusicManager.Instance.SendMessage("FightStart");
		UnPause();
	}
	
	
	public void Pause()
	{
		Screen.lockCursor = false;
		state = State.Paused;
		gui = GUIState.MainMenu;
		Time.timeScale = 0f;
	}
	
	public void UnPause()
	{
		Screen.lockCursor = true;
		state = State.Playing;
		gui = GUIState.NoWindows;
		Time.timeScale = 1f;
	}
	
	public void GameOver()
	{
		Screen.lockCursor = false;
		state = State.GameOver;
		gui = GUIState.GameOver;
	}
	
	void Awake()
	{
		menuSkin = (GUISkin)Resources.Load("Menus", typeof(GUISkin));
		DontDestroyOnLoad(this.gameObject);
		MusicManager.Instance.SendMessage("MainMenu");
	}
	
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape) && state == State.Playing ) Pause();
		else if (Input.GetKeyDown(KeyCode.Escape) && state == State.Paused) UnPause();
	}

	public GUIWindow mainMenu = new GUIWindow();
	void wMainMenu(int windowID)
	{
		GUILayout.Space (100);
		
		GUILayout.BeginHorizontal();
		

		
		if (state == State.Paused) {
			if (GUILayout.Button("Resume", menuSkin.button))
				UnPause();
		}
		else {
			if (GUILayout.Button("Start", menuSkin.button))
				StartGame();
		}
		
		if (GUILayout.Button("How to play", menuSkin.button))
			gui = GUIState.HowToPlay;
		
		if (GUILayout.Button("Options", menuSkin.button))
			gui = GUIState.Options;
		
		if (GUILayout.Button ("Credits", menuSkin.button)) {
			AudioSource.PlayClipAtPoint(eatSushi, Vector3.zero);
			gui = GUIState.Credits;
		}
			
		
		if (GUILayout.Button ("Exit", menuSkin.button))
			Application.Quit();
			
		GUILayout.EndHorizontal();
	}

	public GUIWindow deathScreen = new GUIWindow();
	void wDeathMenu(int windowID)
	{
		GUILayout.Space (100);
		
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Rematch!", menuSkin.button))
			StartGame();
		
		if (GUILayout.Button ("Main menu", menuSkin.button))
			gui = GUIState.MainMenu;
			
		GUILayout.EndHorizontal();
	}
	
	public GUIWindow options = new GUIWindow();
	void wOptions(int windowID)
	{
		GUILayout.Space (100);
		GUILayout.Label ("No options lol", menuSkin.label);
		if (GUILayout.Button("Main Menu",menuSkin.button))
			gui = GUIState.MainMenu;
	}
	
	public GUIWindow credits = new GUIWindow();
	void wCredits(int windowID)
	{
		GUILayout.Space (100);
		
		GUILayout.Label ("SushiSlam", menuSkin.label);
		GUILayout.Label("This game was made by team SUPERCORE", menuSkin.label);
		GUILayout.Label ("at the weekend long SuperCore Game Jam #3", menuSkin.label);
		GUILayout.Label ("June 14th/15th 2014", menuSkin.label);
		
		GUILayout.Space (15);
		
		if (GUILayout.Button("Back",menuSkin.button))
			gui = GUIState.MainMenu;
	}
	
	public GUIWindow howToPlay = new GUIWindow();
	void wHowToPlay(int windowID)
	{
		GUILayout.Space (100);
		GUILayout.Label ("Time your attacks for a combo!\n" +
						"Grab the sushi to deal a fatal SUSHI STRIKE to your opponent!"
						, menuSkin.label);
		GUILayout.Space (10);
		GUILayout.Label ("SushiSlam Default Controls", menuSkin.label);
		GUILayout.Space (10);
		GUILayout.BeginHorizontal();
		GUILayout.Label("Player 1", menuSkin.label);
		GUILayout.Label("Player 2", menuSkin.label);
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		GUILayout.Label ("LEFT:A, D:RIGHT", menuSkin.label);
		GUILayout.Label ("LEFT:LARROW, RIGHT:RARROW", menuSkin.label);
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		GUILayout.Label ("JUMP:W", menuSkin.label);
		GUILayout.Label ("JUMP:UPARROW", menuSkin.label);
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		GUILayout.Label ("ATTACK:SPACE", menuSkin.label);
		GUILayout.Label ("ATTACK:RCTRL", menuSkin.label);
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		GUILayout.Label ("BLOCK:S", menuSkin.label);
		GUILayout.Label ("BLOCK:DARROW", menuSkin.label);
		GUILayout.EndHorizontal();
		GUILayout.Space (15);
		
		if (GUILayout.Button("Main Menu",menuSkin.button))
			gui = GUIState.MainMenu;
	}
	
	void OnGUI()
	{
		GUIWindow currentWindow = new GUIWindow();
		
		// Copy GUIWindow settings to thisWindow
		switch ( gui )
		{
		case GUIState.MainMenu:
			currentWindow = mainMenu;
			break;
		case GUIState.GameOver:
			currentWindow = deathScreen;
			break;
		case GUIState.Options:
			currentWindow = options;
			break;
		case GUIState.Credits:
			currentWindow = credits;
			break;
		case GUIState.HowToPlay:
			currentWindow = howToPlay;
			break;
		default:
			break;
		}
		
		windowSize = new Rect(currentWindow.Left, currentWindow.Top, currentWindow.Width, currentWindow.Height);
		
		// Draw thisWindow (GUILayout.Window)
		switch ( gui )
		{
		case GUIState.MainMenu:
			GUILayout.Window (1, windowSize, wMainMenu, "SUSHI SLAM", menuSkin.window);
			break;
		case GUIState.GameOver:
			GUILayout.Window (1, windowSize, wDeathMenu, "HONOR", menuSkin.window);
			break;
		case GUIState.Credits:
			GUILayout.Window (1, windowSize, wCredits, "CREDITS", menuSkin.window);
			break;
		case GUIState.Options:
			GUILayout.Window (1, windowSize, wOptions, "OPTIONS", menuSkin.window);
			break;
		case GUIState.HowToPlay:
			GUILayout.Window (1, windowSize, wHowToPlay, "CONTROLS", menuSkin.window);
			break;
		case GUIState.NoWindows:
			break;
		default:
			break;
		}
		
	}
}






[System.Serializable]
public class GUIWindow
{
	/* GUIWindow info
	* This class is serializable for the designer to choose window settings
	* from within the unity inspector. Settings include position and size.
	*/
	public enum DimensionMode
	{
		PercentageOfScreen,
		Absolute
	}
	
	public enum Alignment
	{
		UpperLeft,
		UpperCenter,
		UpperRight,
		MiddleLeft,
		MiddleCenter,
		MiddleRight,
		LowerLeft,
		LowerCenter,
		LowerRight
	}
	
	public int DesignHeight;
	public DimensionMode HeightIs = DimensionMode.Absolute;
	
	public int Height 
	{
		get
		{
			if (HeightIs == DimensionMode.Absolute)
				return DesignHeight;
			else
				return Mathf.RoundToInt(Screen.height * DesignHeight / 100);
		}
		set
		{
			DesignHeight = value;
		}
	}
	
	public int DesignWidth;
	public DimensionMode WidthIs = DimensionMode.Absolute;
	
	public int Width
	{
		get
		{
			if (WidthIs == DimensionMode.Absolute)
				return DesignWidth;
			else
				return Mathf.RoundToInt(Screen.width * DesignWidth / 100);
		}
		set
		{
			DesignWidth = value;
		}
	}
	
	
	public Alignment Align = Alignment.UpperLeft;
	
	// Top side of the window in screen pixels
	public int verticalOffset;
	public int Top
	{
		get
		{
			// depends on the alignment mode 
			switch (Align)
			{
			case Alignment.UpperCenter: 
			case Alignment.UpperLeft: 
			case Alignment.UpperRight:
			default:
				return verticalOffset;
				
			case Alignment.MiddleCenter: 
			case Alignment.MiddleLeft: 
			case Alignment.MiddleRight:
				return Mathf.RoundToInt(Screen.height/2 - Height/2) + verticalOffset;
				
			case Alignment.LowerCenter: 
			case Alignment.LowerLeft: 
			case Alignment.LowerRight:
				return Screen.height - Height - verticalOffset;
				
			}
		}
		set
		{
			verticalOffset = value;
		}
	}
	
	// Left side of the window in screen pixels
	public int horizontalOffset;
	public int Left
	{
		get
		{
			// depends on the alignment mode
			switch (Align)
			{
			case Alignment.LowerLeft: 
			case Alignment.MiddleLeft: 
			case Alignment.UpperLeft:
			default:
				return horizontalOffset;
				
			case Alignment.LowerCenter: 
			case Alignment.MiddleCenter: 
			case Alignment.UpperCenter:
				return Mathf.RoundToInt(Screen.width/2 - Width/2) + horizontalOffset;
				
			case Alignment.LowerRight: 
			case Alignment.MiddleRight: 
			case Alignment.UpperRight:
				return Screen.width - Width - horizontalOffset;
				
			}
		}
		set
		{
			horizontalOffset = value;
		}
	}
	
	
}