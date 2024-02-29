using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExSolution
{
    public class Example1
    {

        public bool WordBreak(string s, IList<string> wordDict)
        {
            bool result = false;

            if (string.IsNullOrEmpty(s) || wordDict.Count == 0)
                return false;

            int sLen = s.Length;
            bool[] compareArray = new bool[sLen+1];
            compareArray[0] = true;
            HashSet<string> hashWords = new HashSet<string>(wordDict);


            for(int i = 1; i<= sLen; i++)
            {
                for(int j=0; j<i;j++)
                {
                    string sp = s.Substring(j, i - j);
                    Console.WriteLine(sp);
                    if(hashWords.Contains(sp))
                    {
                       result = true;
                        break;
                    }
                    
                }
            }

            //result = IterateWord(s, hashWords, 0, "", compareArray);
            return result;

        }

        public bool IterateWord(string s, HashSet<string> hs, int start, string c, bool[] boolArr )
        {
            bool result = false; 
            if(start == s.Length)
            {
                return false;
            }

            for(int i = start + 1; i<=s.Length; i++)
            {
                string substr = s.Substring(start, i - start);
                if (boolArr[i] && hs.Contains(substr))
                {

                    Console.WriteLine("SecondPrint - " +substr);
                    IterateWord(s, hs, i, "", boolArr);
                }
            }

            return result;
        }
        public bool CompareString(string s, string t)
        {
            bool result = false;
            if(string.IsNullOrEmpty(s) || string.IsNullOrEmpty(t))
            {
                return result;
            }

            if(s.Contains(t))
            {
                result = true;
            }

            return result;
        }
        public void GetFun()
        {
            Console.WriteLine("PrintStatement");
        }

    }
}
