using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TamanaEngine.Core
{
    delegate void Awake();
    delegate void Start();
    delegate void Update();
    delegate void Render();

    public static class RuntimeUpdater
    {
        public static List<GameObject> gameObjects { get; private set; } =  new List<GameObject>();
        private static List<Update> updateMethods = new List<Update>();
        private static List<Render> renderMethods = new List<Render>();

        public static void AddUpdateMethod(Delegate updateMethod)
        {
            updateMethods.Add((Update)updateMethod);
        }
        public static void AddRenderMethod(Delegate renderMethod)
        {
            renderMethods.Add((Render)renderMethod);
        }

        public static void InvokeUpdateMethods()
        {
            foreach (var update in updateMethods)
                update.Invoke();
        }

        public static void InvokeRenderMethods()
        {
            foreach (var render in renderMethods)
                render.Invoke();
        }

    }
}
