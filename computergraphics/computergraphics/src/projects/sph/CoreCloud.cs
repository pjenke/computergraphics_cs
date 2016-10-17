using System.Collections.Generic;
using OpenTK;

namespace computergraphics
{

    class CoreCloud
    {

        private List<Core> coreList = new List<Core>();
        private Vector3 gravity = new Vector3(0, 0, 0);
        private float h = 0;

        public CoreCloud(Vector3 g)
        {
            gravity = g;
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
