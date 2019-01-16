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

        private Transform _transform;
        public Transform transform { get { return _transform; } }

        private List<ComponentMethodCaller> components = new List<ComponentMethodCaller>();
        private bool _isActive;
        public bool isActive { get { return _isActive; } }

        private Core.OnAddedComponent onAddedComponent;

        public GameObject()
        {
            name = "New GameObject";

            AddGameObjectToRuntimeUpdater();
            AddTransformComponent();
            SetActive(true);
        }

        public GameObject(string name)
        {
            this.name = name;

            AddGameObjectToRuntimeUpdater();
            AddTransformComponent();
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
            var newComponent = components.Find(x => x.component is T);
            if(newComponent != null)
            {
                return newComponent.component as T;
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

            onAddedComponent?.Invoke();

            return component as T;
        }

        public T GetComponent<T>() where T : Component
        {
            try
            {
                return components?.Find(x => x.component is T).component as T ?? null;
            }
            catch
            {
                return null;
            }
        }

        public static T FindObjectOfType<T>() where T : Component
        {
            return RuntimeUpdater.FindObjectOfType<T>();
        }

        private void AddGameObjectToRuntimeUpdater()
        {
            var gameObjectAndComponents = new GameObjectAndComponents(ref components, this);
            RuntimeUpdater.AddGameObject(gameObjectAndComponents);

            onAddedComponent = gameObjectAndComponents.GetType().GetMethod("ReorderRender",
                BindingFlags.NonPublic | BindingFlags.Instance).CreateDelegate
                (typeof(OnAddedComponent), gameObjectAndComponents) as OnAddedComponent;

            if (onAddedComponent == null)
                throw new Exception("Failed to add method with reflection.");
        }

        private void AddTransformComponent()
        {
            _transform = AddComponent<Transform>();
        }

        private void InvokeEveryComponentStartMethod()
        {
            foreach (var component in components)
                component.start?.Invoke();
        }

        private void SetComponentFieldGameObject(Component newComponent)
        {
            Type baseType = newComponent.GetType().BaseType;
            if (newComponent is ComponentUI)
                baseType = baseType.BaseType;

            baseType.GetField("_gameObject",
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
