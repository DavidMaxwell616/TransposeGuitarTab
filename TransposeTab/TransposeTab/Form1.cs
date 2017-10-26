using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TransposeTab
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private string[] lines;
        private string[] newlines;
        private string FileName;

        private void Load_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Text Files|*.txt";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
                FileName = openFileDialog1.FileName;
            else
                return;
            lines = System.IO.File.ReadAllLines(FileName);
            listBox1.Items.Clear();
            listBox1.Items.AddRange(lines);

        }

        private void Close_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private string[] ScaleDown = {"C", "B", "Bb", "A", "Ab", "G", "Gb", "F", "E", "Eb", "D", "Db"};
        private string[] ScaleUp = {"C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "A#", "B"};

        private void Down_Click(object sender, EventArgs e)
        {
            Transpose(-1);
        }

        private void Up_Click(object sender, EventArgs e)
        {
            Transpose(1);
        }

        private void Transpose(int direction)
        {
            int lineNum = 1;
            foreach (var item in lines)
            {
                if (item.ToString().Length > 0 && lineNum > 2)
                {
                    Char[] letters = item.ToString().ToCharArray();
                    if (letters[0] == '-')
                    {
                        for (int index = 0; index < letters.Length; index++)
                        {
                            if (Char.IsNumber(letters[index]))
                            {
                                int oldValue = Convert.ToInt16(letters[index].ToString());
                                int newValue = 0;
                                if (Char.IsNumber(letters[index + 1]))
                                {
                                    oldValue = (Convert.ToInt16(letters[index].ToString())*10) +
                                               (Convert.ToInt16(letters[index + 1].ToString()));
                                }

                                newValue = oldValue + direction;
                                if (newValue < 0)
                                    Debugger.Break();
                                if (newValue < 10 && oldValue > 9)
                                {
                                    letters[index] = '-';
                                    letters[index + 1] = newValue.ToString().ToCharArray()[0];
                                    index++;
                                }
                                else if (newValue < 10 && oldValue < 10)
                                {
                                    letters[index] = newValue.ToString().ToCharArray()[0];
                                }
                                else
                                {
                                    letters[index] = newValue.ToString().ToCharArray()[0];
                                    letters[index + 1] = newValue.ToString().ToCharArray()[1];
                                    index++;
                                }

                                if (index > letters.Length)
                                    break;

                            }
                        }
                        lines[lineNum - 1] = new String(letters);
                    }
                    else
                    {
                        for (int index = 1; index < letters.Length; index++)
                        {
                            if (Char.IsLetter(letters[index]) && Char.IsWhiteSpace(letters[index - 1]) &&
                                Char.IsUpper(letters[index]) &&
                                Array.Exists(ScaleUp, letter => letter == letters[index].ToString()))
                            {
                                if (index + 1 == letters.Length || letters[index + 1] == 'm' ||
                                    Char.IsNumber(letters[index + 1]) || Char.IsWhiteSpace(letters[index + 1]))
                                {
                                    string oldValue = letters[index].ToString();
                                    string newValue = String.Empty;
                                    if(direction<0)
                                    { 
                                        int oldValIndex = Array.IndexOf(ScaleDown, oldValue);
                                        if (oldValIndex < ScaleDown.Length)
                                            newValue = ScaleDown[oldValIndex + 1];
                                        else
                                            newValue = ScaleDown.First();

                                    }
                                    else
                                    {
                                        int oldValIndex = Array.IndexOf(ScaleDown, oldValue);
                                        if (oldValIndex < ScaleUp.Length)
                                            newValue = ScaleUp[oldValIndex + 1];
                                        else
                                            newValue = ScaleUp.First();
                                    }
                                    if (newValue.Length > 1)
                                    {
                                        letters[index - 1] = newValue.ToCharArray()[0];
                                        letters[index] = newValue.ToCharArray()[1];
                                    }
                                    else
                                        letters[index] = newValue.ToCharArray()[0];
                                }
                            }
                        }
                        lines[lineNum - 1] = new String(letters);

                    }
                }
                lineNum++;
            }
            listBox1.Items.Clear();
            listBox1.Items.AddRange(lines);
        }


        private void Clipboard_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Clipboard.SetText(ConvertStringArrayToString(lines));
        }

        static string ConvertStringArrayToString(string[] array)
        {
            // Concatenate all the elements into a StringBuilder.
            StringBuilder builder = new StringBuilder();
            foreach (string value in array)
            {
                builder.AppendLine(value);
            }
            return builder.ToString();
        }

 

    }
}
