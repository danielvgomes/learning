using Godot;
using System;
using System.IO;
using System.Collections.Generic;

public class Main : Node
{
	[Export]
	public PackedScene scene;
	
	List<Texture> tabuleiro = new List<Texture>();
	List<Texture> circulo = new List<Texture>();
	List<Texture> xis = new List<Texture>();
	
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
				tabuleiro.Add((Texture)ResourceLoader.Load("res://art/" + fileName));
				GD.Print("lol = " + fileName);
			}
			if ((fileName.Contains("circulo")) && (!fileName.Contains(".import")))
			{
				circulo.Add((Texture)ResourceLoader.Load("res://art/" + fileName));
				GD.Print("bol = " + fileName);
			}
			if ((fileName.Contains("xis")) && (!fileName.Contains(".import")))
			{
				xis.Add((Texture)ResourceLoader.Load("res://art/" + fileName));
				GD.Print("gez = " + fileName);
			}
			fileName = dir.GetNext();
		}
		dir.ListDirEnd();
    }

public override void _Input(InputEvent @event)
{
	// var scene = ResourceLoader.Load("Circulo.tscn") as PackedScene;
    if (@event is InputEventMouseButton eventMouseButton)
	{
		GD.Print(GetViewport().GetMousePosition());
		var instance = scene.Instance() as RigidBody2D;
		instance.Position = GetViewport().GetMousePosition();
		// GD.Print(xis.Count);
		// GD.Print(tabuleiro.Count);
		// GD.Print(circulo.Count);
		// instance.GetNode<Sprite>("Cray").Texture = xis[3];
        AddChild(instance);
	}
}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}