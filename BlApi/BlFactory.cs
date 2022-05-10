using System;
using System.Collections.Generic;
using System.Text;
using BL;

namespace BlApi
{
    public static class BlFactory
    {
        public static IBL GetBl()
        {
            return Instance;
        }
    }
}
