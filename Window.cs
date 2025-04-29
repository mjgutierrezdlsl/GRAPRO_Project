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
    Vector3[] cubePositions = {
    ( 0.0f,  0.0f,  0.0f),
    ( 2.0f,  5.0f, -15.0f),
    (-1.5f, -2.2f, -2.5f),
    (-3.8f, -2.0f, -12.3f),
    ( 2.4f, -0.4f, -3.5f),
    (-1.7f,  3.0f, -7.5f),
    ( 1.3f, -2.0f, -2.5f),
    ( 1.5f,  2.0f, -2.5f),
    ( 1.5f,  0.2f, -1.5f),
    (-1.3f,  1.0f, -1.5f)
};
    int VertexBufferObject;
    int VertexArrayObject;

    Shader shader;
    Texture texture0;
    Texture texture1;

    Stopwatch timer;

    public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings)
    {
    }

    protected override void OnLoad()
    {
        base.OnLoad();

        GL.Enable(EnableCap.DepthTest);
        GL.ClearColor(0.2f, 0.3f, 0.3f, 1f);

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
    }

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);

        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        texture0.Use(TextureUnit.Texture0);
        texture1.Use(TextureUnit.Texture1);
        shader.Use();

        GL.BindVertexArray(VertexArrayObject);

        var model = Matrix4.Identity;
        var view = Matrix4.Identity;
        var projection = Matrix4.Identity;

        view = Matrix4.CreateTranslation(0.0f, 0.0f, -10.0f);
        projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45.0f), ClientSize.X / (float)ClientSize.Y, 0.1f, 100f);

        shader.SetMatrix4("view", view);
        shader.SetMatrix4("projection", projection);

        for (int i = 0; i < cubePositions.Length; i++)
        {
            var angle = 20.0f * i;
            if (i % 3 == 0)
            {
                angle = (float)timer.Elapsed.TotalSeconds * 25.0f;
            }
            model = Matrix4.CreateFromAxisAngle(new(1.0f, 0.3f, 0.5f), MathHelper.DegreesToRadians(angle)) * Matrix4.CreateTranslation(cubePositions[i]);
            shader.SetMatrix4("model", model);

            GL.DrawArrays(PrimitiveType.Triangles, 0, vertices.Length);
        }

        SwapBuffers();
    }

    protected override void OnFramebufferResize(FramebufferResizeEventArgs e)
    {
        base.OnFramebufferResize(e);

        GL.Viewport(0, 0, e.Width, e.Height);
    }

    protected override void OnUpdateFrame(FrameEventArgs args)
    {
        base.OnUpdateFrame(args);

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
