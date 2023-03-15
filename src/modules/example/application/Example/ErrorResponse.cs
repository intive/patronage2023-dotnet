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
		private string? type;
		private string? title;
		private string? traceId;
		private List<ValidationFailure>? errors;

		/// <summary>
		/// Initializes a new instance of the <see cref="ErrorResponse"/> class.
		/// </summary>
		/// <param name="type">Type.</param>
		/// <param name="title">Title.</param>
		/// <param name="traceId">Trace ID.</param>
		/// <param name="errors">Errors.</param>
		public ErrorResponse(string? type, string? title, string? traceId, List<ValidationFailure>? errors)
		{
			this.Type = type;
			this.Title = title;
			this.TraceId = traceId;
			this.Errors = errors;
		}

		/// <summary>
		/// Type.
		/// </summary>
		public string? Type
		{
			get => this.type;
			set => this.type = value;
		}

		/// <summary>
		/// Title.
		/// </summary>
		public string? Title
		{
			get => this.title;
			set => this.title = value;
		}

		/// <summary>
		/// Trace ID.
		/// </summary>
		public string? TraceId
		{
			get => this.traceId;
			set => this.traceId = value;
		}

		/// <summary>
		/// Errors.
		/// </summary>
		public List<ValidationFailure>? Errors
		{
			get => this.errors;
			set => this.errors = value;
		}

		/// <summary>
		/// Create error response JSON object.
		/// </summary>
		/// <returns>ErrorResponse object.</returns>
		public string CreateResponse()
		{
			return JsonSerializer.Serialize(this);
		}
	}
}
