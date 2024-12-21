using System.Text;

namespace GOL
{
    internal class Program
    {
        static void Main(string[] args)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("Conway's Game of Life: Code by David To");
            stringBuilder.AppendLine("Arg[0] = BoardName - default to RANDOM if not exist, not case-sensitive");
            stringBuilder.AppendLine("\tKeys: RANDOM, bLiNkEr, toad, BEACON");
            stringBuilder.AppendLine("Arg[1] = 1 to use Test Random Example");
            stringBuilder.AppendLine("Arg[2] = Generational Count, Min = 4");
            stringBuilder.AppendLine("Arg[3] = BoardHeight, Min = 8");
            stringBuilder.AppendLine("Arg[4] = BoardWidth, Min = 8");
            stringBuilder.AppendLine("Arg[5] = WaitForKeyPress, 1 = on");
            stringBuilder.AppendLine("------------------------------------------");

            Console.WriteLine(stringBuilder.ToString());

            //separate args
            string boardName = (args.Length > 0) ?
                args[0] : "";
            bool toggleTestRandom = (args.Length > 1) ?
                args[1] == "1" : false;
            int generationCount = ((args.Length > 2) && int.TryParse(args[2], out int outputGenCount)) ?
                Math.Max(outputGenCount, 4) : 4;
            int boardHeight = ((args.Length > 3) && int.TryParse(args[3], out int outputHeight)) ?
                Math.Max(outputHeight, 8) : 8;
            int boardWidth = ((args.Length > 4) && int.TryParse(args[4], out int outputWidth)) ?
                Math.Max(outputWidth, 8) : 8;
            bool waitForKeyPress = (args.Length > 5) ?
                args[5] == "1" : false;

            Program program = new Program();

            //ready to build board
            program.BuildBoard(
                boardName, // the build type
                toggleTestRandom,// if toggle on will use test Random instead of normal Random
                boardHeight, boardWidth
                );

            //print initial board
            program.PrintBoard();

            //iterate through the entire generations
            for (int i = 1; i < generationCount; i++)
            {
                if (waitForKeyPress == true)
                {
                    Console.ReadKey();
                }

                program.NextBoard();
                program.PrintBoard();
            }
        }

        #region Private Var
        private bool[,] _board;
        private StringBuilder _stringBuilder = new StringBuilder();
        #endregion

        #region Build Board
        private void BuildBoard(string name, bool useTestRandom, int height, int width)
        {
            //error check
            if(name == null)
            {
                name = "";
            }

            //clear board
            _board = new bool[height, width];

            //remove case sensitive
            name = name.ToLower();
            switch(name)
            {
                case "blinker":
                    BuildBoard_Blinker();
                    break;
                case "toad":
                    BuildBoard_Toad();
                    break;
                case "beacon":
                    BuildBoard_Beacon();
                    break;
                default: //include "random"
                    if(useTestRandom)
                    {
                        Console.WriteLine("Using Test Random");
                        BuildBoard_TestRandom();
                    }
                    else
                    {
                        BuildBoard_Randomize();
                    }
                    break;
            }
        }

        private void BuildBoard_Beacon()
        {
            _board[1, 5] = true;
            _board[1, 6] = true;

            _board[2, 5] = true;
            _board[2, 6] = true;

            _board[3, 3] = true;
            _board[3, 4] = true;

            _board[4, 3] = true;
            _board[4, 4] = true;
        }

        private void BuildBoard_Toad()
        {
            _board[3, 3] = true;
            _board[3, 4] = true;
            _board[3, 5] = true;

            _board[4, 2] = true;
            _board[4, 3] = true;
            _board[4, 4] = true;
        }

        private void BuildBoard_Blinker()
        {
            _board[3, 4] = true;
            _board[4, 4] = true;
            _board[5, 4] = true;
        }

        private void BuildBoard_Randomize()
        {
            Random rand = new Random();

            int rowSize = _board.GetLength(0);
            int columnSize = _board.GetLength(1);

            //Iterate though the board
            for (int iRow = 0; iRow < rowSize; iRow++)
            {
                for (int iColumn = 0; iColumn < columnSize; iColumn++)
                {
                    //random true or false
                    _board[iRow, iColumn] = (rand.Next(2) == 1);
                }
            }
        }

        private void BuildBoard_TestRandom()
        {
            //the exact values on the instruction for Random
            _board[0, 0] = false;
            _board[0, 1] = false;
            _board[0, 2] = false;
            _board[0, 3] = true;
            _board[0, 4] = true;
            _board[0, 5] = false;
            _board[0, 6] = true;
            _board[0, 7] = true;

            _board[1, 0] = false;
            _board[1, 1] = false;
            _board[1, 2] = false;
            _board[1, 3] = true;
            _board[1, 4] = true;
            _board[1, 5] = true;
            _board[1, 6] = true;
            _board[1, 7] = false;

            _board[2, 0] = false;
            _board[2, 1] = true;
            _board[2, 2] = true;
            _board[2, 3] = true;
            _board[2, 4] = true;
            _board[2, 5] = false;
            _board[2, 6] = true;
            _board[2, 7] = false;

            _board[3, 0] = false;
            _board[3, 1] = true;
            _board[3, 2] = true;
            _board[3, 3] = false;
            _board[3, 4] = false;
            _board[3, 5] = false;
            _board[3, 6] = true;
            _board[3, 7] = true;

            _board[4, 0] = false;
            _board[4, 1] = false;
            _board[4, 2] = true;
            _board[4, 3] = false;
            _board[4, 4] = true;
            _board[4, 5] = true;
            _board[4, 6] = true;
            _board[4, 7] = true;

            _board[5, 0] = true;
            _board[5, 1] = true;
            _board[5, 2] = true;
            _board[5, 3] = false;
            _board[5, 4] = true;
            _board[5, 5] = true;
            _board[5, 6] = true;
            _board[5, 7] = true;

            _board[6, 0] = true;
            _board[6, 1] = true;
            _board[6, 2] = true;
            _board[6, 3] = true;
            _board[6, 4] = false;
            _board[6, 5] = true;
            _board[6, 6] = true;
            _board[6, 7] = false;

            _board[7, 0] = true;
            _board[7, 1] = false;
            _board[7, 2] = true;
            _board[7, 3] = false;
            _board[7, 4] = false;
            _board[7, 5] = false;
            _board[7, 6] = false;
            _board[7, 7] = true;
        }

        #endregion

        #region Other Private Function
        private void PrintBoard()
        {
            if (_board == null)
            {
                Console.WriteLine("There is no board");
                return;
            }

            _stringBuilder.Clear();

            int rowSize = _board.GetLength(0);
            int columnSize = _board.GetLength(1);

            //iterate through the 2D array board
            for (int iRow = 0; iRow < rowSize; iRow++)
            {
                for (int iColumn = 0; iColumn < columnSize; iColumn++)
                {
                    if(_board[iRow, iColumn] == true)
                    {
                        //Live Cell
                        _stringBuilder.Append("X ");
                    }
                    else
                    {
                        //Dead Cell
                        _stringBuilder.Append(". ");
                    }
                }
                _stringBuilder.AppendLine();
            }
            _stringBuilder.AppendLine();

            Console.WriteLine(_stringBuilder.ToString());
        }

        private void NextBoard()
        {
            if(_board == null)
            {
                Console.WriteLine("There is no board");
                return;
            }

            int rowSize = _board.GetLength(0);
            int columnSize = _board.GetLength(1);

            //creating a new board to apply results to the current board
            //everything will default to false when creating a new board
            bool[,] newBoard = new bool[rowSize, columnSize];

            for (int iRow = 0; iRow < rowSize; iRow++)
            {
                for (int iColumn = 0; iColumn < columnSize; iColumn++)
                {

                    // get the amount of alive neighbors here.
                    int aliveNeighbors = GetAliveNeighbors(iRow, iColumn);

                    //Is the current one alive?
                    if (_board[iRow,iColumn] == true)
                    {

                        //Any live cell with two or three live neighbors lives on to the next generation.
                        if (aliveNeighbors == 2 || aliveNeighbors == 3)
                        {
                            newBoard[iRow, iColumn] = true;
                        }

                        //if alive Neighbors is less than 2 or greater than 3. leave as false
                    }
                    else
                    {
                        //Any dead cell with exactly three live neighbors becomes a live cell, as if by reproduction
                        if (aliveNeighbors == 3)
                        {
                            newBoard[iRow, iColumn] = true;
                        }

                        //if not 3 then it stays dead.
                    }

                }
            }

            _board = newBoard;
        }

        private int GetAliveNeighbors(int iRow, int iColumn)
        {
            if (_board == null)
            {
                Console.WriteLine("There is no board");
                return -1;
            }

            int aliveNeighborCount = 0;
            int rowSize = _board.GetLength(0);
            int columnSize = _board.GetLength(1);

            //Check the Neighbors which is 3x3 around (iRow, iColumn) index

            //start on previous row, and end on the next row
            for(int iCurrentRow = iRow - 1; iCurrentRow <= iRow + 1; iCurrentRow++)
            {
                //Wrapping the row index in case it goes out of bound
                //adding the size again to handle negative numbers.
                int trueRow = (iCurrentRow + rowSize) % rowSize;


                //start on previous column, and end on the next row
                for (int iCurrentColumn = iColumn -1; iCurrentColumn <= iColumn + 1; iCurrentColumn++)
                {

                    //skip because this is self and not neighbor
                    if (iRow == iCurrentRow && iColumn == iCurrentColumn)
                    {
                        continue;
                    }

                    //Wrapping the column index in case it goes out of bound
                    //adding the size again to handle negative numbers.
                    int trueColumn = (iCurrentColumn + columnSize) % columnSize;

                    //If this cell is alive, increment count.
                    if (_board[trueRow, trueColumn] == true)
                    {
                        aliveNeighborCount++;
                    }

                }

            }

            return aliveNeighborCount;
        }
        #endregion
    }
}

