using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TamanaEngine
{
    public abstract class Component
    {
        protected GameObject _gameObject;
        public GameObject gameObject { get { return _gameObject; } }
        public string name { get { return gameObject.name; } } 
    }
}
