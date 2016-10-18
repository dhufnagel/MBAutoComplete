using System.Collections.Generic;
using System.Linq;
using Foundation;
using UIKit;

namespace MBAutoComplete
{
	public class DefaultDataSource : MBAutoCompleteViewSource
	{
		private string _cellIdentifier = "DefaultIdentifier";
		private ICollection<object> _suggestions;

		public override void NewSuggestions(ICollection<object> suggestions)
		{
			this._suggestions = suggestions;
		}

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			UITableViewCell cell = tableView.DequeueReusableCell(_cellIdentifier);
			string item = _suggestions.ElementAt(indexPath.Row).ToString();

			if (cell == null)
				cell = new UITableViewCell(UITableViewCellStyle.Default, _cellIdentifier);

			cell.BackgroundColor = UIColor.GroupTableViewBackgroundColor;
			cell.TextLabel.Text = item;

			return cell;
		}



		public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
		{
			base.RowSelected(tableView, indexPath);
		}
	}
}

