using Godot;
using System;

public class Dummy : RigidBody2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }
	
	private void onArea2DAreaShapeEntered(int area_id, object area, int area_shape, int self_shape)
	{
		// Log.p("area_id " + area_id);
		// Log.p("area " + area);
		// Log.p("area_shape " + area_shape);
		// Log.p("self_shape " + self_shape);
	}
	
	private void onArea2DAreaEntered(object area)
	{
		Log.p("bala min acerto " + area);
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}