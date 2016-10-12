using System.Collections.Generic;
using OpenTK;

namespace computergraphics
{

    class CoreCloud
    {

        private List<Core> coreList = new List<Core>();
        private Vector3 pos = new Vector3(0, 0, 0);

        public CoreCloud(Vector3 position)
        {
            pos = position;
        }

        public void Add(Core core)
        {
            coreList.Add(core);
        }

    }
}
