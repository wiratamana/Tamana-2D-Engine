using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TamanaEngine.Core
{
    public delegate void Awake();
    public delegate void Start();
    public delegate void Update();
    public delegate void Render();

    public static class RuntimeUpdater
    {
        public static List<GameObjectAndComponents> gameObjects { get; private set; } =  new List<GameObjectAndComponents>();

        public static void InvokeUpdateMethods()
        {
             foreach (var gameObject in gameObjects)
                gameObject.InvokeUpdate();
        }

        public static void InvokeRenderMethods()
        {
            foreach (var gameObject in gameObjects)
                gameObject.InvokeRender();
        }

    }
}
