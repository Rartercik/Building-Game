using System;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

namespace Data
{
    public class DataSaver
    {
        private readonly string _root = Application.persistentDataPath + Path.AltDirectorySeparatorChar;

        public void Save<T>(T data, string relativePath)
        {
            var path = _root + relativePath;
            var json = JsonConvert.SerializeObject(data);

            File.WriteAllText(path, json);
        }

        public T Load<T>(string relativePath)
        {
            var path = _root + relativePath;
            var json = File.ReadAllText(path);

            return JsonConvert.DeserializeObject<T>(json);
        }

        public bool CheckFileExists(string relativePath)
        {
            return File.Exists(_root + relativePath);
        }
    }
}