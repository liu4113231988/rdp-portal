using System;

namespace RDP_Portal
{
    public class ConnectionHistory
    {
        public int Id { get; set; }
        public int ProfileId { get; set; }
        public string ProfileName { get; set; } = "";
        public string Computer { get; set; } = "";
        public DateTime ConnectedAt { get; set; }
    }
}
