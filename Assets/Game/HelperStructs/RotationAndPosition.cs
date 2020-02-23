using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace com.runtime.GameJamBois.BGJ20201.Util
{
    struct RotationAndPosition
    {
        public Quaternion rotation;
        public Vector3 position;

        public RotationAndPosition(Quaternion rotation, Vector3 position)
        {
            this.rotation = rotation;
            this.position = position;
        }
    }
}
