using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

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
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            
            Mouse.SetPosition(1920 / 2, 1080/2);
            Console.WriteLine("Screen resolution : Height = {0} | Width = {1}", Height, Width);
            
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            FileManager.CopyShadersToGameDirectory();
            FileManager.Write();
            TimeMethodCaller.GetSetDeltaTimeDelegate();
            DefaultFont.InitFont();

            var mainCamera = new GameObject("MainCamera");
            mainCamera.AddComponent<Camera>();
            mainCamera.transform.position = new Vector3(0, 0, 0);

            var go = new GameObject("Wira");
            
            go.AddComponent<Text>().text = "Wiratamana";            
            go.transform.position = new Vector3(0, 0, 50);
            go.GetComponent<Text>().color = System.Drawing.Color.HotPink;

            var image = new GameObject("image");
            image.AddComponent<SpriteRenderer>();
            image.transform.position = new Vector3(0, 0, 50);

            sw.Stop();
            Console.WriteLine("Load time : " + sw.ElapsedMilliseconds + "ms.");
            

            var controller = new GameObject("Controller");
            controller.AddComponent<Controller>();
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
            TimeMethodCaller.InvokeSetDeltaTime((float)UpdatePeriod);

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
