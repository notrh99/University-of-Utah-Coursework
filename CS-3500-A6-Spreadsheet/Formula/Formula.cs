/// <summary> 
/// Author:    Rayyan Hamid 
/// Partner:   None 
/// Date:      07/02/2022 
/// Course:    CS 3500, University of Utah, School of Computing 
/// Copyright: CS 3500 and Rayyan Hamid - This work may not be copied for use in Academic Coursework. 
/// 
/// I, Rayyan Hamid, certify that I wrote this code from scratch and did not copy it in part or whole from  
/// another source.  All references used in the completion of the assignment are cited in my README file. 
/// 
/// </summary>
///         

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SpreadsheetUtilities
{
    /// <summary>
    /// Represents formulas written in standard infix notation using standard precedence
    /// rules.  The allowed symbols are non-negative numbers written using double-precision 
    /// floating-point syntax (without unary preceeding '-' or '+'); 
    /// variables that consist of a letter or underscore followed by 
    /// zero or more letters, underscores, or digits; parentheses; and the four operator 
    /// symbols +, -, *, and /.  
    /// 
    /// Spaces are significant only insofar that they delimit tokens.  For example, "xy" is
    /// a single variable, "x y" consists of two variables "x" and y; "x23" is a single variable; 
    /// and "x 23" consists of a variable "x" and a number "23".
    /// 
    /// Associated with every formula are two delegates:  a normalizer and a validator.  The
    /// normalizer is used to convert variables into a canonical form, and the validator is used
    /// to add extra restrictions on the validity of a variable (beyond the standard requirement 
    /// that it consist of a letter or underscore followed by zero or more letters, underscores,
    /// or digits.)  Their use is described in detail in the constructor and method comments.
    /// </summary>
    public class Formula
    {

        private IEnumerable<string> valid_tokens;
        private string expression = string.Empty;
        /// <summary>
        /// Creates a Formula from a string that consists of an infix expression written as
        /// described in the class comment.  If the expression is syntactically invalid,
        /// throws a FormulaFormatException with an explanatory Message.
        /// 
        /// The associated normalizer is the identity function, and the associated validator
        /// maps every string to true.  
        /// </summary>
        public Formula(String formula) :
        this(formula, s => s, s => true)
        {
        }

        /// <summary>
        /// Creates a Formula from a string that consists of an infix expression written as
        /// described in the class comment.  If the expression is syntactically incorrect,
        /// throws a FormulaFormatException with an explanatory Message.
        /// 
        /// The associated normalizer and validator are the second and third parameters,
        /// respectively.  
        /// 
        /// If the formula contains a variable v such that normalize(v) is not a legal variable, 
        /// throws a FormulaFormatException with an explanatory message. 
        /// 
        /// If the formula contains a variable v such that isValid(normalize(v)) is false,
        /// throws a FormulaFormatException with an explanatory message.
        /// 
        /// Suppose that N is a method that converts all the letters in a string to upper case, and
        /// that V is a method that returns true only if a string consists of one letter followed
        /// by one digit.  Then:
        /// 
        /// new Formula("x2+y3", N, V) should succeed
        /// new Formula("x+y3", N, V) should throw an exception, since V(N("x")) is false
        /// new Formula("2x+y3", N, V) should throw an exception, since "2x+y3" is syntactically incorrect.
        /// </summary>
        public Formula(String formula, Func<string, string> normalize, Func<string, bool> isValid)
        {
            valid_tokens = GetValidTokens(formula, normalize, isValid);
            foreach (string i in valid_tokens)
            {
                expression += i;
            }
        }

        /// <summary>
        /// Evaluates this Formula, using the lookup delegate to determine the values of
        /// variables.  When a variable symbol v needs to be determined, it should be looked up
        /// via lookup(normalize(v)). (Here, normalize is the normalizer that was passed to 
        /// the constructor.)
        /// 
        /// For example, if L("x") is 2, L("X") is 4, and N is a method that converts all the letters 
        /// in a string to upper case:
        /// 
        /// new Formula("x+7", N, s => true).Evaluate(L) is 11
        /// new Formula("x+7").Evaluate(L) is 9
        /// 
        /// Given a variable symbol as its parameter, lookup returns the variable's value 
        /// (if it has one) or throws an ArgumentException (otherwise).
        /// 
        /// If no undefined variables or divisions by zero are encountered when evaluating 
        /// this Formula, the value is returned.  Otherwise, a FormulaError is returned.  
        /// The Reason property of the FormulaError should have a meaningful explanation.
        ///
        /// This method should never throw an exception.
        /// </summary>
        public object Evaluate(Func<string, double> lookup)
        {
            Stack<double> valueStack = new Stack<double>();
            Stack<string> operatorStack = new Stack<string>();
            bool parenthesisCheck = false;

            foreach (string i in valid_tokens)
            {
                /// If i is numerical
                if (double.TryParse(i, out double value))
                {
                    string to_double = value.ToString();
                    value = Convert.ToDouble(to_double);
                    if ((operatorStack.TryPeek(out _) && operatorStack.Peek() is "*"))
                    {
                        double topValue = valueStack.Pop();
                        operatorStack.Pop();
                        valueStack.Push(value * topValue);
                    }

                    else if ((operatorStack.TryPeek(out _) && operatorStack.Peek() is "/"))
                    {
                        if (value != 0)
                        {
                            double topValue = valueStack.Pop();
                            operatorStack.Pop();
                            valueStack.Push(topValue / value);
                        }
                        else
                        {
                            return new FormulaError("Cant divide with zero");
                        }
                    }
                    else
                    {
                        valueStack.Push(value);
                    }
                }
                else if (i is "" or " ")
                {
                    continue;
                }

                ///If i is a variable
                else if (Regex.IsMatch(i, @"[a-zA-Z]+\d+"))
                {
                    double lookUpValue;
                    try
                    {
                        lookUpValue = lookup(i);
                    }
                    catch
                    {
                        return new FormulaError("Undefined Variable");
                    }

                    string to_double = lookUpValue.ToString();
                    lookUpValue = Convert.ToDouble(to_double);
                    if ((operatorStack.TryPeek(out _) && operatorStack.Peek() is "*"))
                    {
                        operatorStack.Pop();
                        double topValue = valueStack.Pop();
                        valueStack.Push(lookUpValue * topValue);
                    }

                    else if ((operatorStack.TryPeek(out _) && operatorStack.Peek() is "/"))
                    {
                        if (lookUpValue != 0)
                        {
                            double topValue = valueStack.Pop();
                            operatorStack.Pop();
                            valueStack.Push(topValue / lookUpValue);
                        }
                        else
                        {
                            return new FormulaError("Cant divide with zero");
                        }
                    }
                    else
                    {
                        valueStack.Push(lookUpValue);
                    }

                    if (operatorStack.Count == 0 && valueStack.Count == 0)
                    {
                        return new FormulaError("Number of values not enough to compute expression1");
                    }
                }

                /// If i is "+" or a "-"
                else if (i is "+" or "-")
                {
                    if (operatorStack.Count == 0 && valueStack.Count == 0)
                    {
                        return new FormulaError("Number of values not enough to compute expression2");
                    }
                    if ((operatorStack.TryPeek(out _) && operatorStack.Peek() is "+"))
                    {
                        if (valueStack.Count >= 2)
                        {
                            operatorStack.Pop();
                            double val1 = valueStack.Pop();
                            double val2 = valueStack.Pop();
                            valueStack.Push(val1 + val2);
                        }
                        else
                        {
                            return new FormulaError("Number of values not enough to compute expression3");
                        }

                    }
                    else if (operatorStack.TryPeek(out _) && operatorStack.Peek() is "-")
                    {
                        if (valueStack.Count >= 2)
                        {
                            operatorStack.Pop();
                            double val1 = valueStack.Pop();
                            double val2 = valueStack.Pop();
                            valueStack.Push(val2 - val1);
                        }
                        else
                        {
                            return new FormulaError("Number of values not enough to compute expression4");
                        }
                    }
                    operatorStack.Push(i);

                }

                /// If i is "*" or a "/"
                else if (i is "*" or "/")
                {
                    if (operatorStack.Count == 0 && valueStack.Count == 0)
                    {
                        return new FormulaError("Number of values not enough to compute expression5");
                    }
                    operatorStack.Push(i);
                }

                ///If i is "("
                else if (i is "(")
                {
                    operatorStack.Push(i);
                    parenthesisCheck = true;
                }

                /// If i is ")"
                else if (i is ")")
                {
                    if (parenthesisCheck != true)
                    {
                        return new FormulaError("Invalid Expression");
                    }
                    /// If the top value on the operatorStack is "+" or a "-"
                    if (operatorStack.Peek() is "+" or "-")
                    {
                        if (valueStack.Count >= 2)
                        {
                            if ((operatorStack.TryPeek(out _) && operatorStack.Peek() is "+"))
                            {
                                double val1 = valueStack.Pop();
                                double val2 = valueStack.Pop();
                                valueStack.Push(val1 + val2);
                                operatorStack.Pop();
                            }
                            else if (operatorStack.TryPeek(out _) && operatorStack.Peek() is "-")
                            {
                                double val1 = valueStack.Pop();
                                double val2 = valueStack.Pop();
                                operatorStack.Pop();
                                valueStack.Push(val2 - val1);
                            }
                        }
                        else
                        {
                            return new FormulaError("Number of values not enough to compute expression6");
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
                            double val1 = valueStack.Pop();
                            double val2 = valueStack.Pop();
                            valueStack.Push(val1 * val2);
                        }

                        else if ((operatorStack.TryPeek(out _) && operatorStack.Peek() is "/"))
                        {
                            if (value != 0)
                            {
                                operatorStack.Pop();
                                double val1 = valueStack.Pop();
                                double val2 = valueStack.Pop();
                                valueStack.Push(val1 / val2);
                            }
                            else
                            {
                                return new FormulaError("Cant divide with zero");
                            }
                        }
                    }
                    else
                    {
                        return new FormulaError("Number of values not enough to compute expression7");
                    }
                }
            }

            ///Checking if any values left in the operatorStack
            if (operatorStack.Count == 0)
            {
                return valueStack.Pop();
            }
            else
            {
                double result = valueStack.Pop();

                while (operatorStack.Count != 0)
                {
                    if (operatorStack.Peek() is "+")
                    {
                        double val1 = valueStack.Pop();
                        operatorStack.Pop();
                        result += val1;
                    }
                    else if (operatorStack.Peek() is "-")
                    {
                        double val1 = valueStack.Pop();
                        operatorStack.Pop();
                        //result -= val1;
                        val1 -= result;
                    }
                    else
                        operatorStack.Pop();

                }
                return result;
            }


            ///The methods returns the interger value of the valid inflix expression. 
        }

        /// <summary>
        /// Enumerates the normalized versions of all of the variables that occur in this 
        /// formula.  No normalization may appear more than once in the enumeration, even 
        /// if it appears more than once in this Formula.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        /// 
        /// new Formula("x+y*z", N, s => true).GetVariables() should enumerate "X", "Y", and "Z"
        /// new Formula("x+X*z", N, s => true).GetVariables() should enumerate "X" and "Z".
        /// new Formula("x+X*z").GetVariables() should enumerate "x", "X", and "z".
        /// </summary>
        public IEnumerable<String> GetVariables()
        {
            HashSet<string> toReturn = new HashSet<string>();
            foreach (string i in valid_tokens)
            {
                if (Regex.IsMatch(i, @"[a-zA-Z_]([a-zA-Z_]|\d)*"))
                {
                    toReturn.Add(i);
                }
            }
            return toReturn.ToList();
        }

        /// <summary>
        /// Returns a string containing no spaces which, if passed to the Formula
        /// constructor, will produce a Formula f such that this.Equals(f).  All of the
        /// variables in the string should be normalized.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        /// 
        /// new Formula("x + y", N, s => true).ToString() should return "X+Y"
        /// new Formula("x + Y").ToString() should return "x+Y"
        /// </summary>
        public override string ToString()
        {
            return expression;
        }

        /// <summary>
        /// If obj is null or obj is not a Formula, returns false.  Otherwise, reports
        /// whether or not this Formula and obj are equal.
        /// 
        /// Two Formulae are considered equal if they consist of the same tokens in the
        /// same order.  To determine token equality, all tokens are compared as strings 
        /// except for numeric tokens and variable tokens.
        /// Numeric tokens are considered equal if they are equal after being "normalized" 
        /// by C#'s standard conversion from string to double, then back to string. This 
        /// eliminates any inconsistencies due to limited floating point precision.
        /// Variable tokens are considered equal if their normalized forms are equal, as 
        /// defined by the provided normalizer.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        ///  
        /// new Formula("x1+y2", N, s => true).Equals(new Formula("X1  +  Y2")) is true
        /// new Formula("x1+y2").Equals(new Formula("X1+Y2")) is false
        /// new Formula("x1+y2").Equals(new Formula("y2+x1")) is false
        /// new Formula("2.0 + x7").Equals(new Formula("2.000 + x7")) is true
        /// </summary>
        public override bool Equals(object? obj)
        {
            return obj is Formula formula && formula.expression == expression;
        }

        /// <summary>
        /// Reports whether f1 == f2, using the notion of equality from the Equals method.
        /// Note that if both f1 and f2 are null, this method should return true.  If one is
        /// null and one is not, this method should return false.
        /// </summary>
        public static bool operator ==(Formula f1, Formula f2)
        {
            return f1.Equals(f2);
        }

        /// <summary>
        /// Reports whether f1 != f2, using the notion of equality from the Equals method.
        /// Note that if both f1 and f2 are null, this method should return false.  If one is
        /// null and one is not, this method should return true.
        /// </summary>
        public static bool operator !=(Formula f1, Formula f2)
        {
            return !(f1 == f2);
        }

        /// <summary>
        /// Returns a hash code for this Formula.  If f1.Equals(f2), then it must be the
        /// case that f1.GetHashCode() == f2.GetHashCode().  Ideally, the probability that two 
        /// randomly-generated unequal Formulae have the same hash code should be extremely small.
        /// </summary>
        public override int GetHashCode()
        {
            return expression.GetHashCode();
        }

        /// <summary>
        /// Given an expression, enumerates the tokens that compose it.  Tokens are left paren;
        /// right paren; one of the four operator symbols; a string consisting of a letter or underscore
        /// followed by zero or more letters, digits, or underscores; a double literal; and anything that doesn't
        /// match one of those patterns.  There are no empty tokens, and no token contains white space.
        /// </summary>
        private static IEnumerable<string> GetTokens(String formula)
        {
            // Patterns for individual tokens
            String lpPattern = @"\(";
            String rpPattern = @"\)";
            String opPattern = @"[\+\-*/]";
            String varPattern = @"[a-zA-Z_](?: [a-zA-Z_]|\d)*";
            String doublePattern = @"(?: \d+\.\d* | \d*\.\d+ | \d+ ) (?: [eE][\+-]?\d+)?";
            String spacePattern = @"\s+";

            // Overall pattern
            String pattern = String.Format("({0}) | ({1}) | ({2}) | ({3}) | ({4}) | ({5})",
                                            lpPattern, rpPattern, opPattern, varPattern, doublePattern, spacePattern);

            // Enumerate matching tokens that don't consist solely of white space.
            foreach (String s in Regex.Split(formula, pattern, RegexOptions.IgnorePatternWhitespace))
            {
                if (!Regex.IsMatch(s, @"^\s*$", RegexOptions.Singleline))
                {
                    yield return s;
                }
            }
        }
        /// <summary>
        /// Helper method to check if a token is an operator('+", '-', '*', '/')
        /// </summary>
        /// <param name="opperator"></param>
        /// <returns></returns>
        private static bool IsOperator(string opperator)
        {
            if (opperator == "+" || opperator == "-" || opperator == "/" || opperator == "*")
                return true;
            return false;
        }
        private static bool IsValid(List<string> incomminglist, int i)
        {
            foreach (string s in incomminglist)
            {

            }
            return false;
        }
        /// <summary>
        /// This is a helper method for our Formula method. It takes in the same parameters as the formula method and return a valid formula. 
        /// </summary>
        /// <param name="formula"></param>
        /// <param name="normalize"></param>
        /// <param name="isValid"></param>
        /// <returns></returns>
        /// <exception cref="FormulaFormatException"></exception>
        private IEnumerable<string> GetValidTokens(string formula, Func<string, string> normalize, Func<string, bool> isValid)
        {

            IEnumerable<string> givenToken = GetTokens(formula);
            List<string> returnToken = new List<string>();
            int openingParenthesisCount = 0;
            int closingParethesisCount = 0;
            int tokenCount = 0;
            int operatorCount = 0;
            int integerCount = 0;
            string varPattern = @"[a-zA-Z_](?: [a-zA-Z_]|\d)*";
            //There must be atleast one token in the formula. 
            if (formula == string.Empty || ReferenceEquals(formula, null))
                throw new FormulaFormatException("Formula is empty");
            else
            {
                //Loop over all the tokens in givenTokens
                for (int i = 0; i < givenToken.Count(); i++)
                {

                    //if the first token is a variable, an opening parenthesis or a number. 
                    if (tokenCount == 0 && ((double.TryParse(givenToken.ElementAt(0), out double result)) || Regex.IsMatch(givenToken.ElementAt(0), varPattern) || givenToken.ElementAt(0) == "("))
                    {
                        if (givenToken.ElementAt(0) == "(")
                        {
                            openingParenthesisCount++;
                            returnToken.Add(givenToken.ElementAt(0));
                            tokenCount++;
                        }
                        else if (Double.TryParse(givenToken.ElementAt(0), out double number))
                        {
                            string temp = number.ToString();
                            returnToken.Add(temp);
                            tokenCount++;
                            integerCount++;
                        }
                        else if (Regex.IsMatch(givenToken.ElementAt(0), varPattern))
                        {
                            if (isValid(givenToken.ElementAt(0)))
                            {
                                string norm = normalize(givenToken.ElementAt(0));
                                returnToken.Add(norm);
                                tokenCount++;
                                integerCount++;
                            }
                            else
                                throw new FormulaFormatException("Cell name is not valid under user's condition.");
                        }
                    }
                    else
                    {
                        int prev_token = i - 1;
                        int cur_token = i;
                        //if its the last token in thr formula
                        if (i == givenToken.Count() - 1)
                        {
                            //it should be a closing parenthesis, a number, a variable or an operator.
                            if (givenToken.ElementAt(i) == ")" || double.TryParse(givenToken.ElementAt(i), out double result_1) || Regex.IsMatch(givenToken.ElementAt(i), varPattern) || IsOperator(givenToken.ElementAt(i)))
                            {
                                if (givenToken.ElementAt(i) == ")")
                                {
                                    closingParethesisCount++;
                                    if (openingParenthesisCount == closingParethesisCount)
                                    {
                                        returnToken.Add(givenToken.ElementAt(i));
                                    }
                                    else
                                    {
                                        throw new FormulaFormatException("number of left parenthesis does not match number of right parenthesis");
                                    }

                                }
                                else if (Regex.IsMatch(givenToken.ElementAt(i), varPattern) && isValid(normalize(givenToken.ElementAt(i))))
                                {
                                    if (!IsOperator(givenToken.ElementAt(prev_token)))
                                    {
                                        throw new FormulaFormatException("Invalid Token");
                                    }
                                    string norm = normalize(givenToken.ElementAt(i));
                                    returnToken.Add(norm);
                                    integerCount++;

                                }
                                else if (double.TryParse(givenToken.ElementAt(i), out double result_2))
                                {
                                    if (!IsOperator(givenToken.ElementAt(prev_token)))
                                    {
                                        throw new FormulaFormatException("Invalid Token");
                                    }
                                    string temp = result_2.ToString();
                                    returnToken.Add(temp);
                                    integerCount++;
                                }
                                else if (IsOperator(givenToken.ElementAt(i)))
                                {
                                    returnToken.Add(givenToken.ElementAt(i));
                                    operatorCount++;
                                }
                                else
                                    throw new FormulaFormatException("Invalid Formula");
                            }
                            else
                                throw new FormulaFormatException("Invalid Formula");

                        }
                        //If the token is a closing parenthesis we add it to the formula. 
                        else if (givenToken.ElementAt(i) == ")")
                        {
                            closingParethesisCount++;
                            if (closingParethesisCount == openingParenthesisCount)
                            {
                                returnToken.Add(givenToken.ElementAt(i));
                            }

                        }
                        //Check if the previous token was a closing parenthesis or an operator.
                        else if (givenToken.ElementAt(prev_token) == "(" || IsOperator(givenToken.ElementAt(prev_token)))
                        {
                            try
                            {
                                if (double.TryParse(givenToken.ElementAt(cur_token), out double result_4))
                                {
                                    string temp = result_4.ToString();
                                    returnToken.Add(temp);
                                    integerCount++;

                                }
                                else if (Regex.IsMatch(givenToken.ElementAt(cur_token), varPattern) && isValid(normalize(givenToken.ElementAt(cur_token))))
                                {
                                    string norm = normalize(givenToken.ElementAt(cur_token));
                                    returnToken.Add(norm);
                                    integerCount++;
                                }
                                else if (givenToken.ElementAt(cur_token) == "(")
                                {
                                    openingParenthesisCount++;
                                    returnToken.Add(givenToken.ElementAt((cur_token)));
                                }
                                else if (IsOperator(givenToken.ElementAt(cur_token)))
                                {
                                    returnToken.Add(givenToken.ElementAt(cur_token));
                                    operatorCount++;
                                }
                            }
                            catch (Exception e)
                            {
                                throw new FormulaFormatException("invalid formula");
                            }
                        }
                        //Check if the previous token is a closing parenthesis, a number or a variable. 
                        else if (givenToken.ElementAt(prev_token) == ")" || double.TryParse(givenToken.ElementAt(prev_token), out double result_1) || Regex.IsMatch(givenToken.ElementAt(prev_token), varPattern))
                        {
                            try
                            {
                                if (givenToken.ElementAt(cur_token) == ")" && givenToken.ElementAt(prev_token) == ")")
                                {
                                    closingParethesisCount++;
                                    closingParethesisCount++;
                                    if (openingParenthesisCount == closingParethesisCount)
                                    {
                                        returnToken.Add(givenToken.ElementAt(cur_token));

                                    }
                                    else
                                    {
                                        throw new FormulaFormatException("number of right parenthesis is not equal to number of left parenthesis");
                                    }

                                }
                                else if (IsOperator(givenToken.ElementAt(cur_token)))
                                {
                                    returnToken.Add(givenToken.ElementAt(cur_token));
                                    operatorCount++;
                                }
                            }
                            catch (Exception e)
                            {
                                throw new FormulaFormatException("Invalid Formula");
                            }
                        }
                        tokenCount++;
                    }


                }
            }
            //If the number of parenthesis do not match, throw a FormulaFormatException
            if (openingParenthesisCount != closingParethesisCount)
                throw new FormulaFormatException("Number of Opening parenthesis is not equal to the number of closing parethesis.");
            //If the number of operator equals or exceeds the number of doubles in the formula we throw a FormulaFormatException
            if (operatorCount >= integerCount || (integerCount > 1 && operatorCount == 0))
                throw new FormulaFormatException("Invalid Expression");

            return returnToken;
        }
    }

    /// <summary>
    /// Used to report syntactic errors in the argument to the Formula constructor.
    /// </summary>
    public class FormulaFormatException : Exception
    {
        /// <summary>
        /// Constructs a FormulaFormatException containing the explanatory message.
        /// </summary>
        public FormulaFormatException(String message)
            : base(message)
        {
        }
    }

    /// <summary>
    /// Used as a possible return value of the Formula.Evaluate method.
    /// </summary>
    public struct FormulaError
    {
        /// <summary>
        /// Constructs a FormulaError containing the explanatory reason.
        /// </summary>
        /// <param name="reason"></param>
        public FormulaError(String reason)
            : this()
        {
            Reason = reason;
        }

        /// <summary>
        ///  The reason why this FormulaError was created.
        /// </summary>
        public string Reason { get; private set; }
    }
}
