
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

public class Camera
{
    private Vector3 Position;
    private Vector3 Front = -Vector3.UnitZ;
    private Vector3 Up = Vector3.UnitY;
    private Vector3 Right = Vector3.UnitX;
    private float Speed = 1.5f;
    private float AspectRatio;
    private float Pitch;
    private float Yaw = -MathHelper.PiOver2;
    private bool _firstMove = true;
    private Vector2 _lastPos;
    private float Sensitivity = 0.2f;
    public Camera(Vector3 Position, float AspectRatio)
    {
        this.Position = Position;
        this.AspectRatio = AspectRatio;
    }
    public Matrix4 GetViewMatrix()
    {
        return Matrix4.LookAt(Position, Position + Front, Up);
    }
    public Matrix4 GetProjectionMatrix()
    {
        return Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45f), AspectRatio, 0.01f, 100f);
    }
    public void ProcessInputs(KeyboardState input, MouseState mouse, FrameEventArgs e)
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
            Position.Y += Speed * deltaTime;
        }
        if (input.IsKeyDown(Keys.LeftShift))
        {
            Position.Y -= Speed * deltaTime;
        }

        if (_firstMove)
        {
            _lastPos = new(mouse.X, mouse.Y);
            _firstMove = false;
        }
        else
        {
            var deltaX = mouse.X - _lastPos.X;
            var deltaY = mouse.Y - _lastPos.Y;
            _lastPos = new(mouse.X, mouse.Y);
            Yaw += deltaX * Sensitivity * deltaTime;
            Pitch -= deltaY * Sensitivity * deltaTime;
            UpdateVectors();
        }
    }
    public void SetAspectRatio(float width, float height)
    {
        AspectRatio = width / height;
    }
    private void UpdateVectors()
    {
        Pitch = MathHelper.Clamp(Pitch, -89f, 89f);
        Front.X = MathF.Cos(Pitch) * MathF.Cos(Yaw);
        Front.Y = MathF.Sin(Pitch);
        Front.Z = MathF.Cos(Pitch) * MathF.Sin(Yaw);

        Front = Vector3.Normalize(Front);

        Right = Vector3.Normalize(Vector3.Cross(Front, Vector3.UnitY));
        Up = Vector3.Normalize(Vector3.Cross(Right, Front));
    }
}
