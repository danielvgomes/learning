using Godot;
using System;
using System.Collections;

public class Network : Node2D
{
	private Panel panel;
	private NetworkedMultiplayerENet peer;
	private int maxPlayers = 500;
	private ArrayList players;
	private Random random = new Random();
	private RigidBody2D myself;
	
	[Remote]
	public int lolRotation;
	
	public override void _Ready()
	{
		panel = GetNode<Panel>("Panel");
		// GetTree().SetNetworkPeer(peer);
		
		GetTree().Connect("network_peer_connected", this, "networkPeerConnected");
		GetTree().Connect("server_disconnected", this, "serverDisconnected");
		
		var playerScene = (PackedScene)ResourceLoader.Load("res://Ridiculous.tscn");
		// RigidBody2D myself = playerScene.GetNode<RigidBody2D>("Ridiculous");
		
		myself = playerScene.Instance() as RigidBody2D;
		
		myself.Position = new Vector2(random.Next(20, 380), random.Next(100, 335));
		// myself.Rotation = Rotation;
		// myself.Position = GetNode<Position2D>("Position2D").GlobalPosition;
		
		GetNode<Panel>("Panel").AddChild(myself);
		
		players = new ArrayList();
		
		// selfie = (RigidBody2D)panel.GetNode<RigidBody2D>("RigidBody2D");
		// var myPeer = GetTree().GetNetworkPeer();
		// GD.Print(GetTree().IsNetworkServer());
	}
	
	private void onConnectPressed()
	{
		peer = new NetworkedMultiplayerENet();
		int port = System.Convert.ToInt32(panel.GetNode<TextEdit>("Port").Text);
		String address = panel.GetNode<TextEdit>("Address").Text;
		
		if (panel.GetNode<OptionButton>("OptionButton").GetSelectedId() == 0)
		{
			peer.CreateServer(port, maxPlayers);
			GetTree().SetNetworkPeer(peer);
			myself.SetNetworkMaster(GetTree().GetNetworkUniqueId());
			panel.changeToRed();
			players.Add(myself);
			// selfie.GetNode<Sprite>("Sprite").Position = new Vector2(80, 365);
		}
		else
		{
			peer.CreateClient(address, port);
			GetTree().SetNetworkPeer(peer);
			panel.changeToBlue();
			// selfie.GetNode<Sprite>("Sprite").Position = new Vector2(80, 365);
		}
		
		// GetTree().SetNetworkPeer(peer);
		// selfie.SetNetworkMaster(GetTree().GetNetworkUniqueId());
		
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
		// Rpc("holyDog");
		// Rpc("registerPlayer");
		int prov = players.Count;
		panel.print("counting players -> " + prov.ToString());
		panel.print("this is meself -> " + GetTree().GetNetworkUniqueId());
	}
	
	public void networkPeerConnected(int id)
	{
		// object[] myObj = new object[] {"132", 12, "hello"};
		panel.print("networkPeerConnected " + id);
		RpcId(id, "registerPlayer");
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
		panel.print("registerPlayer -> GetRpcSenderId() = " + GetTree().GetRpcSenderId());
		players.Add(GetTree().GetRpcSenderId());
	}
}