using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TamanaEngine.Core
{
    delegate void OnAddedComponent();

    public class GameObjectAndComponents
    {
        private List<ComponentMethodCaller> components = new List<ComponentMethodCaller>();

        private List<int> renderIndices = new List<int>();
        private List<int> renderIndicesUI = new List<int>();

        private GameObject gameObject;

        public GameObjectAndComponents(ref List<ComponentMethodCaller> components, GameObject gameObject )
        {
            this.components = components;
            this.gameObject = gameObject;
        }

        public T FindComponent<T>() where T : Component
        {
            try
            {
                return components?.Find(x => x.component is T).component as T ?? null;
            } catch
            {
                return null;
            }
        }

        public void InvokeUpdate()
        {
            if (!gameObject.isActive)
                return;

            foreach (var component in components)
                if (component.component.isEnabled)
                    component.update?.Invoke();
        }

        public void InvokeRender()
        {
            if (!gameObject.isActive)
                return;

            foreach (var index in renderIndices)
                if (components[index].component.isEnabled)
                    components[index].render?.Invoke();
        }

        public void InvokeRenderUI()
        {
            if (!gameObject.isActive)
                return;

            foreach (var index in renderIndicesUI)
                if (components[index].component.isEnabled)
                    components[index].render?.Invoke();
        }

        private void ReorderRender()
        {
            renderIndices.Clear();
            renderIndicesUI.Clear();

            for (int i = 0; i < components.Count; i++)
                if (components[i].component is ComponentUI)
                    renderIndicesUI.Add(i);
                else renderIndices.Add(i);
        }
    }
}
