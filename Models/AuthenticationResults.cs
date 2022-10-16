using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NBA_App.Models
{
    public class AuthenticationResults
    {
        public string Token { get; set; }
        public bool Results { get; set; }
        public List<string> Errors { get; set; }
    }
}