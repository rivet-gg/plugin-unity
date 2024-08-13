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
    /// CreateRequest
    /// </summary>
    [DataContract(Name = "lobbies__create__Request")]
    public partial class CreateRequest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateRequest" /> class.
        /// </summary>
        [JsonConstructorAttribute]
        protected CreateRequest() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateRequest" /> class.
        /// </summary>
        /// <param name="varVersion">varVersion (required).</param>
        /// <param name="region">region (required).</param>
        /// <param name="tags">tags.</param>
        /// <param name="maxPlayers">maxPlayers (required).</param>
        /// <param name="maxPlayersDirect">maxPlayersDirect (required).</param>
        /// <param name="players">players (required).</param>
        /// <param name="noWait">noWait.</param>
        public CreateRequest(string varVersion = default(string), string region = default(string), Dictionary<string, string> tags = default(Dictionary<string, string>), decimal maxPlayers = default(decimal), decimal maxPlayersDirect = default(decimal), List<Object> players = default(List<Object>), bool noWait = default(bool))
        {
            // to ensure "varVersion" is required (not null)
            if (varVersion == null)
            {
                throw new ArgumentNullException("varVersion is a required property for CreateRequest and cannot be null");
            }
            this.VarVersion = varVersion;
            // to ensure "region" is required (not null)
            if (region == null)
            {
                throw new ArgumentNullException("region is a required property for CreateRequest and cannot be null");
            }
            this.Region = region;
            this.MaxPlayers = maxPlayers;
            this.MaxPlayersDirect = maxPlayersDirect;
            // to ensure "players" is required (not null)
            if (players == null)
            {
                throw new ArgumentNullException("players is a required property for CreateRequest and cannot be null");
            }
            this.Players = players;
            this.Tags = tags;
            this.NoWait = noWait;
        }

        /// <summary>
        /// Gets or Sets VarVersion
        /// </summary>
        [DataMember(Name = "version", IsRequired = true, EmitDefaultValue = true)]
        public string VarVersion { get; set; }

        /// <summary>
        /// Gets or Sets Region
        /// </summary>
        [DataMember(Name = "region", IsRequired = true, EmitDefaultValue = true)]
        public string Region { get; set; }

        /// <summary>
        /// Gets or Sets Tags
        /// </summary>
        [DataMember(Name = "tags", EmitDefaultValue = false)]
        public Dictionary<string, string> Tags { get; set; }

        /// <summary>
        /// Gets or Sets MaxPlayers
        /// </summary>
        [DataMember(Name = "maxPlayers", IsRequired = true, EmitDefaultValue = true)]
        public decimal MaxPlayers { get; set; }

        /// <summary>
        /// Gets or Sets MaxPlayersDirect
        /// </summary>
        [DataMember(Name = "maxPlayersDirect", IsRequired = true, EmitDefaultValue = true)]
        public decimal MaxPlayersDirect { get; set; }

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
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("class CreateRequest {\n");
            sb.Append("  VarVersion: ").Append(VarVersion).Append("\n");
            sb.Append("  Region: ").Append(Region).Append("\n");
            sb.Append("  Tags: ").Append(Tags).Append("\n");
            sb.Append("  MaxPlayers: ").Append(MaxPlayers).Append("\n");
            sb.Append("  MaxPlayersDirect: ").Append(MaxPlayersDirect).Append("\n");
            sb.Append("  Players: ").Append(Players).Append("\n");
            sb.Append("  NoWait: ").Append(NoWait).Append("\n");
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
