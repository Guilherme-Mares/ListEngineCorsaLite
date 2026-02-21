using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ListEngineCorsaLite.Models;
using System.Collections.ObjectModel;

namespace ListEngineCorsaLite.ViewModels;

public partial class MainViewModel : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<Motor> _motores = new();

    public MainViewModel()
    {
        CarregarMotoresIniciais();
    }

    private void CarregarMotoresIniciais()
    {
        // Dados iniciais de exemplo
        Motores.Add(new Motor
        {
            Id = 1,
            Modelo = "Corsa GL 1.6",
            Codigo = "C16NZ",
            AnoInicio = 1994,
            AnoFim = 1999,
            Cilindrada = 1598,
            Potencia = 92,
            Combustivel = "Gasolina",
            Favorito = true
        });

        Motores.Add(new Motor
        {
            Id = 2,
            Modelo = "Corsa GSi 1.6",
            Codigo = "X16XEL",
            AnoInicio = 1996,
            AnoFim = 2002,
            Cilindrada = 1598,
            Potencia = 106,
            Combustivel = "Gasolina",
            Favorito = false
        });

        Motores.Add(new Motor
        {
            Id = 3,
            Modelo = "Corsa 1.0 Flex",
            Codigo = "VHC",
            AnoInicio = 2002,
            AnoFim = 2012,
            Cilindrada = 998,
            Potencia = 73,
            Combustivel = "Flex",
            Favorito = true
        });
    }

    [RelayCommand]
    private void ToggleFavorito(Motor motor)
    {
        if (motor != null)
        {
            motor.Favorito = !motor.Favorito;
        }
    }
}
