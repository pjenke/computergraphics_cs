using System.Collections.Generic;
using OpenTK;

namespace computergraphics
{

    class CoreCloud
    {

        private List<Core> coreList = new List<Core>();
        private Vector3 gravity = new Vector3(0, 0, 0);
        private float h = 0;

        public CoreCloud(Vector3 g, float h)
        {
            gravity = g;
            this.h = h;
        }

        public Vector3 Gravity
        {
            get
            {
                return gravity;
            }
        }

        public float H
        {
            get
            {
                return h;
            }
        }

        internal List<Core> CoreList
        {
            get
            {
                return coreList;
            }
        }

        public void Add(Core core)
        {
            coreList.Add(core);
        }

    }
}
