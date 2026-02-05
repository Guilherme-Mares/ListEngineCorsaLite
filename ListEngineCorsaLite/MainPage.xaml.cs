using ListEngineCorsaLite.Models;
using System.Collections.ObjectModel;

namespace ListEngineCorsaLite;

public partial class MainPage : ContentPage
{
    private ObservableCollection<Motor> _motores = new();
    private Motor _motorSelecionado;

    public MainPage()
    {
        InitializeComponent();
        CarregarDadosIniciais();
        AtualizarLista();
    }

    private void CarregarDadosIniciais()
    {
        // Adiciona alguns motores de exemplo
        _motores.Add(new Motor
        {
            Id = 1,
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
            Favorito = true
        });

        _motores.Add(new Motor
        {
            Id = 2,
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
            Favorito = false
        });
    }

    private void AtualizarLista()
    {
        MotoresCollection.ItemsSource = null;
        MotoresCollection.ItemsSource = _motores;

        // Mostra/oculta mensagem de vazio
        EmptyView.IsVisible = _motores.Count == 0;
        MotoresCollection.IsVisible = _motores.Count > 0;

        // Atualiza botão editar
        BtnEditar.IsVisible = _motorSelecionado != null;
    }

    // Quando seleciona um motor (adicione este evento no XAML)
    private void OnMotorSelecionado(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is Motor motor)
        {
            _motorSelecionado = motor;
            BtnEditar.IsVisible = true;
        }
        else
        {
            _motorSelecionado = null;
            BtnEditar.IsVisible = false;
        }
    }

    // Evento do botão favorito
    private void OnFavoritoClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is Motor motor)
        {
            motor.Favorito = !motor.Favorito;

            // Força atualização do item na lista
            var index = _motores.IndexOf(motor);
            if (index >= 0)
            {
                _motores[index] = motor;
            }
        }
    }

    // Botão EDITAR
    private async void OnEditarClicked(object sender, EventArgs e)
    {
        if (_motorSelecionado == null)
        {
            await DisplayAlert("Aviso", "Selecione um motor para editar", "OK");
            return;
        }

        var paginaEdicao = new NovoMotorPage(_motorSelecionado);
        paginaEdicao.MotorSalvo += (motorAtualizado) =>
        {
            // Atualiza o motor na lista
            var index = _motores.IndexOf(_motorSelecionado);
            if (index >= 0)
            {
                _motores[index] = motorAtualizado;
                AtualizarLista();
            }
        };

        await Navigation.PushAsync(paginaEdicao);
    }

    // Botão ADICIONAR (atualizado)
    private async void OnAdicionarClicked(object sender, EventArgs e)
    {
        var paginaNovo = new NovoMotorPage();
        paginaNovo.MotorSalvo += (novoMotor) =>
        {
            novoMotor.Id = _motores.Count + 1;
            _motores.Add(novoMotor);
            AtualizarLista();
        };

        await Navigation.PushAsync(paginaNovo);
    }

    // Botão DELETAR (adicione no XAML também)
    private async void OnDeletarClicked(object sender, EventArgs e)
    {
        if (_motorSelecionado == null)
        {
            await DisplayAlert("Aviso", "Selecione um motor para deletar", "OK");
            return;
        }

        bool confirmar = await DisplayAlert("Confirmar",
            $"Tem certeza que deseja deletar o motor {_motorSelecionado.Modelo}?",
            "Sim", "Não");

        if (confirmar)
        {
            _motores.Remove(_motorSelecionado);
            _motorSelecionado = null;
            AtualizarLista();
            await DisplayAlert("Sucesso", "Motor deletado com sucesso!", "OK");
        }
    }
}
