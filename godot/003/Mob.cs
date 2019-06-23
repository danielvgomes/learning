using Godot;
using System;

public class Mob : RigidBody2D
{
	[Export]
	public int MinSpeed = 150;
	
	[Export]
	public int MaxSpeed = 250;
	
	private String[] _mobTypes = {"walk", "swim", "fly"};
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
	static private Random _random = new Random();
	
    public override void _Ready()
    {
		GetNode<AnimatedSprite>("AnimatedSprite").Animation = _mobTypes[_random.Next(0, _mobTypes.Length)];
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }

	private void _on_Visibility_screen_exited()
	{
    	QueueFree();
	}
}



