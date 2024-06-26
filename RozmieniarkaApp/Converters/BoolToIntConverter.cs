using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace RozmieniarkaApp.Converters
{
    public class BoolToIntConverter : JsonConverter<int>
    {
        public override int Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.True)
            {
                return 1;
            }
            else if (reader.TokenType == JsonTokenType.False)
            {
                return 0;
            }
            else
            {
                return -1;
            }
        }

        public override void Write(Utf8JsonWriter writer, int value, JsonSerializerOptions options)
        {
            if (value == 1)
            {
                writer.WriteBooleanValue(true);
            }
            else if (value == 0)
            {
                writer.WriteBooleanValue(false);
            }
            else
            {
                throw new JsonException("Unexpected value for boolean conversion");
            }
        }
    }
}
