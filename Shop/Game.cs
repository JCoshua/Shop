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

        /// <summary>
        /// Used to initialize Starting Values
        /// </summary>
        private void Start()
        {
            _gameOver = false;
            _player = new Player();
            InitializeItems();
            _shop = new Shop(_shopInventory);
            _currentScene = Scene.STARTMENU;
        }

        /// <summary>
        /// Updates the game every game loop
        /// </summary>
        private void Update()
        {
            DisplayCurrentScene();
            Console.Clear();
        }

        /// <summary>
        /// A simple ending message
        /// </summary>
        private void End()
        {
            Console.WriteLine("Please come again!");
        }

        /// <summary>
        /// Creates the items in the game and puts them into an array
        /// </summary>
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

        /// <summary>
        /// Creates a save
        /// </summary>
        private void Save()
        {
            //Creates a new stream writer
            StreamWriter writer = new StreamWriter("SaveData.txt");

            _player.Save(writer);

            //Closes after finishing
            writer.Close();
        }

        /// <summary>
        /// Loads a Save
        /// </summary>
        /// <returns>If the save was succsessful</returns>
       private bool Load()
        {
            bool loadSuccessful = true;

            //File doesn't exist
            if (!File.Exists("SaveData.txt"))
                loadSuccessful = false;

            //Creates new reader
            StreamReader reader = new StreamReader("SaveData.txt");
            
            //Checks if the Player Loads
            if (!_player.Load(reader))
                loadSuccessful = false;

            
            //Closes reader and returns
            reader.Close();
            return loadSuccessful;
        }

        /// <summary>
        /// Checks what scene should be displayed
        /// </summary>
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

        /// <summary>
        /// The starting screen where the player can start a new game or load an existing game
        /// </summary>
        private void DisplayOpeningMenu()
        {
            int input = GetInput("Welcome to the RPG Shop Simulator! What would you like to do?", "Start Shopping", "Load Inventory");
            
            //If player starts shopping
            if (input == 0)
            {
                //bring to Shop
                _currentScene = Scene.SHOP;
            }

            //If player loads inventory
            else if (input == 1)
            {
                //checks if load unsuccessful
                if (Load())
                {
                    //Tell Player load is successful
                    Console.WriteLine("Loading Successful");
                    Console.ReadKey(true);
                    Console.Clear();
                    
                    //Bring them to shop
                    _currentScene = Scene.SHOP;
                }
                //IF unsuccessful
                else
                {
                    //Tell player an error is occured
                    Console.WriteLine("Woops, something messed up");
                    Console.ReadKey(true);
                    Console.Clear();
                }
            }
        }

        /// <summary>
        /// Gets the items that will be shown in the menu, and adds saving and exiting
        /// </summary>
        /// <returns>The options that will be displayed in the shop menu</returns>
        private string[] GetShopMenuOptions()
        {
            //Creates a new string array two longer that the shopInventory array
            string[] itemNames = new string[_shopInventory.Length + 2];

            //Copies the names of the items
            for (int i = 0; i < _shopInventory.Length; i++)
            {
                itemNames[i] = _shopInventory[i].Name + ": " + _shopInventory[i].Cost + " Gold";
            }

            //Adds Save and Exit and returns the finished array
            itemNames[_shopInventory.Length] = "Save";
            itemNames[_shopInventory.Length + 1] = "Exit";
            return itemNames;
        }

        /// <summary>
        /// The Shop Menu
        /// </summary>
        private void DisplayShopMenu()
        {
            //Displays the players gold
            Console.WriteLine("Your Gold: " + _player.Gold);

            //Displays the player's Inventory
            Console.WriteLine("Your Inventory:");
            
            //Displays each item in the player's inventory
            for (int i = 0; i < _player.GetItemNames().Length; i++)
            {
                Console.WriteLine(_player.GetItemNames()[i]);
            }
            
            //Displays the options
            int input = GetInput("\nWhat would you like to puchase?", GetShopMenuOptions());

            switch (input)
            {
                //Buying a sword
                case 0:
                    if(_shop.Sell(_player, 0))
                    _player.Buy(_sword);
                    break;
                //Buying a shield
                case 1:
                    if (_shop.Sell(_player, 1))
                    _player.Buy(_shield);
                    break;
                //Buying an Arrow
                case 2:
                    if (_shop.Sell(_player, 2))
                    _player.Buy(_arrow);
                    break;
                //Buying a Jewel
                case 3:
                    if (_shop.Sell(_player, 3))
                    _player.Buy(_jewel);
                    break;
                //Saving
                case 4:
                    Save();
                    Console.WriteLine("Saved.");
                    Console.ReadKey(true);
                    break;
                //Exit
                case 5:
                    _gameOver = true;
                    break;
            }
        }
    }
}
