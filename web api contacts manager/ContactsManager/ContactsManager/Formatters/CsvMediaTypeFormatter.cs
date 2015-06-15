using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ContactsManager.Formatters
{
    public class CsvMediaTypeFormatter : MediaTypeFormatter
    {
        public CsvMediaTypeFormatter()
        {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/csv"));
        }

        public CsvMediaTypeFormatter(MediaTypeMapping mediaTypeMapping)
            : this()
        {
            MediaTypeMappings.Add(mediaTypeMapping);
        }

        public CsvMediaTypeFormatter(IEnumerable<MediaTypeMapping> mediaTypeMappings)
            : this()
        {
            foreach (var mediaTypeMapping in mediaTypeMappings)
                MediaTypeMappings.Add(mediaTypeMapping);
        }

        public override bool CanReadType(Type type)
        {
            return false;
        }

        public override bool CanWriteType(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            return IsTypeOfIEnumerable(type);
        }

        public override async Task WriteToStreamAsync(Type type, object value, Stream stream, HttpContent content,
            TransportContext transportContext)
        {
            var itemType = type.GetElementType() ?? type.GetGenericArguments()[0];

            using (var stringWriter = new StringWriter())
            {
                stringWriter.WriteLine(
                    string.Join<string>(
                        ",", itemType.GetProperties().Select(x => x.Name)
                        )
                    );

                foreach (var obj in (IEnumerable<object>) value)
                {
                    var vals = obj.GetType().GetProperties().Select(
                        pi => new
                        {
                            Value = pi.GetValue(obj, null)
                        }
                        );

                    var valueLine = string.Empty;

                    string _val;
                    foreach (var val in vals)
                    {
                        if (val.Value != null)
                        {
                            _val = val.Value.ToString();
                            //Check if the value contans a comma and place it in quotes if so
                            if (_val.Contains(","))
                                _val = string.Concat("\"", _val, "\"");

                            //Replace any \r or \n special characters from a new line with a space
                            if (_val.Contains("\r"))
                                _val = _val.Replace("\r", " ");
                            if (_val.Contains("\n"))
                                _val = _val.Replace("\n", " ");

                            valueLine = string.Concat(valueLine, _val, ",");

                        }
                        else
                        {
                            valueLine = string.Concat(valueLine, ",");
                        }
                    }

                    stringWriter.WriteLine(valueLine.TrimEnd(','));
                }

                var writer = new StreamWriter(stream);
                await writer.WriteAsync(stringWriter.ToString());
                await writer.FlushAsync();
            }
        }

        private static bool IsTypeOfIEnumerable(Type type)
        {
            return type.GetInterfaces().Any(interfaceType => interfaceType == typeof (IEnumerable));
        }
    }
}