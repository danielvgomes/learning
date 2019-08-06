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
	
	private int justVar1;
	private int justVar2;
	
	[Remote]
	public int slaveVar1;
	
	[Remote]
	public int slaveVar2;
	
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
		
		myself = playerScene.Instance();
		
		myself.GetNode<RigidBody2D>("Ridiculous").Position = new Vector2(random.Next(20, 380), random.Next(100, 335));
		// myself.Rotation = Rotation;
		// myself.Position = GetNode<Position2D>("Position2D").GlobalPosition;
		
		GetNode<Panel>("Panel").AddChild(myself);
		players = new List<Node>();
		
		// selfie = (RigidBody2D)panel.GetNode<RigidBody2D>("RigidBody2D");
		// var myPeer = GetTree().GetNetworkPeer();
		// GD.Print(GetTree().IsNetworkServer());
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
			
			myself.SetNetworkMaster(GetTree().GetNetworkUniqueId());
			myself.SetName("mesef is sever");
			
			panel.changeToRed();
			
			panel.print("adding myself as a player");
			players.Add(myself);
			// selfie.GetNode<Sprite>("Sprite").Position = new Vector2(80, 365);
		}
		else
		{
			peer.CreateClient(address, port);
			GetTree().SetNetworkPeer(peer);
			myself.SetName("mesef is clien");
			panel.changeToBlue();
			panel.print("i'm a client, i should register myself");
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
		players = new List<Node>();
		panel.changeToGray();
	}
	
	private void onSendPressed()
	{
		// Rpc("holyDog");
		// Rpc("registerPlayer");
	}
	
	private void onPlusPressed()
	{
		justVar1 += 10;
		justVar2 += 10;
		if (IsNetworkMaster())
		{
			// panel.print("rset a slave");
			// panel.print("rset another slave");
		}
		else
		{
			// panel.print("my var = slaves");
			// panel.print("my othervar = slaves");
		}
		// int prov = players.Count;
		// panel.print("counting players -> " + prov.ToString());
		// panel.print("this is meself -> " + GetTree().GetNetworkUniqueId());
	}
	
	private void onMinusPressed()
	{
		justVar1 -= 1;
		justVar2 -= 1;
		if (IsNetworkMaster())
		{
			// panel.print("rset a slave");
			// panel.print("rset another slave");
		}
		else
		{
			// panel.print("my var = slaves");
			// panel.print("my othervar = slaves");
		}
		// panel.print("players:");
		// for (int i = 0; i < players.Count; i++)
		/* {
			GD.Print(players[i].GetType());
			GD.Print(players[i].GetName());
			GD.Print(players[i].GetNetworkMaster());
			// Node blz = (PackedScene)players[i];
			// panel.print(p.GetName());
			// panel.print(p.GetNetworkMaster());
		} */
	}
	
	public void networkPeerConnected(int id)
	{
		// object[] myObj = new object[] {"132", 12, "hello"};
		panel.print("networkPeerConnected " + id);
		panel.print("someone connected, requesting registration");
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
		panel.print("remote method 'registerPlayer' called");
		panel.print("registerPlayer -> GetRpcSenderId() = " + GetTree().GetRpcSenderId());
		panel.print("adding player to list");
		// players.Add(GetTree().GetRpcSenderId());
	}
	
	private void onTimerTimeout()
	{
		GetNode<Timer>("Timer").SetWaitTime(1f);
		GetNode<Timer>("Timer").Start();
		
		if (IsNetworkMaster())
		{
			Rset("slaveVar1", justVar1);
			Rset("slaveVar2", justVar2);
		}
		else
		{
			justVar1 = slaveVar1;
			justVar2 = slaveVar2;
		}
		
		panel.print(GetTree().GetNetworkUniqueId() + " justVar1 = " + justVar1);
		panel.print(GetTree().GetNetworkUniqueId() + " justVar2 = " + justVar2);
	}
}