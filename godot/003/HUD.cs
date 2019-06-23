using Godot;
using System;

public class HUD : CanvasLayer
{
	[Signal]
	public delegate void StartGame();
	
	public void ShowMessage(string text)
	{
		var messageLabel = GetNode<Label>("MessageLabel");
		messageLabel.Text = text;
		messageLabel.Show();
		GetNode<Timer>("MessageTimer").Start();
	}
	
	async public void ShowGameOver()
	{
		ShowMessage("Feminism Won");
		var messageTimer = GetNode<Timer>("MessageTimer");
		await ToSignal(messageTimer, "timeout");
		
		var messageLabel = GetNode<Label>("MessageLabel");
		messageLabel.Text = "Dodge the Feminists";
		messageLabel.Show();
		
		GetNode<Button>("StartButton").Show();
	}
	
	public void UpdateScore(int score)
	{
		GetNode<Label>("ScoreLabel").Text = score.ToString();
	}

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }

	private void _on_MessageTimer_timeout()
	{
		GetNode<Label>("MessageLabel").Hide();
	}

	private void _on_StartButton_pressed()
	{
		GetNode<Button>("StartButton").Hide();
		EmitSignal("StartGame");
	}
}



