using System.Diagnostics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

public class Window : GameWindow
{
    // Tri-force vertices
    float[] _vertices = {
        -1.0f,-1.0f,0.0f,//BL
         1.0f,-1.0f,0.0f,//BR
        -1.0f, 1.0f,0.0f,//TL
         1.0f, 1.0f,0.0f,//TR
    };

    uint[] _indices = {
        0, 1, 2, // first triangle
        1, 3, 2, // second triangle
    };

    int VertexBufferObject;
    int VertexArrayObject;
    int ElementBufferObject;

    Shader shader;
    int resolutionLocation;
    int timeLocation;

    Stopwatch timer;

    public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings)
    {
    }

    protected override void OnLoad()
    {
        base.OnLoad();

        GL.ClearColor(0.4f, 0.2f, 0.6f, 1);

        VertexBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
        GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);

        VertexArrayObject = GL.GenVertexArray();
        GL.BindVertexArray(VertexArrayObject);
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);

        GL.EnableVertexAttribArray(0);

        ElementBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferObject);
        GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices, BufferUsageHint.StaticDraw);

        shader = new Shader("Shaders/shader.vert", "Shaders/shader.frag");

        resolutionLocation = GL.GetUniformLocation(shader.Handle, "iResolution");
        timeLocation = GL.GetUniformLocation(shader.Handle, "iTime");

        timer = new Stopwatch();
        timer.Start();
    }

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);

        GL.Clear(ClearBufferMask.ColorBufferBit);

        shader.Use();

        GL.Uniform2(resolutionLocation, (float)Size.X, (float)Size.Y);
        GL.Uniform1(timeLocation, (float)timer.Elapsed.TotalSeconds);

        GL.BindVertexArray(VertexArrayObject);
        GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);

        SwapBuffers();
    }

    protected override void OnResize(ResizeEventArgs e)
    {
        base.OnResize(e);

        GL.Viewport(0, 0, e.Width, e.Height);
    }

    protected override void OnUpdateFrame(FrameEventArgs args)
    {
        base.OnUpdateFrame(args);
        if (KeyboardState.IsKeyPressed(Keys.Escape))
        {
            Close();
        }
    }
}
