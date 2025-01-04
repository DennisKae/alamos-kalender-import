using System;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DennisKae.alamos_kalender_import.Core
{
    /// <summary>
    /// <see cref="DateTime"/> System.Text.Json Converter im ISO Format
    /// Quelle: https://stackoverflow.com/a/58103218
    /// </summary>
    public class DateTimeConverter : JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            Debug.Assert(typeToConvert == typeof(DateTime));
            return DateTime.Parse(reader.GetString());
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ssZ"));
        }
        
        // TODO: Gilt das auch für nullable DateTimes?
    }
}