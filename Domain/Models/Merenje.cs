using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Merenje
    {
        public int Id { get; set; } = 0;

        public TipMerenja Tip {  get; set; }

        public DateTime Timestamp { get; set; }

        public int Value { get; set; } = 0;

        public Merenje() { }

        public Merenje(int id, TipMerenja tip, DateTime timestamp, int value)
        {
            Id = id;
            Tip = tip;
            Timestamp = timestamp;
            Value = value;
        }

        public override string ToString()
        {
            return $"[{Timestamp} / {Tip} Merenje {Id}] = {Value}\n";
        }
    }
}
