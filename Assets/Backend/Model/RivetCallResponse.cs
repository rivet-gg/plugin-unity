// This file is auto-generated by the Open Game Backend (https://opengb.dev) build system.
// 
// Do not edit this file directly.
//
// Generated at 2024-07-24T03:59:07.557Z


		using System;
		using System.Collections;
		using System.Collections.Generic;
		using System.Collections.ObjectModel;
		using System.Linq;
		using System.IO;
		using System.Runtime.Serialization;
		using System.Text;
		using System.Text.RegularExpressions;
		using Newtonsoft.Json;
		using Newtonsoft.Json.Converters;
		using Newtonsoft.Json.Linq;
		using OpenAPIDateConverter = Backend.Client.OpenAPIDateConverter;

		namespace Backend.Model.Rivet
		{
			/// <summary>
			/// CallResponse
			/// </summary>
			[DataContract(Name = "rivet__call__Response")]
			public partial class CallResponse
			{
				/// <summary>
				/// Initializes a new instance of the <see cref="CallResponse" /> class.
				/// </summary>
				[JsonConstructorAttribute]
				public CallResponse() { }

				/// <summary>
				/// Returns the string presentation of the object
				/// </summary>
				/// <returns>String presentation of the object</returns>
				public override string ToString()
				{
					StringBuilder sb = new StringBuilder();
					sb.Append("class CallResponse {\n");
					sb.Append("}\n");
					return sb.ToString();
				}

				/// <summary>
				/// Returns the JSON string presentation of the object
				/// </summary>
				/// <returns>JSON string presentation of the object</returns>
				public virtual string ToJson()
				{
					return Newtonsoft.Json.JsonConvert.SerializeObject(this, Newtonsoft.Json.Formatting.Indented);
				}

			}
		}
	

