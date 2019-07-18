using Godot;
using System;

public class Bullet : Area2D
{
	// private Vector2 thrust = new Vector2(0, 500);
	public Vector2 Velocity;
	private Timer t;
	int Speed = 1000;
	
	public Bullet()
	{
		// Log.p("Bullet - constructor");
		t = new Timer();
		t.Connect("timeout", this, nameof(onTimerTimeout));
		AddChild(t);
		t.Start(1f);
	}
	
	public void Start(float r)
	{
		// Log.p("Bullet - Start method called");
		Velocity = new Vector2(Speed, 0).Rotated(r);
		// Log.p("suck my bullet: " + this.GetPath());
	}
	
private void onArea2DAreEntered(object area)
{
	Log.p("asertei ele hue");
	QueueFree();
}
	
	public void onTimerTimeout()
	{
		this.QueueFree();
	}
	
	public override void _PhysicsProcess(float delta)
	{
		Position += Velocity * delta;
	}
}