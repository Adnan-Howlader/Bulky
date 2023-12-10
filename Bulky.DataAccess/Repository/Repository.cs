
using System.Linq.Expressions;
using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.iRepository;
using Microsoft.EntityFrameworkCore;

namespace Bulky.DataAccess.Repository;

public class Repository<T>:iRepository<T> where T : class
{
    private  ApplicationDbContext _db;
    internal DbSet<T> dbSet; //Dbset is a collection of entities that can be queried from the database
    
    //constructor
    public Repository(ApplicationDbContext db)
    {
        _db = db;
        dbSet = _db.Set<T>();//Set is a method that returns a dbset
    }
    
    public IEnumerable<T> GetAll(Expression<Func<T, bool>> filter=null,string? includeProperties=null)//Get All Records and return IEnumerable
    {
        IQueryable<T> query = dbSet;
        if(filter!=null)
        {
            query = query.Where(filter);
        }
        if(includeProperties!=null)
        {
            
            foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))//what does this do? 
            
            {
                query = query.Include(includeProp);
            }
        }
        return query.ToList();
        
    }

    public T Get(Expression<Func<T,bool>>filter,string? includeProperties=null)//T=>filter where T is parameter,filter is the expression and returns bool
    {
        IQueryable<T> query = dbSet;//IQueryable is a collection of entities that can be queried from the database
        query= query.Where(filter);//we are filtering the query
        if(includeProperties!=null)
        {
            
            foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))//what does this do? 
            
            {
                query = query.Include(includeProp);
            }
        }
        return query.FirstOrDefault();//we are returning the first record that matches the filter
        //firstordefault means if there is no record that matches the filter,then return null
        
    }

    public void Add(T entity)
    {
        //add
        dbSet.Add(entity);//add entity to the dbset
    }

    public void Remove(T entity)
    {
        //remove
        dbSet.Remove(entity);//remove entity from the dbset
    }

    public void RemoveRange(IEnumerable<T> entity)
    {
        //remove range
        dbSet.RemoveRange(entity);//remove range of entities from the dbset
    }
}