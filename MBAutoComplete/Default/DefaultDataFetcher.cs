using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MBAutoComplete
{
	public class DefaultDataFetcher : IDataFetcher
	{
		private ICollection<object> _unsortedData; 

		public DefaultDataFetcher(ICollection<object> unsortedData)
		{
			_unsortedData = unsortedData;
		}

		public Task PerformFetch(MBAutoCompleteTextField textfield, Action<ICollection<object>> completionHandler)
		{
			completionHandler(_unsortedData);

            return new Task(() => { });
		}
	}
}

