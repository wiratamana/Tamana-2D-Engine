﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TamanaEngine
{
    public abstract class ComponentUI : Component
    {
        private RectTransform _rectTransform;
        public RectTransform rectTransform
        {
            get { return _rectTransform; }
            set
            {
                _rectTransform = value;
            }
        }


    }
}