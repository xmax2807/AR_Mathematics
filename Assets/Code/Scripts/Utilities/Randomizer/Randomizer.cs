using System;
using System.Collections.Generic;

namespace Project.Utils{
    public class Randomizer<T>{
        private static readonly Random rand = new();
        private IList<T> list;
        public Randomizer(IEnumerable<T> originalList){
            if(originalList == null){
                throw new ArgumentNullException("List was null or empty in Randomizer");
            }
            list = new List<T>(originalList);
        }
        public T Next(){
            int index = rand.Next(list.Count);            
            return list[index];
        }
        public void Add(T newChild){
            if(newChild == null) return;
            list.Add(newChild);
        }
    }
}