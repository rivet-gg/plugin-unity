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
    /// CreateResponseLobby
    /// </summary>
    [DataContract(Name = "lobbies__create__Response_lobby")]
    public partial class CreateResponseLobby
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateResponseLobby" /> class.
        /// </summary>
        [JsonConstructorAttribute]
        protected CreateResponseLobby() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateResponseLobby" /> class.
        /// </summary>
        /// <param name="id">id (required).</param>
        /// <param name="varVersion">varVersion (required).</param>
        /// <param name="tags">tags (required).</param>
        /// <param name="createdAt">createdAt (required).</param>
        /// <param name="readyAt">readyAt.</param>
        /// <param name="players">players (required).</param>
        /// <param name="maxPlayers">maxPlayers (required).</param>
        /// <param name="maxPlayersDirect">maxPlayersDirect (required).</param>
        /// <param name="backend">backend (required).</param>
        public CreateResponseLobby(string id = default(string), string varVersion = default(string), Dictionary<string, string> tags = default(Dictionary<string, string>), decimal createdAt = default(decimal), decimal readyAt = default(decimal), decimal players = default(decimal), decimal maxPlayers = default(decimal), decimal maxPlayersDirect = default(decimal), CreateResponseLobbyBackend backend = default(CreateResponseLobbyBackend))
        {
            // to ensure "id" is required (not null)
            if (id == null)
            {
                throw new ArgumentNullException("id is a required property for CreateResponseLobby and cannot be null");
            }
            this.Id = id;
            // to ensure "varVersion" is required (not null)
            if (varVersion == null)
            {
                throw new ArgumentNullException("varVersion is a required property for CreateResponseLobby and cannot be null");
            }
            this.VarVersion = varVersion;
            // to ensure "tags" is required (not null)
            if (tags == null)
            {
                throw new ArgumentNullException("tags is a required property for CreateResponseLobby and cannot be null");
            }
            this.Tags = tags;
            this.CreatedAt = createdAt;
            this.Players = players;
            this.MaxPlayers = maxPlayers;
            this.MaxPlayersDirect = maxPlayersDirect;
            // to ensure "backend" is required (not null)
            if (backend == null)
            {
                throw new ArgumentNullException("backend is a required property for CreateResponseLobby and cannot be null");
            }
            this.Backend = backend;
            this.ReadyAt = readyAt;
        }

        /// <summary>
        /// Gets or Sets Id
        /// </summary>
        [DataMember(Name = "id", IsRequired = true, EmitDefaultValue = true)]
        public string Id { get; set; }

        /// <summary>
        /// Gets or Sets VarVersion
        /// </summary>
        [DataMember(Name = "version", IsRequired = true, EmitDefaultValue = true)]
        public string VarVersion { get; set; }

        /// <summary>
        /// Gets or Sets Tags
        /// </summary>
        [DataMember(Name = "tags", IsRequired = true, EmitDefaultValue = true)]
        public Dictionary<string, string> Tags { get; set; }

        /// <summary>
        /// Gets or Sets CreatedAt
        /// </summary>
        [DataMember(Name = "createdAt", IsRequired = true, EmitDefaultValue = true)]
        public decimal CreatedAt { get; set; }

        /// <summary>
        /// Gets or Sets ReadyAt
        /// </summary>
        [DataMember(Name = "readyAt", EmitDefaultValue = false)]
        public decimal ReadyAt { get; set; }

        /// <summary>
        /// Gets or Sets Players
        /// </summary>
        [DataMember(Name = "players", IsRequired = true, EmitDefaultValue = true)]
        public decimal Players { get; set; }

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
        /// Gets or Sets Backend
        /// </summary>
        [DataMember(Name = "backend", IsRequired = true, EmitDefaultValue = true)]
        public CreateResponseLobbyBackend Backend { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("class CreateResponseLobby {\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  VarVersion: ").Append(VarVersion).Append("\n");
            sb.Append("  Tags: ").Append(Tags).Append("\n");
            sb.Append("  CreatedAt: ").Append(CreatedAt).Append("\n");
            sb.Append("  ReadyAt: ").Append(ReadyAt).Append("\n");
            sb.Append("  Players: ").Append(Players).Append("\n");
            sb.Append("  MaxPlayers: ").Append(MaxPlayers).Append("\n");
            sb.Append("  MaxPlayersDirect: ").Append(MaxPlayersDirect).Append("\n");
            sb.Append("  Backend: ").Append(Backend).Append("\n");
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
