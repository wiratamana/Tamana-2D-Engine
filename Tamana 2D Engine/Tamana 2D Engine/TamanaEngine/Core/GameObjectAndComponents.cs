using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TamanaEngine.Core
{
    public class GameObjectAndComponents
    {
        private List<ComponentMethodCaller> components = new List<ComponentMethodCaller>();
        private GameObject gameObject;

        public GameObjectAndComponents(ref List<ComponentMethodCaller> components, GameObject gameObject )
        {
            this.components = components;
            this.gameObject = gameObject;
        }

        public T FindComponent<T>() where T : Component
        {
            return components.Find(x => x.componenet is T).componenet as T;
        }

        public void InvokeUpdate()
        {
            if (!gameObject.isActive)
                return;

            foreach (var component in components)
                if (component.componenet.isEnabled)
                    component.update?.Invoke();
        }

        public void InvokeRender()
        {
            if (!gameObject.isActive)
                return;

            foreach (var component in components)
                if (component.componenet.isEnabled)
                    component.render?.Invoke();
        }
    }
}
