using Godot;
using System;

public class Bullet2 : KinematicBody2D
{
	private Vector2 velocity = new Vector2(250, 250);
	private Timer t2;
	
	public Bullet2()
	{
		// t2 = new Timer();
		// t2.Connect("timeout", this, nameof(onTimerTimeout2));
		// AddChild(t2);
		// t2.Start(1f);
	}
	
	public void onTimerTimeout2()
	{
		this.QueueFree();
	}
	
	public override void _PhysicsProcess(float delta)
	{
		var collisionInfo = MoveAndCollide(velocity * delta);
		if (collisionInfo != null)
		// velocity = velocity.Bounce(collisionInfo.Normal);
		this.QueueFree();
	}
}
