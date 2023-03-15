/// <summary> 
/// Author:    Rayyan Hamid 
/// Partner:   None 
/// Date:      21/01/2022 
/// Course:    CS 3500, University of Utah, School of Computing 
/// Copyright: CS 3500 and Rayyan Hamid - This work may not be copied for use in Academic Coursework. 
/// 
/// I, Rayyan Hamid, certify that I wrote this code from scratch and did not copy it in part or whole from  
/// another source.  All references used in the completion of the assignment are cited in my README file. 
/// 
/// </summary>
/// 

using System.Collections;
using System.Text.RegularExpressions;

namespace FormulaEvaluator
{
    public class Evaluator
    {
        public delegate int Lookup(String variable_name);
        /// <summary>
        ///  This function computes the integer value of an Infix Expression.
        /// 
        /// </summary>
        /// <param name="expression"> expression represents the input to this function </param>
        /// <param name="variableEvaluator"> variableEvaluator represents the variable to be evaluated </param>
        /// <returns> The only integer value in valueStack after performing the valid operations on the infix epression. </returns>    
        public static int Evaluate(String expression, Lookup variableEvaluator)
        {
            Stack<int> valueStack = new Stack<int>();
            Stack<string> operatorStack = new Stack<string>();
            bool parenthesisCheck = false;

            ///Trimming the expression and getting rid of white spaces.
            string[] expressionString = Regex.Split(expression, "(\\()|(\\))|(-)|(\\+)|(\\*)|(/)", RegexOptions.IgnorePatternWhitespace); ;
            for (int i = 0; i < expressionString.Length; i++)
            {

                /// If expressionString[i] is an integer
                if (int.TryParse(expressionString[i], out int value))
                {
                    if ((operatorStack.TryPeek(out _) && operatorStack.Peek() is "*"))
                    {
                        int topValue = valueStack.Pop();
                        operatorStack.Pop();
                        valueStack.Push(value * topValue);
                    }

                    else if ((operatorStack.TryPeek(out _) && operatorStack.Peek() is "/"))
                    {
                        if (value != 0)
                        {
                            int topValue = valueStack.Pop();
                            operatorStack.Pop();
                            valueStack.Push(topValue / value);
                        }
                        else
                        {
                            throw new ArgumentException("Cant divide with zero");
                        }
                    }
                    else
                    {
                        valueStack.Push(value);
                    }
                }
                else if (expressionString[i] is "" or " ")
                {
                    continue;
                }

                ///If expressionString[i] is a variable
                else if (Regex.IsMatch(expressionString[i], @"[a-zA-Z]+\d+"))
                {
                    int lookUpValue = variableEvaluator(expressionString[i]);
                    
                        if ((operatorStack.TryPeek(out _) && operatorStack.Peek() is "*"))
                        {
                            operatorStack.Pop();
                            int topValue = valueStack.Pop();
                            valueStack.Push(lookUpValue * topValue);
                        }

                        else if ((operatorStack.TryPeek(out _) && operatorStack.Peek() is "/"))
                        {
                            if (lookUpValue == 0)
                            {
                                throw new ArgumentException("Cant divide with zero");
                            }
                            else
                            {
                                operatorStack.Pop();
                                int topValue = valueStack.Pop();
                                valueStack.Push(topValue / lookUpValue);
                            }
                        }else
                            valueStack.Push(lookUpValue);
                    
                }

                /// If expressionString[i] is "+" or a "-"
                else if (expressionString[i] is "+" or "-")
                {
                    if (operatorStack.Count == 0 && valueStack.Count == 0)
                    {
                        throw new ArgumentException("Number of values not enough to compute expression");
                    }
                    if ((operatorStack.TryPeek(out _) && operatorStack.Peek() is "+"))
                    {
                        if (valueStack.Count >= 2)
                        {
                            operatorStack.Pop();
                            int val1 = valueStack.Pop();
                            int val2 = valueStack.Pop();
                            valueStack.Push(val1 + val2);
                        }
                        else
                        {
                            throw new ArgumentException("Number of values not enough to compute expression");
                        }

                    }
                    else if (operatorStack.TryPeek(out _) && operatorStack.Peek() is "-")
                    {
                        if (valueStack.Count >= 2)
                        {
                            operatorStack.Pop();
                            int val1 = valueStack.Pop();
                            int val2 = valueStack.Pop();
                            valueStack.Push(val2 - val1);
                        }
                        else
                        {
                            throw new ArgumentException("Number of values not enough to compute expression");
                        }
                    }
                    operatorStack.Push(expressionString[i]);

                }

                /// If expressionString[i] is "*" or a "/"
                else if (expressionString[i] is "*" or "/")
                {
                    if (operatorStack.Count == 0 && valueStack.Count == 0)
                    {
                        throw new ArgumentException("Number of values not enough to compute expression");
                    }
                    operatorStack.Push(expressionString[i]);
                }

                ///If expressionString[i] is "("
                else if (expressionString[i] is "(")
                {
                    operatorStack.Push(expressionString[i]);
                    parenthesisCheck = true;
                }

                /// If expressionString[i] is ")"
                else if (expressionString[i] is ")")
                {
                    if (parenthesisCheck != true)
                    {
                        throw new ArgumentException("Invalid Expression");
                    }
                    /// If the top value on the operatorStack is "+" or a "-"
                    if (operatorStack.Peek() is "+" or "-")
                    {
                        if (valueStack.Count >= 2)
                        {
                            if ((operatorStack.TryPeek(out _) && operatorStack.Peek() is "+"))
                            {
                                int val1 = valueStack.Pop();
                                int val2 = valueStack.Pop();
                                valueStack.Push(val1 + val2);
                                operatorStack.Pop();
                            }
                            else if (operatorStack.TryPeek(out _) && operatorStack.Peek() is "-")
                            {
                                int val1 = valueStack.Pop();
                                int val2 = valueStack.Pop();
                                operatorStack.Pop();
                                valueStack.Push(val2 - val1);
                            }
                        }
                        else
                        {
                            throw new ArgumentException("Number of values not enough to compute expression");
                        }
                    }

                    /// If the top value on the operatorStack is "("
                    if (operatorStack.Peek() is "(")
                    {
                        operatorStack.Pop();
                    }

                    /// If the top value on the operatorStack is "*" or a "/"
                    if (valueStack.Count >= 0)
                    {
                        if ((operatorStack.TryPeek(out _) && operatorStack.Peek() is "*"))
                        {
                            operatorStack.Pop();
                            int val1 = valueStack.Pop();
                            int val2 = valueStack.Pop();
                            valueStack.Push(val1 * val2);
                        }

                        else if ((operatorStack.TryPeek(out _) && operatorStack.Peek() is "/"))
                        {
                            if (value != 0)
                            {
                                operatorStack.Pop();
                                int val1 = valueStack.Pop();
                                int val2 = valueStack.Pop();
                                valueStack.Push(val1 / val2);
                            }
                            else
                            {
                                throw new ArgumentException("Cant divide with zero");
                            }
                        }
                    }
                    else
                    {
                        throw new ArgumentException("Number of values not enough to compute expression");
                    }
                }
            }

            ///Checking if any values left in the operatorStack
            if (operatorStack.Count == 0)
            {
                if (valueStack.Count == 1)
                {
                    return valueStack.Pop();
                }
                else
                {
                    throw new ArgumentException("Number of values not enough to compute expression");
                }
            }
            else if (operatorStack.Count == 1 && valueStack.Count == 2)
            {
                if (operatorStack.Peek() is "+")
                {
                    int val1 = valueStack.Pop();
                    int val2 = valueStack.Pop();
                    return val1 + val2;
                }
                else if (operatorStack.Peek() is "-")
                {
                    int val1 = valueStack.Pop();
                    int val2 = valueStack.Pop();
                    return val2 - val1;
                }
            }
            else
            {
                throw new ArgumentException("Number of values not enough to compute expression");
            }
            ///The methods returns the interger value of the valid inflix expression. 
            return valueStack.Pop();
        }

    }
}
