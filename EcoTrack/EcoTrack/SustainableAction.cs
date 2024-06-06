using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace EcoTrack
{
    public class SustainableAction
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string ActionCode { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string ImpactLevel { get; set; }
        public string Frequency { get; set; }
    }
}
