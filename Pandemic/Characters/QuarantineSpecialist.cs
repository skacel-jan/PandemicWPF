using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Pandemic.Characters
{
    public class QuarantineSpecialist : Character
    {
        public override string Role => "Quarantine specialist";

        public override IEnumerable<string> RoleDescription => throw new NotImplementedException();

        public override Color Color => throw new NotImplementedException();
    }
}
