using Godot;
using System;

public class Main : Node2D
{
	[Export]
	PackedScene ship;
	// Ship s;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// ap.Connect("finished", this, nameof(onAudioStreamPlayerFinished));
		GetViewport().Connect("size_changed", this, nameof(onViewportSizeChanged));
		
		Log.debug = true;
		var s = ship.Instance() as RigidBody2D;
		// s = new Ship();
		AddChild(s);
		Log.p("AddChild(s.ship)");
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	// public override void _Process(float delta)
	// {
	// }

	public override void _UnhandledInput(InputEvent @event)
	{
    	if (@event is InputEventKey eventKey)
		{
			if (eventKey.Pressed && eventKey.Scancode == (int)KeyList.Escape)
			{
				GetTree().Quit();
			}
		}
	}

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseButton eventMouseButton)
		{
			Log.p("Click");
			Log.p(GetViewport().GetMousePosition().x + ", " + GetViewport().GetMousePosition().y);
		}
	}
	
	public void onViewportSizeChanged()
	{
		// TODO: Write Method
		Log.p("Viewport size: " + GetViewport().GetSize());
	}
}