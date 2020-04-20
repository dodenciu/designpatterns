using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace State_Design_Pattern.Logic
{
    class BookedState : BookingState
    {
        public override void Cancel(BookingContext booking)
        {
            booking.TransitionToState(new ClosedState("Booking cancelled, expect a refund"));
        }

        public override void DatePassed(BookingContext booking)
        {
            booking.TransitionToState(new ClosedState("Hope u enjoyed the show"));
        }

        public override void EnterDetails(BookingContext booking, string attendee, int tickets)
        {
            booking.View.ShowError("Invalid action for this state", "Closed booking error");
        }

        public override void EnterState(BookingContext booking)
        {
            booking.ShowState("Booked");
            booking.View.ShowStatusPage("Enjoy the show");
        }
    }
}
