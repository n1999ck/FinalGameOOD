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
            _player = new Player(GameWorld.Instance.Entrance);
            _clock = new GameClock(1000);
            NotificationCenter.Instance.AddObserver("GameClockTick", OnGameClockTick);
            NotificationCenter.Instance.AddObserver("PlayerDidInvestigate", OnInvestigation);
        }

        public void OnInvestigation(Notification notification)
        {
            Console.WriteLine("Player investigated " + notification.ToString());
        }

        public void OnGameClockTick(Notification notification)
        {
            //Console.WriteLine("The game time is " + _clock.TimeInGame);
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
            return "Welcome to the World of CSU!\n\n The World of CSU is a new, incredibly boring adventure game.\n\nType 'help' if you need help.\n\n" + _player.CurrentRoom.ToString();
        }

        public string Goodbye()
        {
            return "\nThank you for playing, Goodbye. \n";
        }

    }
}
