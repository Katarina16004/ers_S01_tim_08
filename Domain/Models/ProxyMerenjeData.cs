namespace Domain.Models
{
    public class ProxyMerenjeData : Merenje
    {
        public DateTime LastAccessedForRead { get; set; } = DateTime.Now;

        public ProxyMerenjeData() { }

        public ProxyMerenjeData(DateTime lastAccessedForRead)
        {
            LastAccessedForRead = lastAccessedForRead;
        }
    }
}
