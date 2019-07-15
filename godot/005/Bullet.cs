using Godot;
using System;

public class Bullet : RigidBody2D
{
	private Vector2 thrust = new Vector2(0, 500);
	private Timer t;
    // PackedScene bulletScene;
	
	public Bullet()
	{
		t = new Timer();
		t.Connect("timeout", this, nameof(onTimerTimeout));
		AddChild(t);
		t.Start(1f);
	}
	
	public void onTimerTimeout()
	{
		this.QueueFree();
	}

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	// public override void _Process(float delta)
	// {
	// }

	public override void _IntegrateForces(Physics2DDirectBodyState state)
	{
		SetAppliedForce(thrust.Rotated(Rotation));
	}
}
