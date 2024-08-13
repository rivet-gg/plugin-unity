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

namespace Backend.Model.Lobbies
{
    /// <summary>
    /// ListResponse
    /// </summary>
    [DataContract(Name = "lobbies__list__Response")]
    public partial class ListResponse
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ListResponse" /> class.
        /// </summary>
        [JsonConstructorAttribute]
        protected ListResponse() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="ListResponse" /> class.
        /// </summary>
        /// <param name="lobbies">lobbies (required).</param>
        public ListResponse(List<ListResponseLobbiesInner> lobbies = default(List<ListResponseLobbiesInner>))
        {
            // to ensure "lobbies" is required (not null)
            if (lobbies == null)
            {
                throw new ArgumentNullException("lobbies is a required property for ListResponse and cannot be null");
            }
            this.Lobbies = lobbies;
        }

        /// <summary>
        /// Gets or Sets Lobbies
        /// </summary>
        [DataMember(Name = "lobbies", IsRequired = true, EmitDefaultValue = true)]
        public List<ListResponseLobbiesInner> Lobbies { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("class ListResponse {\n");
            sb.Append("  Lobbies: ").Append(Lobbies).Append("\n");
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
