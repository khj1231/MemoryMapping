using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Test1027
{
    public partial class Form1 : Form
    {
        public List<MyLocation> dictionary = new List<MyLocation>();
        Graphics graphics;
        SolidBrush myBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Red);//画刷
        Pen pen = new Pen(Color.Red, 3);
        public Form1()
        {
            InitializeComponent();
            dictionary.Add(new MyLocation(0, 0));
            dictionary.Add(new MyLocation(255, 255));
            graphics = this.pictureBox1.CreateGraphics();
            refresh();
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            BeginInvoke(new Action(() =>
            {
                label2.Text = "0x"  + Convert.ToString(e.Location.Y / 3, 16).PadLeft(2, '0') + "" + Convert.ToString(e.Location.X / 3, 16).PadLeft(2, '0') ;
            }));
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                dictionary.Add(new MyLocation(e.Location.X / 3, e.Location.Y / 3));
            }
            else if (e.Button == MouseButtons.Right)
            {
                dictionary.Remove(dictionary.Find(newlocation => e.Location.X / 3 == newlocation.Location_x && e.Location.Y / 3 == newlocation.Location_y));
            }
            refresh();

        }

        void refresh()
        {
            sort();
            pictureBox1.Refresh();
            dataGridView1.Rows.Clear();
            for (int i = 0; i < dictionary.Count(); i++)
            {
                if (i > 0)
                    graphics.FillRectangle(myBrush, new Rectangle(dictionary[i].Location_x *3, dictionary[i].Location_y * 3, 3, 3));
                dataGridView1.Rows.Add("0x" +  Convert.ToString(dictionary[i].Location_y, 16).PadLeft(2, '0') +
                    "" + Convert.ToString(dictionary[i].Location_x, 16).PadLeft(2, '0'), dictionary[i].Location_len, (float)dictionary[i].Location_len/1024);
               // graphics.DrawLine(pen, dictionary[i - 1].Location_x * 3, dictionary[i - 1].Location_y * 3, dictionary[i].Location_x * 3, dictionary[i].Location_y * 3);
            }
            //dataGridView1.DataSource;
        }

        void sort()
        {
            dictionary = (from index in dictionary
                          orderby index.Location_y * 256 * 3 + index.Location_x ascending
                          select index).ToList();
            for(int i = 1; i < dictionary.Count; ++i)
            {
                    dictionary[i].Location_len = ((dictionary[i].Location_y - dictionary[i - 1].Location_y) * 256 + dictionary[i].Location_x - dictionary[i - 1].Location_x) * 4;
            }
            dictionary[dictionary.Count - 1].Location_len += 4;
        }

        public class MyLocation
        {
            private int location_x;
            private int location_y;
            private int location_len = 0;
            public int Location_x { get => location_x; set => location_x = value; }
            public int Location_y { get => location_y; set => location_y = value; }
            public int Location_len { get => location_len; set => location_len = value; }

            public MyLocation(int lox, int loy)
            {
                Location_x = lox;
                Location_y = loy;
            }
        }

        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                if (dataGridView1.Rows.Count > 0 && e.RowIndex > 0 && e.RowIndex < dataGridView1.Rows.Count -2)
                {
                    dictionary.RemoveAt(e.RowIndex);
                    //dictionary.Remove(dictionary.Find(newlocation => Convert.ToInt32(dataGridView1[0,e.RowIndex].Value) == newlocation.Location_x && Convert.ToInt32(dataGridView1[1, e.RowIndex].Value) == newlocation.Location_y));
                    refresh();
                }
        }
    }
}
