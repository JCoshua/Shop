using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Shop
{
    class Player
    {
        private int _gold;
        private Item[] _inventory;
       
        public int Gold()
        {
            return _gold;
        }

        public Item[] Invetory()
        {
            return _inventory;
        }
        public Player(int gold)
        {

        }

        public void Buy(Item item)
        {

        }

        public string[] GetItemNames()
        {

        }
        

        public void Save(StreamWriter writer)
        {

        }

        public bool Load(StreamReader reader)
        {

        }

       
    }
}
