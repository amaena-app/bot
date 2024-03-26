using System.Formats.Asn1;
using System.Net.Sockets;

namespace BaseEvents
{
    
    public class Event{
        
        public string name {get; set;}
        public string summary {get; set;}

        public DateOnly from_date {get; set;}

        public DateOnly to_date {get; set;}

        public TimeOnly time {get; set;}

        public Address address {get; set;}

        public string[] tags {get; set;}

        public string[] images {get; set;}
    }


    public class Address{

        public string name {get; set;}
        public string address {get; set;}

        public double latitude {get; set;}

        public double longitude {get; set;}

    }



}