
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

public class Camera
{
    public bool DoGrabCursor;
    // Movement Properties
    private float _speed;
    private Vector3 _position;
    private Vector3 _up = Vector3.UnitY;
    private Vector3 _front = -Vector3.UnitZ;
    private Vector3 _right = Vector3.UnitX;

    // Look Properties
    private float _sensitivity;
    private float _yaw = -90f;
    private float _pitch;
    private Vector2 _lastPos;
    private bool _firstMove = true;

    // Rendering Properties
    private float _aspectRatio;
    private float _fieldOfView;
    private float _depthNear;
    private float _depthFar;

    public Camera(Vector3 position, float aspectRatio, float speed = 1.5f, float sensitivity = 180f, float fieldOfView = 45f, float depthNear = 0.1f, float depthFar = 100f, bool grabCursor = true)
    {
        _position = position;
        _aspectRatio = aspectRatio;
        _speed = speed;
        _sensitivity = sensitivity;
        _fieldOfView = fieldOfView;
        _depthNear = depthNear;
        _depthFar = depthFar;
        DoGrabCursor = grabCursor;
    }

    public Matrix4 GetViewMatrix()
    {
        return Matrix4.LookAt(_position, _position + _front, _up);
    }

    public Matrix4 GetProjectionMatrix()
    {
        return Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(_fieldOfView), _aspectRatio, _depthNear, _depthFar);
    }

    public void ProcessInputs(KeyboardState input, MouseState mouse, FrameEventArgs e)
    {
        var deltaTime = (float)e.Time;

        if (input.IsKeyDown(Keys.W))
        {
            _position += _front * _speed * deltaTime;
        }
        if (input.IsKeyDown(Keys.S))
        {
            _position -= _front * _speed * deltaTime;
        }
        if (input.IsKeyDown(Keys.A))
        {
            _position -= Vector3.Normalize(Vector3.Cross(_front, _up)) * _speed * deltaTime;
        }
        if (input.IsKeyDown(Keys.D))
        {
            _position += Vector3.Normalize(Vector3.Cross(_front, _up)) * _speed * deltaTime;
        }
        if (input.IsKeyDown(Keys.Space))
        {
            _position.Y += _speed * deltaTime;
        }
        if (input.IsKeyDown(Keys.LeftShift))
        {
            _position.Y -= _speed * deltaTime;
        }

        if (_firstMove)
        {
            _lastPos = new Vector2(mouse.X, mouse.Y);
            _firstMove = false;
        }
        else
        {
            var deltaX = mouse.X - _lastPos.X;
            var deltaY = mouse.Y - _lastPos.Y;
            _lastPos = new Vector2(mouse.X, mouse.Y);

            _yaw += deltaX * _sensitivity * deltaTime;
            _pitch -= deltaY * _sensitivity * deltaTime;
        }

        UpdateVectors();
    }

    private void UpdateVectors()
    {
        if (_pitch > 89.0f)
        {
            _pitch = 89.0f;
        }
        if (_pitch < -89.0f)
        {
            _pitch = -89.0f;
        }

        _front.X = MathF.Cos(MathHelper.DegreesToRadians(_pitch)) * MathF.Cos(MathHelper.DegreesToRadians(_yaw));
        _front.Y = MathF.Sin(MathHelper.DegreesToRadians(_pitch));
        _front.Z = MathF.Cos(MathHelper.DegreesToRadians(_pitch)) * MathF.Sin(MathHelper.DegreesToRadians(_yaw));

        _front = Vector3.Normalize(_front);

        _right = Vector3.Normalize(Vector3.Cross(_front, Vector3.UnitY));
        _up = Vector3.Normalize(Vector3.Cross(_right, _front));
    }

    public void SetAspectRatio(float width, float height)
    {
        _aspectRatio = width / height;
    }
}
