using System;
using System.Collections.Generic;
using System.Text;

namespace ReceptBook.Models
{
    public class Recept
    {
        public string Text { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }

        public Recept(string text, string name, string path)
        {
            Text = text;
            Name = name;
            Path = path;
        }
    }
}
