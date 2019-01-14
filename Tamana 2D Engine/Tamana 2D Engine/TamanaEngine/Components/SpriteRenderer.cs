using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TamanaEngine
{
    public class SpriteRenderer : Component
    {
        private Sprite _sprite;
        public Sprite sprite
        {
            get { return _sprite; }
            set
            {
                if (value == null)
                    return;

                _sprite = value;
            }
        }

        private Core.Shader shader;

        public SpriteRenderer()
        {
            shader = new Core.Shader("./res/SpriteRendererVertex.txt", "./res/SpriteRendererFragment.txt");
            _sprite = new Sprite("./res/sprite.png");
        }

        private void Render()
        {
            sprite.BindTexture();

            shader.UseProgram();
            sprite.RenderSprite();
        }
    }
}
