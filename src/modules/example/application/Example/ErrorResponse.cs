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
	[JsonIncludePrivateFields]
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
			this.type = type;
			this.title = title;
			this.traceId = traceId;
			this.errors = errors;
			Console.WriteLine(this.type);
		}

		/// <summary>
		/// Create error response JSON object.
		/// </summary>
		/// <returns>ErrorResponse object.</returns>
		public string CreateResponse()
		{
			var options = new JsonSerializerOptions { TypeInfoResolver = new DefaultJsonTypeInfoResolver { Modifiers = { AddPrivateFieldsModifier } } };
			return JsonSerializer.Serialize(this, options);
		}

		/// <summary>
		/// Private fields serializing.
		/// </summary>
		/// <param name="jsonTypeInfo">json type info.</param>
		private static void AddPrivateFieldsModifier(JsonTypeInfo jsonTypeInfo)
		{
			if (jsonTypeInfo.Kind != JsonTypeInfoKind.Object)
			{
				return;
			}

			if (!jsonTypeInfo.Type.IsDefined(typeof(JsonIncludePrivateFieldsAttribute), inherit: false))
			{
				return;
			}

			foreach (FieldInfo field in jsonTypeInfo.Type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic))
			{
				JsonPropertyInfo jsonPropertyInfo = jsonTypeInfo.CreateJsonPropertyInfo(field.FieldType, field.Name);
				jsonPropertyInfo.Get = field.GetValue;
				jsonPropertyInfo.Set = field.SetValue;

				jsonTypeInfo.Properties.Add(jsonPropertyInfo);
			}
		}

		/// <summary>
		/// Including private fields attributes serialization.
		/// </summary>
		///
		[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
		public class JsonIncludePrivateFieldsAttribute : Attribute
		{
		}
	}
}
