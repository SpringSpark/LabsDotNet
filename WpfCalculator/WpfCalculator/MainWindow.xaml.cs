using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfCalculator
{
    public partial class MainWindow : Window
    {
        string left = "";
        int left_sign = 1;
        string operation = ""; 
        string right = "";

        public MainWindow()
        {
            InitializeComponent();
            foreach (UIElement c in LayoutRoot.Children)
            {
                if (c is Button)
                {
                    ((Button)c).Click += Button_Click;
                }
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string s = (string)((Button)e.OriginalSource).Content;            
            int num;
            if(Int32.TryParse(s, out num))
            {
                if(operation == "")
                {
                    left += s;
                    textBlock.Text += s;
                }
                else if(operation == "=" && right == "")
                {
                    textBlock.Text = s;
                    right += s;
                }
                else
                {
                    right += s;
                    textBlock.Text += s;
                }                
            }
            else if(s == ",")
            {
                if (left == "")
                {
                    left += "0,";
                    textBlock.Text += "0";
                }
                else
                {
                    if (operation == "")
                    {
                        left += ",";
                    }
                    else
                    {
                        if (right == "")
                        {
                            right += "0,";
                            textBlock.Text += "0";
                        }
                        else
                        {
                            right += ",";
                        }
                    }
                }
                textBlock.Text += ",";
            }
            else if (s == "CLEAR")
            {
                left = "";
                right = "";
                operation = "";
                textBlock.Text = "";
                answer.Text = "";
            }
            else if(s == "DEL")
            {
                if(left != "")
                {
                    if(operation != "")
                    {
                        if(right != "")
                        {
                            right = right.Remove(right.Length - 1, 1);
                        }
                        else
                        {
                            operation = "";
                        }
                    }
                    else
                    {
                        left = left.Remove(left.Length - 1, 1);
                    }
                    textBlock.Text = textBlock.Text.Remove(textBlock.Text.Length - 1, 1);
                }
            }
            else
            {                
                if (operation != "" && right == "")
                {
                    textBlock.Text = textBlock.Text.Remove(textBlock.Text.Length - 1, 1);
                    textBlock.Text += s;
                }
                else if (operation == "-" && left == "" && right == "")
                {
                    textBlock.Text += s;
                    left_sign = -1;
                    operation = "";
                }
                else
                {
                    if (operation == "=")
                    {
                        left = right;
                        left_sign = 1;
                        answer.Text = left;
                        textBlock.Text = left;
                        right = "";
                        operation = "";
                    }
                    if (s == "=")
                    {
                        if (operation == "")
                        {
                            operation = "=";
                        }
                        else
                        {
                            Upd_Left();
                            textBlock.Text = left;
                            answer.Text = left;
                            right = "";
                            operation = "";
                        }
                    }
                    else
                    {
                        textBlock.Text += s;
                        if (operation != "")
                        {
                            Upd_Left();
                            answer.Text = left;
                            right = "";
                        }
                        operation = s;
                    }
                }                
            }

        }

        private void Upd_Left()
        {
            double num1 = Double.Parse(left) * left_sign;
            double num2 = Double.Parse(right);
            if(operation == "+")
            {
                left = (num1 + num2).ToString();
            }
            else if (operation == "-")
            {
                left = (num1 - num2).ToString();
            }
            else if (operation == "*")
            {
                left = (num1 * num2).ToString();
            }
            else if (operation == "/")
            {
                left = (num1 / num2).ToString();
            }
            left_sign = 1;
        }
    }
}
