using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wordle.Model.WordLists
{
    public class SimpleWordList : IWordList
    {
        public List<string> GetWordList()
        {
            return new List<string>()
            {
                "alert", "bound", "break", "clear", "close", "codes", "enums", "false", "files", "final", "float", "index",
                "inset", "logic", "mouse", "nodes", "pixel", "print", "scope", "short", "stack", "stage", "super", "throw",
                "token", "value", "while", "world", "write"
            };
        }
    }
}
