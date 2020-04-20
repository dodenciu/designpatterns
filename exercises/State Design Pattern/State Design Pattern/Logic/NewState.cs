using System;

namespace State_Design_Pattern.Logic
{
    class NewState : BookingState
    {
        public override void Cancel(BookingContext booking)
        {
            booking.TransitionToState(new ClosedState("Booking cancelled"));
        }

        public override void DatePassed(BookingContext booking)
        {
            booking.TransitionToState(new ClosedState("Booking expired"));
        }

        public override void EnterDetails(BookingContext booking, string attendee, int tickets)
        {
            booking.Attendee = attendee;
            booking.TicketCount = tickets;
            booking.TransitionToState(new PendingState());
        }

        public override void EnterState(BookingContext booking)
        {
            booking.BookingID = new Random().Next();
            booking.ShowState("New");
            booking.View.ShowEntryPage();
        }
    }
}
