using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace TamanaEngine
{
    public class GameObject
    {
        public string name { get; set; }

        private bool isActive;
        private List<Component> components = new List<Component>();
        private Core.Awake awake;
        private Core.Start start;

        public GameObject()
        {
            name = "New GameObject";

            AddGameObjectToRuntimeUpdater();
        }

        public GameObject(string name)
        {
            this.name = name;

            AddGameObjectToRuntimeUpdater();
        }

        public void SetActive(bool value)
        {
            isActive = value;

            if(value)
            {
                start?.Invoke();
            }
        }

        public T AddComponent<T>() where T : Component, new()
        {
            var newComponent = components.Find(x => x is T);
            if(newComponent != null)
            {
                return newComponent as T;
            }

            // Create new component and add it to components list.
            // ---------------------------------------------------
            newComponent = new T();
            components.Add(newComponent);

            SetComponentFieldGameObject(newComponent as T);
            AddUpdateAndRenderMethod(newComponent);

            // Get 'Awake', 'Start', 'Update', 'Render' methods.
            // -------------------------------------------------
            awake = (Core.Awake)CreateDelegate(newComponent, typeof(Core.Awake), "Awake");
            start = (Core.Start)CreateDelegate(newComponent, typeof(Core.Start), "Start");
            
            // Invoke 'Awake' methods whenever component was added to GameObject.
            // ------------------------------------------------------------------
            awake?.Invoke();

            return newComponent as T;
        }

        public T GetComponent<T>() where T : Component
        {
            return components.Find(x => x is T) as T;
        }

        private void AddGameObjectToRuntimeUpdater()
        {
            Core.RuntimeUpdater.gameObjects.Add(this);
        }

        private void AddUpdateAndRenderMethod(Component newComponent)
        {
            var update = CreateDelegate(newComponent, typeof(Core.Update), "Update");
            if (update != null) Core.RuntimeUpdater.AddUpdateMethod(update);

            var render = CreateDelegate(newComponent, typeof(Core.Render), "Render");
            if (render != null) Core.RuntimeUpdater.AddRenderMethod(render);
        }

        private void SetComponentFieldGameObject(Component newComponent)
        {
            newComponent.GetType().GetField("_gameObject",
                BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy).SetValue(newComponent, this);
        }

        private Delegate CreateDelegate(Component componentInstance, Type delegateType, string methodName)
        {
            var methodInfo = componentInstance.GetType().GetMethod(methodName, BindingFlags.Instance
                | BindingFlags.NonPublic);

            if (methodInfo == null)
                return null;

            return methodInfo.CreateDelegate(delegateType, componentInstance);
        }

    }
}
