using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Systems.Combat
{
    [Flags]
    public enum Phase
    {
        BEGIN = 0,
        DRAW = 2,
        PREP = 4,
        COMBAT = 6,
        PREP2 = 8,
        END = 10
    }

    [Flags]
    public enum SubPhase
    {
        DISABLED = 0,
        NEUTRAL = 2,
        DEPLOYMENT = 4,
        VIEWING = 6
    }

    [Flags]
    public enum Position
    {
        LF = 0,
        CF = 2,
        RF = 4,
        LB = 6,
        CB = 8,
        RB = 10,
        L = 12
    }
}
