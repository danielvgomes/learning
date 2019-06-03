using Godot;
using System;

public class sayhello : Panel
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GetNode("Button").Connect("pressed", this, nameof(_OnButtonPressed));
    }
	
	public void _OnButtonPressed()
	{
		GetNode<Label>("Label").Text = "HELLO!112";
		
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
