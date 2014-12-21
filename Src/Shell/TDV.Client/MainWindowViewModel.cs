using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Collections.ObjectModel;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using TDV.Client.Data;
using TDV.Client.Data.Binding;
using TDV.Client.Data.ResourceManager;
using TDV.Client.Infrastructure;
using TDV.Client.Infrastructure.Commands;
using TDV.Client.Infrastructure.Messaging;

namespace TDV.Client.Shell
{
    public class MainWindowViewModel : BaseViewModel
    {
        private readonly int _hr = Convert.ToInt32(ConfigurationManager.AppSettings["HEARBEAT_MONITOR_RATE"]);
        private readonly int _ht = Convert.ToInt32(ConfigurationManager.AppSettings["HEARBEAT_THRESHOLD"]);

        public ObservableCollection<SelectableDataItem> Heartbeats { get; private set; }
        public ICommand ReloadCommand { get; private set; }

        private string _staleModule;
        public string StaleModule
        {
            get { return _staleModule; }
            set { _staleModule = value; OnPropertyChanged("StaleModule"); }
        }
        private Visibility _heartbeatLost;
        public Visibility HeartbeatLost
        {
            get { return _heartbeatLost; }
            set { _heartbeatLost = value; OnPropertyChanged("HeartbeatLost"); }
        }

        private readonly ConcurrentDictionary<string, Heartbeat> _hearbeatIndex;
        public MainWindowViewModel() : base("Shell", false, true)
        {
            _hearbeatIndex = new ConcurrentDictionary<string, Heartbeat>();

            Mediator.GetInstance.RegisterInterest<Heartbeat>(Topic.ShellStateUpdated, HeartbeatReceived, TaskType.Periodic);

            HeartbeatLost = Visibility.Collapsed;
            Heartbeats = new ObservableCollection<SelectableDataItem>();

            ReloadCommand = new SimpleCommand<Object>(_ => 
                {
                    Heartbeat removed;
                    _hearbeatIndex.TryRemove(StaleModule, out removed);
                    HeartbeatLost = Visibility.Collapsed;
                    Mediator.GetInstance.Broadcast(Topic.BootstrapperUnloadView, StaleModule);
                });

            var timer = new Timer(_hr);
            timer.Elapsed += (s, e) =>
                                 {
                                     var lostHeartbeats = _hearbeatIndex.Values
                                         .Where(i => (!i.NonRepeatable) && (DateTime.UtcNow - i.TimeCreated).TotalMilliseconds > _ht);
                                     foreach (var l in lostHeartbeats)
                                     {
                                         HeartbeatLost = Visibility.Visible;
                                         StaleModule = l.Key;
                                         Log.Warn(String.Format("Lost heartbeat from: {0}",l.Key));
                                     }
                                 };
            timer.Start();
        }

       public void HeartbeatReceived(Heartbeat heartbeat)
        {
            Action w = () =>
                {
                    if (!_hearbeatIndex.ContainsKey(heartbeat.Key))
                    {
                        Heartbeats.Add(new SelectableDataItem(heartbeat.Message));
                        _hearbeatIndex.TryAdd(heartbeat.Key, heartbeat);
                    }
                    else
                    {
                        var item = Heartbeats.FirstOrDefault(s => s.Value.ToString().Contains(heartbeat.Key));
                        item.Value = heartbeat.Message;
                        _hearbeatIndex.AddOrUpdate(heartbeat.Key, heartbeat, (n, oldValue) => heartbeat);

                        // resuscitate
                        if (heartbeat.Key == StaleModule) HeartbeatLost = Visibility.Collapsed;
                    }
                };
            DispatcherFacade.AddToDispatcherQueue(w);
        }
    }
}
