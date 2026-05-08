using Code.Scripts.Src.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Code.Scripts.Src.Configurations;

namespace Code.Scripts.Src.Utils
{
    public static class RuneUtils
    {
        public static int GetRuneClass(RuneTypes runeType)
        {
            int @class = 0;
            var runeClasses = RuneConfigurations.RuneClasses;
            for (int i = 0; i < runeClasses.Count; i++)
            {
                if (runeClasses[i].Contains(runeType))
                {
                    @class = i;
                    break;
                }
            }
            return @class;
        }
    }
}
