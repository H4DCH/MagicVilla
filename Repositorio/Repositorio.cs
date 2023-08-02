using MagicVilla.Datos;
using MagicVilla.Repositorio.IRepositorio;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MagicVilla.Repositorio
{
    public class Repositorio<T> : IRepositorio<T> where T : class
    {
        private readonly Context _Context;
        internal DbSet<T> Dbset;

        public Repositorio(Context context)
        {
            _Context = context;
            this.Dbset = _Context.Set<T>();
            
        }
        public async  Task Crear(T entidad)
        {
            await Dbset.AddAsync(entidad);
            await Grabar();
        }

        public async Task Grabar()
        {
            await _Context.SaveChangesAsync();  
        }

        public async Task<T> Obtener(Expression<Func<T, bool>>? filtro = null, bool tracked = true)
        {
            IQueryable<T> query = Dbset;
            if (!tracked)
            {
                query = query.AsNoTracking();   
            }
            if (filtro!=null)
            {
                query = query.Where(filtro);

            }
            return await query.FirstOrDefaultAsync();
        }

        public async Task<List<T>> ObtenerTodos(Expression<Func<T, bool>>? filtro = null)
        {
            IQueryable<T> query = Dbset;
            if (filtro != null)
            {
                query = query.Where(filtro);

            }

            return await query.ToListAsync();   

        }

        public async Task Remover(T entidad)
        {
            Dbset.Remove(entidad);
            await Grabar();
        }
    }
}
