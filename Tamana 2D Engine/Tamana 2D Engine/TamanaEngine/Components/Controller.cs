using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;

using OpenTK.Input;

namespace TamanaEngine
{
    public class Controller : Component
    {
        private Transform cameraTransform;
        private float time;

        SpriteRenderer spriteRenderer;

        Text text;
        Image image;

        float xMax;
        float xMin;

        List<Sprite> sprites = new List<Sprite>();
    
        private void Start()
        {
            cameraTransform = GameObject.FindObjectOfType<Camera>().transform;
            spriteRenderer = GameObject.FindObjectOfType<SpriteRenderer>();
            var a = new GameObject("GANTENG");
            image = a.AddComponent<Image>();
            text = a.AddComponent<Text>();
            text.color = System.Drawing.Color.Wheat;

            text.transform.position = new OpenTK.Vector3(0, 300, 0);
            text.text = "WIRA GANTENG";
            image.raycastTarget = true;
            image.sprite = new Sprite("./res/sprite.png");

            xMax = spriteRenderer.size.X / 1;
            xMin = spriteRenderer.size.X / -1;

            spriteRenderer.size = new OpenTK.Vector2(256, 256);

            System.Drawing.Rectangle rect = new System.Drawing.Rectangle(0, 0, 100, 100);

            sprites.AddRange(Core.Util.GifToSpriteArray("./res/elza.gif"));
            spriteRenderer.transform.position = new OpenTK.Vector3(0, -200, 0);
            spriteRenderer.sprite = sprites[0];
        }

        int cnt = 0;
        private void Update()
        {
            if (Input.GetKey(Key.Left))
                cameraTransform.position -= new OpenTK.Vector3(-100 * Time.deltaTime, 0, 0);

            if (Input.GetKey(Key.Space))
                text.color = System.Drawing.Color.Orange;

            time += Time.deltaTime;
            if(time > 0.25f)
            {
                spriteRenderer.sprite = sprites[cnt];
                cnt++;
                if (cnt == sprites.Count)
                    cnt = 0;
                time = 0;
            }

            if (image.isMouseOverlap)
            {
                image.color = System.Drawing.Color.Aqua;
                image.size = new OpenTK.Vector2(80, 80);
            }
            else
            {
                image.color = System.Drawing.Color.Firebrick;
                image.size = new OpenTK.Vector2(100, 100);
            }
        }

        private void CameraController()
        {
            var key = Keyboard.GetState();

            if (key.IsKeyDown(Key.Left))
                cameraTransform.position += new OpenTK.Vector3(50f * Time.deltaTime,0,0);

            if (key.IsKeyDown(Key.Right))
                cameraTransform.position += new OpenTK.Vector3(-50f * Time.deltaTime,0,0);
        }

        protected override void DestroyComponent()
        {
            throw new NotImplementedException();
        }
    }
}
