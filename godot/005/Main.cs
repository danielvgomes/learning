using Godot;
using System;

public class Main : Node2D
{
	[Export]
	PackedScene ship;
	
	Camera2D myCam = new Camera2D();
	RigidBody2D s;
	
	public override void _Ready() // initialization here
	{
		// ap.Connect("finished", this, nameof(onAudioStreamPlayerFinished));
		GetViewport().Connect("size_changed", this, nameof(onViewportSizeChanged));
		
		Log.debug = true;
		s = ship.Instance() as RigidBody2D;
		s.SetPosition(new Vector2(10000f, 10000f));
		// cam.MakeCurrent();
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
		
		AddChild(s);
		Log.p("AddChild(s.ship)");
		
		GetNode("Ship").Connect("Shoot", this, nameof(onShipShoot));
		// GetNode("/root/Bullet").Connect("area_entered", this, nameof(holyShit));
		
		Log.p("Me Parnet: " + GetParent());
		Log.p("Me viwpotre: " + GetViewport());
		Log.p("metree: " + GetTree());
		Log.p("me hut: " + GetTree().GetRoot());
		foreach (Godot.Object n in GetViewport().GetChildren())
		{
			Log.p("lol -> " + n);
		}
		Log.p("suck my frog: " + GetTree().GetRoot().GetPath());
	}
	
	/*
	* THE METHOD BELOW SHOWS HOW TO PASS SCENES USING SIGNALS, HOLY SHEET
	*/
	
	public void onShipShoot(PackedScene bullet, float direction, Vector2 location)
	{
		// Log.p("Main - onShipShoot method called");
		// Log.p("Main - calling Instance() method...");
		var bulletInstance = (Bullet)bullet.Instance();
		// Log.p("Main - calling Instance() method... done");
		// Log.p("Main - calling AddChild()...");
		AddChild(bulletInstance);
		// Log.p("Main - calling AddChild()... done");
		// bulletInstance.Rotation = direction;
		bulletInstance.Position = location;
		// Log.p("Main - calling Start()");
		bulletInstance.Start(direction);
		// bulletInstance.Velocity = bulletInstance.Velocity.Rotated(direction);
	}
	
	public void holyShit()
	{
		Log.p("guard: finally a collision");
	}
	
	
	public override void _UnhandledInput(InputEvent @event) // set up keys here
	{
    	if (@event is InputEventKey eventKey)
		{
			if (eventKey.Pressed && eventKey.Scancode == (int)KeyList.Escape)
			{
				GetTree().Quit();
			}
			
			if (eventKey.Pressed && eventKey.Scancode == (int)KeyList.Enter)
			{
				Log.p("Enterdddd");
				// Shoot();
			}
		}
	}
	
	public override void _Input(InputEvent @event) // set up mouse here
	{
		if (@event is InputEventMouseButton eventMouseButton)
		{
			Log.p("Click");
			Log.p(GetViewport().GetMousePosition().x + ", " + GetViewport().GetMousePosition().y);
			Log.p("agrak q saok eaks x: " + s.Position.x + ", " + s.Position.y);
		}
	}
	
	public void onViewportSizeChanged() // window size changed
	{
		Log.p("Viewport size: " + GetViewport().GetSize());
	}
}