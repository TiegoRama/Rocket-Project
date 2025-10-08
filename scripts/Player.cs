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
        isPlayerCrash = false;
        SetProcess(true);
    }
    public void _on_body_entered_player(Node3D body)
    {
        if (isPlayerCrash == false)
        {
            if (body.IsInGroup("win"))
            {
                win((body as LandingPad).filepathlevel);
            }
            if (body.IsInGroup("Obstacle"))
            {
                crash();
            }
        }
    }
    private async void crash()
    {
        isPlayerCrash = true;
        SetProcess(false);
        GetNode<AudioStreamPlayer>("Explosion").Play();
        GetNode<Label>("Score").Text = "Essai : " + (++GameSettings.Try).ToString();
        await ToSignal(GetNode<AudioStreamPlayer>("Explosion"), "finished");
        ResetPosition();
        
        
    }
    private void win(string next_level_file)
    {
        GetNode<AudioStreamPlayer>("Win").Play();
        Tween tween = CreateTween();
        tween.TweenInterval(1.5f);
        tween.TweenCallback(Callable.From(() => GetTree().ChangeSceneToFile(next_level_file)));

    }
}