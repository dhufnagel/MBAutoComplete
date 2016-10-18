using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using UIKit;


namespace MBAutoComplete
{
    public abstract class MBAutoCompleteViewSource : UITableViewSource
    {
        public class AutoCompleteEventArgs : EventArgs
        {
            public object SelectedObject { get; private set; }

            public AutoCompleteEventArgs(object selectedObject)
            {
                SelectedObject = selectedObject;
            }
        }

        public event EventHandler<AutoCompleteEventArgs> RowSelectedEvent;

        private ICollection<object> _suggestions = new List<object>();
        public ICollection<object> Suggestions
        {
            get
            {
                return _suggestions;
            }
            set
            {
                _suggestions = value;
                NewSuggestions(_suggestions);
            }
        }

        public abstract void NewSuggestions(ICollection<object> suggestions);

        public MBAutoCompleteTextField AutoCompleteTextField
        {
            get;
            set;
        }

        public abstract override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath);

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return Suggestions.Count;
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            object selectedElement = Suggestions.ElementAt(indexPath.Row);
            if (selectedElement is string)
            {
                AutoCompleteTextField.Text = selectedElement as string;
            }
            AutoCompleteTextField.AutoCompleteTableView.Hidden = true;

            RowSelectedEvent?.Invoke(this, new AutoCompleteEventArgs(selectedElement));
        }
    }
}

