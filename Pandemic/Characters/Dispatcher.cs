using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Pandemic.Characters
{
    public class Dispatcher : Character
    {
        public override string Role => "Disptacher";

        public override IEnumerable<string> RoleDescription => throw new NotImplementedException();

        public override Color Color => throw new NotImplementedException();
    }
}
