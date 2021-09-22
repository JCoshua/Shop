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
       

        public Player()
        {
            _gold = 100;
            _inventory = new Item[0];
        }

        public int Gold
        {
            get { return _gold; }
        }

        public Player(int tempInt)
        {

        }

        public void Buy(Item item)
        {
            _gold -= item.Cost;

            Console.Clear();
            Console.WriteLine("You bought a " + item.Name + "!");
            Console.ReadKey(true);

            Item[] TempArray = new Item[_inventory.Length + 1];

            for (int i = 0; i < _inventory.Length; i++)
            {
                TempArray[i] = _inventory[i];
            }

            TempArray[TempArray.Length - 1] = item;

            _inventory = TempArray;
        }

        public string[] GetItemNames()
        {
            string[] itemNames = new string[_inventory.Length];

            for (int i = 0; i < _inventory.Length; i++)
            {
                itemNames[i] = _inventory[i].Name;
            }

            return itemNames;
        }
        

        public void Save(StreamWriter writer)
        {
            writer.WriteLine(_gold);

            writer.WriteLine(_inventory.Length);

            for (int i = 0; i < _inventory.Length; i++)
            {
                writer.WriteLine(_inventory[i].Name);
                
            }


        }

        public bool Load(StreamReader reader)
        {
            //Checks if Gold is an int
            if (!int.TryParse(reader.ReadLine(), out _gold))
                return false;

            if (!int.TryParse(reader.ReadLine(), out int invLength))
                return false;

            _inventory = new Item[invLength];

            //Loads each item into your inventory
            for (int i = 0; i < _inventory.Length; i++)
            {
                _inventory[i].Name = reader.ReadLine();
            }

            return true;
        }

       
    }
}
