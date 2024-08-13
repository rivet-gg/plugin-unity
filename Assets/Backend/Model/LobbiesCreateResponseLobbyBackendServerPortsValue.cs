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
    /// CreateResponseLobbyBackendServerPortsValue
    /// </summary>
    [DataContract(Name = "lobbies__create__Response_lobby_backend_server_ports_value")]
    public partial class CreateResponseLobbyBackendServerPortsValue
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
            Udp = 3,

            /// <summary>
            /// Enum Https for value: https
            /// </summary>
            [EnumMember(Value = "https")]
            Https = 4,

            /// <summary>
            /// Enum TcpTls for value: tcp_tls
            /// </summary>
            [EnumMember(Value = "tcp_tls")]
            TcpTls = 5
        }


        /// <summary>
        /// Gets or Sets Protocol
        /// </summary>
        [DataMember(Name = "protocol", IsRequired = true, EmitDefaultValue = true)]
        public ProtocolEnum Protocol { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateResponseLobbyBackendServerPortsValue" /> class.
        /// </summary>
        [JsonConstructorAttribute]
        protected CreateResponseLobbyBackendServerPortsValue() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateResponseLobbyBackendServerPortsValue" /> class.
        /// </summary>
        /// <param name="protocol">protocol (required).</param>
        /// <param name="serverPort">serverPort.</param>
        /// <param name="publicHostname">publicHostname.</param>
        /// <param name="publicPort">publicPort.</param>
        /// <param name="routing">routing (required).</param>
        public CreateResponseLobbyBackendServerPortsValue(ProtocolEnum protocol = default(ProtocolEnum), decimal serverPort = default(decimal), string publicHostname = default(string), decimal publicPort = default(decimal), CreateResponseLobbyBackendServerPortsValueRouting routing = default(CreateResponseLobbyBackendServerPortsValueRouting))
        {
            this.Protocol = protocol;
            // to ensure "routing" is required (not null)
            if (routing == null)
            {
                throw new ArgumentNullException("routing is a required property for CreateResponseLobbyBackendServerPortsValue and cannot be null");
            }
            this.Routing = routing;
            this.ServerPort = serverPort;
            this.PublicHostname = publicHostname;
            this.PublicPort = publicPort;
        }

        /// <summary>
        /// Gets or Sets ServerPort
        /// </summary>
        [DataMember(Name = "serverPort", EmitDefaultValue = false)]
        public decimal ServerPort { get; set; }

        /// <summary>
        /// Gets or Sets PublicHostname
        /// </summary>
        [DataMember(Name = "publicHostname", EmitDefaultValue = false)]
        public string PublicHostname { get; set; }

        /// <summary>
        /// Gets or Sets PublicPort
        /// </summary>
        [DataMember(Name = "publicPort", EmitDefaultValue = false)]
        public decimal PublicPort { get; set; }

        /// <summary>
        /// Gets or Sets Routing
        /// </summary>
        [DataMember(Name = "routing", IsRequired = true, EmitDefaultValue = true)]
        public CreateResponseLobbyBackendServerPortsValueRouting Routing { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("class CreateResponseLobbyBackendServerPortsValue {\n");
            sb.Append("  Protocol: ").Append(Protocol).Append("\n");
            sb.Append("  ServerPort: ").Append(ServerPort).Append("\n");
            sb.Append("  PublicHostname: ").Append(PublicHostname).Append("\n");
            sb.Append("  PublicPort: ").Append(PublicPort).Append("\n");
            sb.Append("  Routing: ").Append(Routing).Append("\n");
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
