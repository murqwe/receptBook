using Newtonsoft.Json;
using ReceptBook.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace receptBook
{
    public partial class FormRecept : Form
    {
        public Recept Recept { get; set; }
        public FormMain FormMain { get; set; }
        public string PathToImage { get; set; }

        private bool IsEdit = false;

        public string Path = @"C:\Users\Elmar\source\repos\receptBook\receptBook\recepts.txt";
        public FormRecept()
        {
            InitializeComponent();
        }

        private void FormRecept_Load(object sender, EventArgs e)
        {
            if (Recept != null)
            {
                pictureBox.Image = Image.FromFile(Recept.Path);
                textBoxName.Text = Recept.Name;
                textBoxDiscription.Text = Recept.Text;

                buttonAdd.Visible = false;
                buttonAddImage.Visible = false;
                buttonEdit.Visible = true;

                textBoxName.Enabled = false;
                textBoxDiscription.Enabled = false;
                buttonDelete.Visible = true;
            }
        }

        private void buttonAddImage_Click(object sender, EventArgs e)
        {
            var dialogResult = openFileDialog.ShowDialog();

            if (dialogResult == DialogResult.Cancel)
            {
                return;
            }
            PathToImage = openFileDialog.FileName;
            pictureBox.Image = Image.FromFile(PathToImage);
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            if (IsEdit)
            {
                var recept = FormMain.recepts.First(x => x.Name == Recept.Name);
                recept.Name = textBoxName.Text;
                recept.Text = textBoxDiscription.Text;
                if (PathToImage != null)
                {
                    recept.Path = PathToImage;
                }
            }
            else
            {
                Recept = new Recept(textBoxDiscription.Text, textBoxName.Text, PathToImage);
                FormMain.recepts.Add(Recept);
            }

            //сохраняем в файл
            using (StreamWriter sw = new StreamWriter(Path))
            {
                sw.WriteLine(JsonConvert.SerializeObject(FormMain.recepts));

            }

            FormMain.InitRecepts();
            Close();
        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            textBoxName.Enabled = true;
            textBoxDiscription.Enabled = true;

            buttonAddImage.Visible = true;
            buttonEdit.Visible = false;
            buttonAdd.Visible = true;
            buttonAdd.Text = "Сохранить";
            IsEdit = true;
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Вы уверены, что хотите удалить рецепт?", "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.No || result == DialogResult.Cancel)
            {
                return;
            }
            
            
            var recept = FormMain.recepts.First(x => x.Name == Recept.Name);
            FormMain.recepts.Remove(recept);

            using (StreamWriter sw = new StreamWriter(Path))
            {
                sw.WriteLine(JsonConvert.SerializeObject(FormMain.recepts));

            }

            FormMain.InitRecepts();
            Close();
        }
    }
}
