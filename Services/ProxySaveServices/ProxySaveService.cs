using Domain.Models;
using Domain.Repositories.MerenjaRepositories;
using Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace Services.ProxySaveServices
{
    public class ProxySaveService : IProxySave
    {
        private readonly IReadData serverReadDataService;
        private readonly IMerenjaRepository merenjaRepo;
        private Dictionary<int, (List<Merenje> podaci, DateTime poslednjiPristup)> lokalnaMerenja;
        private System.Timers.Timer timer;
        private readonly object lokalnaMerenjaLock = new object();
        public ProxySaveService(IReadData serverReadDataService,IMerenjaRepository merenjaRepo)
        {
            this.serverReadDataService = serverReadDataService;
            this.merenjaRepo = merenjaRepo;
            lokalnaMerenja = [];

            timer = new System.Timers.Timer(300000);
            timer.Elapsed += OnTimedEvent;
            timer.Start();
        }

        private void OnTimedEvent(object? sender, ElapsedEventArgs e)
        {
            lock (lokalnaMerenjaLock)
            {
                OcistiZastarelePodatke();
            }
        }
        public Dictionary<int, (List<Merenje> podaci, DateTime poslednjiPristup)> GetLokalnaMerenja()
        {
            return lokalnaMerenja;
        }
        public bool AzurirajLokalnePodatke(int deviceId)
        {
            lock (lokalnaMerenjaLock)
            {
                if (!lokalnaMerenja.ContainsKey(deviceId))
                {
                    lokalnaMerenja[deviceId] = (new List<Merenje>(), DateTime.Now);
                }

                var svaMerenjaPoId = serverReadDataService.ProcitajSvaMerenjaPoDeviceId(deviceId);
                var lokalniPodaci = lokalnaMerenja[deviceId].podaci;
                foreach (var ms in svaMerenjaPoId)
                {
                    bool postoji = false;
                    foreach (var lm in lokalniPodaci)
                    {
                        if (lm.Id == ms.Id)
                        {
                            postoji = true;
                            break;
                        }
                    }

                    if (!postoji)
                    {
                        lokalniPodaci.Add(ms);
                    }
                }
                lokalnaMerenja[deviceId] = (lokalniPodaci, DateTime.Now);
                return true;
            }
        }
        public bool OcistiZastarelePodatke()
        {
            DateTime trenutnoVreme = DateTime.Now;
            List<int> uredjajiZaBrisanje = new List<int>();

            lock (lokalnaMerenjaLock)
            {
                foreach (var lokalnoMerenje in lokalnaMerenja)
                {
                    if ((trenutnoVreme - lokalnoMerenje.Value.poslednjiPristup).TotalHours > 24)
                    {
                        uredjajiZaBrisanje.Add(lokalnoMerenje.Key);
                    }
                }

                foreach (var devId in uredjajiZaBrisanje)
                {
                    lokalnaMerenja.Remove(devId);
                }
            }
            return true;
        }
    }
}
