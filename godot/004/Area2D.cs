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
	
	private bool playerStarts;
	private bool controlsEnabled;
	private bool moveEnabled;
	private int playerIs;
	private int computerIs;
	private Random rand = new Random();
	
	public Moves pMoves;
    public Moves cMoves;
	public Moves availableMoves;
	public MoveGenerator mg;
 	public GameCollection games;
	
	
	
	public enum Status {
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
	
	Vector2 myPos = new Vector2(190, 250);
	boardInstance.Position = myPos;
	
	AddChild(boardInstance);
	
	controlsEnabled = false;
	
	if (rand.Next(2) == 0) { playerStarts = true; } else { playerStarts = false; }
	
	moveEnabled = false;
	
	setStatus(Status.Title);
}

public void setStatus(Status s)
{

	switch (s)
	{
		case Status.Title:
			setUpComputer();
			GD.Print("starting timer");
			GetNode<Timer>("TitleTimer").Start();
			status = Status.Title;
			GD.Print("status = Status.Title;");
			// playerStarts = !playerStarts;
			showTitle();
			hideMessage();
			hideBoard();
			resetBoard();
			if (playerStarts) { playerIs = 1; computerIs = 2; }
			else { playerIs = 2; computerIs = 1; }
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
			GetNode<Timer>("MoveTimer").Start();
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
			hideTitle();
			hideMessage();
			hideBoard();
			GD.Print("status = Status.End;");
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
	if (@event is InputEventMouseButton eventMouseButton && controlsEnabled)
	{
		if (status == Status.Title)
		{
			if (playerStarts) { setStatus(Status.Player); }
			else { setStatus(Status.Computer); }
			return;
		}
		
		if (moveEnabled && status == Status.Player)
		{
			int square = coordToSquares(GetViewport().GetMousePosition().x, GetViewport().GetMousePosition().y);
			if (addMove(square, playerIs))
			{
				setStatus(Status.Computer);
			}
		}
	}
}

public bool addMove(int square, int player)
{
	if (square == -1) { return false; }
	
	IsAvailable iA = new IsAvailable(availableMoves);
	bool test = iA.check(square);
	if (test == false) { return false; }
	
	if ((playerStarts && player == 1) || (!playerStarts && player == 2)) { pMoves.add(square); availableMoves.remove(square); }
	if ((playerStarts && player == 2) || (!playerStarts && player == 1)) { cMoves.add(square); availableMoves.remove(square); }
	
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
		}
		
		if (eventKey.Pressed && eventKey.Scancode == (int)KeyList.Tab)
		{
		}
		
		if (eventKey.Pressed && eventKey.Scancode == (int)KeyList.Left)
		{
			if (status == Status.Title) { setStatus(Status.End); return; }
			if (status == Status.Player) { setStatus(Status.Title); return; }
			if (status == Status.Computer) { setStatus(Status.Player); return; }
			if (status == Status.End) { setStatus(Status.Computer); return; }
		}
		
		if (eventKey.Pressed && eventKey.Scancode == (int)KeyList.Right)
		{
			if (status == Status.Title) { setStatus(Status.Player); return; }
			if (status == Status.Player) { setStatus(Status.Computer); return; }
			if (status == Status.Computer) { setStatus(Status.End); return; }
			if (status == Status.End) { setStatus(Status.Title); return; }
		}
	}
}
private void onTitleTimerTimeout()
{
	GD.Print("enabling controls");
    controlsEnabled = true;
	moveEnabled = false;
	// setMessage("Click to start");
	// showMessage();
	GetNode<Timer>("TitleTimer").Stop();
}

private void onMoveTimerTimeout()
{
    moveEnabled = true;
	GetNode<Timer>("MoveTimer").Stop();
}

private void onComputerMoveTimerTimeout()
{
    GetNode<Timer>("ComputerMoveTimer").Stop();
	if (addMove(mg.getMove(games, pMoves, cMoves, availableMoves), computerIs))
	{
		setStatus(Status.Player);
	}
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
