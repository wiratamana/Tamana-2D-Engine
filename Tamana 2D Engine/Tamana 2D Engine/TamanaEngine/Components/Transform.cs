using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;

namespace TamanaEngine.Core
{
    delegate Matrix4 GetModelMatrix();
    delegate void UploadMatrixMVP(Shader shader, Matrix4 matrixMVP);
}

namespace TamanaEngine
{
    public class Transform : Component
    {
        private Vector3 _position;
        public Vector3 position
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
        private Quaternion _rotation;
        public Quaternion rotation
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
        public Vector3 scale
        {
            get { return _scale; }
            set
            {
                _scale = value;

                model = Matrix4.Identity;
                model *= Matrix4.CreateTranslation(position);
                model *= Matrix4.CreateFromQuaternion(rotation);
                model *= Matrix4.CreateScale(value);
            }
        }

        private Vector3 _localPosition;
        public Vector3 localPosition
        {
            get { return _localPosition; }
            set
            {
                _localPosition = value;
            }
        }

        public Vector3 forward { get { return (rotation * new Vector4(0, 0, 1f, 0)).Xyz; } }
        public Vector3 right { get { return (rotation * new Vector4(1f, 0, 0, 0)).Xyz; } }
        public Vector3 up { get { return (rotation * new Vector4(0, 1f, 0, 0)).Xyz; } }

        private Matrix4 model;
        private Matrix4 GetModelMatrix()
        {
            return model;
        }

        private Transform _parent;
        private Transform parent
        {
            get { return _parent; }
            set
            {
                SetParent(value);
            }
        }

        private void Awake()
        {
            position = Vector3.Zero;
            rotation = Quaternion.Identity;
            scale = Vector3.One;
        }

        /// <summary>
        /// Unparents all children.
        /// </summary>
        public void DetachChildren() { throw new NotImplementedException(); }
        public void Find(string n) { throw new NotImplementedException(); }
        public void GetChild(int index) { throw new NotImplementedException(); }
        public int GetSiblingIndex() { { throw new NotImplementedException(); } }
        public bool IsChildOf(Transform parent) { throw new NotImplementedException(); }
        public void SetAsFirstSibling() { throw new NotImplementedException(); }
        public void SetAsLastSibling() { throw new NotImplementedException(); }
        public void SetParent(Transform parent)
        {
            _parent = parent;
        }
        public void SetSiblingIndex(int index) { throw new NotImplementedException(); }

        protected override void DestroyComponent()
        {
            throw new NotImplementedException();
        }
    }
}