namespace Keyfactor.Models
{
    public partial class ServerDateTime
    {
        private readonly DateTime _dateTime;

        public DateTime Value
        {
            get { return _dateTime; }
        }

        extern public ServerDateTime GetCurrentTime();

        public extern ServerDateTime GetMinValue(); 

        public extern ServerDateTime AddMilliseconds(ServerDateTime source, int millis); 
    }
    
}