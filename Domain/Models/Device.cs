using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Device
    {
        public int Id { get; set; } = 0;

        public string Naziv {  get; set; } = string.Empty;

        public Device() { }

        public Device(int id, string naziv)
        {
            Id = id;
            Naziv = naziv;
        }

        public override string? ToString()
        {
            return $"[Device {Id}] = {Naziv}\n";
        }
    }
}
