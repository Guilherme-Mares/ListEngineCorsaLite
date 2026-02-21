using ListEngineCorsaLite.Models;
using ListEngineCorsaLite.Services;

namespace ListEngineCorsaLite;

[QueryProperty(nameof(Motor), "Motor")]
public partial class DetalhesMotorPage : ContentPage
{
    private Motor _motor;
    private readonly DatabaseService _database;

    public Motor Motor
    {
        get => _motor;
        set
        {
            _motor = value;
            PreencherDados();
        }
    }

    public DetalhesMotorPage(DatabaseService database)
    {
        InitializeComponent();
        _database = database;
    }

    private void PreencherDados()
    {
        if (_motor == null)
            return;

        // Header
        LblModelo.Text = _motor.Modelo;
        LblCodigo.Text = $"Motor {_motor.Codigo}";
        LblPeriodo.Text = $"📅 {_motor.AnoInicio} – {_motor.AnoFim}";

        // Resumo
        LblCilindradaResumo.Text = $"{_motor.Cilindrada} cc";
        LblPotenciaResumo.Text = $"{_motor.Potencia} cv";
        LblTorqueResumo.Text = $"{_motor.Torque} Nm";

        // Imagem
        if (!string.IsNullOrEmpty(_motor.ImagemPath))
        {
            ImgMotor.Source = _motor.ImagemPath;
            FrameImagem.IsVisible = true;
        }
        else
        {
            FrameImagem.IsVisible = false;
        }

        // Descrição
        if (!string.IsNullOrEmpty(_motor.Descricao))
        {
            LblDescricao.Text = _motor.Descricao;
            FrameDescricao.IsVisible = true;
        }
        else
        {
            FrameDescricao.IsVisible = false;
        }

        // Especificações
        LblCilindrada.Text = $"{_motor.Cilindrada} cc";
        LblPotencia.Text = $"{_motor.Potencia} cv";
        LblTorque.Text = $"{_motor.Torque} Nm";
        LblCilindros.Text = _motor.Cilindros.ToString();

        // Configuração
        LblCombustivel.Text = _motor.Combustivel;
        LblAlimentacao.Text = _motor.Alimentacao;
        LblCambio.Text = _motor.Cambio;
        LblTracao.Text = _motor.Tracao;

        // Favorito
        AtualizarBotaoFavorito();
    }

    private void AtualizarBotaoFavorito()
    {
        if (_motor == null) return;
        BtnFavorito.Text = _motor.Favorito ? "★ Favorito" : "☆ Favorito";
        BtnFavorito.TextColor = _motor.Favorito
            ? Color.FromArgb("#FFD700")
            : Colors.White;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (_motor != null && _motor.Id > 0)
        {
            var atualizado = await _database.GetMotorByIdAsync(_motor.Id);
            if (atualizado != null)
            {
                Motor = atualizado;
            }
        }
    }

    private async void OnFavoritoClicked(object sender, EventArgs e)
    {
        if (_motor == null) return;

        _motor.Favorito = !_motor.Favorito;
        await _database.SaveMotorAsync(_motor);
        AtualizarBotaoFavorito();
    }

    private async void OnEditarClicked(object sender, EventArgs e)
    {
        if (_motor == null) return;

        var editPage = new NovoMotorPage(_motor);
        editPage.MotorSalvo += async (Motor motorEditado) =>
        {
            await _database.SaveMotorAsync(motorEditado);
            Motor = motorEditado;
        };
        await Navigation.PushAsync(editPage);
    }

    private async void OnDeletarClicked(object sender, EventArgs e)
    {
        if (_motor == null) return;

        bool confirmar = await DisplayAlert(
            "Confirmar",
            $"Deseja deletar o motor '{_motor.Modelo}'?",
            "Sim", "Não");

        if (!confirmar) return;

        try
        {
            await _database.DeleteMotorAsync(_motor);
            await DisplayAlert("Sucesso", "Motor deletado!", "OK");
            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", $"Falha ao deletar: {ex.Message}", "OK");
        }
    }
}
