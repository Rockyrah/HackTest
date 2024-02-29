using System;
using System.Collections;

namespace ExSolution
{
    public class TestEx
    {
        public void stackprint()
        {
            Stack st = new Stack();

            st.Push(1);
            st.Push(1.1);
            st.Push('z');
            st.Push("Hello");

            foreach(var e in st)
            {
                Console.WriteLine(e);
            }

        }
    }
}
