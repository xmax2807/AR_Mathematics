using System.Collections;

namespace Project.UI.Panel{
    public interface IPreloadablePanel{
        IEnumerator PrepareAsync();
        IEnumerator UnloadAsync();
    }
}