using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sudoku
{
    public partial class Form2 : Form
    {
        int number;
        int dim;
        int width;
        int height; 

        public Form2()
        {
            InitializeComponent();

            this.dim = 3;

            this.width = 45;
            this.height = 45;

            for (int i = 0; i < dim  ; i++)
            {
                for (int j = 0; j < dim; j++)
                {
                    Button btn = new Button();

                    btn.Text = (i * dim + j + 1 ).ToString();

                    btn.Height = this.height;
                    btn.Width = this.width; 


                    btn.Top = i * height;
                    btn.Left = j * this.width;

                    btn.FlatStyle = FlatStyle.Flat;

                    btn.Font = new System.Drawing.Font("Arial", 18, FontStyle.Regular);

                    btn.DialogResult = DialogResult.OK;

                    btn.BackColor = Color.SkyBlue;

                    btn.Click += Btn_Click;

                    btn.Cursor = Cursors.Hand;

                    this.picBox.Controls.Add(btn);
                }
            }

            this.picBox.Height = this.dim * this.height;
            this.picBox.Width = this.dim * this.width;
            this.Height = this.dim * this.height + 45;
            this.Width = this.dim * this.width + 20;

            this.BackColor = Color.Black;

            this.FormBorderStyle = FormBorderStyle.None;

            this.MouseDown += Form2_MouseDown;

            this.Click += Form2_Click;
           
        }

        private void Form2_Click(object sender, EventArgs e)
        {
            this.Close(); 
        }

        private void Form2_MouseDown(object sender, MouseEventArgs e)
        {
            if (Cursor.Position.X < this.Location.X)
            {
                this.Close(); 
            }

            MessageBox.Show(Cursor.Position.ToString());
        }

        private void Btn_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            this.number = int.Parse(button.Text); 
        }

        public int Number
        {
            get => this.number;
            set => this.number = value; 
        }
      
    }
}
