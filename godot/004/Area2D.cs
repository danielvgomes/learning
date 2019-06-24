using Godot;
using System;
using System.IO;
using System.Collections.Generic;

public class Area2D : Godot.Area2D
{
	private Random rand = new Random();
	private int counter = 0;
	private int[] game;
	// private int[] prov = new int[32];
	private int[] available;
	private int[] pMap = { 9, 9, 9, 9, 9, 9, 9, 9, 9}; // squares, NOT A MAP
	private int[] cMap = { 9, 9, 9, 9, 9, 9, 9, 9, 9}; // squares, NOT A MAP
	private int[] score = { 3, 1, 3, 1, 3, 1, 3, 1, 3};
	int[,] Games = {{0,1,2}, {3,4,5}, {0,3,6}, {2,4,6}, {1,4,7}, {2,5,8}, {0,4,8}, {6,7,8}};
	// private int[,] wcs;
	// private int[] lmao;
	private int[] playerMoves = new int[9];
	private int movesCounter = 0;
	private int movesCounter2 = 0;
	private RigidBody2D boardInstance;
	private RigidBody2D instance;
	
	private bool playerStarts;
	private int playerIs;
	private int computerIs;
	
	private enum Status {
		Title,
		Player,
		Computer,
		End
		}
		
	Status status;
		
	[Export]
	public PackedScene ink;
	
	List<Texture> boardList = new List<Texture>();
	List<Texture> player2List = new List<Texture>();
	List<Texture> player1List = new List<Texture>();
	List<RigidBody2D> squareList = new List<RigidBody2D>();
	
	Godot.Directory dir = new Godot.Directory();
	
	public override void _Ready()
    {
		var path = "res://art";
		
		dir.Open(path);
		dir.ListDirBegin();
		var fileName = dir.GetNext();

		while (fileName != "")
		{
			if ((fileName.Contains("board")) && (!fileName.Contains(".import")))
			{
				boardList.Add((Texture)ResourceLoader.Load("res://art/" + fileName));
				GD.Print("lol = " + fileName);
			}
			if ((fileName.Contains("player2")) && (!fileName.Contains(".import")))
			{
				player2List.Add((Texture)ResourceLoader.Load("res://art/" + fileName));
				GD.Print("bol = " + fileName);
			}
			if ((fileName.Contains("player1")) && (!fileName.Contains(".import")))
			{
				player1List.Add((Texture)ResourceLoader.Load("res://art/" + fileName));
				GD.Print("gez = " + fileName);
			}
			fileName = dir.GetNext();
		}
		dir.ListDirEnd();
		
		// bora instanciar
		boardInstance = ink.Instance() as RigidBody2D;
		gameInit();
    }
	
public void gameInit()
{
	// bora configurar
	resetBoard();
	
	Vector2 myPos = new Vector2(190, 250);
	boardInstance.Position = myPos;
	
	hideBoard();
	hideMessage();
	AddChild(boardInstance);
	
	status = Status.Title;
	
	// if (rand.Next(2) > 0) { playerStarts = true; } else { playerStarts = false; }
	playerStarts = false;
	// playerMoves = new int[9];
	/*
		wcs = new int[3,8];
		wcs[0,3] = wcs[0,0] = wcs[0,6] = 0;
		wcs[1,3] = wcs[0,1] = 3;
		wcs[2,3] = wcs[0,2] = wcs[2,7] = 6;
		wcs[0,4] = wcs[1,0] = 1;
		wcs[1,4] = wcs[1,1] = wcs[1,6] = wcs[1,7] = 4;
		wcs[2,4] = wcs[1,2] = 7;	
		wcs[0,5] = wcs[2,0] = wcs[0,7] = 2;
		wcs[1,5] = wcs[2,1] = 5;
		wcs[2,5] = wcs[2,2] = wcs[2,6] = 8;
*/
}

public override void _Input(InputEvent @event)
{
	// var scene = ResourceLoader.Load("Circulo.tscn") as PackedScene;
    if (@event is InputEventMouseButton eventMouseButton)
	{
		switch (status)
		{
			case Status.Title:
				playerStarts = !playerStarts;
				// GD.Print("** title");
				// setMessage("Banana!");
				showTitle();
				hideMessage();
				hideBoard();
				resetBoard();
				resetGame();
				game = new int[9]; // the game itself, 9 squares for you to draw on... bitch
				
				if (playerStarts)
				{
					status = Status.Player;
					playerIs = 1;
					computerIs = 2;
				}
				else
				{
					status = Status.Computer;
					playerIs = 2;
					computerIs = 1;
				
				}
				
				break;
			
			case Status.Computer:
				score = scoreReset();
				hideTitle();
				showBoard();
				updateGame();
				
				// ADD RANDOM MOVE TO BOARD, WORKING! DONT TOUCH IT, IDIOT!
				// if (available.Length != 0) { if (addMove(available[0], computerIs)) { status = Status.Player; } }
				
				// atualizar score usando mapa computador
				
				// ou seja, escolher apenas jogos que facam parte do mapa
				
				int[] gameResult = findGames(cMap);
				score = scoreUpdate(score, gameResult);
				gameResult = findGames(pMap);
				score = scoreUpdate(score, gameResult);
				
				
				// first score check for moves
				
				
				
				 // FINALLY CHOOSE A MOVE BITCH
				 
				int test = 0; // minimum score
				int count = 0;
				
				// find maximum value
				for (int i = 0; i < score.Length; i++)
				{
					if (score[i] > test) { test = score[i]; }
				}
				
				// count max values
				for (int i = 0; i < score.Length; i++)
				{
					if (score[i] == test) { count++; }
				}
				
				
				// take all best moves == same values
				
				int[] go = new int[count];
				count = 0;
				
				for (int i = 0; i < score.Length; i++)
				{
					if (score[i] == test) { go[count++] = i; }
				}
				
				GD.Print("########### GAH TESTING THIS SHIT #########");
				
				for (int i = 0; i < go.Length; i++)
				{
					GD.Print("Best moves: " + go[i]);
				}
				
				/*
				 * AND MOVE IT!
				 */
				
				int meMove = rand.Next(go.Length);
				GD.Print("my random sheep: " + meMove);
				
				int daMove = go[rand.Next(go.Length)];
				
				if (addMove(daMove, computerIs))
				{
					status = Status.Player;
					cMap[movesCounter2++] = daMove;
				}
				
				/*
				 * print out new score
				 */
				/*
				GD.Print("############################");
				GD.Print("Updated scores after opp moves list:");
				
				for (int z = 0; z < available.GetLength(0); z++)
				{
					GD.Print("Move: " + available[z,0] + ", Score: " + available[z,1]);
				}
				 ,0,
				*/
				updateGame();
				
				break;
				
			case Status.Player:
				hideTitle();
				showBoard();
				updateGame();
				score = scoreReset();
				// GD.Print("** player move");
				int fok = coordToSquares(GetViewport().GetMousePosition().x, GetViewport().GetMousePosition().y);
				if (addMove(fok, playerIs))
				{
					status = Status.Computer;
					pMap[movesCounter++] = fok;
				}
				
				updateGame();
				
				break;
				
			case Status.End:
				// GD.Print("** game ended");
				break;
		}
		
		GD.Print(boardInstance == null);
	}
}

public void updateGame()
{

	available = initializeArray();
	for (int i = 0; i < game.Length; i++)
	{
		if (game[i] == 0)
		{
			available[i] = i;
		}
	}
available = trim(available);


}

public int[] scoreReset()
{
	score = new int[9];
	return score;
}

public void resetGame()
{
	pMap = new int[] { 9, 9, 9, 9, 9, 9, 9, 9, 9}; // squares, NOT A MAP
	cMap = new int[] { 9, 9, 9, 9, 9, 9, 9, 9, 9}; // squares, NOT A MAP
	score = new int[] { 3, 1, 3, 1, 3, 1, 3, 1, 3};
}

public int[] findGames(int[] x)
{
	int c = 0;
	int[] selection = {9, 9, 9, 9, 9, 9, 9, 9};
	int[] resultMap = new int[9];
	
	for (int i = 0; i < x.Length; i++)
	{
		for (int k = 0; k < Games.GetLength(0); k++)
		{
			for (int l = 0; l < Games.GetLength(1); l++)
			{
				if (x[i] == Games[k,l]) { selection[c++] = k; } // 0, 4, 9, 9, 9, 9...
			}
		}
	}
	// print(selection, "SELECT");
	
	c = 0;
	for (int i = 0; i < selection.Length; i++)
	{
		if (selection[i] != 9) // 0, 4
		{
			for (int j = 0; j < 3; j++)
			{
				int n = selection[i]; // 0, 4
				int m = Games[n, j]; // 0, 1, 2, 1, 4, 7
				resultMap[m] += 1; // 1, 2, 1, 0, 1, 0, 0, 1, 0
				// score map ->       3, 1, 3, 1, 3, 1, 3, 1, 3
			}
		}
	}
	return resultMap;
}

public int[] scoreUpdate(int[] score, int[] map)
{
	for (int i = 0; i < map.Length; i++)
	{
		if (map[i] > 0)
		{
			score[i] += 3;
		}
	}
	return score;
}


public int[] zeroArray()
{
	int[] myArr = {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};
	return myArr;
}

public int[] initializeArray()
{
	int[] myArr = {15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15};
	return myArr;
}

public int[] trim(int[] x)
{
	int c = 0;
	for (int i = 0; i < x.Length; i++) { if (x[i] != 15) { c++; } }
	int[] prov = new int[c];
	c = 0;
	for (int i = 0; i < x.Length; i++) { if (x[i] != 15) { prov[c++] = x[i]; } }
	return prov;
}

public int[] normalizeArray(int[] x)
{
	for (int i = 0; i < x.Length; i++)
	{
		x[i] = (x[i] - 15);
	}
	return x;
}


public void print(int[] x, string s)
{
	if (x == null) { return; }
	GD.Print("** " + s + " printout **");
	for (int i = 0; i < x.Length; i++) { GD.Print("**" + x[i]); }
	GD.Print("** " + s + " printout **");
}

public void size(int[] x, string s)
{
	if (x == null) { return; }
	GD.Print("** " + s + " size **");
	GD.Print("** " + x.Length);
	GD.Print("** " + s + " size **");
}
/*
public int[] convertToGroup(int a)
{
	int[] groups = new int[8];
		
	for(int i=0; i < 8 ; i++)
	{
		for(int j=0; j < 3; j++)
		{
			if (wcs[j,i] == a)
			{
				groups[i]++;
			}
		}
	}
	return groups;
}
*/
/*
public int[] listOpGames(int a)
{
	int cnt = 0;
	int[] groups = new int[32];
		
	for(int i=0; i<8; i++)
	{
		for(int j=0; j < 3; j++)
		{
			if (wcs[j,i] == a)
			{
				for (int h = 0; h < 3; h++)
				{
					if (wcs[h,i] != a) { groups[cnt++] = wcs[h,i]; }
				}
			}
		}
	}

	int[] result = new int[cnt];
	
	// GD.Print("list of moves OP should try to make a game:");
	for (int i = 0; i < cnt; i++)
	{
		result[i] = groups[i];
		// GD.Print(result[i]);
	}
	return result;
}
*/
public bool addMove(int square, int player)
{
	if (square == 9) { return false; }
	if (game[square] != 0) { return false; }
	
	instance = ink.Instance() as RigidBody2D;
	
	if (player == 1)
	{
		instance.GetNode<Sprite>("Cray").Texture = player1List[rand.Next(5)];
		instance.GetNode<Sprite>("Cray").Scale = new Vector2(0.3f, 0.3f);
		instance.GetNode<Sprite>("Cray").Position = squareToCoords(square);
	}
	else
	{
		instance.GetNode<Sprite>("Cray").Texture = player2List[rand.Next(5)];
		instance.GetNode<Sprite>("Cray").Scale = new Vector2(0.25f, 0.25f);
		instance.GetNode<Sprite>("Cray").Position = squareToCoords(square);
	}
	AddChild(instance);
	squareList.Add(instance);
	game[square] = player;
	
	return true;
}

public int coordToSquares(float x, float y)
{
	// GD.Print("x -> " + (int)x + ", y -> " + (int)y);
	if (x >= 78 && x <= 155 && y >= 88 && y <= 200) { return 0; }
	if (x >= 156 && x <= 230 && y >= 88 && y <= 200) { return 1; }
	if (x >= 231 && x <= 310 && y >= 88 && y <= 200) { return 2; }
	if (x >= 78 && x <= 155 && y >= 201 && y <= 280) { return 3; }
	if (x >= 156 && x <= 230 && y >= 201 && y <= 280) { return 4; }
	if (x >= 231 && x <= 310 && y >= 201 && y <= 280) { return 5; }
	if (x >= 78 && x <= 155 && y >= 281 && y <= 370) { return 6; }
	if (x >= 156 && x <= 230 && y >= 281 && y <= 370) { return 7; }
	if (x >= 231 && x <= 310 && y >= 281 && y <= 370) { return 8; }
	return 9;
}

public Vector2 squareToCoords(int s)
{
	int x = 0, y = 0;
	if (s == 0) { x = 113; y = 160; }
	if (s == 1) { x = 190; y = 160; }
	if (s == 2) { x = 275; y = 160; }
	if (s == 3) { x = 113; y = 243; }
	if (s == 4) { x = 193; y = 243; }
	if (s == 5) { x = 275; y = 243; }
	if (s == 6) { x = 113; y = 320; }
	if (s == 7) { x = 195; y = 320; }
	if (s == 8) { x = 275; y = 320; }
	
	return new Vector2(x, y);
}

public void setMessage(string mess)
{
	GetNode<Panel>("Panel").GetNode<Label>("Message").Text = mess;
}

public void showMessage()
{
	GetNode<Panel>("Panel").GetNode<Label>("Message").Visible = true;
}

public void hideMessage()
{
	GetNode<Panel>("Panel").GetNode<Label>("Message").Visible = false;
}

public void showBoard()
{
	boardInstance.GetNode<Sprite>("Cray").Visible = true;
}

public void hideBoard()
{
	boardInstance.GetNode<Sprite>("Cray").Visible = false;
}

public void resetBoard()
{
	boardInstance.GetNode<Sprite>("Cray").Texture = boardList[rand.Next(5)];
	foreach (RigidBody2D x in squareList)
	{
		x.QueueFree();
	}
	squareList = new List<RigidBody2D>();
}

public void hideTitle()
{
	GetNode<Panel>("Panel").GetNode<Label>("Title").Visible = false;
}

public void showTitle()
{
	GetNode<Panel>("Panel").GetNode<Label>("Title").Visible = true;
}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }

public override void _UnhandledInput(InputEvent @event)
{
    if (@event is InputEventKey eventKey)
	{
		if (eventKey.Pressed && eventKey.Scancode == (int)KeyList.Escape)
		{
            // GetTree().Quit();
			print(available, "available");
			size(available, "available");
			
			print(score, "score");
			size(score, "score");
		}
	}
}

}