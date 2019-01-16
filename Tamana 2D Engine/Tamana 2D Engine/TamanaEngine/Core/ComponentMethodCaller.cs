using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TamanaEngine.Core
{
    public class ComponentMethodCaller
    {
        public ComponentMethodCaller(Component componenet, Awake awake, Start start, Update update, Render render)
        {
            this.component = componenet;
            this.awake = awake;
            this.start = start;
            this.update = update;
            this.render = render;

            gameObject = componenet.gameObject;
        }

        public GameObject gameObject { private set; get; }
        public Component component { private set; get; }
        public Awake awake { private set; get; }
        public Start start { private set; get; }
        public Update update { private set; get; }
        public Render render { private set; get; }
    }
}
