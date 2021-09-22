using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Shop
{
    public enum Scene
    {
        STARTMENU,
        SHOP
    }

    struct Item
    {
        
        public string Name;
        public int Cost;
    }

    class Game
    {
        private Player _player;
        private Shop _shop;
        private bool _gameOver;
        private Scene _currentScene;

        private Item _sword;
        private Item _shield;
        private Item _arrow;
        private Item _jewel;

        public Item[] _shopInventory;
        public void Run()
        {
            Start();

            while(_gameOver == false)
            {
                Update();
            }

            End();
        }

        private void Start()
        {
            _gameOver = false;
            _player = new Player();
            InitializeItems();
            _shop = new Shop(_shopInventory);
            _currentScene = Scene.STARTMENU;
        }

        private void Update()
        {
            DisplayCurrentScene();
            Console.Clear();
        }

        private void End()
        {

        }

        private void InitializeItems()
        {
            //Initializes the Items
            _sword = new Item { Name = "Steel Sword", Cost = 35 };
            _shield = new Item { Name = "Reinforced Shield", Cost = 20 };
            _arrow = new Item { Name = "Arrow", Cost = 5 };
            _jewel = new Item { Name = "Precious Jewel", Cost = 1000 };

            //Puts them in the array
            _shopInventory = new Item[] { _sword, _shield, _arrow, _jewel };
        }

        /// <summary>
        /// Gets an input from the player based on some given decision
        /// </summary>
        /// <param name="description">The context for the input</param>
        /// <param name="option1">The first option the player can choose</param>
        /// <param name="option2">The second option the player can choose</param>
        /// <returns></returns>
        int GetInput(string description, params string[] options)
        {
            string input = "";
            int inputReceived = -1;

            while (inputReceived == -1)
            {
                //Print options
                Console.WriteLine(description);
                for (int i = 0; i < options.Length; i++)
                {
                    Console.WriteLine((i + 1) + ". " + options[i]);
                }
                Console.Write("> ");

                //Get input from player
                input = Console.ReadLine();

                //If the player typed an int...
                if (int.TryParse(input, out inputReceived))
                {
                    //...decrement the input and check if it's within the bounds of the array
                    inputReceived--;
                    if (inputReceived < 0 || inputReceived >= options.Length)
                    {
                        //Set input received to be the default value
                        inputReceived = -1;
                        //Display error message
                        Console.WriteLine("Invalid Input. Not an Option");
                        Console.ReadKey(true);
                    }
                }
                //If the player didn't type an int
                else
                {
                    //Set input received to be the default value
                    inputReceived = -1;
                    //Display error message
                    Console.WriteLine("Invalid Input. Not a Number");
                    Console.ReadKey(true);
                }
            }
            return inputReceived;
        }

        private void Save()
        {

        }

       

        private void DisplayCurrentScene()
        {
            switch (_currentScene)
            {
                case Scene.STARTMENU:
                    DisplayOpeningMenu();
                    break;

                case Scene.SHOP:
                    DisplayShopMenu();
                    break;
                default:
                    Console.WriteLine("Invalid Scene Index");
                    break;
            }
        }

        private void DisplayOpeningMenu()
        {
            int input = GetInput("Welcome to the RPG Shop Simulator! What would you like to do?", "Start Shopping", "Load Inventory");
            
            if (input == 0)
            {
                _currentScene = Scene.SHOP;
            }
            else if (input == 1)
            {
                if (_gameOver)
                {
                    Console.WriteLine("Loading Successful");
                    Console.ReadKey(true);
                    Console.Clear();
                    _currentScene = Scene.SHOP;
                }
                else
                {
                    Console.WriteLine("Woops, something messed up");
                    Console.ReadKey(true);
                    Console.Clear();
                }
            }
        }

        private string[] GetShopMenuOptions()
        {
            string[] itemNames = new string[_shopInventory.Length + 2];

            for (int i = 0; i < _shopInventory.Length; i++)
            {
                itemNames[i] = _shopInventory[i].Name + ": " + _shopInventory[i].Cost + " Gold";
            }

            itemNames[_shopInventory.Length] = "Save";
            itemNames[_shopInventory.Length + 1] = "Exit";
            return itemNames;
        }

        private void DisplayShopMenu()
        {
            Console.WriteLine("Your Gold: " + _player.Gold);
            Console.WriteLine("Your Inventory:");

            for (int i = 0; i < _player.GetItemNames().Length; i++)
            {
                Console.WriteLine(_player.GetItemNames()[i]);
            }
            
            int input = GetInput("\nWhat would you like to puchase?", GetShopMenuOptions());

            switch (input)
            {
                case 0:
                    if(_shop.Sell(_player, 0))
                    {
                        _player.Buy(_sword);
                    }
                    break;
                case 1:
                    if (_shop.Sell(_player, 1))
                    {
                        _player.Buy(_shield);
                    }
                    break;
                case 2:
                    if (_shop.Sell(_player, 2))
                    {
                        _player.Buy(_arrow);
                    }
                    break;
                case 3:
                    if (_shop.Sell(_player, 3))
                    {
                        _player.Buy(_jewel);
                    }
                    break;
                case 4:
                    Save();
                    break;
                case 5:
                    _gameOver = true;
                    break;

            }
        }
    }
}
