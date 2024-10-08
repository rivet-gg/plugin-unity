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
    /// CreateResponseLobbyBackendLocalDevelopmentPortsValue
    /// </summary>
    [DataContract(Name = "lobbies__create__Response_lobby_backend_localDevelopment_ports_value")]
    public partial class CreateResponseLobbyBackendLocalDevelopmentPortsValue
    {
        /// <summary>
        /// Defines Protocol
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public enum ProtocolEnum
        {
            /// <summary>
            /// Enum Http for value: http
            /// </summary>
            [EnumMember(Value = "http")]
            Http = 1,

            /// <summary>
            /// Enum Tcp for value: tcp
            /// </summary>
            [EnumMember(Value = "tcp")]
            Tcp = 2,

            /// <summary>
            /// Enum Udp for value: udp
            /// </summary>
            [EnumMember(Value = "udp")]
            Udp = 3
        }


        /// <summary>
        /// Gets or Sets Protocol
        /// </summary>
        [DataMember(Name = "protocol", IsRequired = true, EmitDefaultValue = true)]
        public ProtocolEnum Protocol { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateResponseLobbyBackendLocalDevelopmentPortsValue" /> class.
        /// </summary>
        [JsonConstructorAttribute]
        protected CreateResponseLobbyBackendLocalDevelopmentPortsValue() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateResponseLobbyBackendLocalDevelopmentPortsValue" /> class.
        /// </summary>
        /// <param name="protocol">protocol (required).</param>
        /// <param name="hostname">hostname (required).</param>
        /// <param name="port">port (required).</param>
        public CreateResponseLobbyBackendLocalDevelopmentPortsValue(ProtocolEnum protocol = default(ProtocolEnum), string hostname = default(string), decimal port = default(decimal))
        {
            this.Protocol = protocol;
            // to ensure "hostname" is required (not null)
            if (hostname == null)
            {
                throw new ArgumentNullException("hostname is a required property for CreateResponseLobbyBackendLocalDevelopmentPortsValue and cannot be null");
            }
            this.Hostname = hostname;
            this.Port = port;
        }

        /// <summary>
        /// Gets or Sets Hostname
        /// </summary>
        [DataMember(Name = "hostname", IsRequired = true, EmitDefaultValue = true)]
        public string Hostname { get; set; }

        /// <summary>
        /// Gets or Sets Port
        /// </summary>
        [DataMember(Name = "port", IsRequired = true, EmitDefaultValue = true)]
        public decimal Port { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("class CreateResponseLobbyBackendLocalDevelopmentPortsValue {\n");
            sb.Append("  Protocol: ").Append(Protocol).Append("\n");
            sb.Append("  Hostname: ").Append(Hostname).Append("\n");
            sb.Append("  Port: ").Append(Port).Append("\n");
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
