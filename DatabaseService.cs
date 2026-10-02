using registroProductos.Models;
using SQLite;

namespace registroProductos
{
    public class DatabaseService
    {
        private SQLiteAsyncConnection _db;

        private async Task Init()
        {
            if (_db != null)
                return;

            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "ProductosDB.db3");
            _db = new SQLiteAsyncConnection(dbPath);
            await _db.CreateTableAsync<Producto>();
        }

        public async Task<int> GuardarProductoAsync(Producto producto)
        {
            await Init();
            return await _db.InsertAsync(producto);
        }

        public async Task<List<Producto>> ObtenerProductosAsync()
        {
            await Init();
            return await _db.Table<Producto>().ToListAsync();
        }
    }
}