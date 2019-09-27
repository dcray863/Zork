using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Zork
{
    class Program
    {
        private static (int Row, int Column) Location = (1, 1);
        private static Room[,] Rooms = {
            { new Room("Rocky Trail"), new Room("South of House"), new Room("Canyon View")},
            { new Room("Forest"), new Room("West of House"), new Room("Behind House") },
            { new Room("Dense Woods"), new Room("North of House"), new Room("Clearing") }
        };
        private static readonly List<Commands> Directions = new List<Commands>
        {
            Commands.NORTH,
            Commands.SOUTH,
            Commands.WEST,
            Commands.EAST
        };
        private static Room CurrentRoom
        {
            get
            {
                return Rooms[Location.Row, Location.Column];
            }
        }

        static void Main(string[] args)
        {
            const string defaultRoomsFilename = "Rooms.json";
            string roomsFilename = (args.Length > 0 ? args[(int)CommandLineArguments.RoomsFilename] : defaultRoomsFilename);
            InitializeRooms(roomsFilename);

            Console.WriteLine("Welcome to Zork!");

            Room previousRoom = null;
            Commands command = Commands.UNKNOWN;
            while(command != Commands.QUIT)
            {
                Console.WriteLine(CurrentRoom);
                if(previousRoom != CurrentRoom)
                {
                    Console.WriteLine(CurrentRoom.Description);
                    previousRoom = CurrentRoom;
                }
                Console.Write("> ");
                command = ToCommand(Console.ReadLine().Trim());

                switch (command)
                {
                    case Commands.QUIT:
                        Console.WriteLine("Thank you for playing!");
                        break;

                    case Commands.LOOK:
                        Console.WriteLine(CurrentRoom.Description);
                        break;

                    case Commands.NORTH:
                    case Commands.SOUTH:
                    case Commands.EAST:
                    case Commands.WEST:
                        if(!Move(command))
                        {
                            Console.WriteLine("The way is shut!");
                        }
                        break;

                    default:
                        Console.WriteLine("Unknown command.");
                        break;
                }
            }
        }

        private static bool Move(Commands command)
        {
            Assert.IsTrue(IsDirection(command), "Invalid Direction");

            bool isValidMove = false;
            switch (command)
            {
                case Commands.NORTH when Location.Row > 0:
                    Location.Row--;
                    isValidMove = true;
                    break;
                case Commands.SOUTH when Location.Row < Rooms.GetLength(0) - 1:
                    Location.Row++;
                    isValidMove = true;
                    break;
                case Commands.WEST when Location.Column > 0:
                    Location.Column--;
                    isValidMove = true;
                    break;
                case Commands.EAST when Location.Column < Rooms.GetLength(1) - 1:
                    Location.Column++;
                    isValidMove = true;
                    break;

                default:
                    break;
            }
            return isValidMove;
        }

        private static void InitializeRooms(string roomsFilename)
        {
            Rooms = JsonConvert.DeserializeObject<Room[,]>(File.ReadAllText(roomsFilename));
        }

        private enum Fields
        {
            Name = 0,
            Description
        }

        private enum CommandLineArguments
        {
            RoomsFilename = 0
        }

        private static bool IsDirection(Commands command) => Directions.Contains(command);

        private static Commands ToCommand(string commandString) => Enum.TryParse(commandString, true, out Commands result) ? result : Commands.UNKNOWN;
    }
}