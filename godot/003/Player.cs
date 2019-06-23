using Godot;
using System;

public class Player : Area2D
{
	[Signal]
	public delegate void Hit();
	
	[Export]
	public int Speed = 400; // player speed in pixels/sec
	
	private Vector2 _screenSize;
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
		Hide();
        _screenSize = GetViewport().GetSize();
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		var velocity = new Vector2(); // player movement vector
		
		if (Input.IsActionPressed("ui_right"))
		{
			velocity.x += 1;
		}
		
		if (Input.IsActionPressed("ui_left"))
		{
			velocity.x -= 1;
		}
		
		if (Input.IsActionPressed("ui_down"))
		{
			velocity.y += 1;
		}
		
		if (Input.IsActionPressed("ui_up"))
		{
			velocity.y -= 1;
		}
		
		var animatedSprite = GetNode<AnimatedSprite>("AnimatedSprite");
		
		if (velocity.Length() > 0)
		{
			velocity = velocity.Normalized() * Speed;
			animatedSprite.Play();
		}
		else
		{
			animatedSprite.Stop();
		}
		
		Position += velocity * delta;
		Position = new Vector2(
			x: Mathf.Clamp(Position.x, 0, _screenSize.x),
			y: Mathf.Clamp(Position.y, 0, _screenSize.y)
		);
		
		if (velocity.x < 0)
		{
			animatedSprite.Rotation = 33;
			animatedSprite.FlipV = true;
		}
		else if (velocity.x > 0)
		{
			animatedSprite.Rotation = 33;
			animatedSprite.FlipV = false;
		}
		else if (velocity.y != 0)
		{
			animatedSprite.Rotation = 0;
			animatedSprite.Animation = "up";
			animatedSprite.FlipV = velocity.y > 0;
		}
	}

	public void Start(Vector2 pos)
	{
		Position = pos;
		Show();
		GetNode<CollisionShape2D>("CollisionShape2D").Disabled = false;
	}

	private void _on_Player_body_entered(object body)
	{
		Hide();
		EmitSignal("Hit");
		GetNode<CollisionShape2D>("CollisionShape2D").Disabled = true;
	}
	
	
	
}
