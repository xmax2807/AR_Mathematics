using UnityEngine;

namespace Project.AssetIO
{
    public class AssetDatabaseExtension
    {
        private static AssetDatabaseExtension _instance;
        public static AssetDatabaseExtension Instance
        {
            get
            {
                _instance ??= new AssetDatabaseExtension();
                return _instance;
            }
        }
    }
}
