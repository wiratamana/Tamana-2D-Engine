using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Input;

using System.Runtime.InteropServices;

namespace TamanaEngine
{
    public class Controller : Component
    {
        private Transform cameraTransform;
        private float time;

        SpriteRenderer spriteRenderer;

        Text text;
        Text text2;
        Text text3;

        Sprite sprite;

        IntPtr tes;
        byte[] source = { 1, 1, 1, 1, 2, 2, 2, 2, 3, 3, 3, 3, 4, 4, 4, 4 };
        byte[] dest;

        private void Awake()
        {
            text2 = new GameObject().AddComponent<Text>();
            text3 = new GameObject().AddComponent<Text>();
            var tex2DW = new Texture2D(100, 100);
            var tex2DB = new Texture2D(100, 100);
            for (int x = 0; x < 100; x++)
                for (int y = 0; y < 100; y++)
                {
                    tex2DW.SetPixel(x, y, System.Drawing.Color.Teal);
                    tex2DB.SetPixel(x, y, System.Drawing.Color.BlueViolet);

                }
            tex2DW.Apply();
            tex2DB.Apply();

            sprite = new Sprite(tex2DB);
        }

        private void Start()
        {
            cameraTransform = GameObject.FindObjectOfType<Camera>().transform;
            time = 20;

            spriteRenderer = GameObject.FindObjectOfType<SpriteRenderer>();
            spriteRenderer.sprite = sprite;

            text = GameObject.FindObjectOfType<Text>();
            text.transform.position = new OpenTK.Vector3(500, 260, 0);
            text2.transform.position = text.transform.position - new OpenTK.Vector3(0, 50, 0);
            text3.transform.position = text2.transform.position - new OpenTK.Vector3(0, 50, 0);

            dest = new byte[source.Length];

            tes = Marshal.AllocHGlobal(source.Length);
            
            Marshal.Copy(source, 0, tes, source.Length);
            Marshal.Copy(tes, dest, 0, source.Length);
            Console.WriteLine(dest[5]);
            Marshal.FreeHGlobal(tes);
            Console.WriteLine(dest[5]);
        }

        private void Update()
        {
            time += Time.deltaTime;
            spriteRenderer.transform.position = new OpenTK.Vector3(time, time, 0);

            text.text = "X : " + (int)spriteRenderer.transform.position.X + " | Y : " + (int)spriteRenderer.transform.position.Y;
            text2.text = "X : " + (int)spriteRenderer.size.X + " | Y : " + (int)spriteRenderer.size.Y;
            text3.text = "X : " + Input.mousePosition.X + " | Y : " + Input.mousePosition.Y;
            if (Input.GetKey(Key.Left))
            {
                time += Time.deltaTime;

                //GameObject.FindObjectOfType<SpriteRenderer>().transform.rotation = OpenTK.Quaternion.FromEulerAngles(new OpenTK.Vector3(0, 0,
                //    OpenTK.MathHelper.DegreesToRadians(time)));

                //Texture2D wira = new Texture2D(100, 100);
                //for(int i = 0; i < 100; i ++)
                //    for(int j = 0; j < 100; j++)
                //    {
                //        wira.SetPixel(i, j, System.Drawing.Color.Aqua);
                //    }
                //
                //wira.Apply();
                //
                //spriteRenderer.sprite = new Sprite(wira);

                spriteRenderer.size = new OpenTK.Vector2(200, 200);
                
               
                //GameObject.FindObjectOfType<SpriteRenderer>().sprite = new Sprite(wira);
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
