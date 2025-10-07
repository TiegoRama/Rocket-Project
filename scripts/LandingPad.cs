using Godot;
using System;

public partial class LandingPad : CsgBox3D
{
    private Player player;
        public void landpad_body_entered(Node3D body)
    {
        if (body is  Player player && !player.isPlayerCrash)
        {
            player.isPlayerCrash = true;
            GD.Print("Gagné");
            player.isPlayerCrash = false;
        }
    }

}
