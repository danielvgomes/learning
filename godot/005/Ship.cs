using Godot;
using System;

public class Ship : RigidBody2D
{
	private Vector2 thrust = new Vector2(0, 250);
	private float torque = 10000;
	private float rotationDir;
	private Vector2 shootingPosition;
	private RigidBody2D bulletInstance;
	private bool shooting;
	
	[Export]
	PackedScene bulletScene;
	
	public Ship()
	{
		// bulletScene = (PackedScene)ResourceLoader.Load("res://Bullet.tscn");
	}
	
	public void Shoot()
	{
		Log.p("Shoot()");
		
		
		// Log.p("meparent: " + GetParent());
		// bulletInstance.GetNode<RigidBody2D>("HelloGirl");
		// Log.p(bulletInstance.GetPosition());
		
		/* // isso fuciona sqn, bora tentar de novo
		Log.p(GetNode<Position2D>("Position2D").GlobalPosition);
		shootingPosition = GetNode<Position2D>("Position2D").GlobalPosition;
		Bullet blah = new Bullet();
		blah.Position = shootingPosition;
		blah.Move();
		Log.p("shootingPosition " + shootingPosition);
		GetParent().AddChild(blah);
		*/
	}
	
	public override void _Process(float delta)
	{
		if (shooting)
		{
		bulletInstance = bulletScene.Instance() as RigidBody2D;
		AddChild(bulletInstance);
		// bulletInstance.Move();
		}
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
			shooting = true;
			Log.p("my rotation: " + Rotation);
		}
		else
		{
			shooting = false;
		}
	
			
		SetAppliedTorque(rotationDir * torque);
		rotationDir = 0;
	}
}