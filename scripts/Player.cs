using Godot;
using System;
using System.Threading.Tasks;

public partial class Player : RigidBody3D
{
    [Export(PropertyHint.Range, "1,3000,1")] public int Thrust = 1000;
    [Export(PropertyHint.Range, "1,500,1")] int Torque = 100;

    public static bool isPlayerCrash = false;
    private Vector3 StartPosition;
    private Vector3 StartRotation;
    private AudioStreamPlayer3D Moteur;
    private AudioStreamPlayer Explosion;
    private AudioStreamPlayer WinSound;
    private GpuParticles3D ParticuleBooster;
    private GpuParticles3D ParticuleBoosterLeft;

    private GpuParticles3D ParticuleBoosterRight;
    private GpuParticles3D ParticuleExplosion;

    private GpuParticles3D ParticuleWin;
    private FollowCamera camera;

    public override void _Ready()
    {
        StartPosition = Position;
        StartRotation = RotationDegrees;
        Moteur = GetNode<AudioStreamPlayer3D>("Engine");
        Explosion = GetNode<AudioStreamPlayer>("Explosion");
        WinSound = GetNode<AudioStreamPlayer>("Win");
        ParticuleBooster = GetNode<GpuParticles3D>("BoosterParticles");
        ParticuleBoosterLeft = GetNode<GpuParticles3D>("BoosterParticlesLeft");
        ParticuleBoosterRight = GetNode<GpuParticles3D>("BoosterParticlesRight");
        ParticuleExplosion = GetNode<GpuParticles3D>("ExplosionParticles");
        ParticuleWin = GetNode<GpuParticles3D>("SuccessParticles");
    }
    public override void _Process(double delta)
    {
        if (Input.IsActionPressed("ui_accept"))
        {
            ApplyCentralForce(Basis.Y * (float)delta * Thrust);

            if (!Moteur.Playing)
            {
                Moteur.Play();
                ParticuleBooster.Emitting = true;
            }
        }
        else
        {
            if (Moteur.Playing)
            {
                Moteur.Stop();
                ParticuleBooster.Emitting = false;
            }
        }
        if (Input.IsActionPressed("ui_left"))
        {
            ApplyTorque(Basis.Z * (float)delta * Torque);
            ParticuleBoosterRight.Emitting = true;
        }
        else {
            ParticuleBoosterRight.Emitting = false;
        }

        if (Input.IsActionPressed("ui_right"))
        {
            ApplyTorque(Basis.Z * (float)delta * -Torque);
            ParticuleBoosterLeft.Emitting = true;
        }
        else
        {
            ParticuleBoosterLeft.Emitting = false;
        }

        if (Input.IsActionPressed("reset"))
        {
            ResetPosition();
        }

    
}
    public void ResetPosition()
    {
        Position = StartPosition;
        RotationDegrees = StartRotation;
        LinearVelocity = Vector3.Zero;
        AngularVelocity = Vector3.Zero;
        isPlayerCrash = false;
        SetProcess(true);
    }
    public void _on_body_entered_player(Node3D body)
    {

        if (isPlayerCrash)return;
        
        if (body.IsInGroup("win"))
            {
                
                win((body as LandingPad).filepathlevel);
            }
        if (body.IsInGroup("Obstacle"))
            {
                crash();
            }
        
    }
    private async void crash()
    {
        isPlayerCrash = true;
        SetProcess(false);
        Explosion.Play();
        ParticuleExplosion.Emitting = true;
        Moteur.Stop();
        ++GameSettings.Try;
        await ToSignal(Explosion, "finished");
        ResetPosition();
        


    }
    
    private void win(string next_level_file)
    {
        WinSound.Play();
        isPlayerCrash = true;
        Tween tween = CreateTween();
        ParticuleWin.Emitting = true;
        tween.TweenInterval(1.5f);
        tween.TweenCallback(Callable.From(() => GetTree().ChangeSceneToFile(next_level_file)));
        tween.TweenCallback(Callable.From(() => ResetPosition()));
    }
}
