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
        private static List<GameObjectAndComponents> gameObjects { get; set; } =  new List<GameObjectAndComponents>();

        public static void InvokeUpdateMethods()
        {
             foreach (var gameObject in gameObjects)
                gameObject.InvokeUpdate();
        }

        public static void InvokeRenderMethods()
        {
            foreach (var gameObject in gameObjects)
                gameObject.InvokeRender();

            foreach (var gameoObject in gameObjects)
                gameoObject.InvokeRenderUI();
        }

        public static void AddGameObject(GameObjectAndComponents gameObjectAndComponents)
        {
            gameObjects.Add(gameObjectAndComponents);
        }

        public static T FindObjectOfType<T>() where T : Component
        {
            foreach (var go in gameObjects)
            {
                var component = go.FindComponent<T>();
                if (component != null)
                    return component;
            }

            return null;
        }
    }
}
