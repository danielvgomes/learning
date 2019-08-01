using Godot;
using System;

public class Network : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
	private Panel panel;
	private NetworkedMultiplayerENet peer;
	private int maxPlayers = 500;
	private RigidBody2D selfie;
	
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
		panel = GetNode<Panel>("Panel");
		// GetTree().SetNetworkPeer(peer);
		
		GetTree().Connect("network_peer_connected", this, "networkPeerConnected");
		GetTree().Connect("server_disconnected", this, "serverDisconnected");
		
		selfie = (RigidBody2D)panel.GetNode<RigidBody2D>("RigidBody2D");
		
		// var myPeer = GetTree().GetNetworkPeer();
		// GD.Print(GetTree().IsNetworkServer());
    }
	
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
	
	private void onConnectPressed()
	{
		peer = new NetworkedMultiplayerENet();
		int port = System.Convert.ToInt32(panel.GetNode<TextEdit>("Port").Text);
		String address = panel.GetNode<TextEdit>("Address").Text;
		
		if (panel.GetNode<OptionButton>("OptionButton").GetSelectedId() == 0)
		{
			peer.CreateServer(port, maxPlayers);
			panel.changeToRed();
			selfie.GetNode<Sprite>("Sprite").Position = new Vector2(80, 365);
			
		}
		else
		{
			peer.CreateClient(address, port);
			panel.changeToBlue();
			selfie.GetNode<Sprite>("Sprite").Position = new Vector2(80, 365);
		}
		
		GetTree().SetNetworkPeer(peer);
		selfie.SetNetworkMaster(GetTree().GetNetworkUniqueId());
		
		panel.print("OptionButton = " + panel.GetNode<OptionButton>("OptionButton").GetSelectedId());
		panel.print("IsNetworkServer = " + GetTree().IsNetworkServer());
		
		// panel.print("tets" + GetTree().GetNetworkPeer());
		panel.print(GetTree().GetNetworkUniqueId());
	}
	
	private void onDisconnectPressed()
	{
		panel.print("disconnecting...");
		GetTree().SetNetworkPeer(null);
		peer = null;
		panel.changeToGray();
	}
	
	private void onSendPressed()
	{
		Rpc("holyDog");
		Rpc("registerPlayer");
	}
	
	public void networkPeerConnected(int id)
	{
		panel.print("networkPeerConnected " + id);
	}
	
	public void serverDisconnected()
	{
		panel.print("serverDisconnected");
		peer = null;
	}
	
	public void holyDog()
	{
		panel.print("FOK FOK FOK FOK FOK");
	}
	
	[Remote]
	public void registerPlayer()
	{
		panel.print("register register register");
	}
	
	
}