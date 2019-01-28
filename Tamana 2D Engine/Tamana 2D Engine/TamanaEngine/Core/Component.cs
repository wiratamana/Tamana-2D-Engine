using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TamanaEngine
{
    public abstract class Component
    {
        private GameObject _gameObject;
        public GameObject gameObject { get { return _gameObject; } }

        public Transform transform { get { return gameObject.transform; } }

        public string name { get { return gameObject.name; } }

        private Core.Start start;

        private bool _isEnabled;
        public bool isEnabled
        {
            get { return _isEnabled; }
            set {
                if(start == null)
                {
                    GetStartMethod();
                }
                if(value)
                    start?.Invoke();

                _isEnabled = value;
            }
        }

        private void GetStartMethod()
        {
            var componentList = gameObject.GetType().GetField("components", System.Reflection.BindingFlags.Instance |
                System.Reflection.BindingFlags.NonPublic).GetValue(gameObject) as List<Core.ComponentMethodCaller>;

            var myComponent = componentList.Find(x => x.component == this);
            if (myComponent != null)
                start = myComponent.start;
        }

        protected abstract void DestroyComponent();

        public T AddComponent<T>() where T : Component, new()
        {
            return gameObject.AddComponent<T>();
        }

        public T GetComponent<T>() where T : Component
        {
            return gameObject.GetComponent<T>();
        }
    }
}
