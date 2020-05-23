using System;
using System.Drawing;
using System.Windows.Forms;
using Sapper.Properties;

namespace Sapper
{
    public partial class Window : Form
    {
        private Field field;
        private GroupBox groupBox;
        
        public Window()
        {
            InitializeComponent();
            Resize += ResizeF;
            NewGame(10);
            listBox1.DataSource = field.Levels;
            listBox1.DisplayMember = "getName";
        }
        
        private void NewGame(int n)   
        {
            if (n < 5) 
                n = 5;
            if (n > 15) 
                n = 15;
            field = listBox1.SelectedItem != null 
                ? new Field(n, ((Levels) listBox1.SelectedItem).Percent) 
                : new Field(n, 10);
            
            groupBox = new GroupBox
            {
                Location = new Point(100, 100), 
                Size = new Size(40 * field.Count, 40 * field.Count), 
                Parent = this
            };

            for (var i = 0; i < field.Count; ++i)
            for (var j = 0; j < field.Count; ++j)
            {
                var b = new ControlButton(field, i, j)
                {
                    Width = 37,
                    Height = 37,
                    Location = new Point(j * 40, i * 40),
                    FlatStyle = FlatStyle.Flat,
                    BackgroundImage = Resources.cell,
                    ForeColor = SystemColors.Control,
                    Parent = groupBox
                };
            }
            
            ResizeF(this, new EventArgs());
            field.Change += ChangeButton;
            field.Lose += (s, e) =>
            {
                FindCell(Resources.cellmine);
                MessageBox.Show("You lose");
                field.Change -= ChangeButton;
            };
            field.Win += (s, e) =>
            {
                FindCell(Resources.cellflag);
                MessageBox.Show("You win");
            };
        }
        
        private void ChangeButton(object s, ChangeArgs e)
        {
            foreach (var c in groupBox.Controls)
            {
                if (!(c is ControlButton) || (c as ControlButton).X != e.X) 
                    continue;
                if ((c as ControlButton).Y != e.Y) 
                    continue;
                if (e.MinArr == "0")
                {
                    (c as ControlButton).Text = "";
                    (c as ControlButton).BackgroundImage = Resources.cellclear;
                }
                else
                {
                    (c as ControlButton).Text = e.MinArr;
                    (c as ControlButton).BackgroundImage = Resources.cellopen;
                }
            }
        }

        private void FindCell(Image z)
        {
            for (var i = 0; i < field.Count; i++)
            for (var j = 0; j < field.Count; j++)
            {
                if (!field.Cells[i, j].HasMine) 
                    continue;
                foreach (ControlButton t in groupBox.Controls)
                    if (t.X == i && t.Y == j)
                        t.BackgroundImage = z;
            }
        }

        private void ButtonClick(object sender, EventArgs e)
        {
            groupBox.Parent = null;
            NewGame(textBox1.Text != "" ? Convert.ToInt32(textBox1.Text) : 10);
        }

        private void ResizeF(object sender, EventArgs e)
        {
            groupBox1.Location = new Point((Width-groupBox1.Size.Width)/2,10);
            groupBox.Location = new Point((Width - groupBox.Size.Width)/2, 100);
        }
    }
}
