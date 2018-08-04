using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MachinaTrader.Helpers
{
    public class MergeObjects
    {
        public static JObject CombineJObjects(JObject object1, JObject object2)
        {
            object1.Merge(object2, new JsonMergeSettings
            {
                MergeArrayHandling = MergeArrayHandling.Replace
            });

            return object1;
        }

        public static JObject CombineObjectWithJobject(object object1, JObject object2)
        {
            JObject sourceObject = JObject.FromObject(object1);
            JObject mergeIntoSourceObject = JObject.FromObject(object2);
            sourceObject.Merge(mergeIntoSourceObject, new JsonMergeSettings
            {
                MergeArrayHandling = MergeArrayHandling.Replace
            });

            return sourceObject;
        }

        public static JObject MergeCsDictionaryAndSave(object csDictionary, string filePath, JObject json = null)
        {
            //Create Config File if not exists
            if (!File.Exists(filePath))
            {
                string jsonString = JsonConvert.SerializeObject(csDictionary, Formatting.Indented);
                File.WriteAllText(filePath, jsonString);
                return JObject.FromObject(csDictionary);
            }

            if (json == null)
            {
                json = JObject.Parse(System.IO.File.ReadAllText(filePath));
            }

            JObject combinedDictJson = CombineObjectWithJobject(csDictionary, json);

            string jsonToFile = JsonConvert.SerializeObject(combinedDictJson, Formatting.Indented);
            //Console.WriteLine(jsonToFile); // single line JSON string
            File.WriteAllText(filePath, jsonToFile);
            return combinedDictJson;
        }
    }
}
