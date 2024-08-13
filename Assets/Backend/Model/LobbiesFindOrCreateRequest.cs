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
    /// FindOrCreateRequest
    /// </summary>
    [DataContract(Name = "lobbies__find_or_create__Request")]
    public partial class FindOrCreateRequest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FindOrCreateRequest" /> class.
        /// </summary>
        [JsonConstructorAttribute]
        protected FindOrCreateRequest() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="FindOrCreateRequest" /> class.
        /// </summary>
        /// <param name="varVersion">varVersion (required).</param>
        /// <param name="regions">regions.</param>
        /// <param name="tags">tags.</param>
        /// <param name="players">players (required).</param>
        /// <param name="noWait">noWait.</param>
        /// <param name="createConfig">createConfig (required).</param>
        public FindOrCreateRequest(string varVersion = default(string), List<string> regions = default(List<string>), Dictionary<string, string> tags = default(Dictionary<string, string>), List<Object> players = default(List<Object>), bool noWait = default(bool), FindOrCreateRequestCreateConfig createConfig = default(FindOrCreateRequestCreateConfig))
        {
            // to ensure "varVersion" is required (not null)
            if (varVersion == null)
            {
                throw new ArgumentNullException("varVersion is a required property for FindOrCreateRequest and cannot be null");
            }
            this.VarVersion = varVersion;
            // to ensure "players" is required (not null)
            if (players == null)
            {
                throw new ArgumentNullException("players is a required property for FindOrCreateRequest and cannot be null");
            }
            this.Players = players;
            // to ensure "createConfig" is required (not null)
            if (createConfig == null)
            {
                throw new ArgumentNullException("createConfig is a required property for FindOrCreateRequest and cannot be null");
            }
            this.CreateConfig = createConfig;
            this.Regions = regions;
            this.Tags = tags;
            this.NoWait = noWait;
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
        /// Gets or Sets Players
        /// </summary>
        [DataMember(Name = "players", IsRequired = true, EmitDefaultValue = true)]
        public List<Object> Players { get; set; }

        /// <summary>
        /// Gets or Sets NoWait
        /// </summary>
        [DataMember(Name = "noWait", EmitDefaultValue = true)]
        public bool NoWait { get; set; }

        /// <summary>
        /// Gets or Sets CreateConfig
        /// </summary>
        [DataMember(Name = "createConfig", IsRequired = true, EmitDefaultValue = true)]
        public FindOrCreateRequestCreateConfig CreateConfig { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("class FindOrCreateRequest {\n");
            sb.Append("  VarVersion: ").Append(VarVersion).Append("\n");
            sb.Append("  Regions: ").Append(Regions).Append("\n");
            sb.Append("  Tags: ").Append(Tags).Append("\n");
            sb.Append("  Players: ").Append(Players).Append("\n");
            sb.Append("  NoWait: ").Append(NoWait).Append("\n");
            sb.Append("  CreateConfig: ").Append(CreateConfig).Append("\n");
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