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
       
        /// <summary>
        /// Constructer of Player
        /// </summary>
        public Player()
        {
            _gold = 100;
            _inventory = new Item[0];
        }

        public int Gold
        {
            get { return _gold; }

            set { return; }
        }

        public void Buy(Item item)
        {
            //Removes the cost of the item from the player
            _gold -= item.Cost;

            //Tells the player that the puchase was successful
            Console.Clear();
            Console.WriteLine("You bought a " + item.Name + "!");
            Console.ReadKey(true);

            //Creates a new array to add the Item
            Item[] TempArray = new Item[_inventory.Length + 1];

            //Copies the old inventory into the array
            for (int i = 0; i < _inventory.Length; i++)
            {
                TempArray[i] = _inventory[i];
            }

            //Add the bought item
            TempArray[TempArray.Length - 1] = item;

            //Makes the inventory becomes the new array
            _inventory = TempArray;
        }

        /// <summary>
        /// Creates an array that contains the names of the inventory
        /// </summary>
        /// <returns></returns>
        public string[] GetItemNames()
        {
            //Creates a new array
            string[] itemNames = new string[_inventory.Length];

            //Copies the items name into the new array
            for (int i = 0; i < _inventory.Length; i++)
            {
                itemNames[i] = _inventory[i].Name;
            }

            //returns the new array
            return itemNames;
        }
        
        /// <summary>
        /// Saves the Player
        /// </summary>
        public void Save(StreamWriter writer)
        {
            writer.WriteLine(_gold);

            writer.WriteLine(_inventory.Length);

            for (int i = 0; i < _inventory.Length; i++)
            {
                writer.WriteLine(_inventory[i].Name);
                
            }


        }

        /// <summary>
        /// Loads the Player
        /// </summary>
        /// <returns>If Loading was successful</returns>
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
