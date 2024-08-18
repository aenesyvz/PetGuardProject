using MimeKit;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.BackgroundServices.Helpers;

public class MailboxAddressConverter : JsonConverter<MailboxAddress>
{
    public override MailboxAddress? ReadJson(JsonReader reader, Type objectType, MailboxAddress? existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var jsonObject = JObject.Load(reader);
        var address = jsonObject["Address"]?.ToString();
        var name = jsonObject["Name"]?.ToString();
        return new MailboxAddress(name, address);
    }

    public override void WriteJson(JsonWriter writer, MailboxAddress value, JsonSerializer serializer)
    {
        var jsonObject = new JObject
        {
            { "Address", value.Address },
            { "Name", value.Name }
        };
        jsonObject.WriteTo(writer);
    }
}
