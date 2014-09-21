using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Windows.Storage;
using Newtonsoft.Json;

namespace WeightWatcher
{
    public class Storage
    {
        private static ApplicationDataContainer cloudStorage = ApplicationData.Current.RoamingSettings;

        public static ObservableCollection<DailyWeight> GetValuesFromCloud()
        {
            var result = new ObservableCollection<DailyWeight>();

            foreach (var item in cloudStorage.Values)
            {
                result.Add(JsonConvert.DeserializeObject<DailyWeight>(item.Value.ToString()));
            }

            return result;
        }

        public static DailyWeight AddValues(string value)
        {
            var addition = GetkeyValuepair(value);
            cloudStorage.Values.Add(addition);

            return JsonConvert.DeserializeObject<DailyWeight>(addition.Value.ToString());
        }

        private static KeyValuePair<string, object> GetkeyValuepair(string value)
        {
            return new KeyValuePair<string, object>(DateTime.Now.ToString(), JsonConvert.SerializeObject( new DailyWeight() { Date = DateTime.Now, Weight = double.Parse(value) }) );
        }
    }
}
