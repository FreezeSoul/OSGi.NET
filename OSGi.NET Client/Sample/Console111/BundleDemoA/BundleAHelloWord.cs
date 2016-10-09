using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ServiceContract;

namespace BundleDemoA
{
    public class BundleAHelloWord : IHelloWord
    {
        public string SayHi(string name)
        {
            return string.Format("{0} answer: Hi {1}!", "BundleDemoA", name);
        }
    }
}
