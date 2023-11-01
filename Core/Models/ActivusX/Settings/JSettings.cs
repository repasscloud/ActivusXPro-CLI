using System.Text.Json.Serialization;

namespace ActivusXPro_CLI.Core.Models.ActivusX.Settings
{
	public class JSettings
	{
		public class DCConfig
		{
			[JsonPropertyName("DC")]
			public DC? DC { get; set; }
		}

		public class DC
		{
			[JsonPropertyName("DistinguishedName")]
			public string? DistinguishedName { get; set; }

			[JsonPropertyName("DefaultUsersCN")]
			public string? DefaultUsersCN { get; set; }
		}
	}
}

