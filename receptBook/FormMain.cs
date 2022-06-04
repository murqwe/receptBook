using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using ReceptBook.Models;

namespace receptBook
{
    public partial class FormMain : Form
    {

        public List<Recept> recepts = new List<Recept>()
        {
              
        };

        public FormMain()
        {
            InitializeComponent();
            string Path = @"C:\Users\Elmar\source\repos\receptBook\receptBook\recepts.txt";

            using (StreamReader sr = new StreamReader(Path))
            {
                string json = sr.ReadToEnd();

                if (!string.IsNullOrEmpty(json))
                {
                    recepts = JsonConvert.DeserializeObject<List<Recept>>(json);
                }
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void panelRecept_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            InitRecepts();
        }

        public void InitRecepts(Recept[] recepts = null) 
        {
            if (recepts == null)
            {
                recepts = this.recepts.ToArray();
            }
            panelRecepts.Controls.Clear();
            int count = 0;
            int maxCountInRow = 2;

            Panel row = null;

            foreach (Recept recept in recepts)
            {
                if (count % maxCountInRow == 0)
                {
                    row = new Panel()
                    {
                        Height = 325,
                        Width = 818,
                        Dock = DockStyle.Top,
                        Padding = new Padding(0, 0, 0, 20)
                    };
                    panelRecepts.Controls.Add(row);
                }

                Panel receptPanel = new Panel()
                {
                    Height = 305,
                    Width = 381,
                    Dock = DockStyle.Left,
                    BackColor = Color.Transparent,
                    BackgroundImage = Image.FromFile(@"C:\Users\Elmar\source\repos\receptBook\receptBook\img\pin.png"),
                    BackgroundImageLayout = ImageLayout.Zoom,
                    Name = recept.Name.Replace(' ','_')
                   
                };

                receptPanel.Controls.Add(new Label
                {
                    Font = new Font("Caveat", 18, FontStyle.Bold),
                    Location = new Point(145, 37),
                    Text = recept.Name,
                    Height = 38
                });

                receptPanel.Controls.Add(new PictureBox
                {
                    Location = new Point(65, 78),
                    Image = Image.FromFile(recept.Path),
                    Width = 256,
                    Height = 193,
                    SizeMode = PictureBoxSizeMode.Zoom

                });
                receptPanel.Click += ReceptPanelClick;
                row.Controls.Add(receptPanel);
                count++;

            }
        }

        private void ReceptPanelClick(object sender, EventArgs e)
        {
            string receptName = ((Panel)sender).Name.Replace('_', ' ');
            Recept recept = recepts.First(x => x.Name == receptName);

            FormRecept formRecept = new FormRecept();
            formRecept.Recept = recept;
            formRecept.FormMain = this;
            formRecept.ShowDialog();
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            FormRecept formRecept = new FormRecept();
            formRecept.FormMain = this;
            formRecept.ShowDialog();
        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            string text = textBoxSearch.Text;
            var receptsFiltered = recepts.Where(x => x.Name.Contains(text)).ToArray();
            InitRecepts(receptsFiltered);
        }
    }
}
