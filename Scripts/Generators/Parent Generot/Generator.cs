using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

public partial class Generator : Node2D
{
	protected Timer _timer;
	protected Vector2 _generatorPosition;
	protected Node2D _parent;
	protected PackedScene _bodyScene;

	protected bool setupOver = false;

	public override void _Ready()
	{
		GetChild<Control>(0).ConnectingUISignals();
		_generatorPosition = GetViewport().GetMousePosition();
		_parent = GetParent<Node2D>();
		_timer = GetChild<Timer>(1);
		Settings.AnyEntity.Add(this);
	}
	public override void _Process(double delta)
	{
		if (!setupOver)
		{
			ReadParametrs();
		}
	}
	public virtual void Generate()
	{
		Console.WriteLine("Generate");
	}
	public virtual void SetupOver()
	{
		GetChild<PanelContainer>(0).Hide();

		ObjectsSpawn.Generators.Add(this);
		setupOver = true;
		_timer.Start();
	}
	public virtual void ReadParametrs()
	{
		_timer.WaitTime = float.Parse(GetNode<LineEdit>("Generator Settings Panel/Columns/Parametrs Change/Time Delay Edit").Text.Replace('.', ','));
	}

	// It is called by clicking on the cross in the settings window of each generator
	public void CloseGeneratorPanel()
	{
		_parent.RemoveChild(this);
		Settings.AnyEntity.Remove(this);
		ObjectsSpawn.Generators.Remove(this);
		Settings.userIsInteractGUI = false;
	}
}
