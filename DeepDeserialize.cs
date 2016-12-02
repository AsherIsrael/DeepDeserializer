using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.WebUtilities;

namespace DeepDeserialize
{
    public class DeepDeserializer
    {

        public static Dictionary<string, object> DeepDeserialize(HttpResponseMessage Response)
        {
            // Read in the response as a string.
            string StringResponse = Response.Content.ReadAsString();
            // Then parse the result into JSON and convert to a dictionary that we can use.
            Dictionary<string, object> JsonResponse = JsonConvert.DeserializeObject<Dictionary<string, object>>(StringResponse);
            // Dig down and continue parsing until the entire object is C# objects
            Dictionary<string, object> ParsedResponse = ParseJObject(JsonResponse);
            return ParsedResponse;
        }

        private static Dictionary<string, object> ParseJObject(Dictionary<string, object> Response)
        {
            var NewResponse = new Dictionary<string, object>();
            foreach( var Entry in Response )
            {
                if(Entry.Value == null)
                {
                    NewResponse.Add($"{Entry.Key}", Entry.Value);
                }
                else
                {
                    object check = Entry.Value;
                    string thing = check.GetType().ToString();
                    switch (check.GetType().ToString())
                    {
                        case "System.String":
                            NewResponse.Add($"{Entry.Key}", Entry.Value);
                            break;
                        case "System.Int64":
                            NewResponse.Add($"{Entry.Key}", Entry.Value);
                            break;
                        case "System.Int32":
                            NewResponse.Add($"{Entry.Key}", Entry.Value);
                            break;
                        case "System.Boolean":
                            NewResponse.Add($"{Entry.Key}", Entry.Value);
                            break;
                        case "System.Float32":
                            NewResponse.Add($"{Entry.Key}", Entry.Value);
                            break;
                        case "System.Float64":
                            NewResponse.Add($"{Entry.Key}", Entry.Value);
                            break;
                        case "Newtonsoft.Json.Linq.JArray":
                            var SubArray = ParseJArray(((JArray)(Entry.Value)).ToObject<List<object>>());
                            NewResponse.Add($"{Entry.Key}", SubArray);
                            break;
                        case "Newtonsoft.Json.Linq.JObject":
                            var JsonObject = ((JObject)(Entry.Value)).ToObject<Dictionary<string, object>>();
                            Dictionary<string, object> SubObject = ParseJObject(JsonObject);
                            NewResponse.Add($"{Entry.Key}", SubObject);
                            break;
                        default:
                            Console.WriteLine("Unexpected DataType");
                            break;
                    }
                }
            }
            return NewResponse;
        }

        private static List<object> ParseJArray(List<object> ParseMe)
        {
            var NewList = new List<object>();
            foreach( var Entry in ParseMe )
            {
                switch (Entry.GetType().ToString())
                {
                    case "System.String":
                        NewList.Add(Entry);
                        break;
                    case "System.Int64":
                        NewList.Add(Entry);
                        break;
                    case "System.Int32":
                        NewList.Add(Entry);
                        break;
                    case "System.Boolean":
                        NewList.Add(Entry);
                        break;
                    case "System.Float32":
                        NewList.Add(Entry);
                        break;
                    case "System.Float64":
                        NewList.Add(Entry);
                        break;
                    case "Newtonsoft.Json.Linq.JArray":
                        var SubArray = ParseJArray(((JArray)Entry).ToObject<List<object>>());
                        NewList.Add(SubArray);
                        break;
                    case "Newtonsoft.Json.Linq.JObject":
                        var JsonObject = ((JObject)Entry).ToObject<Dictionary<string, object>>();
                        Dictionary<string, object> SubObject = ParseJObject(JsonObject);
                        NewList.Add(SubObject);
                        break;
                    default:
                        Console.WriteLine("Unexpected DataType");
                        break;
                }
            }
            return NewList;
        }
    }
}