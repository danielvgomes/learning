using System;

public static class Log
{
	public static bool debug;
	
	public static void p(Object o)
	{
		if (debug)
		{
			Godot.GD.Print(o);
		}
	}
}

