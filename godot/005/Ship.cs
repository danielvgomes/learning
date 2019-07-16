using Godot;
using System;

public class Ship : RigidBody2D
{
	private Vector2 thrust = new Vector2(0, 250);
	private float torque = 10000;
	private float rotationDir;
	
	// private RigidBody2D bulletInstance;
	
	
	private bool shot, shooting;
	private Timer t;
	
	[Export]
	PackedScene bulletScene;
	
	public Ship()
	{
		// bulletScene = (PackedScene)ResourceLoader.Load("res://Bullet.tscn");
	}
	
	public override void _Ready()
	{
		t = new Timer();
		AddChild(t);
		t.Connect("timeout", this, nameof(onTimerTimeout));
	}
	
	public override void _Process(float delta)
	{
		if (shooting)
		{
			if (!shot)
			{
				RigidBody2D bulletInstance = bulletScene.Instance() as RigidBody2D;
				bulletInstance.Rotation = Rotation;
				bulletInstance.Position = GetNode<Position2D>("Position2D").Position;
				AddChild(bulletInstance);
				shot = true;
				t.Start(0.1f);
			}
		}
	}
	
		public void onTimerTimeout()
	{
		shot = false;
	}
	
	public override void _IntegrateForces(Physics2DDirectBodyState state)
	{
		if (Input.IsActionPressed("ui_up") || Input.IsActionPressed("ui_down"))
		{
			if (Input.IsActionPressed("ui_up"))
			{
				SetAppliedForce(thrust.Rotated(Rotation));
			}
			
			if (Input.IsActionPressed("ui_down"))
			{
				SetAppliedForce(-thrust.Rotated(Rotation));
			}
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
		if (Input.IsActionPressed("shoot_1"))
		{
			shooting = true;
		}
		else
		{
			shooting = false;
		}
		if (Input.IsActionPressed("shoot_2"))
		{
			GetNode<CPUParticles2D>("Weapon1").Emitting = true;
			
			Vector2 collisionPoint = GetNode<RayCast2D>("RayCast2D").GetCollisionPoint();
			float distanceFromObject = GetGlobalPosition().DistanceTo(collisionPoint);
			Log.p("distanc/e from object: " + (distanceFromObject/1000));
			Log.p("is colliding: " + GetNode<RayCast2D>("RayCast2D").IsColliding());
			
			
			if (GetNode<RayCast2D>("RayCast2D").IsColliding())
			{
				GetNode<CPUParticles2D>("Weapon1").Lifetime = (distanceFromObject/1000);
				// GetNode<CPUParticles2D>("Weapon1").Amount = (int)(distanceFromObject/100);
			}
			else
			{
				GetNode<CPUParticles2D>("Weapon1").Lifetime = 1f;
				// GetNode<CPUParticles2D>("Weapon1").Amount = 10;
			}
			
			
		}
		else
		{
			GetNode<CPUParticles2D>("Weapon1").Emitting = false;
			// TODO: create method (haha lol)
		}
			
			
		SetAppliedTorque(rotationDir * torque);
		rotationDir = 0;
	}
}