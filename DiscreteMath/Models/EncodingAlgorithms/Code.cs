using DiscreteMath.Controllers.Encoding;
using System;
using System.Collections.Generic;

namespace DiscreteMath.Models
{
    public class Code
    {
        public Dictionary<char, string> ElementaryCodes { get; set; }

        public Code() 
        {
            ElementaryCodes = new Dictionary<char, string>();
        }

        public Code(Dictionary<char,string> elementaryCodes)
        {
            ElementaryCodes = elementaryCodes;
        }

        public virtual void Set(char symbol, string code)
        {
            if (ElementaryCodes.ContainsKey(symbol))
            {
                ElementaryCodes[symbol] = code;
            }
            else
            {
                ElementaryCodes.Add(symbol, code);
            }
        }

        public void Remove(char symbol)
        {
            ElementaryCodes.Remove(symbol);
        }
    }
}