using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using System.Threading.Tasks;
using System.Xml.Linq;
using FluentValidation.Results;

namespace Intive.Patronage2023.Modules.Example.Application.Example
{
	/// <summary>
	/// Error response class.
	/// </summary>
	public class ErrorResponse
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ErrorResponse"/> class.
		/// </summary>
		/// <param name="type">Type.</param>
		/// <param name="traceId">Trace ID.</param>
		/// <param name="errors">Errors.</param>
		public ErrorResponse(string? type, string? traceId, List<ValidationFailure>? errors)
		{
			this.Type = type;
			this.Title = "One or more validation errors occured.";
			this.Status = 400;
			this.TraceId = traceId;
			this.Errors = errors;
			this.FullMessage = JsonSerializer.Serialize(this);
		}

		/// <summary>
		/// Type.
		/// </summary>
		public string? Type { get; set; }

		/// <summary>
		/// Title.
		/// </summary>
		public string? Title { get; set; }

		/// <summary>
		/// Status code.
		/// </summary>
		public int? Status { get; set; }

		/// <summary>
		/// Trace ID.
		/// </summary>
		public string? TraceId { get; set; }

		/// <summary>
		/// Errors.
		/// </summary>
		public List<ValidationFailure>? Errors { get; set; }

		/// <summary>
		/// Full error message serialized to JSON.
		/// </summary>
		public string? FullMessage { get; set; }
	}
}
