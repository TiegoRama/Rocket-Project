using Godot;

public partial class FollowCamera : Camera3D
{
    [Export] public Player target;
    [Export] public Vector3 offset = new Vector3(0, 5, 10);
    [Export] public float smoothSpeed = 5.0f;
    

    public override void _Process(double delta)
    {

        Vector3 desiredPosition = target.GlobalPosition + offset;

        desiredPosition.X = target.GlobalPosition.X + offset.Y; 

        desiredPosition.Z = target.GlobalPosition.Z + offset.Z; 

        GlobalPosition = GlobalPosition.Lerp(desiredPosition, smoothSpeed * (float)delta);
    }
    
}