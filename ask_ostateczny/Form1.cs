using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ask_ostateczny
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        string[] registers = new string[8] { "01", "02", "03", "04", "05", "06", "07", "08" };
        static string[] registers_names = new string[8] { "AL", "AH", "BL", "BH", "CL", "CH", "DL", "DH" };
        string[] memory = new string[65536];


        void MemoryReset()
        {
            for(int i = 0; i < 65536; i++)
            {
                if (memory[i] == null)
                    memory[i] = "00";
            }
        }

        void Reset()
        {
            MemoryReset();
            label9.Text = "";
            label10.Text = "";
            label11.Text = "";
            label12.Text = "";
            label13.Text = "";
            label14.Text = "";
            label15.Text = "";
            label16.Text = "";
            label17.Text = "";
            label18.Text = "";
            textBox9.Text = "";
            textBox10.Text = "";
            textBox11.Text = "";
            textBox12.Text = "";
            textBox13.Text = "";
            textBox14.Text = "";
            textBox15.Text = "";
            textBox16.Text = "";
            textBox17.Text = "";
            textBox18.Text = "";
            textBox19.Text = "";
            textBox20.Text = "";
            textBox21.Text = "";
            textBox22.Text = "";
            textBox23.Text = "";
            textBox24.Text = "";
            textBox25.Text = "";
            textBox1.Text = registers[0];
            textBox2.Text = registers[1];
            textBox3.Text = registers[2];
            textBox4.Text = registers[3];
            textBox5.Text = registers[4];
            textBox6.Text = registers[5];
            textBox7.Text = registers[6];
            textBox8.Text = registers[7];
        }

        string NameCheck(string x)
        {
            if (x.Length == 2 && (x[0] >= 'A' && x[0] <= 'D') && (x[1] == 'H' || x[1] == 'L'))      //z rejestrów
            {
                return x;
            }
            else if(x.Length == 4 && Int32.Parse(x, System.Globalization.NumberStyles.HexNumber) > 0 && Int32.Parse(x, System.Globalization.NumberStyles.HexNumber) <= 65535) //z pamięci
            {
                return x;
            }
            else
            {
                DialogResult wynik2 = MessageBox.Show("Blednie podana nazwa. Podaj nazwe rejestru lub komórki pamięci.", " ", MessageBoxButtons.OK);
                return "error";
            }
        }

        private void button6_Click(object sender, EventArgs e)      //INC
        {
            string register = NameCheck(textBox17.Text), wynik = "000";

            if (register != "error")
            {
                if (register.Length == 2)
                {
                    int a = 0;
                    while (registers_names[a] != register)
                        a++;

                    string first = registers[a];

                    if (registers[a] == "FF")
                        registers[a] = "00";
                    else
                    {
                        int value = Int32.Parse(first, System.Globalization.NumberStyles.HexNumber);
                        value++;
                        string hex = value.ToString("X");
                        registers[a] = hex;

                        if (hex.Length == 1)
                            registers[a] = "0" + hex;
                    }
                    wynik = registers[a];
                }
                else if (registers.Length == 4)
                {
                    int i = Int32.Parse(register, System.Globalization.NumberStyles.HexNumber);

                    if (memory[i] == "FF")
                        memory[i] = "00";
                    else
                    {
                        int value = Int32.Parse(memory[i], System.Globalization.NumberStyles.HexNumber);
                        value++;
                        string hex = value.ToString("X");
                        memory[i] = hex;

                        if (hex.Length == 1)
                            memory[i] = "0" + hex;
                    }
                    wynik = memory[i];
                }
                Reset();
                textBox17.Text = register;
                label13.Text = $"Wynik = {wynik}";
                button1_Click(sender, e);
            }
        }

        private void button1_Click(object sender, EventArgs e)  //wpisywanie wartości rejestrów
        {
            MemoryReset();
            registers[0] = textBox1.Text;
            registers[1] = textBox2.Text;
            registers[2] = textBox3.Text;
            registers[3] = textBox4.Text;
            registers[4] = textBox5.Text;
            registers[5] = textBox6.Text;
            registers[6] = textBox7.Text;
            registers[7] = textBox8.Text;

            int i = 0;

            for (int znak = 0; znak < 8; znak++)
            {
                if ((registers[znak].Length != 2) || registers[znak][0] < '0' || (registers[znak][0] > '9' && registers[znak][0] < 'A') || registers[znak][0] > 'F' || (registers[znak][1] < '0' || (registers[znak][1] > '9' && registers[znak][1] < 'A') || registers[znak][1] > 'F'))
                {
                    DialogResult wynik = MessageBox.Show($"Niepopeawnie wprowadzona wartość rejestru {registers_names[znak]}. Podaj ponownie.", " ", MessageBoxButtons.OK);
                    i++;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)      //MOV
        {
            string x = NameCheck(textBox9.Text);
            string y = NameCheck(textBox10.Text);
            string hex = "0";
            if (x != "error" && y != "error")
            {
                if (x.Length == 2 && y.Length == 2)
                {
                    int i = 0, j = 0;
                    while (x != registers_names[i])
                        i++;

                    while (y != registers_names[j])
                        j++;

                    registers[i] = registers[j];
                    hex = registers[i];
                }
                else if (x.Length == 2 && y.Length == 4)
                {
                    int j = 0;
                    while (x != registers_names[j])
                        j++;

                    int index = Int32.Parse(y, System.Globalization.NumberStyles.HexNumber);
                    registers[j] = memory[index];
                    hex = registers[j];
                }
                else if (x.Length == 4 && y.Length == 2)
                {
                    int i = 0;
                    while (y != registers_names[i])
                        i++;

                    int index = Int32.Parse(x, System.Globalization.NumberStyles.HexNumber);
                    memory[index] = registers[i];
                    hex = memory[index];
                }
                else if (x.Length == 4 && y.Length == 4)
                {
                    int i = Int32.Parse(x, System.Globalization.NumberStyles.HexNumber);
                    int j = Int32.Parse(y, System.Globalization.NumberStyles.HexNumber);
                    memory[i] = memory[j];
                    hex = memory[i];
                }
                Reset();
                textBox9.Text = x;
                textBox10.Text = y;
                label9.Text = $"Wynik = {hex}";
                button1_Click(sender, e);
            }
        }

        private void button3_Click(object sender, EventArgs e)      //ADD
        {
            string x = NameCheck(textBox11.Text);
            string y = NameCheck(textBox12.Text);
            string hex = "00";

            if (x != "error" && y != "error")
            {
                if (x.Length == 2 && y.Length == 2)
                {
                    int a = 0;
                    while (registers_names[a] != x)
                        a++;

                    string first = registers[a];

                    int b = 0;
                    while (registers_names[b] != y)
                        b++;

                    string second = registers[b];

                    int value1 = Int32.Parse(first, System.Globalization.NumberStyles.HexNumber);
                    int value2 = Int32.Parse(second, System.Globalization.NumberStyles.HexNumber);

                    int value = value1 + value2;

                    if (value > 255)
                        value -= 255;

                    hex = value.ToString("X");

                    if (hex.Length == 1)
                        hex = "0" + hex;

                    registers[a] = hex;
                }
                else if (x.Length == 2 && y.Length == 4 || x.Length == 4 && y.Length == 2)
                {
                    int a = 0, index;
                    string first, second;
                    if (x.Length == 2)
                    {
                        while (registers_names[a] != x)
                            a++;
                        first = registers[a];
                        index = Int32.Parse(y, System.Globalization.NumberStyles.HexNumber);
                        second = memory[index];
                    }
                    else
                    {
                        while (registers_names[a] != y)
                            a++;
                        second = registers[a];
                        index = Int32.Parse(x, System.Globalization.NumberStyles.HexNumber);
                        first = memory[index];
                    }

                    int value1 = Int32.Parse(first, System.Globalization.NumberStyles.HexNumber);
                    int value2 = Int32.Parse(second, System.Globalization.NumberStyles.HexNumber);

                    int value = value1 + value2;

                    if (value > 255)
                        value -= 255;

                    hex = value.ToString("X");

                    if (hex.Length == 1)
                        hex = "0" + hex;
                    if (x.Length == 2)
                        registers[a] = hex;
                    else
                        memory[index] = hex;
                }

                else if (x.Length == 4 && y.Length == 4)
                {
                    int index1 = Int32.Parse(x, System.Globalization.NumberStyles.HexNumber);
                    string first = memory[index1];

                    int index = Int32.Parse(y, System.Globalization.NumberStyles.HexNumber);
                    string second = memory[index];

                    int value1 = Int32.Parse(first, System.Globalization.NumberStyles.HexNumber);
                    int value2 = Int32.Parse(second, System.Globalization.NumberStyles.HexNumber);

                    int value = value1 + value2;

                    if (value > 255)
                        value -= 255;

                    hex = value.ToString("X");

                    if (hex.Length == 1)
                        hex = "0" + hex;

                    memory[index1] = hex;
                }
                Reset();
                textBox11.Text = x;
                textBox12.Text = y;
                label10.Text = $"Wynik = {hex}";
                button1_Click(sender, e);
            }
        }

        private void button4_Click(object sender, EventArgs e)      //SUB
        {
            string x = NameCheck(textBox13.Text);
            string y = NameCheck(textBox14.Text);
            string hex = "00";

            if (x != "error" && y != "error")
            {
                if (x.Length == 2 && y.Length == 2)
                {
                    int a = 0, b = 0;
                    while (registers_names[a] != x)
                        a++;

                    string first = registers[a];

                    while (registers_names[b] != y)
                        b++;

                    string second = registers[b];

                    int value1 = Int32.Parse(first, System.Globalization.NumberStyles.HexNumber);
                    int value2 = Int32.Parse(second, System.Globalization.NumberStyles.HexNumber);
                    int value;

                    if (value1 < value2)
                        value = value1 - value2 + 256;
                    else
                        value = value1 - value2;

                    hex = value.ToString("X");
                    if (hex.Length == 1)
                        hex = "0" + hex;

                    registers[a] = hex;
                }
                else if (x.Length == 2 && y.Length == 4)
                {
                    int a = 0;

                    while (registers_names[a] != x)
                        a++;

                    string first = registers[a];
                    int index = Int32.Parse(y, System.Globalization.NumberStyles.HexNumber);
                    string second = memory[index];

                    int value1 = Int32.Parse(first, System.Globalization.NumberStyles.HexNumber);
                    int value2 = Int32.Parse(second, System.Globalization.NumberStyles.HexNumber);
                    int value;

                    if (value1 < value2)
                        value = value1 - value2 + 256;
                    else
                        value = value1 - value2;

                    hex = value.ToString("X");
                    if (hex.Length == 1)
                        hex = "0" + hex;

                    registers[a] = hex;
                }
                else if (x.Length == 4 && y.Length == 2)
                {
                    int a = 0;

                    while (registers_names[a] != y)
                        a++;

                    string second = registers[a];
                    int index = Int32.Parse(x, System.Globalization.NumberStyles.HexNumber);
                    string first = memory[index];

                    int value1 = Int32.Parse(first, System.Globalization.NumberStyles.HexNumber);
                    int value2 = Int32.Parse(second, System.Globalization.NumberStyles.HexNumber);
                    int value;

                    if (value1 < value2)
                        value = value1 - value2 + 256;
                    else
                        value = value1 - value2;

                    hex = value.ToString("X");
                    if (hex.Length == 1)
                        hex = "0" + hex;

                    memory[index] = hex;
                }
                else if (x.Length == 4 && y.Length == 4)
                {
                    int index1 = Int32.Parse(x, System.Globalization.NumberStyles.HexNumber);
                    string first = memory[index1];
                    int index = Int32.Parse(y, System.Globalization.NumberStyles.HexNumber);
                    string second = memory[index];

                    int value1 = Int32.Parse(first, System.Globalization.NumberStyles.HexNumber);
                    int value2 = Int32.Parse(second, System.Globalization.NumberStyles.HexNumber);
                    int value;

                    if (value1 < value2)
                        value = value1 - value2 + 256;
                    else
                        value = value1 - value2;

                    hex = value.ToString("X");
                    if (hex.Length == 1)
                        hex = "0" + hex;

                    memory[index1] = hex;
                }
                Reset();
                textBox13.Text = y;
                textBox14.Text = x;
                label11.Text = $"Wynik = {hex}";
                button1_Click(sender, e);
            }
        }

        private void button5_Click(object sender, EventArgs e)      //XCHG
        {
            string x = NameCheck(textBox15.Text);
            string y = NameCheck(textBox16.Text);
            string first = "00", second = "00";

            if (x != "error" && y != "error")
            {
                if (x.Length == 2 && y.Length == 4)
                {
                    int a = 0;
                    while (registers_names[a] != x)
                        a++;

                    first = registers[a];

                    int index = Int32.Parse(y, System.Globalization.NumberStyles.HexNumber);
                    second = memory[index];

                    int value1 = Int32.Parse(first, System.Globalization.NumberStyles.HexNumber);
                    int value2 = Int32.Parse(second, System.Globalization.NumberStyles.HexNumber);

                    string val = registers[a];
                    registers[a] = memory[index];
                    memory[index] = val;
                }
                else if (x.Length == 2 && y.Length == 2)
                {
                    int a = 0;
                    while (registers_names[a] != x)
                        a++;

                    first = registers[a];

                    int b = 0;
                    while (registers_names[b] != y)
                        b++;

                    second = registers[b];

                    int value1 = Int32.Parse(first, System.Globalization.NumberStyles.HexNumber);
                    int value2 = Int32.Parse(second, System.Globalization.NumberStyles.HexNumber);

                    string val = registers[a];
                    registers[a] = registers[b];
                    registers[b] = val;
                }
                else if (x.Length == 4 && y.Length == 2)
                {
                    int index = Int32.Parse(x, System.Globalization.NumberStyles.HexNumber);
                    first = memory[index];

                    int b = 0;
                    while (registers_names[b] != y)
                        b++;

                    second = registers[b];

                    int value1 = Int32.Parse(first, System.Globalization.NumberStyles.HexNumber);
                    int value2 = Int32.Parse(second, System.Globalization.NumberStyles.HexNumber);

                    string val = memory[index];
                    memory[index] = registers[b];
                    registers[b] = val;
                }
                else if (x.Length == 4 && y.Length == 4)
                {
                    int index = Int32.Parse(x, System.Globalization.NumberStyles.HexNumber);
                    first = memory[index];

                    int index1 = Int32.Parse(y, System.Globalization.NumberStyles.HexNumber);
                    second = memory[index1];

                    int value1 = Int32.Parse(first, System.Globalization.NumberStyles.HexNumber);
                    int value2 = Int32.Parse(second, System.Globalization.NumberStyles.HexNumber);

                    string val = memory[index];
                    memory[index] = memory[index1];
                    memory[index1] = val;
                }
                Reset();
                textBox15.Text = x;
                textBox16.Text = y;
                label12.Text = $"{first} <-> {second}";
                button1_Click(sender, e);
            }
        }

        private void button7_Click(object sender, EventArgs e)      //DEC
        {
            string x = NameCheck(textBox18.Text), hex = "00";

            if (x != "error")
            {
                if (x.Length == 2)
                {
                    int a = 0;
                    while (registers_names[a] != x)
                        a++;

                    string first = registers[a];

                    if (registers[a] == "00")
                        registers[a] = "FF";
                    else
                    {
                        int value = Int32.Parse(first, System.Globalization.NumberStyles.HexNumber);
                        value--;
                        hex = value.ToString("X");
                        registers[a] = hex;

                        if (hex.Length == 1)
                            registers[a] = "0" + hex;
                    }
                }
                else if (x.Length == 4)
                {
                    int index = Int32.Parse(x, System.Globalization.NumberStyles.HexNumber);
                    string first = memory[index];

                    if (memory[index] == "00")
                        memory[index] = "FF";
                    else
                    {
                        int value = Int32.Parse(first, System.Globalization.NumberStyles.HexNumber);
                        value--;
                        hex = value.ToString("X");
                        memory[index] = hex;

                        if (hex.Length == 1)
                            memory[index] = "0" + hex;
                    }
                }
                Reset();
                textBox18.Text = x;
                label14.Text = $"Wynik = {hex}";
                button1_Click(sender, e);
            }
        }

        private void button8_Click(object sender, EventArgs e)      //AND
        {
            string first = NameCheck(textBox19.Text);
            string second = NameCheck(textBox20.Text);
            string name = first, name2 = second;
            string hex = "00";

            if (first != "error" && second != "error")
            {
                if (first.Length == 2 && second.Length == 2)
                {
                    int a = 0;
                    while (registers_names[a] != first)
                        a++;

                    first = registers[a];

                    int b = 0;
                    while (registers_names[b] != second)
                        b++;

                    second = registers[b];

                    int value1 = Int32.Parse(first, System.Globalization.NumberStyles.HexNumber);
                    int value2 = Int32.Parse(second, System.Globalization.NumberStyles.HexNumber);

                    int value = value1 & value2;
                    hex = value.ToString("X");

                    if (hex.Length == 1)
                        hex = "0" + hex;

                    registers[a] = hex;
                }
                else if (first.Length == 2 && second.Length == 4)
                {
                    int a = 0;
                    while (registers_names[a] != first)
                        a++;

                    first = registers[a];

                    int index = Int32.Parse(second, System.Globalization.NumberStyles.HexNumber);
                    second = memory[index];

                    int value1 = Int32.Parse(first, System.Globalization.NumberStyles.HexNumber);
                    int value2 = Int32.Parse(second, System.Globalization.NumberStyles.HexNumber);

                    int value = value1 & value2;
                    hex = value.ToString("X");

                    if (hex.Length == 1)
                        hex = "0" + hex;

                    registers[a] = hex;
                }
                else if (first.Length == 4 && second.Length == 2)
                {
                    int a = 0;
                    while (registers_names[a] != second)
                        a++;

                    second = registers[a];

                    int index = Int32.Parse(first, System.Globalization.NumberStyles.HexNumber);
                    first = memory[index];

                    int value1 = Int32.Parse(first, System.Globalization.NumberStyles.HexNumber);
                    int value2 = Int32.Parse(second, System.Globalization.NumberStyles.HexNumber);

                    int value = value1 & value2;
                    hex = value.ToString("X");

                    if (hex.Length == 1)
                        hex = "0" + hex;

                    memory[index] = hex;
                }
                else if (first.Length == 4 && second.Length == 4)
                {
                    int index1 = Int32.Parse(first, System.Globalization.NumberStyles.HexNumber);
                    first = memory[index1];

                    int index = Int32.Parse(second, System.Globalization.NumberStyles.HexNumber);
                    second = memory[index];

                    int value1 = Int32.Parse(first, System.Globalization.NumberStyles.HexNumber);
                    int value2 = Int32.Parse(second, System.Globalization.NumberStyles.HexNumber);

                    int value = value1 & value2;
                    hex = value.ToString("X");

                    if (hex.Length == 1)
                        hex = "0" + hex;

                    memory[index1] = hex;
                }
                Reset();
                textBox19.Text = name;
                textBox20.Text = name2;
                label15.Text = $"Wynik = {hex}";
                button1_Click(sender, e);
            }
        }

        private void button9_Click(object sender, EventArgs e)      //OR
        {
            string first = NameCheck(textBox21.Text);
            string second = NameCheck(textBox22.Text);
            string x = first, y = second;
            string hex = "00";

            if (first != "error" && second != "error")
            {
                if (first.Length == 2 && second.Length == 2)
                {
                    int a = 0;
                    while (registers_names[a] != first)
                        a++;

                    first = registers[a];

                    int b = 0;
                    while (registers_names[b] != second)
                        b++;

                    second = registers[b];

                    int value1 = Int32.Parse(first, System.Globalization.NumberStyles.HexNumber);
                    int value2 = Int32.Parse(second, System.Globalization.NumberStyles.HexNumber);

                    int value = value1 | value2;
                    hex = value.ToString("X");

                    if (hex.Length == 1)
                        hex = "0" + hex;

                    registers[a] = hex;
                }
                else if (first.Length == 2 && second.Length == 4)
                {
                    int a = 0;
                    while (registers_names[a] != first)
                        a++;

                    first = registers[a];

                    int index = Int32.Parse(second, System.Globalization.NumberStyles.HexNumber);
                    second = memory[index];

                    int value1 = Int32.Parse(first, System.Globalization.NumberStyles.HexNumber);
                    int value2 = Int32.Parse(second, System.Globalization.NumberStyles.HexNumber);

                    int value = value1 | value2;
                    hex = value.ToString("X");

                    if (hex.Length == 1)
                        hex = "0" + hex;

                    registers[a] = hex;
                }
                else if (first.Length == 4 && second.Length == 2)
                {
                    int a = 0;
                    while (registers_names[a] != second)
                        a++;

                    second = registers[a];

                    int index = Int32.Parse(first, System.Globalization.NumberStyles.HexNumber);
                    first = memory[index];

                    int value1 = Int32.Parse(first, System.Globalization.NumberStyles.HexNumber);
                    int value2 = Int32.Parse(second, System.Globalization.NumberStyles.HexNumber);

                    int value = value1 | value2;
                    hex = value.ToString("X");

                    if (hex.Length == 1)
                        hex = "0" + hex;

                    memory[index] = hex;
                }
                else if (first.Length == 4 && second.Length == 4)
                {
                    int index = Int32.Parse(first, System.Globalization.NumberStyles.HexNumber);
                    first = memory[index];

                    int index1 = Int32.Parse(second, System.Globalization.NumberStyles.HexNumber);
                    second = memory[index1];

                    int value1 = Int32.Parse(first, System.Globalization.NumberStyles.HexNumber);
                    int value2 = Int32.Parse(second, System.Globalization.NumberStyles.HexNumber);

                    int value = value1 | value2;
                    hex = value.ToString("X");

                    if (hex.Length == 1)
                        hex = "0" + hex;

                    memory[index] = hex;
                }
                Reset();
                textBox21.Text = x;
                textBox22.Text = y;
                label16.Text = $"Wynik = {hex}";
                button1_Click(sender, e);
            }
        }

        private void button10_Click(object sender, EventArgs e)     //XOR
        {
            string x = NameCheck(textBox23.Text), y = NameCheck(textBox24.Text), hex = "000";

            if (x != "error" && y != "error")
            {
                if (x.Length == 2 && y.Length == 2)
                {
                    int a = 0;
                    while (registers_names[a] != x)
                        a++;

                    string first = registers[a];

                    int b = 0;
                    while (registers_names[b] != y)
                        b++;

                    string second = registers[b];

                    int value1 = Int32.Parse(first, System.Globalization.NumberStyles.HexNumber);
                    int value2 = Int32.Parse(second, System.Globalization.NumberStyles.HexNumber);

                    int value = value1 ^ value2;

                    hex = value.ToString("X");

                    if (hex.Length == 1)
                        hex = "0" + hex;

                    registers[a] = hex;
                }
                else if (x.Length == 2 && y.Length == 4)
                {
                    int a = 0;
                    while (registers_names[a] != x)
                        a++;

                    string first = registers[a];

                    int index = Int32.Parse(y, System.Globalization.NumberStyles.HexNumber);
                    string second = memory[index];

                    int value1 = Int32.Parse(first, System.Globalization.NumberStyles.HexNumber);
                    int value2 = Int32.Parse(second, System.Globalization.NumberStyles.HexNumber);

                    int value = value1 ^ value2;

                    hex = value.ToString("X");

                    if (hex.Length == 1)
                        hex = "0" + hex;

                    registers[a] = hex;
                }
                else if (x.Length == 4 && y.Length == 2)
                {
                    int index = Int32.Parse(x, System.Globalization.NumberStyles.HexNumber);
                    string first = memory[index];

                    int b = 0;
                    while (registers_names[b] != y)
                        b++;

                    string second = registers[b];

                    int value1 = Int32.Parse(first, System.Globalization.NumberStyles.HexNumber);
                    int value2 = Int32.Parse(second, System.Globalization.NumberStyles.HexNumber);

                    int value = value1 ^ value2;

                    hex = value.ToString("X");

                    if (hex.Length == 1)
                        hex = "0" + hex;

                    memory[index] = hex;
                }
                else if (x.Length == 4 && y.Length == 4)
                {
                    int index = Int32.Parse(x, System.Globalization.NumberStyles.HexNumber);
                    string first = memory[index];

                    int index1 = Int32.Parse(y, System.Globalization.NumberStyles.HexNumber);
                    string second = memory[index1];

                    int value1 = Int32.Parse(first, System.Globalization.NumberStyles.HexNumber);
                    int value2 = Int32.Parse(second, System.Globalization.NumberStyles.HexNumber);

                    int value = value1 ^ value2;

                    hex = value.ToString("X");

                    if (hex.Length == 1)
                        hex = "0" + hex;

                    memory[index] = hex;
                }
                Reset();
                textBox23.Text = x;
                textBox24.Text = y;
                label17.Text = $"Wynik = {hex}";
                button1_Click(sender, e);
            }
        }

        private void button11_Click(object sender, EventArgs e)     //NOT
        {
            string x = NameCheck(textBox25.Text), hex = "00";

            if (x != "error")
            {
                if (x.Length == 2)
                {
                    int a = 0;
                    while (registers_names[a] != x)
                        a++;
                    string first = registers[a];

                    int value = Int32.Parse(first, System.Globalization.NumberStyles.HexNumber);
                    
                    int pom = 128, number = 0;
                    while (pom != 0)
                    {
                        if (value >= pom)
                            value -= pom;
                        else
                            number += pom;
                        pom /= 2;
                    }

                    hex = number.ToString("X");

                    if (hex.Length == 1)
                        hex = "0" + hex;

                    registers[a] = hex;
                }
                else if (x.Length == 4)
                {
                    int index = Int32.Parse(x, System.Globalization.NumberStyles.HexNumber);
                    string first = memory[index];

                    int value = Int32.Parse(first, System.Globalization.NumberStyles.HexNumber);

                    int pom = 128, number = 0;
                    while (pom != 0)
                    {
                        if (value >= pom)
                            value -= pom;
                        else
                            number += pom;
                        pom /= 2;
                    }

                    hex = number.ToString("X");

                    if (hex.Length == 1)
                        hex = "0" + hex;

                    memory[index] = hex;
                }
                Reset();
                textBox25.Text = x;
                label18.Text = $"Wynik = {hex}";
                button1_Click(sender, e);
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            registers[0] = "00";
            registers[1] = "11";
            registers[2] = "22";
            registers[3] = "33";
            registers[4] = "44";
            registers[5] = "55";
            registers[6] = "66";
            registers[7] = "77";
            textBox1.Text = registers[0];
            textBox2.Text = registers[1];
            textBox3.Text = registers[2];
            textBox4.Text = registers[3];
            textBox5.Text = registers[4];
            textBox6.Text = registers[5];
            textBox7.Text = registers[6];
            textBox8.Text = registers[7];
            MemoryReset();

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
    
}
