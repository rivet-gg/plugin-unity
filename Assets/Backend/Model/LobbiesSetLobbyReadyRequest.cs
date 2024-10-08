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
    /// SetLobbyReadyRequest
    /// </summary>
    [DataContract(Name = "lobbies__set_lobby_ready__Request")]
    public partial class SetLobbyReadyRequest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SetLobbyReadyRequest" /> class.
        /// </summary>
        /// <param name="lobbyId">lobbyId.</param>
        /// <param name="lobbyToken">lobbyToken.</param>
        public SetLobbyReadyRequest(string lobbyId = default(string), string lobbyToken = default(string))
        {
            this.LobbyId = lobbyId;
            this.LobbyToken = lobbyToken;
        }

        /// <summary>
        /// Gets or Sets LobbyId
        /// </summary>
        [DataMember(Name = "lobbyId", EmitDefaultValue = false)]
        public string LobbyId { get; set; }

        /// <summary>
        /// Gets or Sets LobbyToken
        /// </summary>
        [DataMember(Name = "lobbyToken", EmitDefaultValue = false)]
        public string LobbyToken { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("class SetLobbyReadyRequest {\n");
            sb.Append("  LobbyId: ").Append(LobbyId).Append("\n");
            sb.Append("  LobbyToken: ").Append(LobbyToken).Append("\n");
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
