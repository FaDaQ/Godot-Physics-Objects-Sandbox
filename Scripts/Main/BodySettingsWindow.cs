using Godot;
using System.Collections.Generic;

public partial class BodySettingsWindow : PanelContainer
{
	private Dictionary<string, float> parametrsValues = new Dictionary<string, float>();
	private bool mouseEntered = false;
	private Godot.Collections.Array<Node> namesArr;
	private Godot.Collections.Array<Node> parametrsArr;
	public override void _Ready()
	{
		namesArr = GetChild(0).GetChild(0).GetChildren();
		parametrsArr = GetChild(0).GetChild(1).GetChildren();

		for (int i = 0; i < namesArr.Count; i++)
		{
			parametrsValues.Add(((Label)namesArr[i]).Text, float.Parse(((LineEdit)parametrsArr[i]).Text.Replace('.', ',')));
		}
		foreach (var i in parametrsValues)
		{
			Settings.physicsParametrs[i.Key] = i.Value;
		}
		Settings.Scale = new Vector2(Settings.physicsParametrs["Scale"], Settings.physicsParametrs["Scale"]);

	}
	public override void _Process(double delta)
	{
		if (IsVisibleInTree())
		{
			ReadParametrs();
			foreach (var i in parametrsValues)
			{
				Settings.physicsParametrs[i.Key] = i.Value;
			}
			Settings.Scale = new Vector2(Settings.physicsParametrs["Scale"], Settings.physicsParametrs["Scale"]);
		}
	}
	// Call in pressed signal in Body Signal button
	private void ShowWindow()
	{
		if (IsVisibleInTree())
			Hide();
		else
			Show();
	}
	private void ReadParametrs()
	{
		
		int j = 0;
		foreach (var i in parametrsValues)
		{
			parametrsValues[i.Key] = float.Parse(((LineEdit)parametrsArr[j++]).Text.Replace('.', ','));
		}
	}
}
