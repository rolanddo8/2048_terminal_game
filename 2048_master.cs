using System;
using Test;

namespace TwentyFortyEight
{
    /// <summary>
    /// Runs the game 2048
    /// </summary>
    /// <author>YOUR NAME</author>
    /// <student_id>YOUR STUDENT ID</student_id>
    public static class Program
    {
        /// <summary>
        /// Specifies possible moves in the game
        /// </summary>
        public enum Move { Up, Left, Down, Right, Restart, Quit };

        /// <summary>
        /// Generates random numbers
        /// </summary>
        static Random numberGenerator = new Random();
        /// <summary>
        /// Number of initial digits on a new 2048 board
        /// </summary>
        const int NUM_STARTING_DIGITS = 2;

        /// <summary>
        /// The width of a cell when drawn to the screen
        /// </summary>
        const int CELL_WIDTH = 4;

        /// <summary>
        /// The chance of a two spawning
        /// </summary>
        const double CHANCE_OF_TWO = 0.9; // 90% chance of a two; 10% chance of a four
        /// <summary>
        /// The amount of cells
        /// </summary>
        const int cellNum = 16;

        /// /// <summary>
        /// The size of the 2048 board
        /// </summary>
        const int BOARD_SIZE = 4; // 4x4

        /// <summary>
        /// Runs the game of 2048
        /// </summary>
        static void Main()
        {
            TestRunner.RunTests(true);
            /// <summary>
            /// Declare a board in 2D array and variable to store move counter
            /// </summary>
            int[,] board = MakeBoard();
            int moveCounter = 0;

            /// <summary>
            /// The While loop to perform the game. When the user can not make any more move (GameOver == true) 
            /// </summary>
            while (GameOver(board) == false)
            {
                DisplayBoard(board);
                Console.WriteLine($"Move: {moveCounter}");

                Move myMove = ChooseMove();
                bool moveChanged = MakeMove(myMove, board);

                if (moveChanged == false)
                {
                    Console.WriteLine("\nGoodbye!, Press ENTER to exit!!!");
                    break;
                }

                PopulateAnEmptyCell(board);
                /// <summary>
                /// when the board is full, repeat until the user choose the right move
                /// </summary>
                /// <returns></returns>
                if (IsFull(board) == true)
                {
                    while (moveChanged == false)
                    {
                        MakeMove(myMove, board);
                    }
                }
                else //Increase the move when user choose the right move
                {
                    moveCounter += 1;
                }
                Console.Clear(); //Clear old screen

            }

            /// <summary>
            /// Send the message to restart or quit when the game is over
            /// </summary>
            while (GameOver(board) == true)
            {
                MakeMove(ChooseMove(prompt: "No moves can be make.\n Press R: Restart | Q: Quit"), board);
            }
            Console.ReadLine();

        }

        /// <summary>
        /// Display the given 2048 board in the console
        /// </summary>
        /// <param name="board">The 2048 board to display</param>
        public static void DisplayBoard(int[,] board)
        {
            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    if (board[i, j] == 0)
                        Console.Write($"{"-",4}");
                    else
                        Console.Write($"{board[i, j],CELL_WIDTH}");
                }
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Display controls and instrutions, and gets a Move from the user
        /// such as UP, LEFT, DOWN, RIGHT, RESTART or QUIT
        /// </summary>
        /// <returns>The chosen Move</returns>
        public static Move ChooseMove(bool gameOver = false, string prompt = ("WASD: Move | R: Restart | Q: Quit"))
        {
            Console.WriteLine(prompt);
            Move userMove = Move.Up;
            char userOption = Console.ReadKey().KeyChar;

            switch (userOption)
            {
                case 'W':
                    return userMove = Move.Up;

                case 'S':
                    return userMove = Move.Down;

                case 'A':
                    return userMove = Move.Left;

                case 'D':
                    return userMove = Move.Right;

                case 'R':
                    return userMove = Move.Restart;

                case 'Q':
                    return userMove = Move.Quit;

            }
            return userMove;
        }

        /// --------------------------------------------------------------------------
        /// -------------- UNIT TESTED METHODS BELOW ---------------------------------
        /// --------------------------------------------------------------------------

        /// <summary>
        /// Returns true if the given board is in a 'game over' state, meaning
        /// that no moves can be made that affect the board.
        /// 
        /// No moves are possible when there are no zeros left on the board,
        /// AND there are no adjacent cells containing the same number.
        /// </summary>
        /// <param name="board">A 2048 board to check</param>
        /// <returns>True if no moves can be made on the board</returns>
        public static bool GameOver(int[,] board)
        {
            bool gameOver = true;
            /// <summary>
            /// Check all cells in array if there are no adjacent cell containing the same number
            /// </summary>
            /// <returns> The game is over (true) or not (false) </returns>
            for (int i = 0; i < board.GetLength(0) - 1; i++)
            {
                for (int j = 0; j < board.GetLength(1) - 1; j++)
                {
                    if (board[i, j] == board[i, j + 1] || board[i + 1, j] == board[i + 1, j + 1])
                    { gameOver = false; }
                    else if (board[i, j] == board[i + 1, j] || board[i, j + 1] == board[i + 1, j + 1])
                    { gameOver = false; }
                }
            }
            return gameOver;
        }

        /// <summary>
        /// Returns true if the given 2048 board is full (contains no zeros)
        /// </summary>
        /// <param name="board">A 2048 board to check</param>
        /// <returns>True if the board is full; false otherwise</returns>
        public static bool IsFull(int[,] board)
        {
            bool Checkzero = true;
            /// <summary>
            /// Check all cells in array if it is containing zero
            /// </summary>
            /// <returns> If all cells do not conatain zero, return the board is full (true), false otherwise </returns>
            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    if (board[i, j] == 0)
                        Checkzero = false;
                }
            }
            return Checkzero;
        }

        /// <summary>
        /// If the board is not full, choose a random empty cell and add a two or a four.
        /// There should be a 90% chance of adding a two, and a 10% chance of adding a four.
        /// </summary>
        /// <param name="board">The board to add a new number to</param>
        /// <returns>False if the board is already full; true otherwise</returns>
        public static bool PopulateAnEmptyCell(int[,] board)
        {
            // HINT: You can use a random number generator to choose a random double
            //       between 0 and 1 (rng.NextDouble()).
            //       Example: 
            //          rng.NextDouble() <= 0.4 will be true 40% of the time.

            bool Populatecheck = !IsFull(board);
            double randNum = numberGenerator.NextDouble();
            int randomRow = 0;
            int randomCol = 0;

            ///<summary>
            /// Using the loop to chose random celll and choose if it is emty
            /// </summary>
            while (Populatecheck == true)
            {
                randomRow = numberGenerator.Next(0, board.GetLength(0));
                randomCol = numberGenerator.Next(0, board.GetLength(1));
                if (board[randomRow, randomCol] == 0)
                {
                    break;
                }
            }

            ///<summary>
            /// If the board is not full, 90% adding 2 and 10% adding 4
            ///</summary>
            if (Populatecheck == true)
            {
                if (randNum <= 0.9)
                {
                    board[randomRow, randomCol] = 2;
                }
                else
                {
                    board[randomRow, randomCol] = 4;
                }
            }
            return Populatecheck;
            // throw new NotImplementedException(); // DELETE ME
        }

        /// <summary>
        /// Creates a new 2048 board as a 2D int array, with a size of 4x4,
        /// populated with two initial cell values (using PopulateAnEmptyCell)
        /// </summary>
        /// <returns>A new 2048 board</returns>
        public static int[,] MakeBoard()
        {
            int[,] board = new int[4, 4] {
                { 0, 0, 0, 0 },
                { 0, 0, 0, 0 },
                { 0, 0, 0, 0 },
                { 0, 0, 0, 0 }
            };
            PopulateAnEmptyCell(board);
            PopulateAnEmptyCell(board);
            return (board);
        }

        /// <summary>
        /// Applies the chosen Move on the given 2048 board
        /// </summary>
        /// <param name="move">A move such as UP, LEFT, RIGHT or DOWN</param>
        /// <param name="board">A 2048 board</param>
        /// <returns>True if the move had an effect on the game; false otherwise</returns>
        public static bool MakeMove(Move move, int[,] board)
        {
            /// <summary>
            /// Declear all columns and rows
            /// </summary>
            int[] col1 = MatrixExtensions.GetCol(board, 0);
            int[] col2 = MatrixExtensions.GetCol(board, 1);
            int[] col3 = MatrixExtensions.GetCol(board, 2);
            int[] col4 = MatrixExtensions.GetCol(board, 3);

            int[] row1 = MatrixExtensions.GetRow(board, 0);
            int[] row2 = MatrixExtensions.GetRow(board, 1);
            int[] row3 = MatrixExtensions.GetRow(board, 2);
            int[] row4 = MatrixExtensions.GetRow(board, 3);


            bool moved = false; //Variable to check if the moved affected

            ///<summary>
            /// Check the user move and apply it to the game
            /// </summary>
            if (move == Move.Up)
            {
                ShiftCombineShift(col1, shiftLeft: true);
                MatrixExtensions.SetCol(board, 0, col1);

                ShiftCombineShift(col2, shiftLeft: true);
                MatrixExtensions.SetCol(board, 1, col2);

                ShiftCombineShift(col3, shiftLeft: true);
                MatrixExtensions.SetCol(board, 2, col3);

                ShiftCombineShift(col4, shiftLeft: true);
                MatrixExtensions.SetCol(board, 3, col4);
                moved = true;
            }
            else if (move == Move.Down)
            {
                ShiftCombineShift(col1, shiftLeft: false);
                MatrixExtensions.SetCol(board, 0, col1);

                ShiftCombineShift(col2, shiftLeft: false);
                MatrixExtensions.SetCol(board, 1, col2);

                ShiftCombineShift(col3, shiftLeft: false);
                MatrixExtensions.SetCol(board, 2, col3);

                ShiftCombineShift(col4, shiftLeft: false);
                MatrixExtensions.SetCol(board, 3, col4);
                moved = true;

            }
            else if (move == Move.Left)
            {
                ShiftCombineShift(row1, shiftLeft: true);
                MatrixExtensions.SetRow(board, 0, row1);

                ShiftCombineShift(row2, shiftLeft: true);
                MatrixExtensions.SetRow(board, 1, row2);

                ShiftCombineShift(row3, shiftLeft: true);
                MatrixExtensions.SetRow(board, 2, row3);

                ShiftCombineShift(row4, shiftLeft: true);
                MatrixExtensions.SetRow(board, 3, row4);
                moved = true;
            }
            else if (move == Move.Right)
            {
                ShiftCombineShift(row1, shiftLeft: false);
                MatrixExtensions.SetRow(board, 0, row1);

                ShiftCombineShift(row2, shiftLeft: false);
                MatrixExtensions.SetRow(board, 1, row2);

                ShiftCombineShift(row3, shiftLeft: false);
                MatrixExtensions.SetRow(board, 2, row3);

                ShiftCombineShift(row4, shiftLeft: false);
                MatrixExtensions.SetRow(board, 3, row4);
                moved = true;
            }
            else if (move == Move.Restart)
            {
                Console.Clear();
                Main();

            }
            else if (move == Move.Quit)
            {
                return false;
            }
            return moved;
        }

        /// <summary>
        /// Shifts the non-zero integers in the given 1D array to the left
        /// </summary>
        /// <param name="nums">A 1D array of integers</param>
        /// <returns>True if shifting had an effect; false otherwise</returns>
        /// <example>
        ///   If nums has the values:
        ///       { 0, 2, 2, 4, 4, 0, 0, 8, 8, 5, 3 }
        ///   It will be modified to:
        ///       { 2, 2, 4, 4, 8, 8, 5, 3, 0, 0, 0 }
        /// </example>
        public static bool ShiftLeft(int[] nums)
        {
            bool changed = false; //Variable to check if the ShiftLeft affected

            /// <summary>
            /// Repeat from the beginning to check if the cell is non-zero, 
            /// repeat from the non-zero back to the beginning to find any emty cell and replace
            /// </summary>
            for (int i = 0; i < nums.Length; i++)
            {
                if (nums[i] != 0)
                {
                    for (int j = i; j >= 0; j--)
                    {
                        if (nums[j] == 0)
                        {
                            nums[j] = nums[i];
                            nums[i] = 0;
                            i = 0;
                            changed = true;
                            break;
                        }
                    }
                }
            }
            return changed;
        }

        /// <summary>
        /// Combines identical, non-zero integers that are adjacent to one another by summing 
        /// them in the left integer, and replacing the right-most integer with a zero
        /// </summary>
        /// <param name="nums">A 1D array of integers</param>
        /// <returns>True if combining had an effect; false otherwise</returns>
        /// <example>
        ///   If nums has the values:
        ///       { 0, 2, 2, 4, 4, 0, 0, 8,  8, 5, 3 }
        ///   It will be modified to:
        ///       { 0, 4, 0, 8, 0, 0, 0, 16, 0, 5, 3 }
        /// </example>
        public static bool CombineLeft(int[] nums)
        {
            bool changed = false;//Variable to check if the CombineLeft  affected

            for (int i = 0; i < nums.Length - 1; i++)
            {
                if (nums[i] != 0 && nums[i] == nums[i + 1])
                {
                    nums[i] = nums[i] + nums[i + 1];
                    nums[i + 1] = 0;
                    changed = true;
                }
            }
            return changed;
        }

        /// <summary>
        /// Shifts the numbers in the array in the specified direction, then combines them, then 
        /// shifts them again.
        /// </summary>
        /// <param name="nums">A 1D array of integers</param>
        /// <param name="left">True if numbers should be shifted to the left; false otherwise</param>
        /// <returns>True if shifting and combining had an effect; false otherwise</returns>
        /// <example>
        ///   If nums has the values below, and shiftLeft is true:
        ///       { 0, 2,  2, 4, 4, 0, 0, 8, 8, 5, 3 }
        ///   It will be modified to:
        ///       { 4, 8, 16, 5, 3, 0, 0, 0, 0, 0, 0 }
        ///       
        ///   If nums has the values below, and shiftLeft is false:
        ///       { 0, 2, 2, 4, 4, 0, 0, 8,  8, 5, 3 }
        ///   It will be modified to:
        ///       { 0, 0, 0, 0, 0, 0, 4, 8, 16, 5, 3 }
        /// </example>
        public static bool ShiftCombineShift(int[] nums, bool shiftLeft
        )
        {

            // HINT: Use Array.Reverse(nums) to reverse the array.
            //       You will need to do this before and after shifting/combining
            //       when shiftLeft is false (i.e. when we need to shift/combine right)

            bool changed = false; //Variable to check if the ShiftCombineShift affected

            if (shiftLeft == true)
            {
                changed = ShiftLeft(nums);
                CombineLeft(nums);
                ShiftLeft(nums);
            }
            else if (shiftLeft == false)
            {
                Array.Reverse(nums);
                changed = ShiftLeft(nums);
                CombineLeft(nums);
                ShiftLeft(nums);
                Array.Reverse(nums);
            }

            return changed;
        }
    }
}
