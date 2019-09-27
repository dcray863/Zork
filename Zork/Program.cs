using System;
using System.Collections.Generic;

namespace Zork
{
    class Program
    {
        private static int rowIndex = 1;
        private static int colIndex = 1;
        private static readonly string[,] Rooms = {
            { "Rocky Trail", "South of House", "Canyon View"},
            { "Forest", "West of House", "Behind House" },
            { "Dense Woods", "North of House", "Clearing" }
        };
        private static readonly List<Commands> Directions = new List<Commands>
        {
            Commands.NORTH,
            Commands.SOUTH,
            Commands.WEST,
            Commands.EAST
        };
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Zork!");

            Commands command = Commands.UNKNOWN;
            while(command != Commands.QUIT)
            {
                Console.WriteLine(Rooms[rowIndex, colIndex]);
                Console.Write("> ");
                command = ToCommand(Console.ReadLine().Trim());

                string outputString;
                switch (command)
                {
                    case Commands.QUIT:
                        outputString = "Thank you for playing!";
                        break;

                    case Commands.LOOK:
                        outputString = "This is an open field west of a white house, with a boarded front door. \n" +
                            "A rubber mat saying 'Welcome to Zork!' lies by the door.";
                        break;

                    case Commands.NORTH:
                    case Commands.SOUTH:
                    case Commands.EAST:
                    case Commands.WEST:
                        if(Move(command) == false)
                        {
                            outputString = "The way is shut!";
                        }
                        else
                            outputString = $"You moved {command}.";
                        break;

                    default:
                        outputString = "Unknown command.";
                        break;
                }

                Console.WriteLine(outputString);
            }
        }

        private static bool Move(Commands command)
        {
            Assert.IsTrue(IsDirection(command), "Invalid Direction");

            bool moveOK = false;
            switch (command)
            {
                case Commands.NORTH when rowIndex > 0:
                    rowIndex--;
                    moveOK = true;
                    break;
                case Commands.SOUTH when rowIndex < Rooms.GetLength(0) - 1:
                    rowIndex++;
                    moveOK = true;
                    break;
                case Commands.WEST when colIndex > 0:
                    colIndex--;
                    moveOK = true;
                    break;
                case Commands.EAST when colIndex < Rooms.GetLength(1) - 1:
                    colIndex++;
                    moveOK = true;
                    break;

                default:
                    break;
            }
            return moveOK;
        }

        private static bool IsDirection(Commands command) => Directions.Contains(command);

        private static Commands ToCommand(string commandString) => Enum.TryParse(commandString, true, out Commands result) ? result : Commands.UNKNOWN;
    }
}