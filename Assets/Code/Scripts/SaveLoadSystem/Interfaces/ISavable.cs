using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.SaveLoad{

    /// <summary>
    /// Interface for Savable objects
    /// </summary>
    public interface ISavable{
        void Save();
        void Load();
    }
}
