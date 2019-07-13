using Godot;
using System;

public class Main : Node2D
{
	[Export]
	PackedScene ship;
	
	Camera2D myCam = new Camera2D();
	RigidBody2D s;

	// Ship s;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// ap.Connect("finished", this, nameof(onAudioStreamPlayerFinished));
		GetViewport().Connect("size_changed", this, nameof(onViewportSizeChanged));
		
		Log.debug = true;
		s = ship.Instance() as RigidBody2D;
		s.SetPosition(new Vector2(10000f, 10000f));
		// var cam = (Camera2D)GetNode("Camera2D");
		// cam.MakeCurrent();
		// Node2D menode = GetNode<Node2D>("Node2D");
		s.AddChild(myCam);
		myCam.MakeCurrent();
		// var p = (CPUParticles2D)GetNode("ParallaxBackground/ParallaxLayer/CPUParticles2D");
		// Log.p("p emmiting: " + p.Emitting);
		// p.Emitting = true;
		// Log.p(myCam.AnchorMode);

		myCam.SetZoom(new Vector2(1.5f, 1.5f));
		myCam.DragMarginLeft = 0;
		myCam.DragMarginRight = 0;
		myCam.DragMarginTop = 0;
		myCam.DragMarginBottom = 0;
		// myCam.SmoothingEnabled = true;
		
		// Log.p(cam.Position);
		// Log.p("esa cameras e' corete: " + cam.Current);
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
			Log.p("agrak q saok eaks x: " + s.Position.x + ", " + s.Position.y);
		}
	}
	
	public void onViewportSizeChanged()
	{
		// TODO: Write Method
		Log.p("Viewport size: " + GetViewport().GetSize());
	}
}