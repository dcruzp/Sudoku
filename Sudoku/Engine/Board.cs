using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku.Engine
{
    class Board
    {
        Cell[,] board;
        int rows;
        int columns;
        int sqrt; 
        public Board()
        {
            this.rows = 9;
            this.columns = 9;
            this.sqrt = (int)Math.Sqrt(9); 
            this.board = new Cell[this.rows, this.columns];

            for (int i = 0; i < this.board.GetLength(0); i++)
            {
                for (int j = 0; j < this.board.GetLength(1); j++)
                {
                    this.board[i, j] = new Cell(); 
                }
            }
        }
        public int Rows
        {
            get => this.rows;
            set => this.rows = value; 
        }

        public int Columns
        {
            get => this.columns;
            set => this.columns = value;
        }

        public int Sqrt
        {
            get => sqrt;
            set => sqrt = value; 

        }

        public Cell this[int row , int column ]
        {
            get => this.board[row,column];
            set => this.board[row, column] = value; 
        }

        public void Generate ()
        {
            for (int i = 0; i < this.board.GetLength(0); i++)
            {
                for (int j = 0; j < this.board.GetLength(1); j++)
                {
                    this.board[i, j] = new Cell();
                }
            }

            int count = 30;
            Random rnd = new Random(); 
            while (count > 0 )
            {
                int f = rnd.Next(this.board.GetLength(0));
                int c = rnd.Next(this.board.GetLength(1));

                if (board[f, c].Type != typecell.empty) continue;

                int aux = rnd.Next(1,10);
                board[f, c].Type = (typecell)aux;
                if (checkSimilarElementRowColumnRegion(f, c))
                {
                    this.board[f, c].Type = typecell.empty;
                    continue; 
                }
                count = count - 1; 
            }
        }

        public bool checkSimilarElementRowColumnRegion (int row , int column )
        {
            return checkSimilarElementRegion(row, column) || checkSimilarElementRowandColmun(row, column);
        }

        private bool checkSimilarElementRegion (int row , int column)
        {
            int result; 
            int rf = Math.DivRem(row,sqrt,out result);
            int rc = Math.DivRem(column , sqrt , out result);

            for (int i = 0; i < this.sqrt; i++)
            {
                for (int j = 0; j < this.sqrt; j++)
                {
                    int df = rf * sqrt + i;
                    int dc = rc * sqrt + j;

                    if (df == row && dc == column) continue; 

                    if (this.board[df, dc].Type == this.board[row, column].Type)
                        return true; 
                }
            }
            return false;
        }

        private bool checkSimilarElementRowandColmun (int row , int column)
        {
            int[] dirf = { -1, 0, 1, 0 };
            int[] dirc = { 0, 1, 0, -1 };
            typecell curret = this.board[row, column].Type;

            for (int k = 1; k < Math.Max(this.rows ,this.columns); k++)
            {
                for (int i = 0; i < 4 ; i++)
                {
                    int df = row + dirf[i] * k;
                    int dc = column + dirc[i] * k; 

                    if (inrange(df,dc) && this.board[df,dc].Type == curret)
                    {
                        return true ; 
                    }
                }
            }
            return false; 
        }

        private bool inrange (int row , int column)
        {
            return row >= 0 && column >= 0 && row < this.rows && column < this.columns; 
        }
    }
}
