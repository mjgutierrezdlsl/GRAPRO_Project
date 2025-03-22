using System.Diagnostics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

public class Window : GameWindow
{

    float[] vertices = {
    //Position          Texture coordinates
     1.0f,  1.0f, 0.0f, 1.0f, 1.0f, // top right
     1.0f, -1.0f, 0.0f, 1.0f, 0.0f, // bottom right
    -1.0f, -1.0f, 0.0f, 0.0f, 0.0f, // bottom left
    -1.0f,  1.0f, 0.0f, 0.0f, 1.0f  // top left
    };

    uint[] indices = {
        0,1,3, //first triangle
        1,2,3  // second triangle
     };

    int VertexBufferObject;
    int ElementBufferObject;
    int VertexArrayObject;

    Shader shader;
    Texture texture0;
    Texture texture1;
    Texture noiseTexture;

    Stopwatch _timer;
    int _timerLocation;

    public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings)
    {
    }

    protected override void OnLoad()
    {
        base.OnLoad();

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

        ElementBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferObject);
        GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);

        shader = new Shader("Shaders/shader.vert", "Shaders/shader.frag");

        shader.Use();

        shader.SetInt("texture0", 0);
        shader.SetInt("texture1", 1);
        shader.SetInt("noiseTexture", 2);

        texture0 = Texture.LoadFromFile("Textures/container.jpg");
        texture1 = Texture.LoadFromFile("Textures/awesomeface.png");
        noiseTexture = Texture.LoadFromFile("Textures/noise_tiled.png");

        _timer = new Stopwatch();
        _timer.Start();

        _timerLocation = GL.GetUniformLocation(shader.Handle, "iTime");
    }

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);

        GL.Clear(ClearBufferMask.ColorBufferBit);
        GL.Uniform1(_timerLocation, (float)_timer.Elapsed.TotalSeconds);
        texture0.Use(TextureUnit.Texture0);
        texture1.Use(TextureUnit.Texture1);
        noiseTexture.Use(TextureUnit.Texture2);
        shader.Use();

        GL.BindVertexArray(VertexArrayObject);

        GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);

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
