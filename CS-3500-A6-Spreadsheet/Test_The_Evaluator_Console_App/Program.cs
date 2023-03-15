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

using FormulaEvaluator;

if (FormulaEvaluator.Evaluator.Evaluate("1 * 2 + 4", null) != 6)
{
    Console.WriteLine("TEST FAILED1");
}

if (FormulaEvaluator.Evaluator.Evaluate("(5+5)", null) != 10)
{
    Console.WriteLine("TEST FAILED2");
}
if (FormulaEvaluator.Evaluator.Evaluate("8 / 4", null) != 2)
{
    Console.WriteLine("TEST FAILED3");
}


if (FormulaEvaluator.Evaluator.Evaluate("1 * 0", null) != 0)
{
    Console.WriteLine("TEST FAILED4");
}

if (FormulaEvaluator.Evaluator.Evaluate("(1 * 2) * 3", null) != 6)
{
    Console.WriteLine("TEST FAILED5");
}

if (FormulaEvaluator.Evaluator.Evaluate("1 + Z6 - 4", SimpleLookup) != 7)
{
    Console.WriteLine("TEST FAILED6");
}

if (FormulaEvaluator.Evaluator.Evaluate("10-6", null) != 4)
{
    Console.WriteLine("TEST FAILED7");
}

if (FormulaEvaluator.Evaluator.Evaluate("2+3*(8/4)", null) != 8)
{
    Console.WriteLine("TEST FAILED8");
}

static int SimpleLookup(string v)
{
    return 10;
}

try
{
    Evaluator.Evaluate(" -A- ", null);
}
catch (ArgumentException e)
{
    Console.WriteLine("Exception Catched");
}
try
{
    Evaluator.Evaluate("5/0", null);
}
catch (ArgumentException e)
{
    Console.WriteLine(e.Message);
}