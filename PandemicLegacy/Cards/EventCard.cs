using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PandemicLegacy.Cards
{
    public class EventCard : Card
    {
        public Action Event { get; set; }
    }
}
