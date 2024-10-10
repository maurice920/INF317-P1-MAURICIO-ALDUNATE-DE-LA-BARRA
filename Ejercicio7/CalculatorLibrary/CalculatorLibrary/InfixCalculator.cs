using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorLibrary
{
    public class InfixCalculator
    {
        public double Calculate(string input)
        {
            try
            {
                // Convertir la expresión a postfix (1 + 2) --> 1 2 +
                var postfix = InfixToPostfix(input);
                // Evaluar la expresión postfix
                return EvaluatePostfix(postfix);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Expresión inválida.", ex);
            }
        }
        // con este metodo convertirmos de infix a postfix
        private List<string> InfixToPostfix(string input)
        {
            // inicializamos una lista vacia que almacenara los tokens (elementos) de la expresion en notacion postfija
            List<string> output = new List<string>();
            // inicializamos una pila para almacenar los operadores aritmeticos y parentesis
            Stack<string> operators = new Stack<string>();
            // dividimos la expresion de entrada en tokens individuales utilizando como delimitador el ' '
            string[] tokens = input.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            // iteramos sobre cada token
            foreach (var token in tokens)
            {
                // variable para convertir el token a numero
                double num;
                // analizamos el token como numero
                if (double.TryParse(token, out num))
                {
                    // si el token es un numero agregamos a la lista output
                    output.Add(token);
                }
                else if (token == "(")
                {
                    // si fuese parentesis de apertura, hacemos un push a la pila operators
                    operators.Push(token);
                }
                else if (token == ")")
                {
                    // mientras la pila operators no este vacia y el elemnto superior no sea un (
                    while (operators.Count > 0 && operators.Peek() != "(")
                    {
                        // hacemos un pop al operador y agregamos a la lista output
                        output.Add(operators.Pop());
                    }
                    // verificar si falta un parentesis de apertura
                    if (operators.Count == 0)
                    {
                        throw new ArgumentException("Paréntesis no coinciden.");
                    }
                    operators.Pop(); // Quitar el paréntesis abierto
                }
                // verificar si el token es un operador aritmetico
                else if (IsOperator(token))
                {
                    // mientras haya operadores en la pila y el operador en la cima de la pila tenga >= precedencia que el operador actual
                    while (operators.Count > 0 && Precedence(operators.Peek()) >= Precedence(token))
                    {
                        // hacemos un pop al operador de la pila  y agregamos al output
                        output.Add(operators.Pop());
                    }
                    // hacemos un push del operador actual a la pila
                    operators.Push(token);
                }
                else
                {
                    throw new ArgumentException($"Token inválido: {token}");
                }
            }
            // desempilamos los operadores restantes de la pila
            while (operators.Count > 0)
            {
                // verificamos si se encuentra un parentesis de apertura sin su cierre
                if (operators.Peek() == "(")
                {
                    throw new ArgumentException("Paréntesis no coinciden.");
                }
                // desempeniamos el operador y agregamos al output
                output.Add(operators.Pop());
            }
            // devuelve la expresion en postfija
            return output;
        }

        // evaluamos la expresion postfix
        private double EvaluatePostfix(List<string> postfix)
        {
            // inicializamos una pila para almacenar los operandos
            Stack<double> stack = new Stack<double>();

            // iteramos sobre cada token
            foreach (var token in postfix)
            {
                // variablen number para convertir de token a number
                double number;
                // verificamos si es numero
                if (double.TryParse(token, out number))
                {
                    // si el token es numero, lo llevamos a la pila
                    stack.Push(number);
                } // verificamos si es operador
                else if (IsOperator(token))
                {
                    // verifica que haya almenos 2 operandos en la pila
                    if (stack.Count < 2)
                        throw new ArgumentException("Expresión inválida.");
                    // desempenia el primer operando 
                    double b = stack.Pop();
                    // desempenia el segundo operando
                    double a = stack.Pop();
                    // aplicamos el operador respectivo a los operandos
                    double result = ApplyOperator(a, b, token);
                    // hace un push del resultado de vuelta a la pila
                    stack.Push(result);
                }
                else
                {
                    throw new ArgumentException($"Token inválido en postfix: {token}");
                }
            }
            // verificamos que quede solo 1 resultado en la pila
            if (stack.Count != 1)
                throw new ArgumentException("Expresión inválida.");

            return stack.Pop();// devuelve le resultado final
        }

        // verificamos si es un operador aritmetico
        private bool IsOperator(string token)
        {
            return token == "+" || token == "-" || token == "*" || token == "/";
        }
        // metodo que toma una cadena y devuelve un entero que representa su precedencia
        private int Precedence(string op)
        {
            if (op == "+" || op == "-")
                return 1;
            if (op == "*" || op == "/")
                return 2;
            return 0;
        }
        // con este metodo devolvmemos la operacion aplicada en si
        private double ApplyOperator(double a, double b, string op)
        {
            if (op == "+")
                return a + b;
            if (op == "-")
                return a - b;
            if (op == "*")
                return a * b;
            if (op == "/")
                return b != 0 ? a / b : throw new DivideByZeroException("División por cero.");
            throw new ArgumentException($"Operador inválido: {op}");
        }
    }
}
