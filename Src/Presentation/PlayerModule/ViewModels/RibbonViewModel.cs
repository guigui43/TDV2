using System.ComponentModel.Composition;
using System.Windows.Input;
using PlayerModule.Views;
using TDV.Client.Data.ResourceManager;
using TDV.Client.Infrastructure;
using TDV.Client.Infrastructure.Commands;
using TDV.Client.Infrastructure.Interfaces;
using TDV.Client.Infrastructure.Messaging;

namespace PlayerModule.ViewModels
{
    [Export(typeof(IStaticViewModel))]
    [View(typeof(RibbonView))]
    public class RibbonViewModel : BaseViewModel, IStaticViewModel
    {
        #region Private members only
        private bool _canGetData;
        #endregion

        #region IStaticView Members
        private string _staticViewName;
        public string StaticViewName
        {
            get { return _staticViewName; }
            set { _staticViewName = value; OnPropertyChanged("StaticViewName"); }
        }
        private string _openButtonContent;
        public string OpenButtonContent
        {
            get { return _openButtonContent; }
            set { _openButtonContent = value; OnPropertyChanged("OpenButtonContent"); }
        }
        #endregion

        public ICommand GetDataCommand { get; private set; }
        public ICommand OpenModuleCommand { get; private set; }

        public RibbonViewModel() : base("PlayerModule Ribbon", true, false)
        {
            StaticViewName = "PlayerModule Ribbon";
            GetDataCommand = new SimpleCommand<object>(o => _canGetData, GetData);
            OpenModuleCommand = new SimpleCommand<bool>(OpenModule);
            OpenButtonContent = "Open Player Module";
        }

        private void OpenModule(bool state)
        {
            _canGetData = state;
            OpenButtonContent = state ? "Close Player Module" : "Open Player Module";
            Mediator.GetInstance.Broadcast(Topic.PlayerModuleOpen, state);
        }

        private static void GetData(object _)
        {
            Mediator.GetInstance.Broadcast(Topic.PlayerServiceGetData);
        }
    }
}
