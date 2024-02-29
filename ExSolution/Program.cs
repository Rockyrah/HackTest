// See https://aka.ms/new-console-template for more information
using ExSolution;

Console.WriteLine("Hello, World!");

//Example1 ex1 = new Example1();
//ex1.GetFun();
//var tt = Football.getTotalGoals("Chelsea", 2014);

//Console.WriteLine("Barcelon in both team1 + team2 = " + tt);

//var tt = Football.getNumDraws(2011);

//Console.WriteLine("Barcelon in both team1 + team2 = " + tt);

var tt = Article.getUserNames(300);

foreach (var t in tt.Result)
{
    Console.WriteLine(t);
}

Console.Write("[");
for (int i = 0; i < tt.Result.Count; i++)
{
    Console.Write("\"" + tt.Result[i] + "\"");
    if (i < tt.Result.Count - 1)
    {
        Console.Write(", ");
    }
}
Console.WriteLine("]");

//var tt = Football.getWinnerTotalGoals("UEFA Champions League", 2011);

//Console.WriteLine("Barcelon in both team1 + team2 = " +tt);

//Example1 ex1 = new Example1();
//bool result = ex1.CompareString("Test", "Te");
//string s = "catdog";
//List<string> wordD = new List<string> { "ddd", "ddfc", "mat" };

//bool resu = ex1.WordBreak(s, wordD);

//Console.WriteLine("Result - " + resu);
