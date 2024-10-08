/*
 * Open Game Backend
 *
 * No description provided (generated by Openapi Generator https://github.com/openapitools/openapi-generator)
 *
 * The version of the OpenAPI document: 1.0.0
 * Generated by: https://github.com/openapitools/openapi-generator.git
 */


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

namespace Backend.Model.Users
{
    /// <summary>
    /// FetchRequest
    /// </summary>
    [DataContract(Name = "users__fetch__Request")]
    public partial class FetchRequest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FetchRequest" /> class.
        /// </summary>
        [JsonConstructorAttribute]
        protected FetchRequest() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="FetchRequest" /> class.
        /// </summary>
        /// <param name="userIds">userIds (required).</param>
        public FetchRequest(List<string> userIds = default(List<string>))
        {
            // to ensure "userIds" is required (not null)
            if (userIds == null)
            {
                throw new ArgumentNullException("userIds is a required property for FetchRequest and cannot be null");
            }
            this.UserIds = userIds;
        }

        /// <summary>
        /// Gets or Sets UserIds
        /// </summary>
        [DataMember(Name = "userIds", IsRequired = true, EmitDefaultValue = true)]
        public List<string> UserIds { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("class FetchRequest {\n");
            sb.Append("  UserIds: ").Append(UserIds).Append("\n");
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
