using System;
using System.Configuration;
using System.Threading.Tasks;
using TDV.Client.Data;
using TDV.Client.Data.Interfaces;

namespace TDV.Client.Server
{
    public class PlayerServer
    {
        private static readonly Random Rand = new Random();
        private static readonly bool IsProcessorAffinity = Convert.ToBoolean(ConfigurationManager.AppSettings["PROCESSOR_AFFINITY"]);

        public static void GetData(IRemotePublishingService proxy)
        {
            Task.Factory.StartNew(() =>
            {
                {
                    for (; ; )
                    {
                        if (IsProcessorAffinity) ProcessorAffinity.BeginAffinity(0);

                        var key = String.Intern("PLAYER" + Rand.Next(1, 300));
                        int prop = Rand.Next(1, 3);
                        string propName = string.Empty;
                        object propValue = null;
                        switch (prop)
                        {
                            case 1:
                                propName = "PlayList1";
                                propValue = Rand.NextDouble();
                                break;
                            case 2:
                                propName = "PlayList2";
                                propValue = Rand.NextDouble();
                                break;
                            case 3:
                                propName = "PlayList3";
                                propValue = Rand.NextDouble();
                                break;
                        }
                        proxy.DataSetRecordsChanged(new DataRecord(key, String.Intern(propName), propValue));
                    }
                }
            });
        }

    }
}
