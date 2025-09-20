using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Mathematics;

namespace HelloTriangle;

internal abstract class Program
{
    public static void Main(string[] args)
    {
        var gws = GameWindowSettings.Default;
        var nws = new NativeWindowSettings
        {
            Flags = ContextFlags.ForwardCompatible,
            Title = "Hello Triangle",
            ClientSize = new Vector2i(1200, 800)
        };

        var window = new Window(gws, nws);
        window.Run();

        Console.WriteLine("Hello World!");
    }
}