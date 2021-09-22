using System;
using System.Collections.Generic;
using System.Text;

namespace Shop
{
    class Shop
    {
        private int _gold;
        private Item[] _shopInventory;

        public Shop(Item[] items)
        {
            _gold = 100;
            _shopInventory = items;

        }

        //Checks if player can buy the item
        public bool Sell(Player player, int itemIndex)
        {
            Item selectedItem = _shopInventory[itemIndex];

            if (player.Gold < selectedItem.Cost)
            {
                Console.Clear();
                Console.WriteLine("You cannot afford that.");
                Console.ReadKey(true);
                return false;
            }
            else
            {
                return true;
            }
        }

        public string[] GetItemNames()
        {
            string[] itemNames = new string[_shopInventory.Length + 2];

            for (int i = 0; i < _shopInventory.Length; i++)
            {
                itemNames[i] = _shopInventory[i].Name;
            }

            return itemNames;
        }
    }
}
