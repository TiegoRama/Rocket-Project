using Godot;
using System;

public partial class Score : Control
{

    public override void _Process(double delta)
    {
        GetNode<Label>("Try").Text = "Essai : " + GameSettings.Try.ToString();
    }
}
