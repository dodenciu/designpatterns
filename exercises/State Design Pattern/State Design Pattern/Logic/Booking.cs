using State_Design_Pattern.UI;
using System;
using System.Threading;

namespace State_Design_Pattern.Logic
{
    public class Booking
    {
        private MainWindow View { get; set; }
        public string Attendee { get; set; }
        public int TicketCount { get; set; }
        public int BookingID { get; set; }

        private bool isNew;
        private bool isPending;
        private bool isBooked;

        private CancellationTokenSource cancelToken;

        public Booking(MainWindow view)
        {
            isNew = true;
            View = view;
            BookingID = new Random().Next();
            ShowState("New");
            View.ShowEntryPage();
        }

        public void SubmitDetails(string attendee, int ticketCount)
        {
            if (isNew)
            {
                isNew = false;
                isPending = true;

                Attendee = attendee;
                TicketCount = ticketCount;

                cancelToken = new CancellationTokenSource();

                StaticFunctions.ProcessBooking(this, ProcessingComplete, cancelToken);

                ShowState("Pending");
                View.ShowStatusPage("Processing Booking");
            }

        }

        public void Cancel()
        {
            if (isNew)
            {
                ShowState("Closed");
                View.ShowStatusPage("Canceled by user");
                isNew = false;
            }
            else if (isPending)
            {
                cancelToken.Cancel();
            }
            else if (isBooked)
            {
                isBooked = false;
                ShowState("Close");
                View.ShowStatusPage("Booking closed: Expect a refund");
            }
            else
            {
                View.ShowError("Closed booking can not be cancelled");
            }

        }

        public void DatePassed()
        {
            if (isNew)
            {
                ShowState("Closed");
                View.ShowStatusPage("Booking expired");
                isNew = false;
            }
            else if (isBooked)
            {
                isBooked = false;
                ShowState("Closed");
                View.ShowStatusPage("We hope you enjoyed the event");                                
            }
            
        }

        public void ProcessingComplete(Booking booking, ProcessingResult result)
        {
            isPending = false;
            switch (result)
            {
                case ProcessingResult.Sucess:
                    isBooked = true;
                    ShowState("Booked");
                    View.ShowStatusPage("Enjoy the Event");                    
                    break;
                case ProcessingResult.Fail:
                    View.ShowProcessingError();
                    Attendee = string.Empty;
                    BookingID = new Random().Next();
                    isNew = true;
                    ShowState("New");                    
                    View.ShowEntryPage();
                    break;
                case ProcessingResult.Cancel:
                    ShowState("Closed");
                    View.ShowStatusPage("Processing Canceled");
                    break;
            }
        }

        public void ShowState(string stateName)
        {
            View.grdDetails.Visibility = System.Windows.Visibility.Visible;
            View.lblCurrentState.Content = stateName;
            View.lblTicketCount.Content = TicketCount;
            View.lblAttendee.Content = Attendee;
            View.lblBookingID.Content = BookingID;
        }



    }
}


