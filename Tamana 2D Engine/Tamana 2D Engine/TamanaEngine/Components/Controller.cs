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

        float xMax;
        float xMin;
    
        private void Start()
        {
            cameraTransform = GameObject.FindObjectOfType<Camera>().transform;
            spriteRenderer = GameObject.FindObjectOfType<SpriteRenderer>();
            var a = new GameObject("GANTENG");
            text = a.AddComponent<Text>();
            text.color = System.Drawing.Color.Aqua;

            text.transform.position = new OpenTK.Vector3(0, 300, 50);
            text.text = "WIRA GANTENG";

            xMax = spriteRenderer.size.X / 1;
            xMin = spriteRenderer.size.X / -1;

            System.Drawing.Rectangle rect = new System.Drawing.Rectangle(0, 0, 100, 100);
        }

        private void Update()
        {
            if (Input.GetKey(Key.Left))
                cameraTransform.position -= new OpenTK.Vector3(-100 * Time.deltaTime, 0, 0);

            if (Input.GetKey(Key.Space))
                text.color = System.Drawing.Color.Orange;
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
