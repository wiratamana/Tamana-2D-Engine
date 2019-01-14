using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace TamanaEngine.Core
{
    class MainWindow : GameWindow
    {
        public MainWindow()
            : base(Screen.width, // initial width
        Screen.height, // initial height
        GraphicsMode.Default,
        "Tamana Engine",  // initial title
        GameWindowFlags.Default,
        DisplayDevice.Default,
        4, // OpenGL major version
        0, // OpenGL minor version
        GraphicsContextFlags.ForwardCompatible)
        {
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);


            var mainCamera = new GameObject("MainCamera");
            mainCamera.AddComponent<Camera>();

            var go = new GameObject("Wira");
            go.AddComponent<Transform>();
            Console.WriteLine(go.GetComponent<Transform>().gameObject.GetType().Name);
            go.AddComponent<SpriteRenderer>();
            Console.WriteLine(go.GetComponent<SpriteRenderer>().name);
            go.transform.position = new Vector3(0, 0, 5);
        }

        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(0, 0, Width, Height);
        }

        protected override void OnLoad(EventArgs e)
        {
            CursorVisible = true;
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            RuntimeUpdater.InvokeUpdateMethods();
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            Title = $"(Vsync: {VSync}) FPS: {1f / e.Time:0}";

            Color4 backColor;
            backColor.A = 1.0f;
            backColor.R = 0.1f;
            backColor.G = 0.1f;
            backColor.B = 0.3f;
            GL.ClearColor(backColor);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            RuntimeUpdater.InvokeRenderMethods();

            SwapBuffers();
        }
    }
}
