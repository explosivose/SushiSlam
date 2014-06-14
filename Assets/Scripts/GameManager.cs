using UnityEngine;
using System.Collections;

public class GameManager : Singleton<GameManager> 
{
	public bool GameIsPaused
	{
		get 
		{
			return (state != State.Playing);
		}
	}
	
	private Rect windowSize = new Rect();
	private GUISkin menuSkin;
	
	private int p1select = 0;
	private int p2select = 1;
	
	
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
		StartScreen,
		PauseMenu,
		DeathScreen,
		Options,
		HowToPlay,
		Credits
	}
	
	private GUIState gui = GUIState.StartScreen;

	public void Initialise()
	{
		Debug.Log ("init");
	}


	void StartGame()
	{
		// spawn two players here!
		UnPause();

	}
	
	void Restart()
	{
		Application.LoadLevel(Application.loadedLevel);
		AudioListener.volume = 1f;
	}
	
	
	public void Pause()
	{
		Screen.lockCursor = false;
		state = State.Paused;
		gui = GUIState.PauseMenu;
		Time.timeScale = 0f;
	}
	
	public void UnPause()
	{
		Screen.lockCursor = true;
		state = State.Playing;
		gui = GUIState.NoWindows;
		Time.timeScale = 1f;
	}
	
	public void GameOver(int score)
	{
		Screen.lockCursor = false;
		state = State.GameOver;
		gui = GUIState.DeathScreen;
	}
	
	void Awake()
	{
		menuSkin = (GUISkin)Resources.Load("Menus", typeof(GUISkin));
	}
	
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape) && state == State.Playing ) Pause();
		else if (Input.GetKeyDown(KeyCode.Escape) && state == State.Paused) UnPause();
	}

	public GUIWindow startScreen = new GUIWindow();
	void wStartScreen(int windowID)
	{
		GUILayout.Space (100);
	}

	public GUIWindow deathScreen = new GUIWindow();
	void wDeathMenu(int windowID)
	{
		GUILayout.Space (100);
		
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Restart", menuSkin.button))
			Restart();
		
		if (GUILayout.Button ("Credits", menuSkin.button))
			gui = GUIState.Credits;
		
		if (GUILayout.Button ("Exit", menuSkin.button))
			Application.Quit();
		GUILayout.EndHorizontal();
	}
	
	public GUIWindow pauseMenu = new GUIWindow();
	void wPauseMenu(int windowID)
	{
		GUILayout.Space (100);
		
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Resume", menuSkin.button))
			UnPause();
		
		if (GUILayout.Button ("Restart", menuSkin.button))
			Restart();
		
		if (GUILayout.Button ("Exit", menuSkin.button))
			Application.Quit();
		GUILayout.EndHorizontal();
	}
	
	public GUIWindow options = new GUIWindow();
	void wOptions(int windowID)
	{
		GUILayout.Space (100);
		
		if (GUILayout.Button("Main Menu",menuSkin.button))
			gui = GUIState.PauseMenu;
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
			gui = GUIState.DeathScreen;
	}
	
	public GUIWindow howToPlay = new GUIWindow();
	void wHowToPlay(int windowID)
	{
		GUILayout.Space (100);
		
		GUILayout.Label ("FUTILE CODA", menuSkin.label);
		GUILayout.Label("MOVE: WASD", menuSkin.label);
		GUILayout.Label ("CHANGE WEAPON: Q (q)", menuSkin.label);
		GUILayout.Label ("LEFT CLICK: FIRE", menuSkin.label);
		
		GUILayout.Space (15);
		
		if (GUILayout.Button("Weapon Select",menuSkin.button))
			gui = GUIState.StartScreen;
	}
	
	void OnGUI()
	{
		GUIWindow currentWindow = new GUIWindow();
		
		// Copy GUIWindow settings to thisWindow
		switch ( gui )
		{
		case GUIState.StartScreen:
			currentWindow = startScreen;
			break;
		case GUIState.DeathScreen:
			currentWindow = deathScreen;
			break;
		case GUIState.PauseMenu:
			currentWindow = pauseMenu;
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
		case GUIState.StartScreen:
			GUILayout.Window (1, windowSize, wStartScreen, "SUSHI SLAM", menuSkin.window);
			break;
		case GUIState.DeathScreen:
			GUILayout.Window (1, windowSize, wDeathMenu, "HONOR", menuSkin.window);
			break;
		case GUIState.PauseMenu:
			GUILayout.Window (1, windowSize, wPauseMenu, "WAIT", menuSkin.window);
			break;
		case GUIState.Credits:
			GUILayout.Window (1, windowSize, wCredits, "CREDITS", menuSkin.window);
			break;
		case GUIState.Options:
			GUILayout.Window (1, windowSize, wCredits, "OPTIONS", menuSkin.window);
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