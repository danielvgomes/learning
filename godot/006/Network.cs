using Godot;
using System;

public class Network : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
	

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
		var peer = new NetworkedMultiplayerENet();
		// peer.CreateServer(5000, 2);
		// GetTree().SetNetworkPeer(peer);
		
		
		// var myPeer = GetTree().GetNetworkPeer();
		// GD.Print(GetTree().IsNetworkServer());
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
