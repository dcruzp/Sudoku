using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Sudoku.Engine; 

namespace Sudoku
{
    public partial class Form1 : System.Windows.Forms.Form
    {

        int time; 
        public static Dictionary<string, Bitmap> images;
        string timeResult = "000";
        Board board;

        Form2 selector;
        Button[,] buttons;

        bool error; 

        Button btnCurrentSelect = null; 

        Color colorbutton = Color.LightCyan;
        Color colorselectbutton = Color.Cyan; 


        public Form1()
        {
            InitializeComponent();

            images = new Dictionary<string, Bitmap>();

            images.Add("guion", Resource1.numeros0);
            images.Add("numeroReloj1", Resource1.numeros10);
            images.Add("numeroReloj0", Resource1.numeros11);
            images.Add("numeroReloj9", Resource1.numeros2);
            images.Add("numeroReloj8", Resource1.numeros3);
            images.Add("numeroReloj7", Resource1.numeros4);
            images.Add("numeroReloj6", Resource1.numeros5);
            images.Add("numeroReloj5", Resource1.numeros6);
            images.Add("numeroReloj4", Resource1.numeros7);
            images.Add("numeroReloj3", Resource1.numeros8);
            images.Add("numeroReloj2", Resource1.numeros9);
            timer.Start();

            board = new Board();
            board.Generate();
            this.picBox.Size = new Size(400, 400);
            buttons = new Button[this.board.Rows, this.board.Columns];
            this.error = false; 
            
            InsertBottomDialog(); 
            InsertButton();

            
        }

       

        public void InsertBottomDialog ()
        {
            int width = 50;
            int height = 50; 

            for (int i = 0; i < this.board.Sqrt; i++)
            {
                for (int j = 0; j < this.board.Sqrt; j++)
                {
                    Button btn = new Button();
                    btn.Width = width;
                    btn.Height = height;

                    btn.Top = i * height;
                    btn.Left = j * width;

                    btn.Text = (i * this.board.Sqrt + j).ToString(); 

                    btn.BackColor = Color.LightSlateGray;
                    btn.FlatStyle = FlatStyle.Flat;
                                  

                    this.picBoxbuttons.Controls.Add(btn);
                }
            }
        }

        private void InsertButton()
        {
            for (int i = 0; i < this.board.Rows; i++)
            {
                for (int j = 0; j < this.board.Columns; j++)
                {
                    int width = this.picBox.Width / this.board.Columns;
                    int height = this.picBox.Height / this.board.Rows; 

                    Button btn = new Button();
                    btn.BackColor = colorbutton;
                    
                    btn.Width = width -1;
                    btn.Height = height -1;

                    btn.Font =  new System.Drawing.Font("Arial", 20, FontStyle.Regular);

                    btn.Text = board[i,j].Type != typecell.empty ?  ((int)this.board[i, j].Type).ToString() : "";

                    btn.Top = i * width;
                    btn.Left = j * height;

                    btn.Location = new Point(i*width, j*height);

                    btn.FlatStyle = FlatStyle.Flat;

                    btn.TextAlign = ContentAlignment.MiddleCenter;
                    btn.MouseClick += Btn_MouseClick;
                    btn.MouseEnter += Btn_MouseEnter;
                    btn.MouseLeave += Btn_MouseLeave;
                   
                   
                    btn.Name = "btn" + (i * this.board.Rows + j).ToString();

                    this.buttons[i, j] = btn;
                    this.picBox.Controls.Add(this.buttons[i,j]);
                }
            }
        }

        private void Btn_MouseLeave(object sender, EventArgs e)
        {
            Button current = (Button)sender;
            var index = findByReference(buttons, current);
            if (!this.board[index.Item1,index.Item2].BadCell)
                current.BackColor = Color.LightCyan;

            for (int i = 0; i < this.buttons.GetLength(0); i++)
            {
                for (int j = 0; j < this.buttons.GetLength(1); j++)
                {
                    if (this.buttons[i, j].Name == current.Name)
                    {
                        PrintSimilarElementRowandColmun(i, j, colorbutton);
                        PrintSimilarElementRegion(i, j, colorbutton);
                    }
                }
            }
        }

        private void PrintSimilarElementRowandColmun(int row, int column, Color color)
        {
            int[] dirf = { -1, 0, 1, 0 };
            int[] dirc = { 0, 1, 0, -1 };
           
            for (int k = 1; k < Math.Max(this.board.Rows, this.board.Columns); k++)
            {
                for (int i = 0; i < 4; i++)
                {
                    int df = row + dirf[i] * k;
                    int dc = column + dirc[i] * k;

                    if (inrange(df, dc)&& !this.board[df,dc].BadCell)
                    {
                        this.buttons[df, dc].BackColor = color; 
                    }
                }
            }            
        }

        private void PrintSimilarElementRegion(int row, int column , Color color )
        {
            int result;
            int rf = Math.DivRem(row, this.board.Sqrt, out result);
            int rc = Math.DivRem(column, this.board.Sqrt, out result);

            for (int i = 0; i < this.board.Sqrt; i++)
            {
                for (int j = 0; j < this.board.Sqrt; j++)
                {
                    int df = rf * this.board.Sqrt + i;
                    int dc = rc * this.board.Sqrt + j;

                    if (this.board[df, dc].BadCell)
                        continue;
                    this.buttons[df, dc].BackColor = color;              
                }
            }
        }

        private bool inrange(int row, int column)
        {
            return row >= 0 && column >= 0 && row < this.board.Rows && column < this.board.Columns;
        }

        private void Btn_MouseEnter(object sender, EventArgs e)
        {
            Button current = (Button)sender;
            
            for (int i = 0; i < this.buttons.GetLength(0); i++)
            {
                for (int j = 0; j < this.buttons.GetLength(1); j++)
                {
                    if (this.board[i, j].BadCell)
                        continue;
                    if (this.buttons[i, j].Name == current.Name)
                    {
                        PrintSimilarElementRowandColmun(i, j , colorselectbutton);
                        PrintSimilarElementRegion(i, j, colorselectbutton);
                    }
                }
            }

            var index = findByReference(buttons, current);
            if (!this.board[index.Item1,index.Item2].BadCell)
                current.BackColor = Color.Yellow;
        }

        private void Btn_MouseClick(object sender, MouseEventArgs e)
        {  

            Button button = (Button)sender;
            this.btnCurrentSelect = button;

            if (selector == null)
            {
                selector = new Form2();
            }

            var index = findByReference(this.buttons, button);
            if (index.Item1 == -1 && index.Item2 == -1)
                throw new IndexOutOfRangeException("Invalid index of button");

            if (this.board[index.Item1, index.Item2].Type != typecell.empty &&
                !this.board[index.Item1, index.Item2].BadCell)
                return;

            selector.Location = Cursor.Position;

            if (selector.ShowDialog() == DialogResult.OK)
            {
                this.board[index.Item1, index.Item2].Type = (typecell)selector.Number;
                button.Text = selector.Number.ToString();
                this.error = false;
                this.board[index.Item1, index.Item2].BadCell = false;
                button.BackColor = colorbutton;

                if (this.board.checkSimilarElementRowColumnRegion(index.Item1,index.Item2))
                {
                    button.BackColor = Color.Red;
                    this.board[index.Item1, index.Item2].BadCell = true;
                    this.error = true;
                }
                selector.Hide();
            }


        }

        private Tuple<int,int> findByReference<T>(T[,] buttons, T button)
        {
            var rows = buttons.GetLength(0);
            var columns = buttons.GetLength(1);
            
            for (int i = 0; i <rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    if (buttons[i, j].Equals(button))
                    {
                        return new Tuple<int, int>(i, j); 
                    }
                }
            }
            return new Tuple<int, int>(-1, -1);
        }

        private void picBox_Paint(object sender, PaintEventArgs e)
        {
            int amount = 9;

            Graphics g = e.Graphics;
            Font font = new Font(familyName: "Aria", 20);

            SolidBrush solidBrush = new SolidBrush(Color.Black); 
            //Draw lines on board 
            Pen pen1 = new Pen(Color.Black,3); 
            Pen pen2 = new Pen(Color.Black ,3);
            var pen = pen1;
            for (int i = 0; i <= amount; i++)
            {
                if (i % 3 == 0)
                { 
                    //var pen  = ? pen2 : pen1;
                    Point p1 = new Point((this.picBox.Width / amount * i), 0);
                    Point p2 = new Point((this.picBox.Width / amount * i), this.picBox.Height);
                    Point p3 = new Point(0, (this.picBox.Height / amount * i));
                    Point p4 = new Point(this.picBox.Width, (this.picBox.Height / amount * i));
                    g.DrawLine(pen, p1, p2);
                    g.DrawLine(pen, p3, p4);
                }
            }


            ///Drawing the Numbers in the pictureBox 
            ///
            //for (int i = 0; i < this.board.Rows; i++)
            //{
            //    for (int j = 0; j < this.board.Columns; j++)
            //    {
            //        Point p = new Point(this.picBox.Width / 9 * j + 8, this.picBox.Height / 9 * i + 8);
            //        if (this.board[i, j].Type == typecell.empty) continue;
            //        g.DrawString(((int)this.board[i, j].Type).ToString(), font, solidBrush, p);
            //    }
            //}
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            time++;
            if (time <= 999)
            {
                string timeString = time.ToString();

                if (timeString.Length < 3)
                {
                    timeResult = "";
                    int tempDifference = 3 - timeString.Length;

                    while (tempDifference > 0)
                    {
                        timeResult += '0';
                        tempDifference--;
                    }
                    timeResult += timeString;
                }
                else timeResult = timeString;
                picBoxTimer.Refresh();
            }
            else timer.Stop();
        }

        private void picBoxTimer_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
           
            for (int i = 0; i < timeResult.Length; i++)
            {
                switch (timeResult[i])
                {
                    case '0':
                        g.DrawImage(images["numeroReloj0"], i * 15, 0, 15, 22);
                        break;
                    case '1':
                        g.DrawImage(images["numeroReloj1"], i * 15, 0, 15, 22);
                        break;
                    case '2':
                        g.DrawImage(images["numeroReloj2"], i * 15, 0, 15, 22);
                        break;
                    case '3':
                        g.DrawImage(images["numeroReloj3"], i * 15, 0, 15, 22);
                        break;
                    case '4':
                        g.DrawImage(images["numeroReloj4"], i * 15, 0, 15, 22);
                        break;
                    case '5':
                        g.DrawImage(images["numeroReloj5"], i * 15, 0, 15, 22);
                        break;
                    case '6':
                        g.DrawImage(images["numeroReloj6"], i * 15, 0, 15, 22);
                        break;
                    case '7':
                        g.DrawImage(images["numeroReloj7"], i * 15, 0, 15, 22);
                        break;
                    case '8':
                        g.DrawImage(images["numeroReloj8"], i * 15, 0, 15, 22);
                        break;
                    case '9':
                        g.DrawImage(images["numeroReloj9"], i * 15, 0, 15, 22);
                        break;
                }
            }
        }

        private void picBox_MouseDown(object sender, MouseEventArgs e)
        {
            //Point p = Cursor.Position;

            //Button button = (Button)sender;
            //int row = e.Y / (this.picBox.Height / this.board.Columns);
            //int column = e.X /( this.picBox.Width / this.board.Rows);

            //if (this.board[row, column].Type != typecell.empty) return; 

            //if (selector == null)           
            //    selector = new Form2();

            //Point location = new Point(this.Location.X + this.picBox.Location.X + button.Left + 10, this.Location.Y + this.picBox.Location.Y + button.Top + 30);

            //selector.Location = location;

            //if (selector.ShowDialog() == DialogResult.OK)
            //{

            //    this.board[row, column].Type = (typecell)selector.number;
            //    this.picBox.Refresh(); 
            //}
            //selector.Hide();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close(); 
        }
    }
}
