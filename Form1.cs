using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FormCalculator
{
    public partial class Form1 : Form
    {
        Calc calc;

        public Form1() {
            InitializeComponent();
            calc = new Calc();
        }

        private void button2_MouseClick(object sender, MouseEventArgs e) {
            calc.history.Add((sender as Button).Text);
            calc.countZnak = 0;

            if (calc.IsClickSign) {
                calc.IsClickSign = false;
                paneloutput.Text = null;
            }
            if (!calc.IsClickSign)
                if (paneloutput.Text == "0")
                    paneloutput.Text = null;
            paneloutput.Text += (sender as Button).Text;
        }

        private void button16_Click(object sender, EventArgs e) {  // *-+/
            if (calc.countZnak == 0)
                calc.history.Add((sender as Button).Text);
            calc.countZnak++; //знак использовался
            calc.IsComma = false;  //запятая не использовалась
            panelZnak.Text = (sender as Button).Text;
            if (calc.IsEqually)
                textSave.Text = null;
            if (calc.countZnak > 1) { //смена знака
                if ((sender as Button).Text != calc.history[calc.history.Count - 1] && !calc.IsEqually) {
                    calc.history.Add((sender as Button).Text);
                    calc.znak = (sender as Button).Text[0];
                    panelZnak.Text = (sender as Button).Text;
                    textSave.Text = textSave.Text.Remove(textSave.Text.Length - 2, 2) + (sender as Button).Text + " ";
                }
                return;
            }

            //if (paneloutput.Text.Length > 1 && paneloutput.Text[0] == '0' && paneloutput.Text[1] != ',') { //удаление лишних нулей
            //    RemoveZero();  //удаление лишних нулей
            //}
            else if (paneloutput.Text != "" && paneloutput.Text[0] == ',')
                paneloutput.Text = "0" + paneloutput.Text;  //добавление нуля перед запятой
            calc.summ = Convert.ToDouble(paneloutput.Text); //запоминание числа в поле
            if (!calc.IsNum1) {
                if (paneloutput.Text == "") {
                    calc.num1 = 0;
                    paneloutput.Text = "0";
                }
                else
                    calc.num1 = Convert.ToDouble(paneloutput.Text);
                calc.IsNum1 = true;
            }
            else {
                if (!calc.IsEqually)
                    switch (calc.znak) {
                        case '+':
                            calc.num1 += Convert.ToDouble(paneloutput.Text);
                            break;
                        case '-':
                            calc.num1 -= Convert.ToDouble(paneloutput.Text);
                            break;
                        case '*':
                            calc.num1 *= Convert.ToDouble(paneloutput.Text);
                            break;
                        case '/':
                            calc.num1 /= Convert.ToDouble(paneloutput.Text);
                            break;
                    }
                paneloutput.Text = calc.num1.ToString();
            }
            if (!calc.IsX2)
                textSave.Text += calc.summ + " ";   //записать в историю число
            textSave.Text += (sender as Button).Text + " "; //записать в историю знак
            calc.IsX2 = false; //умножение само на себя
            calc.znak = (sender as Button).Text[0];
            calc.IsClickSign = true; //если нажат знак (/*-+) при следующем вводе сотрётся поле
            calc.IsEqually = false; //нажат ли знак 'равно'
        }

        private void RemoveZero() {
            paneloutput.Text = paneloutput.Text.Remove(0, 1);
            if (paneloutput.Text.Length > 1 && paneloutput.Text[0] == '0' && paneloutput.Text[1] == '0')
                RemoveZero();
            else
                return;
        }

        private void button1_MouseClick(object sender, MouseEventArgs e) { //запятая 
            if (calc.countZnak > 0)
                paneloutput.Text = null;

            if (!paneloutput.Text.Contains(","))
                paneloutput.Text += (sender as Button).Text;

            if (paneloutput.Text[0] == ',') {
                paneloutput.Text = '0' + paneloutput.Text;
                calc.IsClickSign = false;
            }


            calc.IsComma = true; //запятая использовалась
        }

        private void button17_MouseClick(object sender, MouseEventArgs e) {   //равно 
            panelZnak.Text = "=";
            calc.IsX2 = false;  //сброс
            calc.IsComma = false;   //сброс
            if (paneloutput.Text != "" && paneloutput.Text[0] == ',') {   //добавление нуля перед запятой //----------------
                paneloutput.Text = "0" + paneloutput.Text;
            }
            if (!(calc.znak == '+' ||
                calc.znak == '-' ||
                calc.znak == '*' ||
                calc.znak == '/')) {
                return;   //нет знака - нет истории
            }
            if (!calc.IsX2) { //проверка на то что бы не записало в историю число из корня и действия квадрата
                if (calc.IsEqually)
                    textSave.Text += calc.znak + " ";
                if (!calc.IsEqually)
                    textSave.Text += paneloutput.Text + " ";
                else
                    textSave.Text += calc.num2 + " ";
            }
            if (paneloutput.Text == "") {  //что бы не было ошибки что поле пустое, заполняется 0
                paneloutput.Text = "0";
                calc.IsEqually = true;
                return;
            }

            if (!calc.IsEqually)
                calc.num2 = Convert.ToDouble(paneloutput.Text);
            calc.IsEqually = true;

            switch (calc.znak) {
                case '+':
                    calc.num1 += calc.num2;
                    break;
                case '-':
                    calc.num1 -= calc.num2;
                    break;
                case '*':
                    calc.num1 *= calc.num2;
                    break;
                case '/':
                    if (calc.num2 != 0)
                        calc.num1 /= calc.num2;
                    else {
                        paneloutput.Text = "Error";   //деление на ноль
                        return;
                    }
                    break;
            }
            paneloutput.Text = calc.num1.ToString();
        }

        private void button20_Click(object sender, EventArgs e) {  //стирает всё
            calc = new Calc();
            paneloutput.Text = "0";
            panelZnak.Text = null;
            textSave.Text = null;
        }

        private void button19_Click(object sender, EventArgs e) { //стирает строку
            paneloutput.Text = "0";
        }

        private void button18_Click(object sender, EventArgs e) {   //стирает 1 цифру
            if (paneloutput.Text != null && paneloutput.Text != "")
                paneloutput.Text = paneloutput.Text.Remove(paneloutput.Text.Length - 1, 1);
            if (paneloutput.Text == "")
                paneloutput.Text = "0";
        }

        private void button3_Click(object sender, EventArgs e) { //меняет знак -+
            if (paneloutput.Text[0] != '-') {
                paneloutput.Text = '-' + paneloutput.Text;
            }
            else {
                paneloutput.Text = paneloutput.Text.Remove(0, 1);
            }
            if (!calc.IsNum1)
                calc.num1 = Convert.ToDouble(paneloutput.Text);
            else
                calc.num2 = Convert.ToDouble(paneloutput.Text);
        }

        private void button21_MouseClick(object sender, MouseEventArgs e) { //умножить на само себя
            double x = Convert.ToDouble(paneloutput.Text);
            x *= x;
            if (!calc.IsX2)
                textSave.Text += paneloutput.Text + " * " + paneloutput.Text + " ";
            paneloutput.Text = x.ToString();
            if (!calc.IsNum1) {
                calc.num1 = x;
                calc.IsNum1 = true;
            }
            else
                calc.num2 = x;
            calc.IsX2 = true;
        }

        private void button22_MouseClick(object sender, MouseEventArgs e) { //корень из числа
            double x = Convert.ToDouble(paneloutput.Text);
            x = Math.Sqrt(x);
            if (!calc.IsX2)
                textSave.Text += "√" + paneloutput.Text;
            paneloutput.Text = x.ToString();
            if (!calc.IsNum1) {
                calc.num1 = x;
                calc.IsNum1 = true;
            }
            else
                calc.num2 = x;
            calc.IsX2 = true;
        }

        private void button16_KeyPress(object sender, KeyPressEventArgs e) {  //ввод цифр с клавиатуры
            calc.symbolKey = e.KeyChar;
            switch (calc.symbolKey) {
                case '0':
                    button2_MouseClick(button2, null);
                    break;
                case '1':
                    button2_MouseClick(button4, null);
                    break;
                case '2':
                    button2_MouseClick(button7, null);
                    break;
                case '3':
                    button2_MouseClick(button8, null);
                    break;
                case '4':
                    button2_MouseClick(button5, null);
                    break;
                case '5':
                    button2_MouseClick(button6, null);
                    break;
                case '6':
                    button2_MouseClick(button9, null);
                    break;
                case '7':
                    button2_MouseClick(button11, null);
                    break;
                case '8':
                    button2_MouseClick(button10, null);
                    break;
                case '9':
                    button2_MouseClick(button12, null);
                    break;

                case '+':
                    button16_Click(button13, null);
                    break;
                case '-':
                    button16_Click(button14, null);
                    break;
                case '*':
                    button16_Click(button15, null);
                    break;
                case '/':
                    button16_Click(button16, null);
                    break;

                case '.':
                    button1_MouseClick(button1, null);
                    break;
                case ',':
                    button1_MouseClick(button1, null);
                    break;

                case '=':
                    button17_MouseClick(button17, null);
                    break;
            }
        }

        bool click = false;
        int CoorX;
        int CoorY;

        private void panel1_MouseUp(object sender, MouseEventArgs e) {
            click = false;
            this.Padding = new Padding(0);
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e) {
            if (click) 
                this.SetDesktopLocation(MousePosition.X - CoorX, MousePosition.Y - CoorY);
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e) {
            click = true;
            CoorX = e.X;
            CoorY = e.Y;
            this.Padding = new Padding(1);
        }

        private void button23_MouseClick(object sender, MouseEventArgs e) {
            this.Close();
        }

        private void button23_MouseLeave(object sender, EventArgs e) {
            button23.BackColor = Color.FromArgb(2, 3, 6);
            button23.ForeColor = Color.Gray;
        }

        private void button23_MouseEnter(object sender, EventArgs e) {
            button23.BackColor = Color.Red;
            button23.ForeColor = Color.White;
        }

        private void button16_KeyDown(object sender, KeyEventArgs e) {
            var key = e.KeyCode;
            switch (key) {
                case Keys.Delete:
                    button20_Click(button20, null);
                    break;
            }
        }
    }
}
