using System;
using System.Collections.Generic;
using System.Text;

namespace spb2xml
{
    class SPBException : Exception
    {
        public SPBException(string msg)
            : base(msg)
        {
        }
    }
}
