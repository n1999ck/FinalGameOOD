using System.Collections;
using System.Collections.Generic;
using System;

namespace StarterGame
{
    /*
     * Spring 2023
     * Essential object
     */
    public class Game
    {
        //Three private objects/instances - player, parser, bool for play state
        private Player _player;
        private Parser _parser;
        private bool _playing;
        private GameClock _clock;


        public Game()
        {
            
            _playing = false; //not playing yet
            _parser = new Parser(new CommandWords()); //give commandwords, pretty much serves as a dictionary
            //CreateWorld method creates game world with everything needed, 
            //returns entrance to that world as player in starter room.
            _player = new Player(GameWorld.Instance.Entrance);
            _clock = new GameClock(1000);
            NotificationCenter.Instance.AddObserver("GameClockTick", OnGameClockTick);
        }

        public void OnGameClockTick(Notification notification)
        {
            //Console.WriteLine("The game time is " + _clock.TimeInGame);
        }

        public Room CreateWorld()
        {
            Room outside = new Room("outside the main entrance of the university");
            Room scctparking = new Room("in the parking lot at SCCT");
            Room boulevard = new Room("on the boulevard");
            Room universityParking = new Room("in the parking lot at University Hall");
            Room parkingDeck = new Room("in the parking deck");
            Room scct = new Room("in the SCCT building");
            Room theGreen = new Room("in the green in from of Schuster Center");
            Room universityHall = new Room("in University Hall");
            Room schuster = new Room("in the Schuster Center");
            Room clockTower = new Room("at the Clock Tower");
            Room greekCenter = new Room("in the Greek Center");
            Room davidson = new Room("in the Davidson");

            /*
            utside.SetExit("west", boulevard);

            boulevard.SetExit("east", outside);
            boulevard.SetExit("south", scctparking);
            boulevard.SetExit("west", theGreen);
            boulevard.SetExit("north", universityParking);

            scctparking.SetExit("west", scct);
            scctparking.SetExit("north", boulevard);

            scct.SetExit("east", scctparking);
            scct.SetExit("north", schuster);

            schuster.SetExit("south", scct);
            schuster.SetExit("north", universityHall);
            schuster.SetExit("east", theGreen);

            theGreen.SetExit("west", schuster);
            theGreen.SetExit("east", boulevard);

            universityHall.SetExit("south", schuster);
            universityHall.SetExit("east", universityParking);

            universityParking.SetExit("south", boulevard);
            universityParking.SetExit("west", universityHall);
            universityParking.SetExit("north", parkingDeck);

            parkingDeck.SetExit("south", universityParking); */
            
            //definitely not all connected correctly
            Door door = Door.Connect(outside, boulevard, "west", "east");
            door = Door.Connect(theGreen, boulevard, "east", "west");
            door = Door.Connect(scct, scctparking, "east", "west");
            door = Door.Connect(scctparking, boulevard, "north", "south");
            door = Door.Connect(scct, schuster, "north", "south");
            door = Door.Connect(schuster,theGreen, "east", "west");
            door = Door.Connect(universityHall,universityParking, "east", "west");
            door = Door.Connect(universityParking, parkingDeck, "north", "south");
            door = Door.Connect(davidson, clockTower, "west", "east");
            door = Door.Connect(clockTower, greekCenter, "north", "south");
            door = Door.Connect(schuster, universityHall, "north", "south");
            RegularLock rl = new RegularLock();
            door.Lockable = rl;
            rl.Lock();
            door = Door.Connect(universityParking, boulevard, "north", "south");

            return outside;
        }

        /**
     *  Main play routine.  Loops until end of play.
     */
        public void Play()
        {

            // Enter the main command loop.  Here we repeatedly read commands and
            // execute them until the game is over.

            bool finished = false;
            while (!finished)
            {
                Console.Write("\n>");
                Command command = _parser.ParseCommand(Console.ReadLine());
                if (command == null)
                {
                    _player.ErrorMessage("I don't understand...");
                }
                else
                {
                    finished = command.Execute(_player);
                }
            }
        }


        public void Start()
        {
            //Sets playing bool, sends first message to player
            //Not using console - more flexible
            _playing = true;
            _player.InfoMessage(Welcome());
        }

        public void End()
        {
            //reverse of Start
            _playing = false;
            _player.InfoMessage(Goodbye());
        }

        public string Welcome()
        {
            return "Welcome to the World of CSU!\n\n The World of CSU is a new, incredibly boring adventure game.\n\nType 'help' if you need help." + _player.CurrentRoom.Description();
        }

        public string Goodbye()
        {
            return "\nThank you for playing, Goodbye. \n";
        }

    }
}
