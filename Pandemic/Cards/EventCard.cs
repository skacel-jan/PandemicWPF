using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandemic.Cards
{
    public class EventCard : Card
    {
        public EventCard(string name, Action @event, string description) : base(name)
        {
            Event = @event ?? throw new ArgumentNullException(nameof(@event));
            Description = description ?? throw new ArgumentNullException(nameof(description));
        }

        public Action Event { get;  private set; }

        public string Description { get; private set; }
    }
}
