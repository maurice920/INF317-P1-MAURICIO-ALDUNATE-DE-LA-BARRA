using CalculatorLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CalculatorApp
{
    public partial class Form1 : Form
    {
        // creamos una instancia privada para calcular expresiones matematicas en notacion infija
        private InfixCalculator calculator = new InfixCalculator();
        // en esta cadena privada iremos guardando o almacenando la expresion matematica que el usuario ira construyendo
        private string expression = "";

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // convertimos el objeto sender (boton que fue clicado) al tipo button
            Button bu = sender as Button;
            // obtenemos el texto del boton que fue seleccionado
            string buttonText = bu.Text;

            // Verificamos si el boton seleccionado es igual a "="
            if (buttonText.Equals("="))
            {
                try
                {
                    // si es "=" calculamos el resultado de la expresion usando el "InfixCalculator"
                    double result = calculator.Calculate(expression);
                    // asignamos a nuestro text box el resultado obtenido
                    textBox1.Text += " = " + result;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (buttonText.Equals("Limpiar"))
            {
                // si el boton es limpiar seteamos en vacio tanto la expresion como el textbox1
                expression = "";
                textBox1.Text = "";
            }
            else
            {
                // verificamos si el boton es un operador aritmetico o un parentesis
                if (IsOperator(buttonText) || buttonText == "(" || buttonText == ")")
                {
                    // si fuese verdad, agregamos el operador o espacio a la expresion
                    expression += " " + buttonText + " ";
                }
                else
                {
                    // caso contrario si es un numero agregamos el nro a la expresion
                    expression += buttonText;
                }
                // actualizamos nuestro textbox con la expresion actual
                textBox1.Text = expression;
            }
        }
        // en caso de no ser = o Limpiar con este metodo nos determinara si la cadena ingresada o mejor dicho
        // el boton es un operador aritmetico
        private bool IsOperator(string op)
        {
            return op == "+" || op == "-" || op == "*" || op == "/";
        }

        private void button17_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
