using TDV.Client.Data.Interfaces;

namespace TDV.Client.Data.Binding
{
    public class SelectableDataItem : DataItem, ISelectable
    {
        #region private members
        private bool _isSelected;
        #endregion

        #region Constructors
        public SelectableDataItem(object value) : base(value)
        {
        }

        #endregion

        #region ISelectableItem Members

        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }
            set
            {
                _isSelected = value;
                OnPropertyChanged("IsSelected");

            }
        }

        #endregion
    }
}
