using Godot;
using System;

public class Ridiculous : Godot.RigidBody2D
{
	private Vector2 thrust = new Vector2(0, 250);
	private float torque = 10000;
	private float rotationDir;
	
	[Remote]
	public float rotation;

    public override void _Ready()
    {
        
    }
	
	public override void _IntegrateForces(Physics2DDirectBodyState state)
	{
		if (IsNetworkMaster())
		{
			
			if (Input.IsActionPressed("ui_right"))              
			{
				rotationDir += 1;
			}
			if (Input.IsActionPressed("ui_left"))
			{
				rotationDir -= 1;
			}
				
			SetAppliedTorque(rotationDir * torque);
			rotationDir = 0;
			
			Rset("rotation", RotationDegrees);
		}
		else
		{
			RotationDegrees = rotation;
		}
	}
}
