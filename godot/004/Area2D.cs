using Godot;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Area2D : Godot.Area2D
{
	private RigidBody2D boardInstance;
	private RigidBody2D instance;
	private RigidBody2D bictoryInstance;
	
	private bool playerStarts;
	private bool controlsEnabled;
	private bool moveEnabled;
	private bool bictoryOrBeia;
	private int playerIs;
	private int computerIs;
	private Random rand = new Random();
	private hasBictory findWinningGames = new hasBictory();
	
	public Moves pMoves;
	public Moves cMoves;
	public Moves availableMoves;
	public MoveGenerator mg;
	public GameCollection games;
	
	public int greatGrandCounter = 0;
	public Vector2 pos;
	public int xPos = 190;
	public int yPos = 250;
	public bool showBictoryBool = true;
	public bool wait = false;
	
	
	
	public enum Status {
		Title,
		Player,
		Computer,
		End,
		OldLady,
		Start
		}
		
	Status status;
		
	[Export]
	public PackedScene ink;
	
	[Export]
	public PackedScene bictory;
	
	List<Texture> boardList = new List<Texture>();
	List<Texture> player2List = new List<Texture>();
	List<Texture> player1List = new List<Texture>();
	List<Texture> bictoryList = new List<Texture>();
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
			}
			if ((fileName.Contains("player2")) && (!fileName.Contains(".import")))
			{
				player2List.Add((Texture)ResourceLoader.Load("res://art/" + fileName));
			}
			if ((fileName.Contains("player1")) && (!fileName.Contains(".import")))
			{
				player1List.Add((Texture)ResourceLoader.Load("res://art/" + fileName));
			}
			if ((fileName.Contains("bictory")) && (!fileName.Contains(".import")))
			{
				bictoryList.Add((Texture)ResourceLoader.Load("res://art/" + fileName));
			}
			fileName = dir.GetNext();
		}
		dir.ListDirEnd();
		
	// bora instanciar
	boardInstance = ink.Instance() as RigidBody2D;
	bictoryInstance = bictory.Instance() as RigidBody2D;
	
	Vector2 myPos = new Vector2(190, 250);
	boardInstance.Position = myPos;
	
	AddChild(boardInstance);
	AddChild(bictoryInstance);
	
	setStatus(Status.Title);
}

public void setStatus(Status s)
{

	switch (s)
	{
		case Status.Title:
			setUpComputer();
			GD.Print("starting timer");
			GetNode<Timer>("WaitTimer").Start();
			wait = true;
			status = Status.Title;
			GD.Print("status = Status.Title;");
			showTitle(); // title of the game
			hideMessage(); // duh
			hideBoard(); // hide the grid
			resetBoard(); // remove pieces
		break;
		case Status.Computer:
			moveEnabled = false;
			setMessage("computer to move");
			showMessage();
			status = Status.Computer;
			GD.Print("status = Status.Computer;");
			hideTitle();
			// hideMessage();
			showBoard();
			GetNode<Timer>("ComputerMoveTimer").SetWaitTime((float)rand.NextDouble());
			GetNode<Timer>("ComputerMoveTimer").Start();
		break;
				
		case Status.Player:
			// GetNode<Timer>("MoveTimer").Start();
			setMessage("player to move");
			showMessage();
			// moveEnabled = true;
			status = Status.Player;
			GD.Print("status = Status.Player;");
			hideTitle();
			// hideMessage();
			showBoard();
		break;
				
		case Status.End:
			status = Status.End;
			// hideTitle();
			int lol = rand.Next(4);
			if (lol == 0) { setMessage("this is a pointless game"); }
			if (lol == 0) { setMessage("thanks - I guess"); }
			if (lol == 0) { setMessage("you can't beat me forever"); }
			if (lol == 0) { setMessage("computers are never really random"); }
			showMessage();
		break;
		
		case Status.OldLady:
			setMessage("Cat's Game!");
			showMessage();
		break;
		
		case Status.Start:
			hideTitle();
			if (rand.Next(2) == 0) { playerStarts = true; } else { playerStarts = false; }
			if (playerStarts)
			{
				playerIs = 1; computerIs = 2;
				setStatus(Status.Player);
			}
			else
			{
				playerIs = 2; computerIs = 1;
				setStatus(Status.Computer);
			}
		break;
	}
}

public void setUpComputer()
{
	pMoves = new Moves();
    cMoves = new Moves();
	availableMoves = new Moves();
	mg = new MoveGenerator();
 
	games = new GameCollection(new Game(0, 1, 2), new Game(3, 4, 5), new Game(0, 3, 6), new Game(2, 4, 6),
												new Game(1, 4, 7), new Game(2, 5, 8), new Game(0, 4, 8), new Game(6, 7, 8));
 
	availableMoves = games.toMoveArray();
}


public override void _Process(float delta)
{
    if (Input.IsActionPressed("ui_right"))
    {
    }
}

public override void _Input(InputEvent @event)
{
	if (@event is InputEventMouseButton eventMouseButton)
	{
		if (status == Status.Title)
		{
			if (!wait) { setStatus(Status.Start); }
		}
		
		if (status == Status.Player)
		{
			int square = coordToSquares(GetViewport().GetMousePosition().x, GetViewport().GetMousePosition().y);
			if (addMove(square, playerIs))
			{
			}
		}
		if (status == Status.OldLady)
		{
			if (!wait) { setStatus(Status.Start); }
		}
	}
}

public bool addMove(int square, int player)
{
	if (square == -1) { return false; }
	
	IsAvailable iA = new IsAvailable(availableMoves);
	bool test = iA.check(square);
	if (test == false) { return false; }
	
	/*
	* ################################################################
	* ################################################################
	*/
	
	if ((playerStarts && player == 1) || (!playerStarts && player == 2))
	{
		pMoves.add(square); availableMoves.remove(square);
		if (!testBictory()) { setStatus(Status.Computer); }
	}
	if ((playerStarts && player == 2) || (!playerStarts && player == 1))
	{
		cMoves.add(square); availableMoves.remove(square);
		if (!testBictory()) { setStatus(Status.Player); }
	}
	
	/*
	* ################################################################
	* ################################################################
	*/
	
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
	
	return true;
}

public int coordToSquares(float x, float y)
{
	if (x >= 78 && x <= 155 && y >= 88 && y <= 200) { return 0; }
	if (x >= 156 && x <= 230 && y >= 88 && y <= 200) { return 1; }
	if (x >= 231 && x <= 310 && y >= 88 && y <= 200) { return 2; }
	if (x >= 78 && x <= 155 && y >= 201 && y <= 280) { return 3; }
	if (x >= 156 && x <= 230 && y >= 201 && y <= 280) { return 4; }
	if (x >= 231 && x <= 310 && y >= 201 && y <= 280) { return 5; }
	if (x >= 78 && x <= 155 && y >= 281 && y <= 370) { return 6; }
	if (x >= 156 && x <= 230 && y >= 281 && y <= 370) { return 7; }
	if (x >= 231 && x <= 310 && y >= 281 && y <= 370) { return 8; }
	return -1;
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

public void showBictory(int r)
{
	bictoryInstance.GetNode<Sprite>("Bictory").SetZIndex(1);
	pos = new Vector2(xPos, yPos);
	
	if (r == 0) { bictoryInstance.GetNode<Sprite>("Bictory").Texture = bictoryList[rand.Next(0, 1)]; }
	if (r == 1) { bictoryInstance.GetNode<Sprite>("Bictory").Texture = bictoryList[rand.Next(2, 3)]; }
	if (r == 2) { bictoryInstance.GetNode<Sprite>("Bictory").Texture = bictoryList[rand.Next(4, 5)]; }
	if (r == 3) { bictoryInstance.GetNode<Sprite>("Bictory").Texture = bictoryList[rand.Next(6, 7)]; }
	if (r == 4) { bictoryInstance.GetNode<Sprite>("Bictory").Texture = bictoryList[rand.Next(0, 1)]; }
	if (r == 5) { bictoryInstance.GetNode<Sprite>("Bictory").Texture = bictoryList[rand.Next(2, 3)]; }
	if (r == 6) { bictoryInstance.GetNode<Sprite>("Bictory").Texture = bictoryList[rand.Next(4, 5)]; }
	if (r == 7) { bictoryInstance.GetNode<Sprite>("Bictory").Texture = bictoryList[rand.Next(6, 7)]; }

	bictoryInstance.GetNode<Sprite>("Bictory").Position = pos;
	bictoryInstance.GetNode<Sprite>("Bictory").Visible = true;
}

public void showBictory()
{
	bictoryInstance.GetNode<Sprite>("Bictory").Visible = true;
}

public void hideBictory()
{
	bictoryInstance.GetNode<Sprite>("Bictory").Visible = false;
}

public bool testBictory()
{
	GD.Print("testBictory()");
	int result = findWinningGames.doesIt(cMoves, pMoves, games);
	GD.Print("BICTORY IS: " + result);
	
	if (result != -1)
	{
		hideMessage();
		GetNode<Timer>("WaitTimer").Start();
		showBictory(result);
		setStatus(Status.End);
		return true;
	}
	
	if ((cMoves.getMoves().Length + pMoves.getMoves().Length) >= 9)
	{
		setStatus(Status.OldLady);
		return true;
	}
	return false;
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
			if (!showBictoryBool) { hideBictory(); } else { showBictory(); }
			showBictoryBool = !showBictoryBool;
		}
		
		if (eventKey.Pressed && eventKey.Scancode == (int)KeyList.Tab)
		{
			showBictory(greatGrandCounter++);
			if (greatGrandCounter >= 9) { greatGrandCounter = 0; }
		}
		
		if (eventKey.Pressed && eventKey.Scancode == (int)KeyList.Left)
		{
			// if (status == Status.Title) { setStatus(Status.End); return; }
			// if (status == Status.Player) { setStatus(Status.Title); return; }
			// if (status == Status.Computer) { setStatus(Status.Player); return; }
			// if (status == Status.End) { setStatus(Status.Computer); return; }
			xPos -= 1;
			GD.Print("xPos: " + xPos);
			pos = new Vector2(xPos, yPos);
			bictoryInstance.GetNode<Sprite>("Bictory").Position = pos;

		}
		
		if (eventKey.Pressed && eventKey.Scancode == (int)KeyList.Right)
		{
			// if (status == Status.Title) { setStatus(Status.Player); return; }
			// if (status == Status.Player) { setStatus(Status.Computer); return; }
			// if (status == Status.Computer) { setStatus(Status.End); return; }
			// if (status == Status.End) { setStatus(Status.Title); return; }
			xPos += 1;
			GD.Print("xPos: " + xPos);
			pos = new Vector2(xPos, yPos);
			bictoryInstance.GetNode<Sprite>("Bictory").Position = pos;
		}
				if (eventKey.Pressed && eventKey.Scancode == (int)KeyList.Down)
		{
			yPos += 1;
			GD.Print("yPos: " + yPos);
			pos = new Vector2(xPos, yPos);
			bictoryInstance.GetNode<Sprite>("Bictory").Position = pos;
		}
		
		if (eventKey.Pressed && eventKey.Scancode == (int)KeyList.Up)
		{
			yPos -= 1;
			GD.Print("yPos: " + yPos);
			pos = new Vector2(xPos, yPos);
			bictoryInstance.GetNode<Sprite>("Bictory").Position = pos;
		}
	}
}

private void onComputerMoveTimerTimeout()
{
    GetNode<Timer>("ComputerMoveTimer").Stop();
	if (addMove(mg.getMove(games, pMoves, cMoves, availableMoves), computerIs))
	{
		// setStatus(Status.Player);
	}
}

private void onWaitTimerTimeout()
{
    wait = false;
}


}






































































































































































































public class IsAvailable
{
	Moves av;
	
	public IsAvailable(Moves available)
	{
		av = available;
	}
					   
	public bool check(int m)
	{
		for (int i = 0; i < av.getMoves().Length; i++)
		{
			if (m == av.getMoves()[i]) { return true; }
		}
		return false;
	}
}


public class MoveGenerator
{
    public int getMove(GameCollection games, Moves pMoves, Moves cMoves, Moves av)
    {
        Random r = new Random();
        Moves finalM = new Moves();
        IsAvailable iA = new IsAvailable(av);
       
        /*
        * ####################################################################
        * ####################################################################
        * ####################################################################
        */
        if (cMoves.getMoves().Length + pMoves.getMoves().Length == 0) // Console.WriteLine("### 000 ###");
        {
            Moves z = games.toMoveMap();
            for (int i = 0; i < 300; i++)
            {
                int mySelection = z.getMoves()[r.Next(z.getMoves().Length)];
                if (iA.check(mySelection))
                {
                    Console.WriteLine("selected move: " + mySelection);
                    return mySelection;
                }
                if (i >= 299) { Console.WriteLine("giving up"); }
            }
        }
		
        /*
        * ####################################################################
        * ####################################################################
        * ####################################################################
        */

        if (cMoves.getMoves().Length == 0 && pMoves.getMoves().Length == 1) // Console.WriteLine("### 001 ###");
        {
            GameCollection mm = new GameCollection();
			
			int theMove;
			theMove = pMoves.getMoves()[0];
			
            Index myIdenx = games.getIndexGameContains(theMove);
			
			Console.WriteLine("index -> " + myIdenx);
			Console.WriteLine("games -> " + games);

			bool skip = false;
			
            for (int i = 0; i < games.getSize(); i++)
			{
				for (int j = 0; j < myIdenx.getSize(); j++) { if (i == myIdenx.getIndexArray()[j]) { skip = true; }	}
				if (skip) { skip = false; continue; }
				mm.addGame(games.getGame(i));
            }
			
			Console.WriteLine("selection -> " + mm);
           
            Moves z = mm.toMoveMap();
            for (int i = 0; i < 300; i++)
            {
                int mySelection = z.getMoves()[r.Next(z.getMoves().Length)];
                if (iA.check(mySelection))
                {
                    Console.WriteLine("selected move: " + mySelection);
                    return mySelection;
                }
                if (i >= 299) { Console.WriteLine("giving up"); }
            }
        }
       
        /*
        * ####################################################################
        * ####################################################################
        * ####################################################################
        */
        
        if (cMoves.getMoves().Length + pMoves.getMoves().Length >= 3) // Console.WriteLine("### 002 ###");
        {
            Gen lol = new Gen();
            
            GameCollection p = new GameCollection();
			GameCollection c = new GameCollection();
			
			GameCollection playerSelection = new GameCollection();
			GameCollection computerSelection = new GameCollection();
			
            lol.combinations(pMoves.getMoves(), p);
			lol.combinations(cMoves.getMoves(), c);
            
			Console.WriteLine("player combination: " + p);
			Console.WriteLine("computer combination: " + c);
           
            // combinacoes prontas, pesquisar jogos
            for (int i = 0; i < p.getSize(); i++)
            {
                int myIdenx = games.getIndexGameContains(p.getGame(i).getGameArray()[0], p.getGame(i).getGameArray()[1]);

                if (myIdenx != -1)
                {
					playerSelection.addGame(games.getGame(myIdenx));
                }
            }
			
			for (int i = 0; i < c.getSize(); i++)
            {
                int myIdenx = games.getIndexGameContains(c.getGame(i).getGameArray()[0], c.getGame(i).getGameArray()[1]);

                if (myIdenx != -1)
                {
					computerSelection.addGame(games.getGame(myIdenx));
                }
            }
 
			Console.WriteLine("playerSelection = " + playerSelection);
			Console.WriteLine("computerSelection = " + computerSelection);
			
			Console.WriteLine("playerSelection.toMoveArray = " + playerSelection.toMoveArray());
			Console.WriteLine("computerSelection.toMoveArray = " + computerSelection.toMoveArray());
			
			if (computerSelection.getSize() > 0)
			{
				for (int i = 0; i < computerSelection.toMoveArray().getMoves().Length; i++)
            	{
                	int mySelection = computerSelection.toMoveArray().getMoves()[i];
					Console.WriteLine("computer iA.check -> " + iA.check(mySelection));
					Console.WriteLine("mySelection -> " + mySelection);
                	if (iA.check(mySelection))
                	{
                    	return mySelection;
                	}
            	}
			}

			if (playerSelection.getSize() > 0)
			{
				for (int i = 0; i < playerSelection.toMoveArray().getMoves().Length; i++)
            	{
                	int mySelection = playerSelection.toMoveArray().getMoves()[i];
					Console.WriteLine("player iA.check -> " + iA.check(mySelection));
					Console.WriteLine("mySelection -> " + mySelection);
                	if (iA.check(mySelection))
                	{
                    	return mySelection;
                	}
            	}
			}
			else
			{
				Console.WriteLine("foideus");
			}

			/*
            for (int i = 0; i < computerSelection.toMoveArray().getMoves().Length; i++)
            {
                int mySelection = computerSelection.toMoveArray().getMoves()[i];
                if (iA.check(mySelection))
                {
                    return mySelection;
                }
            }
			*/
        }
		
		Console.WriteLine("tudo errado.");
		
		/*
        * ####################################################################
        * ####################################################################
        * ####################################################################
        */
		
        if (true) // last resort
        {
            GameCollection mm = new GameCollection();
			
			int theMove;
			theMove = cMoves.getMoves()[0];
			
			Index myIdenx = games.getIndexGameContains(theMove);
			
			Console.WriteLine("index -> " + myIdenx);
			Console.WriteLine("games -> " + games);
			
			for (int i = 0; i < games.getSize(); i++)
			{
				for (int j = 0; j < myIdenx.getSize(); j++)
				{
					if (i == myIdenx.getIndexArray()[j]) { mm.addGame(games.getGame(i)); }
				}
            }
			
			Console.WriteLine("selection -> " + mm);
			
            Moves z = mm.toMoveMap();
            for (int i = 0; i < 300; i++)
            {
                int mySelection = z.getMoves()[r.Next(z.getMoves().Length)];
                if (iA.check(mySelection))
                {
                    Console.WriteLine("selected move: " + mySelection);
                    return mySelection;
                }
                if (i >= 299) { Console.WriteLine("giving up"); }
            }
		}
		// real last resort
		if (true)
		{
			GameCollection mm = new GameCollection();
			Console.WriteLine("Last Resort");
			Console.WriteLine("Available moves -> " + av);
			Console.WriteLine("Games -> " + games);
			Console.WriteLine("cMoves -> " + cMoves);
			Console.WriteLine("pMoves -> " + pMoves);
				
			GameCollection gaminhoLastResourt = new GameCollection();
			GameCollection pGames = new GameCollection();
			
			for (int i = 0; i < cMoves.getMoves().Length; i++)
			{
				Index holyDog = games.getIndexGameContains(cMoves.getMoves()[i]);
				for (int j = 0; j < holyDog.getIndexArray().Length; j++)
				{
					gaminhoLastResourt.addGame(games.getGame(holyDog.getIndexArray()[j]));
				}
			}
			
			Console.WriteLine("gaminho -> " + gaminhoLastResourt.toMoveArray());
			
			if (gaminhoLastResourt.getSize() > 0)
			{
				for (int i = 0; i < gaminhoLastResourt.toMoveArray().getMoves().Length; i++)
            	{
                	int mySelection = gaminhoLastResourt.toMoveArray().getMoves()[i];
						Console.WriteLine("player iA.check -> " + iA.check(mySelection));
						Console.WriteLine("mySelection -> " + mySelection);
                		if (iA.check(mySelection))
                		{
                    		return mySelection;
                		}
            	}
			}
			else
			{
				Console.WriteLine("foideus de novo, #semata");
			}
		}
        return -1;
    }
}

public class Gen
{
	public void combinations(int[] n, GameCollection x)
	{
		int k = 2;		
		foreach (IEnumerable<int> i in Combinations(n, k))
		{
			x.addGame(new Game(i.ToArray()[0], i.ToArray()[1]));
		}
	}
	
	private static bool NextCombination(IList<int> num, int n, int k)
      {
         bool finished;
 
         var changed = finished = false;
 
         if (k <= 0) return false;
 
         for (var i = k - 1; !finished && !changed; i--)
         {
            if (num[i] < n - 1 - (k - 1) + i)
            {
               num[i]++;
 
               if (i < k - 1)
                  for (var j = i + 1; j < k; j++)
                     num[j] = num[j - 1] + 1;
               changed = true;
            }
            finished = i == 0;
         }
 
         return changed;
      }
 
      private static IEnumerable Combinations<T>(IEnumerable<T> elements, int k)
      {
         var elem = elements.ToArray();
         var size = elem.Length;
 
         if (k > size) yield break;
 
         var numbers = new int[k];
 
         for (var i = 0; i < k; i++)
            numbers[i] = i;
 
         do
         {
            yield return numbers.Select(n => elem[n]);
         } while (NextCombination(numbers, size, k));
      }
}

public class Gen3
{
	public void combinations(int[] n, GameCollection x)
	{
		int k = 3;
		foreach (IEnumerable<int> i in Combinations(n, k))
		{
			x.addGame(new Game(i.ToArray()[0], i.ToArray()[1], i.ToArray()[2]));
		}
	}
	
	private static bool NextCombination(IList<int> num, int n, int k)
      {
         bool finished;
 
         var changed = finished = false;
 
         if (k <= 0) return false;
 
         for (var i = k - 1; !finished && !changed; i--)
         {
            if (num[i] < n - 1 - (k - 1) + i)
            {
               num[i]++;
 
               if (i < k - 1)
                  for (var j = i + 1; j < k; j++)
                     num[j] = num[j - 1] + 1;
               changed = true;
            }
            finished = i == 0;
         }
 
         return changed;
      }
 
      private static IEnumerable Combinations<T>(IEnumerable<T> elements, int k)
      {
         var elem = elements.ToArray();
         var size = elem.Length;
 
         if (k > size) yield break;
 
         var numbers = new int[k];
 
         for (var i = 0; i < k; i++)
            numbers[i] = i;
 
         do
         {
            yield return numbers.Select(n => elem[n]);
         } while (NextCombination(numbers, size, k));
      }
}

public class hasBictory
{
	public int doesIt(Moves c, Moves p, GameCollection g)
	{
		GD.Print("hasBictory: " + c);
		GD.Print("hasBictory: " + p);
		
		GD.Print(g);
		
		GD.Print("ver se 147 existe: " + g.getIndexGameContains(1, 4, 7));
		GD.Print("ver se 714 existe: " + g.getIndexGameContains(7, 1, 4));
		GD.Print("ver se 148 existe: " + g.getIndexGameContains(1, 4, 8));
		GD.Print("ver se 258 existe: " + g.getIndexGameContains(2, 5, 8));
		GD.Print("ver se 582 existe: " + g.getIndexGameContains(5, 8, 2));
		GD.Print("ver se 048 existe: " + g.getIndexGameContains(0, 4, 8));
		
		GD.Print("cMoves: " + c);
		
		// public class Gen
		// public void combinations(int[] n, GameCollection x)
		GameCollection myG = new GameCollection();
		
		Gen3 lol = new Gen3();
		lol.combinations(c.getMoves(), myG);
		
		GD.Print("n = 3, sera: " + myG);
		
		GD.Print("myG Length: " + myG.getArray().Length);
		
		for (int i = 0; i < myG.getArray().Length; i++)
		{
			
			// GD.Print("vendo finalmente se esse jogo é bictory:");
			int big = g.getIndexGameContains(myG.getArray()[i].getInt(0), myG.getArray()[i].getInt(1), myG.getArray()[i].getInt(2));
			GD.Print("OMFG OQ Q E ISO: " + myG.getArray()[i].getInt(0), myG.getArray()[i].getInt(1), myG.getArray()[i].getInt(2));
			if (big != -1) { return big; };
				// callBictory(big);
				// GD.Print("BICTORY!");

		}
		return -1;
	}
}

public class Moves
{
    int c = 0;
    int[] mv = new int[0];
    int[] prov;
   
    public int[] getMoves()
    {
        return mv;
    }
   
    public void add(int i)
    {
        c = mv.Length;
        for (int t = 0; t < mv.Length; t++) { if (mv[t] == i) { return; } }
        c++;
        prov = mv; mv = new int[c];
        for (int j = 0; j < prov.Length; j++) { mv[j] = prov[j]; }
        mv[c-1] = i;
    }
	
	    public void addKeepDupes(int i)
    {
        c = mv.Length;
        c++;
        prov = mv; mv = new int[c];
        for (int j = 0; j < prov.Length; j++) { mv[j] = prov[j]; }
        mv[c-1] = i;
    }
   
    public void remove(int i)
    {
        bool found = false;
        for (int j = 0; j < mv.Length; j++) { if (mv[j] == i) found = true; }
        if (!found) return;
       
        c = mv.Length;
        c--;
        int counter = 0;
        prov = mv; mv = new int[c];
        for (int k = 0; k < prov.Length; k++)
        {
            if (prov[k] == i)
            {
                continue;
            }
           
            mv[counter] = prov[k];
            counter++;
        }
    }
   
    public override string ToString()
    {
        string myReturn = "";
        myReturn += "[";
        for (int i = 0; i < mv.Length; i++)
        {
            myReturn += mv[i];
            if (i == mv.Length-1) { break; }
            myReturn += ", ";
        }
        myReturn += "]";
        return myReturn;
    }
}
 
public class GameCollection
{
	int size = 0;
    Game[] gameArray = {};
    Game[] prov;
   
    public GameCollection(params Game[] g)
    {
        gameArray = g;
    }
   
    public Game[] getArray()
    {
        return gameArray;
    }
   
    public Moves toMoveArray()
    {
        Moves prov = new Moves();
       
        for (int i = 0; i < gameArray.Length; i++)
        {
            for (int j = 0; j < gameArray[i].getGameArray().Length; j++)
            {
                prov.add(gameArray[i].getGameArray()[j]);
            }
        }
       
        return prov;
    }
	
	    public Moves toMoveMap()
    {
        Moves prov = new Moves();
       
        for (int i = 0; i < gameArray.Length; i++)
        {
            for (int j = 0; j < gameArray[i].getGameArray().Length; j++)
            {
                prov.addKeepDupes(gameArray[i].getGameArray()[j]);
            }
        }
       
        return prov;
    }
   
	public void addGame(Game g)
    {
        size = gameArray.Length;
        size++;
        prov = gameArray;
        gameArray = new Game[size];
       
        for (int i = 0; i < prov.Length; i++)
        {
            gameArray[i] = prov[i];
        }
        gameArray[prov.Length] = g;
    }
 
    public Game removeGame(int x)
    {
        Game prov = null;
        if (x < 0) { return prov; }
        Game[] newArray = new Game[(gameArray.Length-1)];
        int c = 0;
        for (int i = 0; i < gameArray.Length; i++)
        {
            if (i == x) { prov = gameArray[i]; continue; }
            newArray[c] = gameArray[i];
            c++;
        }
        gameArray = newArray;
        return prov;
    }
 
    public Index getIndexGameContains(int m)
    {
        Index indexReturn = new Index();
        for (int i = 0; i < gameArray.Length; i++)
        {
            if (gameArray[i].contains(m))
            {
                indexReturn.add(i);
            }
        }
        return indexReturn;
    }
 
    public int getIndexGameContains(int m, int n)
    {
        for (int i = 0; i < gameArray.Length; i++)
        {
            if (gameArray[i].contains(m, n))
            {
                return i;
            }
        }
        return -1;
    }

    public int getIndexGameContains(int m, int n, int o)
    {
        for (int i = 0; i < gameArray.Length; i++)
        {
            if (gameArray[i].contains(m, n, o))
            {
                return i;
            }
        }
        return -1;
    }
 
    public Game getGame(int n)
    {
        return gameArray[n];
    }
 
    public int getSize()
    {
        return gameArray.Length;
    }
	
	public override string ToString()
    {
        string myReturn = "";
        for (int i = 0; i < gameArray.Length; i++)
        {
            myReturn += gameArray[i].ToString();
            if (i == gameArray.Length - 1) { break; }
            myReturn += ", ";
        }
        return myReturn;
    }
}
 
public class Index
{
    int i = 0;
    int[] id = new int[0];
    int[] prov;
	
    public void add(int index)
    {
        i++;
        prov = id;
        id = new int[i];
        for (int k = 0; k < prov.Length; k++) { id[k] = prov[k]; }
        id[i-1] = index;
    }
   
    public int[] getIndexArray()
    {
        return id;
    }
   
    public int getSize()
    {
        return id.Length;
    }
   
        public override string ToString()
    {
        string myReturn = "";
        myReturn += "[";
        for (int i = 0; i < id.Length; i++)
        {
            myReturn += id[i];
            if (i == id.Length-1) { break; }
            myReturn += ", ";
        }
        myReturn += "]";
        return myReturn;
    }
   
}

public class Game
{
	int[] game;
	
	public int getInt(int k)
	{
		return game[k];
	}
	
    public Game(int x, int y, int z)
    {
		game = new int[3];
		game[0] = x;
		game[1] = y;
		game[2] = z;
    }
	
	public Game(int x, int y)
    {
		game = new int[2];
		game[0] = x;
		game[1] = y;
    }
	
    public bool contains(int n)
    {
        for (int i = 0; i < game.Length; i++)
        {
            if (game[i] == n) { return true; }
        }
        return false;
    }
 
        public bool contains(int m, int n)
        {
        int myReturn = 0;
        for (int i = 0; i < game.Length; i++)
            {
                if (game[i] == n || game[i] == m) { myReturn++; }
            }
        if (myReturn == 2)
            {
                return true;
            }
                else
            {
                return false;
            }
        }
   
            public bool contains(int m, int n, int o)
        {
        int myReturn = 0;
        for (int i = 0; i < game.Length; i++)
            {
                if (game[i] == n || game[i] == m || game[i] == o) { myReturn++; }
            }
        if (myReturn == 3)
            {
                return true;
            }
                else
            {
                return false;
            }
        }
 
    public int[] getGameArray()
    {
        return game;
    }
 
    public bool find(int x)
    {
        for (int i = 0; i < game.Length; i++)
        {
            if (game[i] == x) { return true; }
        }
        return false;
    }
 
    public override string ToString()
    {
        string myReturn = "";
        myReturn += "[";
        for (int i = 0; i < game.Length; i++)
        {
            myReturn += game[i];
            if (i == game.Length-1) { break; }
            myReturn += ", ";
        }
        myReturn += "]";
        return myReturn;
    }
}