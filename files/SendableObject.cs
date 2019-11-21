namespace EPSIC_Bataille_Navale.Models
{
    public class SendableObject
    {
        public string message;
        public object data;

        public SendableObject(string message, object data)
        {
            this.message = message;
            this.data = data;
        }
    }
}
