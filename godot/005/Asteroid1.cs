using Godot;
using System;

public class Asteroid1 : RigidBody2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GetNode<Sprite>("Sprite").GetNode<AnimationPlayer>("AnimationPlayer").Play("moving");
	}
	
	private void onArea2DAreaEntered(object area)
	{
		Log.p("min acetou no meio do ufo1");
	}
}