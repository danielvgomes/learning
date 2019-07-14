using Godot;
using System;

public class Ship : RigidBody2D
{
	private Vector2 thrust = new Vector2(0, 250);
	private float torque = 10000;
	private float rotationDir;
	private Vector2 shootingPosition;
	
	[Export]
	PackedScene bullet;
	
	public Ship()
	{
		// bulletScene = (PackedScene)ResourceLoader.Load("res://Bullet.tscn");
	}
	
	public void Shoot()
	{
		Log.p("Shoot()");
		Log.p(GetNode<Position2D>("Position2D").GlobalPosition);
		shootingPosition = GetNode<Position2D>("Position2D").GlobalPosition;
	}
	
	
	public override void _IntegrateForces(Physics2DDirectBodyState state)
	{
		if (Input.IsActionPressed("ui_up"))
		{
			SetAppliedForce(thrust.Rotated(Rotation));
		}
		else
		if (Input.IsActionPressed("ui_down"))
		{
			SetAppliedForce(-thrust.Rotated(Rotation));
		}
		else
		{
			SetAppliedForce(new Vector2());
		}
		
		if (Input.IsActionPressed("ui_right"))                
		{
			rotationDir += 1;
		}
		if (Input.IsActionPressed("ui_left"))
		{
			rotationDir -= 1;
		}
		if (Input.IsActionPressed("ui_home"))
		{
			Shoot();
		}
		
		SetAppliedTorque(rotationDir * torque);
		rotationDir = 0;
	}
}