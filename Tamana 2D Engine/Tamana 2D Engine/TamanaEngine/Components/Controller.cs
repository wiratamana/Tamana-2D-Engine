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

        private void Start()
        {
            cameraTransform = GameObject.FindObjectOfType<Camera>().transform;        
        }

        private void Update()
        {
            CameraController();

            time += 90 * Time.deltaTime;

            GameObject.FindObjectOfType<SpriteRenderer>().transform.rotation = OpenTK.Quaternion.FromEulerAngles(new OpenTK.Vector3(0, 0,
                OpenTK.MathHelper.DegreesToRadians(time)));
        }

        private void CameraController()
        {
            var key = Keyboard.GetState();

            if (key.IsKeyDown(Key.Left))
                cameraTransform.position += new OpenTK.Vector3(50f * Time.deltaTime,0,0);

            if (key.IsKeyDown(Key.Right))
                cameraTransform.position += new OpenTK.Vector3(-50f * Time.deltaTime,0,0);
        }
    }
}
