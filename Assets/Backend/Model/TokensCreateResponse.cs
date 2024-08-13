// This file is auto-generated by the Open Game Backend (https://opengb.dev) build system.
// 
// Do not edit this file directly.
//
// Generated at 2024-07-29T07:22:49.261Z


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

		namespace Backend.Model.Tokens
		{
			/// <summary>
			/// CreateResponse
			/// </summary>
			[DataContract(Name = "tokens__create__Response")]
			public partial class CreateResponse
			{
				/// <summary>
				/// Initializes a new instance of the <see cref="CreateResponse" /> class.
				/// </summary>
				[JsonConstructorAttribute]
				public CreateResponse() { }

				/// <summary>
				/// Returns the string presentation of the object
				/// </summary>
				/// <returns>String presentation of the object</returns>
				public override string ToString()
				{
					StringBuilder sb = new StringBuilder();
					sb.Append("class CreateResponse {\n");
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
	

