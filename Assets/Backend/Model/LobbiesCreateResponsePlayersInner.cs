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
    /// CreateResponsePlayersInner
    /// </summary>
    [DataContract(Name = "lobbies__create__Response_players_inner")]
    public partial class CreateResponsePlayersInner
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateResponsePlayersInner" /> class.
        /// </summary>
        [JsonConstructorAttribute]
        protected CreateResponsePlayersInner() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateResponsePlayersInner" /> class.
        /// </summary>
        /// <param name="token">token (required).</param>
        /// <param name="id">id (required).</param>
        public CreateResponsePlayersInner(string token = default(string), string id = default(string))
        {
            // to ensure "token" is required (not null)
            if (token == null)
            {
                throw new ArgumentNullException("token is a required property for CreateResponsePlayersInner and cannot be null");
            }
            this.Token = token;
            // to ensure "id" is required (not null)
            if (id == null)
            {
                throw new ArgumentNullException("id is a required property for CreateResponsePlayersInner and cannot be null");
            }
            this.Id = id;
        }

        /// <summary>
        /// Gets or Sets Token
        /// </summary>
        [DataMember(Name = "token", IsRequired = true, EmitDefaultValue = true)]
        public string Token { get; set; }

        /// <summary>
        /// Gets or Sets Id
        /// </summary>
        [DataMember(Name = "id", IsRequired = true, EmitDefaultValue = true)]
        public string Id { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("class CreateResponsePlayersInner {\n");
            sb.Append("  Token: ").Append(Token).Append("\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
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
