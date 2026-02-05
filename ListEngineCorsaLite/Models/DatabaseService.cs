using ListEngineCorsaLite.Models;
using SQLite;
using System.Collections.ObjectModel;

namespace ListEngineCorsaLite.Models;

public class DatabaseService
{
    private SQLiteAsyncConnection _database;
    private bool _initialized = false;

    public DatabaseService()
    {
        // A inicialização será feita no primeiro uso
    }

    private async Task InitializeDatabaseAsync()
    {
        if (!_initialized)
        {
            // Caminho do banco de dados no dispositivo
            var databasePath = Path.Combine(FileSystem.AppDataDirectory, "motores.db3");

            _database = new SQLiteAsyncConnection(databasePath);

            // Cria a tabela se não existir
            await _database.CreateTableAsync<Motor>();

            // Insere dados iniciais se a tabela estiver vazia
            await SeedDatabaseAsync();

            _initialized = true;
        }
    }

    private async Task SeedDatabaseAsync()
    {
        var count = await _database.Table<Motor>().CountAsync();

        if (count == 0)
        {
            // Insere alguns motores de exemplo
            await _database.InsertAllAsync(new[]
            {
                new Motor
                {
                    Modelo = "Corsa Wind 1.0 MPFi",
                    Codigo = "C16NZ",
                    AnoInicio = 1994,
                    AnoFim = 1999,
                    Cilindrada = 1598,
                    Potencia = 92,
                    Torque = 128,
                    Cilindros = 4,
                    Combustivel = "Gasolina",
                    Cambio = "Manual 5 marchas",
                    Tracao = "Dianteira",
                    Alimentacao = "Injeção Multiponto",
                    Favorito = true
                },
                new Motor
                {
                    Modelo = "Corsa GSi 1.6 16V",
                    Codigo = "X16XEL",
                    AnoInicio = 1996,
                    AnoFim = 2002,
                    Cilindrada = 1598,
                    Potencia = 106,
                    Torque = 150,
                    Cilindros = 4,
                    Combustivel = "Gasolina",
                    Cambio = "Manual 5 marchas",
                    Tracao = "Dianteira",
                    Alimentacao = "Injeção Multiponto",
                    Favorito = false
                }
            });
        }
    }

    // ========== OPERAÇÕES CRUD ==========

    // Obter todos os motores
    public async Task<ObservableCollection<Motor>> GetMotoresAsync()
    {
        await InitializeDatabaseAsync();
        var motores = await _database.Table<Motor>().ToListAsync();
        return new ObservableCollection<Motor>(motores);
    }

    // Salvar ou atualizar motor
    public async Task<int> SaveMotorAsync(Motor motor)
    {
        await InitializeDatabaseAsync();

        if (motor.Id == 0)
        {
            // Novo motor
            return await _database.InsertAsync(motor);
        }
        else
        {
            // Atualizar motor existente
            return await _database.UpdateAsync(motor);
        }
    }

    // Deletar motor
    public async Task<int> DeleteMotorAsync(Motor motor)
    {
        await InitializeDatabaseAsync();
        return await _database.DeleteAsync(motor);
    }

    // Buscar motor por ID
    public async Task<Motor> GetMotorAsync(int id)
    {
        await InitializeDatabaseAsync();
        return await _database.Table<Motor>()
            .Where(m => m.Id == id)
            .FirstOrDefaultAsync();
    }
}
