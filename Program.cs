

using System;
using System.Text.RegularExpressions;

namespace toyrobot2
{
    class Program
    {
        static void Main(string[] args)
        {
            //Main class for calling methods move, right, left and report using robot class object
            #region UserIssueUserCommands
            var robot = new Robot();
            bool UserCommandPlaced = false;

            while (true)
            {
                var UserCommand = Console.ReadLine();

                //UserCommand to clear the console screen
                if (UserCommand == ("clear"))
                    Console.Clear();

         if (robot.Validation(UserCommand))
           { 
                 if (UserCommand.StartsWith("place("))
                {
                        //   removing place
                        UserCommand = UserCommand.Replace("place", "");

                        //removing bracket or special Characters and spliiting string with ',' 
                      //  var name = name.replace(/[!@#$%^&*]/g, "");
                        var paramValues = Regex.Replace(UserCommand.ToString(), @"\s+|\(+|\)+", "").Split(',');

                        //checking x y cordinates and direction parameters
                        if (paramValues.Length != 3)
                    {
                        Console.WriteLine("Invalid User Command. Please try again.");
                    }




                    int x, y;
                    if (!int.TryParse(paramValues[0], out x) || !int.TryParse(paramValues[1], out y))
                    {
                        Console.WriteLine("Invalid parameters. Pleae try again.");
                        continue;
                    }

                    //remove ' if a direction is issued like 'NORTH' instead of NORTH to match values in enum class
                    if (!Enum.TryParse(paramValues[2].Replace("'",""), true, out Direction facing))
                    {
                        Console.WriteLine("Invalid direction. Please try again.");
                        continue;
                    }

                    if (!robot.Place(x, y, facing))
                    {
                        Console.WriteLine("Placement out of bounds.");
                        continue;
                    }

                    UserCommandPlaced = true;
                }
           
                else if (!UserCommandPlaced)
                {
                    Console.WriteLine("'Place' instruction must be issued before other User Commands.");
                    continue;
                }
                else if (UserCommand == "move()")
                {
                    if (!robot.Move())
                    {
                        Console.WriteLine("Movement out of bounds. Please try again.");
                    }
                }
                else if (UserCommand == "left()")
                {
                    robot.TurnLeft();
                }
                else if (UserCommand == "right()")
                {
                    robot.TurnRight();
                }
                else if (UserCommand == "report()")
                {
                    Console.WriteLine(robot.Report());
                }
                }
                else
                {
                    Console.WriteLine("Invalid User Command.Please try again.");
                }
            }
            #endregion


        
        }
    }

    //Robot class having method defination for Place, Move, Left Right and Report User Commands
    public class Robot
    {
        //declaring  variables
        // initialising variables from 0 to 4 for 5*5 grid
        
        private int x;
        private int y;
        private Direction facing;

        private const int x_Min = 0;
        private const int y_Min = 0;
        private const int x_Max = 4;
        private const int y_Max = 4;

        #region Place
        public bool Place(int x, int y, Direction facing)
        {
            //Exit with return false if robot x, y values are out of bounds of 5*5 grid
            if (x < x_Min || x > x_Max || y < y_Min || y > y_Max)
            return false;
            
            //setting the robot current position and facing
            this.x = x;
            this.y = y;
            this.facing = facing;
            return true;
        }
        #endregion

        #region Move
        public bool Move()
        {
            //depending on the facing direction and considering the Min/Max range of x and y cordinates, robot will take a move i.e increment or decrement by 1.
            switch (facing)
            {
                case Direction.NORTH:
                    if (y < y_Max)
                    {
                        y++;
                        return true;
                    }
                    break;
                case Direction.SOUTH:
                    if (y > y_Min)
                    {
                        y--;
                        return true;
                    }
                    break;
                case Direction.EAST:
                    if (x < x_Max)
                    {
                        x++;
                        return true;
                    }
                    break;
                case Direction.WEST:
                    if (x > x_Min)
                    {
                        x--;
                        return true;
                    }
                    break;
            }
            return false;
        }
        #endregion
        
        //for understanding of left and right method, imagine the directions N,S,E,W with thier actual positions and they are in a circle
        #region Left
        public void TurnLeft()
        {
            //setting the direction facing of robot using current direction and a turn left move 
            switch (facing)
            {
                case Direction.NORTH:
                    facing = Direction.WEST;
                    break;
                case Direction.SOUTH:
                    facing = Direction.EAST;
                    break;
                case Direction.EAST:
                    facing = Direction.NORTH;
                    break;
                case Direction.WEST:
                    facing = Direction.SOUTH;
                    break;
            }
        }
        #endregion

        #region Right
        public void TurnRight()
        {
           //setting the direction facing of robot using current direction and a turn right move 
            switch (facing)
            {
                case Direction.NORTH:
                    facing = Direction.EAST;
                    break;
                case Direction.SOUTH:
                    facing = Direction.WEST;
                    break;
                case Direction.EAST:
                    facing = Direction.SOUTH;
                    break;
                case Direction.WEST:
                    facing = Direction.NORTH;
                    break;
            }
        }
        #endregion

        #region Report
        public string Report()
        {
            //report function returning x cordinate , y cordinate and facing direction of the Robot
            return $"{x}, {y}, {facing}";
        }
        #endregion


        //checks all the validation of User Commands and return true if satisfied. 
        public bool Validation(string str)
        {
           
            if (str.StartsWith("place(") && str.EndsWith(")") && str.Split(',').Length - 1 == 2)
                return true;
            else if (str == "move()")
                return true;

            else if (str == "left()")
                return true;
            
                else if (str == "right()")
                return true;
            
                else if (str == "report()")
                return true;

            else
         return false;

        }


    }

    //declaring special enum class for 4 constant directions
    public enum Direction
    {
        NORTH,
        SOUTH,
        EAST,
        WEST
    }
}