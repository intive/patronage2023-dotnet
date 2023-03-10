using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using FluentValidation.Results;

namespace Intive.Patronage2023.Modules.Example.Application.Example
{
	/// <summary>
	/// Error response class.
	/// </summary>
	public class ErrorResponse
	{
		private string? type;
		private string? title;
		private string? traceId;
		private List<ValidationFailure>? errors;

		/// <summary>
		/// Initializes a new instance of the <see cref="ErrorResponse"/> class.
		/// </summary>
		/// <param name="type">type.</param>
		/// <param name="title">title.</param>
		/// <param name="traceId">traceId.</param>
		/// <param name="errors">errors.</param>
		public ErrorResponse(string? type, string? title, string? traceId, List<ValidationFailure>? errors)
		{
			this.type = type;
			this.title = title;
			this.traceId = traceId;
			this.errors = errors;
		}

		/// <summary>
		/// Convert error object to JSON.
		/// </summary>
		/// <returns>Error response in json format.</returns>
		public string ToJson()
		{
			string json = JsonSerializer.Serialize(this);
			return json;
		}
	}
}
