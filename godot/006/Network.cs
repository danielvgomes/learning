using Godot;
using System;
using System.Collections.Generic;

public class Network : Node2D
{
	private Panel panel;
	private NetworkedMultiplayerENet peer;
	private int maxPlayers = 500;
	private List<Node> players;
	private Random random = new Random();
	private Node myself;
	private PackedScene playerScene;
	private Node otterMyself;
	
	public override void _Ready()
	{
		panel = GetNode<Panel>("Panel");
		
		GetTree().Connect("network_peer_connected", this, "networkPeerConnected");
		GetTree().Connect("server_disconnected", this, "serverDisconnected");
		
		playerScene = (PackedScene)ResourceLoader.Load("res://Ridiculous.tscn");
		players = new List<Node>();
	}
	
	private void onConnectPressed()
	{
		players = new List<Node>();
		peer = new NetworkedMultiplayerENet();
		int port = System.Convert.ToInt32(panel.GetNode<TextEdit>("Port").Text);
		String address = panel.GetNode<TextEdit>("Address").Text;
		
		if (panel.GetNode<OptionButton>("OptionButton").GetSelectedId() == 0)
		{
			peer.CreateServer(port, maxPlayers);
			GetTree().SetNetworkPeer(peer);
			
			panel.changeToRed();
			players.Add(myself);
			myself = playerScene.Instance();
			myself.GetNode<RigidBody2D>("Ridiculous").Position = new Vector2(random.Next(20, 380), random.Next(100, 335));
			myself.SetNetworkMaster(GetTree().GetNetworkUniqueId());
			myself.SetName(GetTree().GetNetworkUniqueId().ToString());
			GetNode<Panel>("Panel").AddChild(myself);
		
		}
		else
		{
			peer.CreateClient(address, port);
			GetTree().SetNetworkPeer(peer);
			panel.changeToBlue();
			
			myself = playerScene.Instance();
			myself.GetNode<RigidBody2D>("Ridiculous").Position = new Vector2(random.Next(20, 380), random.Next(100, 335));
			myself.SetNetworkMaster(GetTree().GetNetworkUniqueId());
			myself.SetName(GetTree().GetNetworkUniqueId().ToString());
			GetNode<Panel>("Panel").AddChild(myself);
		}
	}
	
	private void onDisconnectPressed()
	{
		GetTree().SetNetworkPeer(null);
		peer = null;
		players = new List<Node>();
		panel.changeToGray();
	}
	
	public void networkPeerConnected(int id)
	{
			otterMyself = playerScene.Instance();
			otterMyself.GetNode<RigidBody2D>("Ridiculous").Position = new Vector2(random.Next(20, 380), random.Next(100, 335));
			otterMyself.SetNetworkMaster(id);
			otterMyself.SetName(id.ToString());
			GetNode<Panel>("Panel").AddChild(otterMyself);
	}
	
	public void serverDisconnected()
	{
		peer = null;
	}
}