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

        Text text;
        Image image;

        float xMax;
        float xMin;
    
        private void Start()
        {
            cameraTransform = GameObject.FindObjectOfType<Camera>().transform;
            spriteRenderer = GameObject.FindObjectOfType<SpriteRenderer>();
            var a = new GameObject("GANTENG");
            text = a.AddComponent<Text>();
            text.color = System.Drawing.Color.Aqua;

            text.transform.position = new OpenTK.Vector3(0, 300, 0);
            text.text = "WIRA GANTENG";
            image = text.AddComponent<Image>();
            image.raycastTarget = true;

            xMax = spriteRenderer.size.X / 1;
            xMin = spriteRenderer.size.X / -1;

            spriteRenderer.size = new OpenTK.Vector2(100, 100);

            System.Drawing.Rectangle rect = new System.Drawing.Rectangle(0, 0, 100, 100);
        }

        private void Update()
        {
            if (Input.GetKey(Key.Left))
                cameraTransform.position -= new OpenTK.Vector3(-100 * Time.deltaTime, 0, 0);

            if (Input.GetKey(Key.Space))
                text.color = System.Drawing.Color.Orange;

            if (image.isMouseOverlap)
                image.color = System.Drawing.Color.Aqua;
            else image.color = System.Drawing.Color.Firebrick;
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
