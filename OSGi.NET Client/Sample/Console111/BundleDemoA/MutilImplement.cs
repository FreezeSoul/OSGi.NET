using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ServiceContract;

namespace BundleDemoA
{
    public class MutilImplementA : IMutilImplement
    {
        public string TestMethod()
        {
            return this.GetType().Name;
        }
    }

    public class MutilImplementB : IMutilImplement
    {
        public string TestMethod()
        {
            return this.GetType().Name;
        }
    }

    public class MutilImplementC : IMutilImplement
    {
        public string TestMethod()
        {
            return this.GetType().Name;
        }
    }
}
