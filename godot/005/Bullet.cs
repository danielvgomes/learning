using Godot;
using System;

public class Bullet : RigidBody2D
{
	private Vector2 thrust = new Vector2(0, 500);
    // PackedScene bulletScene;
	
	public Bullet()
	{
		// bulletScene = (PackedScene)ResourceLoader.Load("res://Bullet.tscn");
		// Log.p("meu (bbulked) parets: " + GetParent());
		// Log.p("Bullet created");
	}
	
	public void Move()
	{
		// Log.p("bullet moving");
	}

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Move();
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
	public override void _IntegrateForces(Physics2DDirectBodyState state)
	{
		SetAppliedForce(thrust.Rotated(Rotation));
	}
}
