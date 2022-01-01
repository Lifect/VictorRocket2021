using System.Collections.Generic;

namespace ShopUI.DataAccess
{
    public interface IDataAccess<T>
    {
        public List<T> GetAll();
        void SerializeItems(List<T> Products);
        public T GetById(int id);
    }
    
}
