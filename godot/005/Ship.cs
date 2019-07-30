using Godot;
using System;

public class Ship : RigidBody2D
{
	private Vector2 thrust = new Vector2(0, 250);
	private float torque = 10000;
	private float rotationDir;
	
	private bool shot, shooting, shooting2, shot2, shieldUp = false;
	private Timer t, t2;
	
	[Export]
	PackedScene bulletScene;
	
	[Export]
	PackedScene bulletScene2;
	
	[Signal]
	delegate void Shoot(PackedScene bullet, float direction, Vector2 location);
	
	public Ship()
	{
		// bulletScene = (PackedScene)ResourceLoader.Load("res://Bullet.tscn");
	}
	
	public override void _Ready()
	{
		t = new Timer();
		t2 = new Timer();
		AddChild(t);
		AddChild(t2);
		t.Connect("timeout", this, nameof(onTimerTimeout));
		t2.Connect("timeout", this, nameof(onTimerTimeout2));
		Log.p("suck my ship: " + GetTree().GetRoot().GetPath());
		// GetNode<Sprite>("Sprite").SetTexture(sS);
	}
	
	public override void _Process(float delta)
	{
		if (shooting)
		{
			if (!shot)
			{
				// Log.p("Ship - _Process");
				shot = true;
				t.Start(0.1f);
				// Log.p("Ship - emitting...");
				EmitSignal(nameof(Shoot), bulletScene, (Rotation + Math.PI * 90 / 180.0), GetNode<Position2D>("Position2D").GlobalPosition);
				// Log.p("Ship - emitting... done");
				// Log.p("vamos testar essa merda:");
				// Log.p("rotation -> " + (Rotation * (180.0 / Math.PI)));
				// Log.p("globalpos -> " + GetNode<Position2D>("Position2D").GlobalPosition);
			}
		}
		
		if (shooting2)
		{
			if (!shot2)
			{
				KinematicBody2D bulletInstance2 = bulletScene2.Instance() as KinematicBody2D;
				bulletInstance2.Rotation = Rotation;
				bulletInstance2.Position = GetNode<Position2D>("Position2D").GlobalPosition;
				GetParent().AddChild(bulletInstance2);
				shot2 = true;
				t2.Start(0.1f);
			}
		}
	}
	
	public void onTimerTimeout()
	{
		shot = false;
	}
	
	public void onTimerTimeout2()
	{
		shot2 = false;
	}
	
	private void onAnimationPlayerAnimationFinished(String anim_name)
	{
    	if (anim_name == "shield_up")
		{
			GetNode<Sprite>("sS").GetNode<AnimationPlayer>("AnimationPlayer").Play("shield_loop");
		}
	
		if (anim_name == "shield_down")
		{
		}
	}
	
	
	public override void _IntegrateForces(Physics2DDirectBodyState state)
	{
		
		if (Input.IsActionPressed("shield_up"))
		{
			if (!shieldUp)
			{
				GetNode<Sprite>("sS").GetNode<AnimationPlayer>("AnimationPlayer").Play("shield_up");
				shieldUp = true;
			}
		}
		
		if (Input.IsActionPressed("shield_down"))
		{
			if (shieldUp)
			{
				GetNode<Sprite>("sS").GetNode<AnimationPlayer>("AnimationPlayer").Play("shield_down");
				shieldUp = false;
			}
		}
		
		
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
			shooting2 = true;
			// GetNode<CPUParticles2D>("Weapon1").Emitting = true;
			Vector2 collisionPoint = GetNode<RayCast2D>("RayCast2D").GetCollisionPoint();
			float distanceFromObject = GetGlobalPosition().DistanceTo(collisionPoint);
			Log.p("distance from object: " + (distanceFromObject));
			Log.p("is colliding: " + GetNode<RayCast2D>("RayCast2D").IsColliding());
			
			if (GetNode<RayCast2D>("RayCast2D").IsColliding())
			{
				// GetNode<CPUParticles2D>("Weapon1").Lifetime = (distanceFromObject/1000);
			}
			else
			{
				// GetNode<CPUParticles2D>("Weapon1").Lifetime = 1f;
			}
		}
		else
		{
			shooting2 = false;
			// GetNode<CPUParticles2D>("Weapon1").Emitting = false;
		}
			
		SetAppliedTorque(rotationDir * torque);
		rotationDir = 0;
	}
}