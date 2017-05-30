using OpenTK;
using System;
using System.Collections.Generic;

namespace computergraphics.src.projects.sph
{
    public class LoadFile
    {

        private System.IO.StreamReader _file;

        private List<Core> _coreList = new List<Core>();

        private RootNode _root;

        private int _coresCount;

        public LoadFile(String file, RootNode root)
        {
            _root = root;
            _file = System.IO.File.OpenText(file);
            _coresCount = int.Parse(_file.ReadLine());
            CreateCores();
        }

        private void CreateCores()
        {
            for(int i = 0; i < _coresCount; i++)
            {
                Vector3 pos = new Vector3();
                pos.X = float.Parse(_file.ReadLine());
                pos.Y = float.Parse(_file.ReadLine());
                pos.Z = float.Parse(_file.ReadLine());
                Core c = new Core(pos, 0f, Vector3.Zero, _root);
                _coreList.Add(c);

            }
        }

        public void PlayScene()
        {
            int i = 0;
            System.Threading.Thread.Sleep(5000);
            while (!_file.EndOfStream)
            {
                if (i % _coresCount == 0)
                    System.Threading.Thread.Sleep(20);
                Vector3 pos = new Vector3();
                pos.X = float.Parse(_file.ReadLine());
                pos.Y = float.Parse(_file.ReadLine());
                pos.Z = float.Parse(_file.ReadLine());
                _coreList[i%_coresCount].SetPosition(pos);
                i++;
            }
        }

    }
}
