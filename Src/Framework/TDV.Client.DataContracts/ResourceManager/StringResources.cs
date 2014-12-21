﻿namespace TDV.Client.Data.ResourceManager
{
    public static class Topic
    {
        public const string ShellStateUpdated = "Shell.StateUpdated";
        public const string BootstrapperLoadViews = "Bootstrapper.LoadViews";
        public const string BootstrapperUnloadView = "Bootstrapper.UnloadView";
        public const string BondModuleOpen = "BondModule.Open";
        public const string BondModuleHang = "BondModule.Hang";
        public const string FutureModuleHang = "FutureModule.Hang";
        public const string FutureModuleOpen = "FutureModule.Open";
        public const string PlayerModuleHang = "PlayerModule.Hang";
        public const string PlayerModuleOpen = "PlayerModule.Open";
        public const string BondServiceGetData = "BondService.GetData";
        public const string FutureServiceGetData = "FutureService.GetData";
        public const string PlayerServiceGetData = "PlayerService.GetData";
        public const string BondServiceDataReceived = "BondService.DataReceived";
        public const string FutureServiceDataReceived = "FutureService.DataReceived";
        public const string PlayerServiceDataReceived = "PlayerService.DataReceived";
    }
}
