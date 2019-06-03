using Godot;
using System;

public class Label : Godot.Label
{
	
	private float _accum;
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }
	
	public override void _Process(float delta)
	{
		_accum += delta;
		Text = _accum.ToString();
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
