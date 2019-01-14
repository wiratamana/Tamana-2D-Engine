using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

using TamanaEngine.Core;

namespace TamanaEngine
{
    public class GameObject
    {
        public string name { get; set; }

        private List<ComponentMethodCaller> components = new List<ComponentMethodCaller>();
        private bool _isActive;
        public bool isActive { get { return _isActive; } }

        public GameObject()
        {
            name = "New GameObject";

            AddGameObjectToRuntimeUpdater();
            SetActive(true);
        }

        public GameObject(string name)
        {
            this.name = name;

            AddGameObjectToRuntimeUpdater();
            SetActive(true);
        }

        public void SetActive(bool value)
        {
            _isActive = value;

            if(value)
            {
                InvokeEveryComponentStartMethod();
            }
        }

        public T AddComponent<T>() where T : Component, new()
        {
            var newComponent = components.Find(x => x.componenet is T);
            if(newComponent != null)
            {
                return newComponent as T;
            }

            // Create new component and add it to components list.
            // ---------------------------------------------------
            var component = new T();
            SetComponentFieldGameObject(component as T);
            component.isEnabled = true;


            // Get 'Awake', 'Start', 'Update', 'Render' methods.
            // -------------------------------------------------
            var awake  = CreateDelegate(component as T, typeof(Awake),  "Awake")  as Awake;
            var start  = CreateDelegate(component as T, typeof(Start),  "Start")  as Start;
            var update = CreateDelegate(component as T, typeof(Update), "Update") as Update;
            var render = CreateDelegate(component as T, typeof(Render), "Render") as Render;

            components.Add(new ComponentMethodCaller(component, awake, start, update, render));
            
            // Invoke 'Awake' methods whenever component was added to GameObject.
            // ------------------------------------------------------------------
            awake?.Invoke();

            if (_isActive)
                start?.Invoke();

            return newComponent as T;
        }

        public T GetComponent<T>() where T : Component
        {
            return components.Find(x => x is T) as T;
        }

        private void AddGameObjectToRuntimeUpdater()
        {
            RuntimeUpdater.gameObjects.Add(new GameObjectAndComponents(ref components, this));
        }

        private void InvokeEveryComponentStartMethod()
        {
            foreach (var component in components)
                component.start.Invoke();
        }

        private void SetComponentFieldGameObject(Component newComponent)
        {
            newComponent.GetType().BaseType.GetField("_gameObject",
                BindingFlags.Instance | BindingFlags.NonPublic).SetValue(newComponent, this);
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
