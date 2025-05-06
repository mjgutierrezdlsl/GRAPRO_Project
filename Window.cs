using System.Diagnostics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

public class Window : GameWindow
{

    float[] vertices = {
    -0.5f, -0.5f, -0.5f,  0.0f, 0.0f,
     0.5f, -0.5f, -0.5f,  1.0f, 0.0f,
     0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
     0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
    -0.5f,  0.5f, -0.5f,  0.0f, 1.0f,
    -0.5f, -0.5f, -0.5f,  0.0f, 0.0f,

    -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
     0.5f, -0.5f,  0.5f,  1.0f, 0.0f,
     0.5f,  0.5f,  0.5f,  1.0f, 1.0f,
     0.5f,  0.5f,  0.5f,  1.0f, 1.0f,
    -0.5f,  0.5f,  0.5f,  0.0f, 1.0f,
    -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,

    -0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
    -0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
    -0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
    -0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
    -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
    -0.5f,  0.5f,  0.5f,  1.0f, 0.0f,

     0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
     0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
     0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
     0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
     0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
     0.5f,  0.5f,  0.5f,  1.0f, 0.0f,

    -0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
     0.5f, -0.5f, -0.5f,  1.0f, 1.0f,
     0.5f, -0.5f,  0.5f,  1.0f, 0.0f,
     0.5f, -0.5f,  0.5f,  1.0f, 0.0f,
    -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
    -0.5f, -0.5f, -0.5f,  0.0f, 1.0f,

    -0.5f,  0.5f, -0.5f,  0.0f, 1.0f,
     0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
     0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
     0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
    -0.5f,  0.5f,  0.5f,  0.0f, 0.0f,
    -0.5f,  0.5f, -0.5f,  0.0f, 1.0f
    };


    int VertexBufferObject;
    int VertexArrayObject;

    Shader shader;
    Texture texture0;
    Texture texture1;

    Camera camera;

    Stopwatch timer;

    public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings)
    {
    }

    protected override void OnLoad()
    {
        base.OnLoad();

        GL.ClearColor(0.2f, 0.3f, 0.3f, 1f);

        GL.Enable(EnableCap.DepthTest);

        VertexBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
        GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

        VertexArrayObject = GL.GenVertexArray();
        GL.BindVertexArray(VertexArrayObject);

        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);
        GL.EnableVertexAttribArray(0);

        GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));
        GL.EnableVertexAttribArray(1);

        shader = new Shader("Shaders/shader.vert", "Shaders/shader.frag");

        shader.Use();

        shader.SetInt("texture0", 0);
        shader.SetInt("texture1", 1);

        texture0 = Texture.LoadFromFile("Textures/container.jpg");
        texture1 = Texture.LoadFromFile("Textures/awesomeface.png");

        timer = new();
        timer.Start();

        camera = new(new Vector3(0f, 0f, 3f), Size.X / (float)Size.Y, grabCursor: true);
        CursorState = camera.DoGrabCursor ? CursorState.Grabbed : CursorState.Normal;
    }

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);

        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        texture0.Use(TextureUnit.Texture0);
        texture1.Use(TextureUnit.Texture1);
        shader.Use();

        GL.BindVertexArray(VertexArrayObject);

        shader.SetMatrix4("view", camera.GetViewMatrix());
        shader.SetMatrix4("projection", camera.GetProjectionMatrix());

        DrawCube(Vector3.Zero, Vector3.UnitY, (float)timer.Elapsed.TotalSeconds * 4.0f);

        SwapBuffers();
    }

    private void DrawCube(Vector3 position, Vector3 axis, float angleInDegrees, float scale = 1f)
    {
        var model = Matrix4.Identity;
        model *= Matrix4.CreateScale(scale);
        model *= Matrix4.CreateFromAxisAngle(axis, MathHelper.DegreesToRadians(angleInDegrees));
        model *= Matrix4.CreateTranslation(position);

        shader.SetMatrix4("model", model);

        GL.DrawArrays(PrimitiveType.Triangles, 0, vertices.Length);
    }

    protected override void OnFramebufferResize(FramebufferResizeEventArgs e)
    {
        base.OnFramebufferResize(e);

        GL.Viewport(0, 0, e.Width, e.Height);
        camera.SetAspectRatio((float)e.Width, (float)e.Height);
    }

    protected override void OnUpdateFrame(FrameEventArgs args)
    {
        base.OnUpdateFrame(args);
        camera.ProcessInputs(KeyboardState, MouseState, args);
        if (KeyboardState.IsKeyDown(Keys.Escape))
        {
            Close();
        }
    }
    protected override void OnUnload()
    {
        shader.Dispose();
        base.OnUnload();
    }
}
