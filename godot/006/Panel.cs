using Godot;
using System;

public class Panel : Godot.Panel
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
		var redColor = new Color(0.4f, 0.15f, 0.15f);
		var blueColor = new Color(0.15f, 0.15f, 0.4f);
		var grayColor = new Color(0.15f, 0.15f, 0.15f);
		var style = new StyleBoxFlat();
		
		style.SetBgColor(grayColor);
		Set("custom_styles/panel", style);

    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
