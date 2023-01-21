using System.Collections.ObjectModel;

public abstract class PageViewModel<T>{
    public ObservableCollection<T> Items;
    
    private void Init(){
    }
}