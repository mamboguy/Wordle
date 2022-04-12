using System.Collections.Generic;

namespace Wordle.Model.WordLists
{
    public interface IWordList
    {
        public List<string> GetWordList();
    }
}
