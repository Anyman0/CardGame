// See https://aka.ms/new-console-template for more information

using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata.Ecma335;

namespace CardGame;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
        Deck d = new Deck(new Card[0]);
        var newDeck = d.CreateDeck();
        //int max = MaxValue("mmerytsm");
        ShortestPath(new string[] { "5", "A", "B", "C", "D", "E", "A-B", "A-E", "B-C", "B-D", "C-D", "D-E" });
        
    }

    // Task 1
    // Get the maximum value between 2 similar characters from a string (IE. "MMMWEAM" --> Return should be 3, since there are 'WEA' between M's.
    public static int MaxValue(string str) 
    {
        Dictionary<char, int> differentChars = new Dictionary<char, int>();
        List<int> gaps = new List<int>();
        
        // Loop through every character in given string        
        for(int i = 0; i < str.Length; i++)
        {
            if (!differentChars.ContainsKey(str[i]))
            {
                differentChars.Add(str[i], 1);
                gaps.Add(CountGap(str[i].ToString(), str)); // Find all gaps between similar characters and return the largest cap
            }            
        }
        foreach(var s in differentChars) 
        { 
            Console.WriteLine("{0} - {1}", s.Key, s.Value);
        }
        Console.WriteLine(gaps.Max());
        return gaps.Max();
    }

    public static int CountGap(string toCount, string wholeString)
    {
        int count = -1;
        List<int> results = new List<int>();
        bool canCount = false;
        for(int i = 0; i < wholeString.Length; i++)
        {
            try
            {
                if (canCount)
                {
                    count++;
                    if (wholeString[i].ToString() == toCount)
                    {
                        results.Add(count);
                        canCount = false;                       
                    }
                }
                if (toCount == wholeString[i].ToString() && wholeString[i + 1].ToString() != toCount && !canCount)
                {
                    canCount = true;
                    count = -1;
                }                

            }
            catch
            {

            }
           
        }

        if (results.Count > 0)
        {
            return results.Max();
        }
        else return 0;
    }


    // Task 2
    // Find the shortest path from given array. (IE. "String[] {"5", "A", "B", "C", "D", "E", "A-C", "A-B", "B-C", B-D", "C-D", "D-E"}. --> Return A-C-D-E. We can skip B in this case.
    public static string[] ShortestPath(string[] strArr)
    {
        int startIndex = int.Parse(strArr[0]);
        string endAlphabet = strArr[startIndex];
        List<string> pathList = new List<string>();
        string nextPath = string.Empty;
        List<string> result = new List<string>();
        string stringResult = string.Empty;
        string resultWithoutDublicates = string.Empty;
        
        // Populate pathList
        for(int i = strArr.Length - 1; i > startIndex; i--)
        {
            pathList.Add(strArr[i]);
        }
        
        try
        {                    
            pathList.Sort();
            nextPath = pathList[0].Split('-')[0];
            result.Add(NextPath(pathList, nextPath));
            while (result[result.Count - 1].Split('-')[1] != endAlphabet)
            {
                nextPath = result[result.Count - 1].Split('-')[1];
                result.Add(NextPath(pathList, nextPath));
            }
            
        }
        catch
        {

        }

        stringResult = string.Join('-', result);
        for(int i = 0; i < stringResult.Length; i++)
        {
            try
            {
                if (!resultWithoutDublicates.Contains(stringResult[i]))
                {
                    resultWithoutDublicates += stringResult[i];
                }
                else if (resultWithoutDublicates.Contains(stringResult[i]) && stringResult[i] == '-' && resultWithoutDublicates.LastOrDefault() != '-')
                {
                    resultWithoutDublicates += stringResult[i];
                }
            }
            catch
            {

            }
        }
        Console.WriteLine(resultWithoutDublicates);
        return strArr;
    }

    public static string NextPath(List<string> pathList, string moveFrom)  
    {
        string result = string.Empty;
        List<string> options = new List<string>(); 
        pathList.Sort(); // List sorted alphabetically

        for(int i = 0; i < pathList.Count; i++)
        {
            if (pathList[i].Split('-')[0] == moveFrom)
            {
                options.Add(pathList[i]);
            }
        }
        options.Sort();
        options.Reverse();

        result = options[0];

        return result;
    }

}