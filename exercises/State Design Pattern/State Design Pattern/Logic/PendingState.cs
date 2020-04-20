using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace State_Design_Pattern.Logic
{
    class PendingState : BookingState
    {
        private CancellationTokenSource cancelToken;
        public override void Cancel(BookingContext booking)
        {
            cancelToken.Cancel();
        }

        public override void DatePassed(BookingContext booking)
        {
            booking.TransitionToState(new ClosedState("Event date passed, wait for refund"));
        }

        public override void EnterDetails(BookingContext booking, string attendee, int tickets)
        {
            booking.View.ShowError("Invalid action for this state", "Closed booking error");
        }

        public override void EnterState(BookingContext booking)
        {
            cancelToken = new CancellationTokenSource();

            booking.ShowState("Pending");
            booking.View.ShowStatusPage("Processing booking");

            StaticFunctions.ProcessBooking(booking, ProcessingComplete, cancelToken);
        }

        public void ProcessingComplete(BookingContext booking, ProcessingResult result)
        {
            switch(result)
            {
                case ProcessingResult.Sucess:
                    booking.TransitionToState(new BookedState());
                    break;
                case ProcessingResult.Fail:
                    booking.View.ShowProcessingError();
                    booking.TransitionToState(new NewState());
                    break;
                case ProcessingResult.Cancel:
                    booking.TransitionToState(new ClosedState("Booking cancelled before processing"));
                    break;
                default:
                    booking.View.ShowProcessingError();
                    break;
            }
        }
    }
}
