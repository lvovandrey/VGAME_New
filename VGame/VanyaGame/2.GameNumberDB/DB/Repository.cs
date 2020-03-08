using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LevelSetsEditor.DB
{
    public class Repository
    {
        public static IQueryable<TEntity> Select<TEntity>()
            where TEntity : class
        {
            LevelSetContext context = new LevelSetContext();

            // Здесь мы можем указывать различные настройки контекста,
            // например выводить в отладчик сгенерированный SQL-код
            context.Database.Log =
                (s => System.Diagnostics.Debug.WriteLine(s));

            // Загрузка данных с помощью универсального метода Set
            return context.Set<TEntity>();
        }
    }
}
