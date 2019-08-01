using Godot;
using System;

public class Panel : Godot.Panel
{
	private Color redColor = new Color(0.4f, 0.15f, 0.15f);
	private Color blueColor = new Color(0.15f, 0.15f, 0.4f);
	private Color grayColor = new Color(0.15f, 0.15f, 0.15f);
	
	private StyleBoxFlat style = new StyleBoxFlat();
	
    public override void _Ready()
    {
		style.SetBgColor(grayColor);
		Set("custom_styles/panel", style);
		
		GetNode<OptionButton>("OptionButton").AddItem("Host", 0);
		GetNode<OptionButton>("OptionButton").AddItem("Client", 1);
		
    }
	
	public void changeToRed()
	{
		style.SetBgColor(redColor);
	}
	
	public void changeToBlue()
	{
		style.SetBgColor(blueColor);
	}
	
	public void changeToGray()
	{
		style.SetBgColor(grayColor);
	}
	
	public void print(String s)
	{
		GetNode<TextEdit>("Text").InsertTextAtCursor(s + "\n");
	}
	
	public void print(int i)
	{
		GetNode<TextEdit>("Text").InsertTextAtCursor(System.Convert.ToInt32(i) + "\n");
	}
}
