using Godot;
using System;

public partial class ObstacleAnine : AnimatableBody3D
{
    [Export] public Vector3 destination;
    [Export] public float duree;
    [Export] public float delay;
    [Export] public Tween.TransitionType transitionType = Tween.TransitionType.Sine;
    private Vector3 startPosition;

    public override void _Ready()
    {
        startPosition = GlobalPosition;
        var tween = CreateTween();
        tween.TweenInterval(delay);
        tween.SetTrans(Tween.TransitionType.Sine);
        tween.TweenProperty(this, "global_position", startPosition + destination, duree);
        tween.TweenProperty(this, "global_position", startPosition, duree);
        tween.SetLoops();
        
    }
}