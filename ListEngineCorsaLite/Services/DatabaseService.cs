using ListEngineCorsaLite.Models;
using SQLite;
using System.Diagnostics;

namespace ListEngineCorsaLite.Services;

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
                new Motor
                {
                    Modelo = "Corsa Wind 1.0",
                    Codigo = "OHC",
                    AnoInicio = 1994,
                    AnoFim = 1998,
                    Cilindrada = 993,
                    Potencia = 50,
                    Torque = 74,
                    Cilindros = 4,
                    Combustivel = "Gasolina",
                    Alimentacao = "Carburador",
                    Cambio = "Manual 5 marchas",
                    Tracao = "Dianteira",
                    Descricao = "Motor 1.0 OHC de origem Opel, utilizado nos primeiros Corsa brasileiros. Simples e robusto, com comando de válvulas no cabeçote acionado por correia dentada. Alimentação por carburador mono-corpo Weber. Motor econômico projetado para uso urbano."
                },
                new Motor
                {
                    Modelo = "Corsa GL 1.6",
                    Codigo = "C16NZ",
                    AnoInicio = 1995,
                    AnoFim = 1999,
                    Cilindrada = 1598,
                    Potencia = 92,
                    Torque = 138,
                    Cilindros = 4,
                    Combustivel = "Gasolina",
                    Alimentacao = "Injeção Multiponto",
                    Cambio = "Manual 5 marchas",
                    Tracao = "Dianteira",
                    Descricao = "Motor 1.6 8V de origem alemã (Família I da Opel). Bloco de ferro fundido com cabeçote de alumínio, comando de válvulas único no cabeçote (SOHC). Linha de injeção Multec 700 com 4 bicos injetores, proporcionando boa resposta e torque em baixas rotações. Foi a motorização mais potente do Corsa brasileiro na época."
                },
                new Motor
                {
                    Modelo = "Corsa GSi 1.6 16V",
                    Codigo = "X16XE",
                    AnoInicio = 1996,
                    AnoFim = 1997,
                    Cilindrada = 1598,
                    Potencia = 106,
                    Torque = 150,
                    Cilindros = 4,
                    Combustivel = "Gasolina",
                    Alimentacao = "Injeção Multiponto",
                    Cambio = "Manual 5 marchas",
                    Tracao = "Dianteira",
                    Descricao = "Motor Ecotec 1.6 16V, um dos mais avançados de sua época. Duplo comando de válvulas no cabeçote (DOHC) com 4 válvulas por cilindro. Bloco e cabeçote projetados para alta eficiência volumétrica 3 com cruzamento de válvulas otimizado. Utilizado exclusivamente no Corsa GSi, a versão esportiva da linha. Importado da Europa, considerado um dos melhores motores de 4 cilindros de sua geração."
                },
                new Motor
                {
                    Modelo = "Corsa Super 1.0 MPFI",
                    Codigo = "MPFI 1.0",
                    AnoInicio = 1998,
                    AnoFim = 2002,
                    Cilindrada = 993,
                    Potencia = 60,
                    Torque = 86,
                    Cilindros = 4,
                    Combustivel = "Gasolina",
                    Alimentacao = "Injeção Multiponto",
                    Cambio = "Manual 5 marchas",
                    Tracao = "Dianteira",
                    Descricao = "Evolução do motor 1.0 OHC com sistema de injeção eletrônica multiponto Multec. Ganho significativo de desempenho e economia em relação ao carburado. Motor de bloco de ferro fundido com cabeçote de alumínio, 8 válvulas e comando simples. Ideal para uso urbano com bom equilíbrio entre potência e consumo."
                },
                new Motor
                {
                    Modelo = "Corsa Sedan 1.6 16V",
                    Codigo = "Z16XE",
                    AnoInicio = 2001,
                    AnoFim = 2005,
                    Cilindrada = 1598,
                    Potencia = 102,
                    Torque = 148,
                    Cilindros = 4,
                    Combustivel = "Gasolina",
                    Alimentacao = "Injeção Multiponto",
                    Cambio = "Manual 5 marchas",
                    Tracao = "Dianteira",
                    Descricao = "Segunda geração do motor Ecotec 1.6 16V, evolução do X16XE. Mantém a arquitetura DOHC 16V com melhorias no controle de emissões e consumo. Sistema de injeção Multec eletrônica de segunda geração. Oferecia a melhor relação potência/cilindrada da linha Corsa com excelente dirigibilidade."
                },
                new Motor
                {
                    Modelo = "Corsa 1.0 VHC Flex",
                    Codigo = "VHC Flex",
                    AnoInicio = 2005,
                    AnoFim = 2012,
                    Cilindrada = 999,
                    Potencia = 78,
                    Torque = 95,
                    Cilindros = 4,
                    Combustivel = "Flex",
                    Alimentacao = "Injeção Multiponto",
                    Cambio = "Manual 5 marchas",
                    Tracao = "Dianteira",
                    Descricao = "Motor VHC (Vortex High Combustion) 1.0 Flex, desenvolvido pela GM do Brasil. Tecnologia de câmara de combustão turbulenta para melhor queima. Primeiro motor flex da linha Corsa, operando com gasolina, etanol ou qualquer mistura. Sistema de gerenciamento eletrônico com sonda lambda de banda larga para adaptação automática ao combustível."
                },
                new Motor
                {
                    Modelo = "Corsa 1.4 Econoflex",
                    Codigo = "Econo 1.4",
                    AnoInicio = 2008,
                    AnoFim = 2012,
                    Cilindrada = 1389,
                    Potencia = 97,
                    Torque = 128,
                    Cilindros = 4,
                    Combustivel = "Flex",
                    Alimentacao = "Injeção Multiponto",
                    Cambio = "Manual 5 marchas",
                    Tracao = "Dianteira",
                    Descricao = "Motor 1.4 Econoflex 8V, a motorização mais equilibrada dos últimos anos do Corsa. Derivado da família SOHC com avanços em economia de combustível. Sistema EconoFlex com tecnologia de gerenciamento inteligente de mistura ar/combustível. Oferecia bom desempenho urbano com alto rendimento por litro, sendo a opção preferida para quem buscava economia com desempenho adequado."
                },
                new Motor
                {
                    Modelo = "Corsa 1.8 Flex",
                    Codigo = "MPFI 1.8",
                    AnoInicio = 2003,
                    AnoFim = 2012,
                    Cilindrada = 1796,
                    Potencia = 114,
                    Torque = 165,
                    Cilindros = 4,
                    Combustivel = "Flex",
                    Alimentacao = "Injeção Multiponto",
                    Cambio = "Manual 5 marchas",
                    Tracao = "Dianteira",
                    Descricao = "Motor 1.8 MPFI Flex, a motorização mais potente disponível para o Corsa. Bloco robusto de ferro fundido com cabeçote de alumínio, 8 válvulas com comando simples (SOHC). Projetado para alto torque em baixas rotações, ideal para quem precisa de desempenho em rodovia. Versão Flex com adaptação para gasolina e etanol com potência de até 114 cv com etanol."
                }
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

    public async Task<int> DeleteMotorAsync(Motor motor)
    {
        await InitializeAsync();

        if (motor == null)
        {
            Debug.WriteLine("DeleteMotorAsync chamado com motor null");
            return 0;
        }

        if (motor.Id == 0)
        {
            Debug.WriteLine("DeleteMotorAsync chamado com motor sem Id");
            return 0;
        }

        Debug.WriteLine($"Deletando motor Id={motor.Id}");

        return await _database.DeleteAsync<Motor>(motor.Id);
    }

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
