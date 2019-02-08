using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Input;

namespace TamanaEngine
{
    public class Controller : Component
    {
        private Transform cameraTransform;
        private float time;

        SpriteRenderer spriteRenderer;
    
        private void Start()
        {
            cameraTransform = GameObject.FindObjectOfType<Camera>().transform;
            spriteRenderer = GameObject.FindObjectOfType<SpriteRenderer>();
        }

        private void Update()
        {
            if (Input.GetKey(Key.Left))
            {
                time += 90 * Time.deltaTime;

                spriteRenderer.transform.rotation = OpenTK.Quaternion.FromEulerAngles(new OpenTK.Vector3(0, 0,
                    OpenTK.MathHelper.DegreesToRadians(time)));

                Texture2D wira = new Texture2D(100, 100);
                for(int i = 0; i < 100; i ++)
                    for(int j = 0; j < 100; j++)
                    {
                        wira.SetPixel(i, j, System.Drawing.Color.Aqua);
                    }

                wira.Apply();

                spriteRenderer.sprite = new Sprite(wira);
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
