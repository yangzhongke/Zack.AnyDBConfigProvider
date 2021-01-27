using Microsoft.Extensions.Configuration;
using System;
using System.Diagnostics;
using System.Text.Json;

namespace Zack.AnyDBConfigProvider
{
    public class DBConfigurationProvider : ConfigurationProvider
    {
        private DBConfigOptions options;
        public DBConfigurationProvider(DBConfigOptions options)
        {
            this.options = options;     
        }

        public override void Load()
        {
            base.Load();
            string tableName = options.TableName;
            using (var conn = options.CreateDbConnection())
            {
                conn.Open();
                DoLoad(tableName, conn);
            }
        }

        private void DoLoad(string tableName, System.Data.IDbConnection conn)
        {         
            //
            using (var cmd = conn.CreateCommand())
            {                
                cmd.CommandText = $"select Name,Value from {tableName} where Id in(select Max(Id) from {tableName} group by Name)";
                using (var reader = cmd.ExecuteReader())
                {                    
                    while (reader.Read())
                    {
                        string name = reader.GetString(0);
                        string value = reader.GetString(1);
                        if(value==null)
                        {
                            this.Data[name] = value;
                            continue;
                        }
                        value = value.Trim();
                        //if the value is like [...] or {} , it may be a json array value or json object value,
                        //so try to parse it as json
                        if(value.StartsWith("[") && value.EndsWith("]")
                            || value.StartsWith("{") && value.EndsWith("}"))
                        {
                            TryLoadAsJson(name, value);
                        }
                        else
                        {
                            this.Data[name] = value;
                        }
                    }
                }
            }
        }

        private void LoadJsonElement(string name, JsonElement jsonRoot)
        {
            if (jsonRoot.ValueKind == JsonValueKind.Array)
            {
                int index = 0;
                foreach (var item in jsonRoot.EnumerateArray())
                {
                    //https://andrewlock.net/creating-a-custom-iconfigurationprovider-in-asp-net-core-to-parse-yaml/
                    //parse as "a:b:0"="hello";"a:b:1"="world"
                    string path = name + ConfigurationPath.KeyDelimiter + index;
                    this.Data[path] = item.GetValueForConfig();
                    index++;
                }
            }
            else if (jsonRoot.ValueKind == JsonValueKind.Object)
            {
                foreach (var jsonObj in jsonRoot.EnumerateObject())
                {
                    string pathOfObj = name + ConfigurationPath.KeyDelimiter + jsonObj.Name;
                    LoadJsonElement(pathOfObj, jsonObj.Value);
                }
            }
            else
            {
                //if it is not json array or object, parse it as plain string value
                this.Data[name] = jsonRoot.GetValueForConfig();
            }
        }

        private void TryLoadAsJson(string name, string value)
        {
            var jsonOptions = new JsonDocumentOptions { AllowTrailingCommas = true, CommentHandling = JsonCommentHandling.Skip };
            try
            {
                var jsonRoot = JsonDocument.Parse(value, jsonOptions).RootElement;
                LoadJsonElement(name,jsonRoot);
            }
            catch (JsonException ex)
            {
                //if it is not valid json, parse it as plain string value
                this.Data[name] = value;
                Debug.WriteLine($"When trying to parse {value} as json object, exception was thrown. {ex}");
            }
        }
    }
}
