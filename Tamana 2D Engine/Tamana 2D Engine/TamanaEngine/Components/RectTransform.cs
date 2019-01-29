using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;

namespace TamanaEngine
{
    public class RectTransform : Transform
    {
        private Vector2 _sizeDelta;
        public Vector2 sizeDelta
        {
            get { return _sizeDelta; }
            set
            {
                _sizeDelta = value;
            }
        }

        private Vector3 _position;
        public new Vector3 position
        {
            get { return _position; }
            set
            {
                _position = value;

                model = Matrix4.Identity;
                model *= Matrix4.CreateTranslation(value);
                model *= Matrix4.CreateFromQuaternion(_rotation);
                model *= Matrix4.CreateScale(scale);
            }
        }

        private Vector3 _localPosition;
        public new Vector3 localPosition
        {
            get { return _localPosition; }
            set
            {

            }
        }

        private Quaternion _rotation;
        public new Quaternion rotation
        {
            get { return _rotation; }
            set
            {
                _rotation = value;

                model = Matrix4.Identity;
                model *= Matrix4.CreateTranslation(position);
                model *= Matrix4.CreateFromQuaternion(value);
                model *= Matrix4.CreateScale(scale);
            }
        }

        private Vector3 _scale;
        public new Vector3 scale
        {
            get { return _scale; }
            set
            {
                model = Matrix4.Identity;
                model *= Matrix4.CreateTranslation(position);
                model *= Matrix4.CreateFromQuaternion(rotation);
                model *= Matrix4.CreateScale(value);
            }
        }

        private Matrix4 model;
        private Matrix4 GetModelMatrix()
        {
            return model;
        }

        private void Awake()
        {
            position = Vector3.Zero;
            rotation = Quaternion.Identity;
            scale = Vector3.One;
        }

        protected override void DestroyComponent()
        {
            throw new NotImplementedException();
        }
    }
}
