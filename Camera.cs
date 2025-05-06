
using OpenTK.Graphics.ES11;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

public class Camera
{
    private float _speed;
    private float _sensitivity;

    private Vector3 _position;
    private float _aspectRatio;
    private float _fieldOfView;
    private float _depthNear;
    private float _depthFar;

    private Vector3 _up = Vector3.UnitY;
    private Vector3 _front = -Vector3.UnitZ;
    private Vector3 _right = Vector3.UnitX;

    public Camera(Vector3 position, float aspectRatio, float speed = 1.5f, float sensitivity = 0.2f, float fieldOfView = 45f, float depthNear = 0.1f, float depthFar = 100f)
    {
        _position = position;
        _aspectRatio = aspectRatio;
        _speed = speed;
        _sensitivity = sensitivity;
        _fieldOfView = fieldOfView;
        _depthNear = depthNear;
        _depthFar = depthFar;
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
    }

    public void SetAspectRatio(float width, float height)
    {
        _aspectRatio = width / height;
    }
}
