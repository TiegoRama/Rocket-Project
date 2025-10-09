using Godot;
using System;

public partial class MenuPrincipal : Control
{
    private Score chrono = new Score();
    private void _on_button_pressed()
    {
        GetTree().ChangeSceneToFile("res://scenes/level1.tscn");
        reset_score();
    }
    private void reset_score()
    {
        GameSettings.Try = 0;
    }   
}
