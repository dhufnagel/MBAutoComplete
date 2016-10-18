using System;
using System.Collections.Generic;

namespace MBAutoComplete
{
	public interface ISortingAlghorithm
	{
		ICollection<object> DoSort(string userInput, ICollection<object> inputStrings);
	}
}

