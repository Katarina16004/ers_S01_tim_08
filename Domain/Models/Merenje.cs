using Domain.Enums;

namespace Domain.Models
{
    public class Merenje
    {
        public int Id { get; set; } = 0;

        public TipMerenja Tip { get; set; }

        public DateTime Timestamp { get; set; }

        public int Value { get; set; } = 0;

        public int DeviceId { get; set; } = 0;

        public Merenje() { }

        public Merenje(int id, TipMerenja tip, DateTime timestamp, int value, int deviceId)
        {
            Id = id;
            Tip = tip;
            Timestamp = timestamp;
            Value = value;
            DeviceId = deviceId;
        }

        public override string ToString()
        {
            return $"[{Timestamp} / {Tip} Merenje {Id}] = {Value}\n";
        }
    }
}
