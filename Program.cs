using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;

var nativeWindowSettings = new NativeWindowSettings()
{
    ClientSize = new Vector2i(800, 800),
    Title = "GRAPRO_Test"
};

using (Window window = new Window(GameWindowSettings.Default, nativeWindowSettings))
{
    window.Run();
}