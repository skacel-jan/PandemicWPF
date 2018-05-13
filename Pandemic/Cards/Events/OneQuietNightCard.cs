using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandemic.Cards
{
    public class OneQuietNightCard : EventCard
    {
        public OneQuietNightCard() : base("One Quiet Night")
        {
        }

        public event EventHandler SkipInfectionPhase;
        
        public override void PlayEvent()
        {
            SkipInfectionPhase?.Invoke(this, EventArgs.Empty);

            OnEventFinished(EventArgs.Empty);
        }
    }
}
