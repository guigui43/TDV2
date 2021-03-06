﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BondModule.Views;
using TDV.Client.Data;
using TDV.Client.Data.Binding;
using TDV.Client.Data.Entities;
using TDV.Client.Data.ResourceManager;
using TDV.Client.Infrastructure;
using TDV.Client.Infrastructure.AttachedCommand;
using TDV.Client.Infrastructure.Collections;
using TDV.Client.Infrastructure.Commands;
using TDV.Client.Infrastructure.Interfaces;
using TDV.Client.Infrastructure.Messaging;

namespace BondModule.ViewModels
{
    [Export(typeof(IDynamicViewModel))]
    [View(typeof(View))]
    public class BondViewModel : BaseViewModel, IDynamicViewModel
    {
        #region IDynamicView Members
        private string _dynamicViewName;
        private readonly DataGrid _grid;
        public string DynamicViewName
        {
            get { return _dynamicViewName; }
            set { _dynamicViewName = value; OnPropertyChanged("DynamicViewName"); }
        }
        #endregion

        public ICommand CreateColumnsCommand { get; private set; }
        public NotifyCollection<Entity> Entities { get; private set; }
 
        public BondViewModel() : base("Bond Module", true, true)
        {
            DynamicViewName = "Bond Module";
            _grid = GetRef<DataGrid>("MainGrid");
 
            // InputManager.Current.PreProcessInput += new PreProcessInputEventHandler(Current_PreProcessInput);
            // InputManager.Current.PostProcessInput += new ProcessInputEventHandler(Current_PostProcessInput);

            Entities = new NotifyCollection<Entity>(EntityBuilder.LoadMetadata(AssetType.Common, AssetType.Bond));
            CreateColumnsCommand = new SimpleCommand<Object, EventToCommandArgs>((parameter) => true, CreateColumns);
        }

        [RegisterInterest(Topic.BondServiceDataReceived, TaskType.Periodic)]
        private void DataReceived(IEnumerable<Entity> entities)
        {
            Action w = () => Entities.AddOrUpdate(entities, false);
            DispatcherFacade.AddToDispatcherQueue(w);
        }

        [RegisterInterest(Topic.BondModuleHang, TaskType.Sporadic)]
        private void Hang()
        {
            ProcessOnDispatcherThread(() => Thread.Sleep(10000));
        }

        [RegisterInterest(Topic.BondModuleOpen, TaskType.Sporadic)]
        private void OpenClose(bool  state)
        {
            ProcessOnDispatcherThread(() =>
            {
                if (state)
                    ((Window)ViewReference).Show();
                else
                    ((Window)ViewReference).Hide();
            });
        }

        private static void CreateColumns(EventToCommandArgs args)
        {
            var ea = (DataGridAutoGeneratingColumnEventArgs)args.EventArgs;

            ea.Column = new WpfGridColumn
                        {
                            Header = ea.PropertyName,
                            Width = ((ea.PropertyType == typeof(String))||(ea.PropertyType == typeof(DateTime)))?
                                        DataGridLength.SizeToCells:DataGridLength.SizeToHeader
                        };
        }
    }
}
