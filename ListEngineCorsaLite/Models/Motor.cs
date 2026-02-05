using SQLite;
using System.ComponentModel;
using System.Runtime.CompilerServices;
namespace ListEngineCorsaLite.Models;

public class Motor
{
    public int Id { get; set; }
    public string Modelo { get; set; } = string.Empty;
    public string Codigo { get; set; } = string.Empty;
    public int AnoInicio { get; set; } = 2000;
    public int AnoFim { get; set; } = 2005;
    public int Cilindrada { get; set; } = 1600;
    public int Potencia { get; set; } = 100;
    public int Torque { get; set; } = 140;
    public int Cilindros { get; set; } = 4;
    public string Combustivel { get; set; } = "Gasolina";
    public string Alimentacao { get; set; } = "Injeção Multiponto";
    public string Cambio { get; set; } = "Manual 5 marchas";
    public string Tracao { get; set; } = "Dianteira";
    public bool Favorito { get; set; }

    public string Periodo => $"{AnoInicio}-{AnoFim}";
    public string Especificacoes => $"{Cilindrada}cc • {Potencia}cv • {Torque}Nm";
}
