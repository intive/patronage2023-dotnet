using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.Results;

namespace Intive.Patronage2023.Modules.Budget.Application.Budget
{
	/// <summary>
	/// AppException class.
	/// </summary>
	public class AppException : Exception
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="AppException"/> class.
		/// </summary>
		public AppException()
			: base()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="AppException"/> class.
		/// </summary>
		/// <param name="message">Error message.</param>
		/// <param name="args">Arguments.</param>
		public AppException(string message, params object[] args)
		: base(string.Format(CultureInfo.CurrentCulture, message, args))
		{
		}
	}
}
