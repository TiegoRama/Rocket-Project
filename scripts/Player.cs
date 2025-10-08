using Godot;
using System;
using System.Threading.Tasks;

public partial class Player : RigidBody3D
{
    [Export(PropertyHint.Range, "1,3000,1")] public int Thrust = 1000;
    [Export(PropertyHint.Range, "1,500,1")] int Torque = 100;

    public bool isPlayerCrash = false;
    private Vector3 startPosition;
    private Vector3 startRotation;
    public override void _Ready()
    {
        startPosition = Position;
        startRotation = RotationDegrees;

    }
    public override void _Process(double delta)
    {
        if (Input.IsActionPressed("ui_accept"))
        {
            ApplyCentralForce(Basis.Y * (float)delta * Thrust);
            GetNode<AudioStreamPlayer>("Engine").Play();
        }
        if (Input.IsActionPressed("ui_left"))
        {
            ApplyTorque(Basis.Z * (float)delta * Torque);
        }
        if (Input.IsActionPressed("ui_right"))
        {
            ApplyTorque(Basis.Z * (float)delta * -Torque);
        }
        if (Input.IsActionPressed("reset"))
        {
            ResetPosition();
        }
    }
    public void ResetPosition()
    {
        Position = startPosition;
        RotationDegrees = startRotation;
        LinearVelocity = Vector3.Zero;
        AngularVelocity = Vector3.Zero;
    }
    public void _on_body_entered_player(Node3D body)
    {
        if (body.IsInGroup("win") && !isPlayerCrash)
        {
            win((body as LandingPad).filepathlevel);
        }
        if (body.IsInGroup("Obstacle") && !isPlayerCrash)
        {
            crash();
        }
    }
    private async void crash()
    {
        isPlayerCrash = true;
        GetNode<AudioStreamPlayer>("Explosion").Play();
        await ToSignal(GetNode<AudioStreamPlayer>("Explosion"), "finished");
        GetNode<Label>("Score").Text = "Essai : " + (++GameSettings.Try).ToString();
        ResetPosition();
        isPlayerCrash = false;
    }
    private async void win(string next_level_file)
    {
            GetNode<AudioStreamPlayer>("Win").Play();
            await ToSignal(GetNode<AudioStreamPlayer>("Win"), "finished");
            GetTree().ChangeSceneToFile(next_level_file);
    }
}
