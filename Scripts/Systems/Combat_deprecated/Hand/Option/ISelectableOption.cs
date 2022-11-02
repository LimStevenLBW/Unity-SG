using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Systems.Combat
{
    public interface ISelectableOption
    {
       void ExecuteTasks();
       string GetName();
    }
}
