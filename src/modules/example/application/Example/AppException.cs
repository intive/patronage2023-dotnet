using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intive.Patronage2023.Modules.Example.Application.Example
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
		/// <param name="message">a.</param>
		public AppException(string message)
			: base(message)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="AppException"/> class.
		/// </summary>
		/// <param name="message">Message.</param>
		/// <param name="args">Args.</param>
		public AppException(string message, params object[] args)
			: base(string.Format(CultureInfo.CurrentCulture, message, args))
		{
		}
	}
}
