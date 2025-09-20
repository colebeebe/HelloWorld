using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Runtime.InteropServices;

namespace HelloTriangle;

internal class Window : GameWindow
{

    private readonly float[] _vertices = [
         0.0f,  0.5f, 0.0f,
        -0.5f, -0.5f, 0.0f,
         0.5f, -0.5f, 0.0f,
    ];

    private int _width, _height;

    private int _vao;
    private int _vbo;
    private int _shader;

    public Window(GameWindowSettings gws, NativeWindowSettings nws) : base(gws, nws)
    {
        _width = ClientSize.X;
        _height = ClientSize.Y;

        if (!RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            GL.Viewport(0, 0, _width, _height);
        }
    }

    protected override void OnLoad()
    {
        base.OnLoad();

        GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

        _vao = GL.GenVertexArray();
        GL.BindVertexArray(_vao);

        _vbo = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
        GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);

        var vertexShaderSource = "#version 330 core\n" +
                                 "layout (location = 0) in vec3 aPos;\n" +
                                 "void main() {\n" +
                                 "    gl_Position = vec4(aPos, 1.0f);\n" +
                                 "}";

        var vertexShader = GL.CreateShader(ShaderType.VertexShader);
        GL.ShaderSource(vertexShader, vertexShaderSource);
        GL.CompileShader(vertexShader);

        var fragmentShaderSource = "#version 330 core\n" +
                                   "out vec4 FragColor;\n" +
                                   "void main() {\n" +
                                   "    FragColor = vec4(1.0f, 0.5f, 0.2f, 1.0f);\n" +
                                   "}";

        var fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
        GL.ShaderSource(fragmentShader, fragmentShaderSource);
        GL.CompileShader(fragmentShader);

        _shader = GL.CreateProgram();
        GL.AttachShader(_shader, vertexShader);
        GL.AttachShader(_shader, fragmentShader);
        GL.LinkProgram(_shader);

        GL.DetachShader(_shader, vertexShader);
        GL.DetachShader(_shader, fragmentShader);
        GL.DeleteShader(vertexShader);
        GL.DeleteShader(fragmentShader);

        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
        GL.EnableVertexAttribArray(0);

    }

    protected override void OnUpdateFrame(FrameEventArgs args)
    {
        base.OnUpdateFrame(args);

        if (KeyboardState.IsKeyPressed(Keys.Escape))
        {
            Close();
        }
    }

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        GL.Clear(ClearBufferMask.ColorBufferBit);

        GL.UseProgram(_shader);
        GL.BindVertexArray(_vao);
        GL.DrawArrays(PrimitiveType.Triangles, 0, _vertices.Length);

        SwapBuffers();
        base.OnRenderFrame(args);
    }

    protected override void OnResize(ResizeEventArgs e)
    {
        base.OnResize(e);

        _width = e.Width;
        _height = e.Height;

        if (!RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            GL.Viewport(0, 0, _width, _height);
        }
    }

    protected override void OnUnload()
    {
        base.OnUnload();

        GL.DeleteVertexArray(_vao);
        GL.DeleteBuffer(_vbo);
        GL.DeleteProgram(_shader);
    }

}

