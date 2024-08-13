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
    /// ListRequest
    /// </summary>
    [DataContract(Name = "lobbies__list__Request")]
    public partial class ListRequest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ListRequest" /> class.
        /// </summary>
        [JsonConstructorAttribute]
        protected ListRequest() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="ListRequest" /> class.
        /// </summary>
        /// <param name="varVersion">varVersion (required).</param>
        /// <param name="regions">regions.</param>
        /// <param name="tags">tags.</param>
        public ListRequest(string varVersion = default(string), List<string> regions = default(List<string>), Dictionary<string, string> tags = default(Dictionary<string, string>))
        {
            // to ensure "varVersion" is required (not null)
            if (varVersion == null)
            {
                throw new ArgumentNullException("varVersion is a required property for ListRequest and cannot be null");
            }
            this.VarVersion = varVersion;
            this.Regions = regions;
            this.Tags = tags;
        }

        /// <summary>
        /// Gets or Sets VarVersion
        /// </summary>
        [DataMember(Name = "version", IsRequired = true, EmitDefaultValue = true)]
        public string VarVersion { get; set; }

        /// <summary>
        /// Gets or Sets Regions
        /// </summary>
        [DataMember(Name = "regions", EmitDefaultValue = false)]
        public List<string> Regions { get; set; }

        /// <summary>
        /// Gets or Sets Tags
        /// </summary>
        [DataMember(Name = "tags", EmitDefaultValue = false)]
        public Dictionary<string, string> Tags { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("class ListRequest {\n");
            sb.Append("  VarVersion: ").Append(VarVersion).Append("\n");
            sb.Append("  Regions: ").Append(Regions).Append("\n");
            sb.Append("  Tags: ").Append(Tags).Append("\n");
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