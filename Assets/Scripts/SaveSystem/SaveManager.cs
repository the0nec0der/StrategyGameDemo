using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Newtonsoft.Json;

using UnityEngine;

namespace SaveSystem
{
    public class SaveManager : MonoBehaviour
    {
        public event OnSavedEventHandler OnSaved = null;
        public event OnLoadedEventHandler OnLoaded = null;

        public int CurrentSlot { get; private set; } = 0;

        public void SaveGame(int? slot = null)
        {
            int slotToSave = slot ?? CurrentSlot;

            IEnumerable<ISaveable> saveables = FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None).OfType<ISaveable>();

            List<StorableMemento> saveableMementos = new();
            foreach (ISaveable saveable in saveables)
                saveableMementos.Add(new(saveable));

            string data = JsonConvert.SerializeObject(saveableMementos);

            string saveFilePath = GetSaveSlotPath(slotToSave);

            Directory.CreateDirectory(Path.GetDirectoryName(saveFilePath));
            File.WriteAllText(saveFilePath, data);

            OnSaved?.Invoke(this, slotToSave);
        }

        public void LoadGame(int? slot = null)
        {
            int slotToLoad = slot ?? CurrentSlot;

            List<ISaveable> saveables = new(FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None).OfType<ISaveable>());

            string path = GetSaveSlotPath(slotToLoad);

            if (!File.Exists(path))
                throw new SaveNotFoundException(slotToLoad);

            string data = File.ReadAllText(path);
            List<StorableMemento> storableMementos = JsonConvert.DeserializeObject<List<StorableMemento>>(data);

            foreach (StorableMemento storableMemento in storableMementos)
            {
                IMemento memento = storableMemento.GetMemento();
                bool isSaveableFound = false;

                foreach (ISaveable saveable in saveables)
                {
                    try
                    {
                        saveable.RestoreSnapshot(memento);
                        saveables.Remove(saveable);
                        isSaveableFound = true;
                        break;
                    }
                    catch { }
                }

                if (!isSaveableFound)
                    Debug.LogError($"No eligible {nameof(ISaveable)} found for {storableMemento.TypeName}");
            }

            CurrentSlot = slotToLoad;
            OnLoaded?.Invoke(this, slotToLoad);
        }

        private static string GetSaveSlotPath(int slot)
            => Path.Combine(Application.persistentDataPath, "Saves", $"Save-{slot}.json");

        public delegate void OnSavedEventHandler(SaveManager sender, int slot);
        public delegate void OnLoadedEventHandler(SaveManager sender, int slot);

        private readonly struct StorableMemento
        {
            public readonly string TypeName;
            public readonly string Data;

            [JsonConstructor]
            public StorableMemento(string typeName, string data)
            {
                TypeName = typeName;
                Data = data;
            }

            public StorableMemento(ISaveable saveable)
            {
                IMemento memento = saveable.GetSnapshot();

                TypeName = memento.GetType().FullName;
                Data = JsonConvert.SerializeObject(memento);
            }

            public IMemento GetMemento()
            {
                string typeName = TypeName;
                foreach (System.Reflection.Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
                {
                    Type[] types = assembly.GetTypes();
                    IEnumerable<Type> typesFound = types.Where(t =>
                    {
                        if (!t.IsValueType)
                            return false;

                        return t.FullName.CompareTo(typeName) == 0;
                    });
                    if (typesFound.FirstOrDefault() is not Type mementoType)
                        continue;

                    return (IMemento)JsonConvert.DeserializeObject(Data, mementoType);
                }
                throw new ArgumentException($"{TypeName} couldn't be found", nameof(TypeName));
            }
        }

        [Serializable]
        public class SaveNotFoundException : Exception
        {
            public SaveNotFoundException() { }
            public SaveNotFoundException(int slot) : base($"Can't find the save game with slot: {slot}.") { }
        }
    }
}
