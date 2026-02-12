using ListEngineCorsaLite.Models;
using SQLite;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace ListEngineCorsaLite.Models;

public class DatabaseService
{
    private SQLiteAsyncConnection _database;
    private bool _initialized = false;
    private readonly SemaphoreSlim _semaphore = new(1, 1);

    public DatabaseService()
    {
        // Inicialização lazy
    }

    private async Task InitializeAsync()
    {
        if (!_initialized)
        {
            await _semaphore.WaitAsync();
            try
            {
                if (!_initialized)
                {
                    var databasePath = Path.Combine(FileSystem.AppDataDirectory, "motores.db3");
                    _database = new SQLiteAsyncConnection(databasePath);
                    await _database.CreateTableAsync<Motor>();
                    await SeedDataAsync();
                    _initialized = true;
                }
            }
            finally
            {
                _semaphore.Release();
            }
        }
    }

    private async Task SeedDataAsync()
    {
        var count = await _database.Table<Motor>().CountAsync();

        if (count == 0)
        {
            await _database.InsertAllAsync(new[]
            {
                new Motor { Modelo = "Corsa GL 1.6", Codigo = "C16NZ", Cilindrada = 1598, Potencia = 92 },
                new Motor { Modelo = "Corsa GSi 1.6", Codigo = " Ecotec X16XE", Cilindrada = 1598, Potencia = 106, Torque = 14 }
            });
        }
    }

    // CRUD METHODS

    public async Task<List<Motor>> GetAllMotoresAsync()
    {
        await InitializeAsync();
        return await _database.Table<Motor>().OrderBy(m => m.Modelo).ToListAsync();
    }

    public async Task<Motor> GetMotorByIdAsync(int id)
    {
        await InitializeAsync();
        return await _database.Table<Motor>().FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task<int> SaveMotorAsync(Motor motor)
    {
        await InitializeAsync();

        if (motor == null)
            throw new ArgumentNullException(nameof(motor));

        if (motor.Id == 0)
            return await _database.InsertAsync(motor);
        else
            return await _database.UpdateAsync(motor);
    }

    // 🔥 DELETE À PROVA DE CRASH
    public async Task<int> DeleteMotorAsync(Motor motor)
    {
        await InitializeAsync();

        // Evita crash por null
        if (motor == null)
        {
            Debug.WriteLine("DeleteMotorAsync chamado com motor null");
            return 0;
        }

        // Evita crash por objeto não persistido
        if (motor.Id == 0)
        {
            Debug.WriteLine("DeleteMotorAsync chamado com motor sem Id");
            return 0;
        }

        Debug.WriteLine($"Deletando motor Id={motor.Id}");

        return await _database.DeleteAsync<Motor>(motor.Id);
    }

    // Métodos extras úteis
    public async Task<List<Motor>> GetFavoritosAsync()
    {
        await InitializeAsync();
        return await _database.Table<Motor>()
            .Where(m => m.Favorito)
            .ToListAsync();
    }

    public async Task<int> GetTotalCountAsync()
    {
        await InitializeAsync();
        return await _database.Table<Motor>().CountAsync();
    }
}
