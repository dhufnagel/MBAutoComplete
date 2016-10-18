using System.Collections.Generic;

namespace MBAutoComplete
{
	public class NoSortingAlghorithm : ISortingAlghorithm
	{
		public NoSortingAlghorithm(){}

		public ICollection<object> DoSort(string userInput, ICollection<object> inputStrings)
		{
			return inputStrings;
		}
	}
}

