using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wordle.Model.Settings
{
    public interface IGameSettings
    {
        public int WordLength();
        public int GuessCount();
    }
}
