using Godot;
using System;

public partial class ObstacleAnine : AnimatableBody3D
{
    [Export] public Vector3 destination;
    [Export] public float duree;
    private Vector3 startPosition;


    public override void _Ready()
    {
        var tween = CreateTween();
        tween.SetTrans(Tween.TransitionType.Sine);
        tween.TweenProperty(this, "global_position", startPosition + destination, duree);
        tween.TweenProperty(this, "global_position", startPosition, duree);
        tween.SetLoops();



    }
}
