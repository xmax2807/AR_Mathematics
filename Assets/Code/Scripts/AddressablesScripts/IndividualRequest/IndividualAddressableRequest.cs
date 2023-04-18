using UnityEngine;
using System.Threading.Tasks;

namespace Project.Addressable{
    public abstract class IndividualAddressableRequest : MonoBehaviour{
        public abstract Task<Object[]> GetResult();
    }
    public abstract class IndividualAddressableRequest<T> 
    : IndividualAddressableRequest 
    where T : Object{
        public override async Task<Object[]> GetResult() => await Request();
        public async Task<T[]> GetResultT() => await Request();
        protected abstract Task<T[]> Request();
    }
}