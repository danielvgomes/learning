using Godot;
using System;
using System.IO;
using System.Collections.Generic;

public class Area2D : Godot.Area2D
{
	private Random rand = new Random();
	private int counter = 0;
	private int[] game;
	private int[,] available;
	private int[,] wcs;
	private int[] lmao;
	private int[] playerMoves = {15, 15, 15, 15, 15, 15};
	private int cnter = 0;
	
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
				GD.Print("Title!");
				// setMessage("Banana!");
				showTitle();
				hideMessage();
				hideBoard();
				resetBoard();
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
				hideTitle();
				showBoard();
				GD.Print("Computer move!");
				
				// listar jogadas posssivel
				int arrayDim = 0;
				int cnt = 0;
				
				for (int i = 0; i < game.Length; i++)
				{
					if (game[i] == 0) { arrayDim++; }
					// GD.Print("game[" + i + "] == 0: ");
					// GD.Print(game[i] == 0);
				}
				GD.Print("arrayDim = " + arrayDim);
				available = new int[arrayDim,2];
				for (int i = 0; i < 9; i++) { if (game[i] == 0) { available[cnt++,0] = i; } }
				
				/* 
				if (available.Length != 0)
				{
					for (int i = 0; i < available.Length; i++) { GD.Print(available[i,0]); }
					if (addMove(available[0,0], computerIs))
					{
						status = Status.Player;
					}
				}
				*/
				
				// first score check for moves
				int[] blerg = new int[available.GetLength(0)];
				for (int i = 0; i < available.GetLength(0); i++)
				{
					blerg = convertToGroup(available[i,0]);
					for (int f = 0; f < blerg.GetLength(0); f++)
					{
						if (blerg[f] == 1) { available[i,1]++; }
					}
				}
				
				GD.Print("### Bora printar FAMWE");
				
				for (int i = 0; i < game.Length; i++)
				{
					GD.Print(game[i]);
				}
				
				GD.Print("############################");
				GD.Print("Score for // normalized // moves:");
				
				for (int z = 0; z < available.GetLength(0); z++)
				{
					available[z,1] /= 3; // "normalize" scores
					GD.Print("Move: " + available[z,0] + ", Score: " + available[z,1]);
				}
				
				/*
				 * list opponents moves
				 */
				
				GD.Print("############################");
				GD.Print("List of Opponent moves:");
				
				// O CODIGO ABAIXO E FRACASSATION
				/*
				arrayDim = 0;
				cnt = 0;
				
				for (int i = 0; i < 9; i++) { if (game[i] != 0) { arrayDim++; } }
				int[,] opMoves = new int[arrayDim,2];
				for (int i = 0; i < 9; i++) { if (game[i] != 0) { opMoves[cnt++,0] = i; } }
				
				for (int i = 0; i < opMoves.GetLength(0); i++)
				{
					GD.Print("Moves -> " + opMoves[i,0]);
				}
				*/
				int [,] opMoves = new int[playerMoves.Length,2];
				
				for (int i = 0; i < playerMoves.Length; i++)
				{
					if (playerMoves[i] != 15)
					{
						opMoves[i,0] = playerMoves[i];
					}
				}
				
				/*
				 * list opGames, based on opMoves
				 */
				
				/*
				GD.Print("############################");
				GD.Print("Listing opponent available game moves:");
				
				int[] opGamesHelo = new int[1];
				
				for (int r = 0; r < opMoves.GetLength(0); r++)
				{
					opGamesHelo = listOpGames(opMoves[r,0]);
				}
				
				for (int i = 0; i < opGamesHelo.Length; i++) { GD.Print("-> " + opGamesHelo[i]); }
				*/
				/*
 				 * change scores
				 */
				GD.Print("############################");
				GD.Print("Listing avaialbledj moves again:");
				for (int z = 0; z < available.GetLength(0); z++)
				{
					GD.Print("Move: " + available[z,0] + ", Score: " + available[z,1]);
				}
				
				/*
				for (int i = 0; i < opGamesHelo.Length; i++)
				{
					for (int j = 0; j < available.GetLength(0); j++)
					{
						if (opGamesHelo[i] == available[j,0]) { available[j,1] += 2; }
					}
				}
				*/
				/*
				 * LAST FRONTIER
				 */
				
				
				// LIST PLAYER MOVES
				GD.Print("LIST PLAYER MOVES");
				for (int i = 0; i < playerMoves.Length; i++)
				{
					if (playerMoves[i] != 15) { GD.Print(playerMoves[i]); }
				}
				
				
				// FIND GAMES WITH TWO MOVES
				
				
				
				// ADD VERY HIGH SCORE TO THAT GAME!!! FOK
				
				
				
				
				
				
				
				
				
				
				/*
				 * FINALLY CHOOSE A MOVE BITCH
				 */
				int test = 0; // minimum score
				int count = 0;
				
				// find maximum value
				for (int i = 0; i < available.GetLength(0); i++)
				{
					if (available[i,1] > test) { test = available[i,1]; }
				}
				
				// count max values
				for (int i = 0; i < available.GetLength(0); i++)
				{
					if (available[i,1] == test) { count++; }
				}
				
				// take all best moves == same values
				
				int[] go = new int[count];
				count = 0;
				
				for (int i = 0; i < available.GetLength(0); i++)
				{
					if (available[i,1] == test) { go[count++] = available[i,0]; }
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
				
				if (addMove(go[rand.Next(go.Length)], computerIs))
				{
					status = Status.Player;
				}
				
				
				/*
				 * print out new score
				 */
				
				GD.Print("############################");
				GD.Print("Updated scores after opp moves list:");
				
				for (int z = 0; z < available.GetLength(0); z++)
				{
					GD.Print("Move: " + available[z,0] + ", Score: " + available[z,1]);
				}

				break;
				
			case Status.Player:
				hideTitle();
				showBoard();
				GD.Print("Player move!");
				int fok = coordToSquares(GetViewport().GetMousePosition().x, GetViewport().GetMousePosition().y);
				
				if (addMove(fok, playerIs))
				{
					status = Status.Computer;
					playerMoves[cnter++] = fok;
				}
				
				for (int i = 0; i < playerMoves.Length; i++)
				{
					GD.Print("MOOOOOOVES: " + playerMoves[i]);
				}
				
				break;
				
			case Status.End:
				GD.Print("Game ended!");
				break;
		}
		
		GD.Print(boardInstance == null);
	}
}

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

}