using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace Bulky.DataAccess.Repository.iRepository;

public interface iRepository<T> where T : class
{
   IEnumerable<T> GetAll(Expression<Func<T, bool>> filter=null,string? includeProperties=null);//Get All Records and return IEnumerable
   T Get(Expression<Func<T, bool>> filter,string? includeProperties=null);//Get Record by filter and return T
   void Add(T entity);//Add Record
   void Remove(T entity);//Remove Record 
   void RemoveRange(IEnumerable<T> entity);//Remove Range of Records
   


}