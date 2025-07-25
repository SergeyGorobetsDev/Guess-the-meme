using Newtonsoft.Json;
using System;
using UnityEngine;

namespace Assets.Project.Code.Progress.ProgressData
{
    [Serializable]
    public class UserData
    {
        [field: SerializeField, JsonProperty]
        public int ID { get; set; }

        [field: SerializeField, JsonProperty]
        public string Version { get; set; }
    }
}
