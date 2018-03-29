using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace GoalList.Helpers
{
    public class Serializer
    {
        private static readonly JsonSerializerSettings settings = new JsonSerializerSettings()
        {
            TypeNameHandling = TypeNameHandling.All,
            Formatting = Formatting.Indented
        };

        public static string Serialize(object obj)
        {
            if (obj == null)
            {
                throw new NullReferenceException();
            }

            string jsonString = JsonConvert.SerializeObject(obj, settings);
            return jsonString;
        }

        public static T Deserialize<T>(string jsonString)
        {
            T obj = JsonConvert.DeserializeObject<T>(jsonString, settings);
            return obj;
        }

        private static StorageFolder localFolder = ApplicationData.Current.LocalFolder;
        public async static Task SaveUserAsync(string content)
        {
            await Task.Run(async () =>
            {
                StorageFile file = await localFolder.CreateFileAsync("User.json", CreationCollisionOption.ReplaceExisting);
                await FileIO.WriteTextAsync(file, content);
            });
            await Task.CompletedTask;
        }

        public async static Task<string> ReadUserAsync()
        {
            StorageFile file = await localFolder.GetFileAsync("User.json");
            string userAsJson = await FileIO.ReadTextAsync(file);
            return userAsJson;
        }

        public async static Task SaveSoundChoiceAsync(string content)
        {
            await Task.Run(async () =>
            {
                StorageFile file = await localFolder.CreateFileAsync("SoundChoice.json", CreationCollisionOption.ReplaceExisting);
                await FileIO.WriteTextAsync(file, content);
            });
            await Task.CompletedTask;
        }

        public async static Task<string> ReadSoundChoiceAsync()
        {
            StorageFile file = await localFolder.GetFileAsync("SoundChoice.json");
            string userAsJson = await FileIO.ReadTextAsync(file);
            return userAsJson;
        }
    }
}
