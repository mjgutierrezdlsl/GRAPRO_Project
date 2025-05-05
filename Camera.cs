
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

public class Camera
{
    private Vector3 Position;
    private Vector3 Front = -Vector3.UnitZ;
    private Vector3 Up = Vector3.UnitY;
    private float Speed = 1.5f;
    public Camera(Vector3 Position)
    {
        this.Position = Position;
    }
    public Matrix4 GetViewMatrix()
    {
        return Matrix4.LookAt(Position, Position + Front, Up);
    }
    public void ProcessInputs(KeyboardState input, FrameEventArgs e)
    {
        float deltaTime = (float)e.Time;

        if (input.IsKeyDown(Keys.W))
        {
            Position += Front * Speed * deltaTime;
        }
        if (input.IsKeyDown(Keys.S))
        {
            Position -= Front * Speed * deltaTime;
        }
        if (input.IsKeyDown(Keys.A))
        {
            Position -= Vector3.Normalize(Vector3.Cross(Front, Up)) * Speed * deltaTime;
        }
        if (input.IsKeyDown(Keys.D))
        {
            Position += Vector3.Normalize(Vector3.Cross(Front, Up)) * Speed * deltaTime;
        }
        if (input.IsKeyDown(Keys.Space))
        {
            Position += Up * Speed * deltaTime;
        }
        if (input.IsKeyDown(Keys.LeftShift))
        {
            Position -= Up * Speed * deltaTime;
        }
    }
}
