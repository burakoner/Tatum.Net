﻿using Newtonsoft.Json;

namespace Tatum.Net.RestObjects
{
    public class OffchainAddressPrivateKeyPair
    {
        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("privateKey")]
        public string PrivateKey { get; set; }
    }
}
