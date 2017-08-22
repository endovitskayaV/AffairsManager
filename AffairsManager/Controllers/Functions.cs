using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AffairsManager.Controllers
{
    public static class Functions
    {
        public static int UnixTimeSeconds()
        {
            return (int)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        }
    }
}