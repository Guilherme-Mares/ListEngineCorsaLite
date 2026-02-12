using ListEngineCorsaLite.Models;

namespace ListEngineCorsaLite;

public partial class NovoMotorPage : ContentPage
{
    private Motor? _motorEditando;
    private bool _modoEdicao = false;

    public delegate void MotorSalvoHandler(Motor motor);
    public event MotorSalvoHandler? MotorSalvo;

    public NovoMotorPage()
    {
        InitializeComponent();
        InicializarValoresPadrao();
        Title = "Adicionar Motor";
    }

    public NovoMotorPage(Motor motor) : this()
    {
        _motorEditando = motor;
        _modoEdicao = true;
        Title = "Editar Motor";
        BtnSalvar.Text = "ATUALIZAR MOTOR";

        PreencherCampos(motor);
    }

    private void InicializarValoresPadrao()
    {
        EntryCilindrada.Text = "1600";
        EntryPotencia.Text = "100";
        EntryTorque.Text = "140";
        EntryCilindros.Text = "4";
        EntryAnoInicio.Text = "2000";
        EntryAnoFim.Text = "2005";

        PickerCombustivel.SelectedIndex = 0;
        PickerCambio.SelectedIndex = 0;
    }

    private void PreencherCampos(Motor motor)
    {
        EntryModelo.Text = motor.Modelo;
        EntryCodigo.Text = motor.Codigo;
        EntryAnoInicio.Text = motor.AnoInicio.ToString();
        EntryAnoFim.Text = motor.AnoFim.ToString();
        EntryCilindrada.Text = motor.Cilindrada.ToString();
        EntryPotencia.Text = motor.Potencia.ToString();
        EntryTorque.Text = motor.Torque.ToString();
        EntryCilindros.Text = motor.Cilindros.ToString();

        if (!string.IsNullOrEmpty(motor.Combustivel))
        {
            var indexCombustivel = PickerCombustivel.Items.IndexOf(motor.Combustivel);
            if (indexCombustivel >= 0)
                PickerCombustivel.SelectedIndex = indexCombustivel;
        }

        if (!string.IsNullOrEmpty(motor.Cambio))
        {
            var indexCambio = PickerCambio.Items.IndexOf(motor.Cambio);
            if (indexCambio >= 0)
                PickerCambio.SelectedIndex = indexCambio;
        }
    }

    private void ValidarFormulario()
    {
        bool camposValidos =
            !string.IsNullOrWhiteSpace(EntryModelo.Text) &&
            !string.IsNullOrWhiteSpace(EntryCodigo.Text) &&
            !string.IsNullOrWhiteSpace(EntryAnoInicio.Text) &&
            !string.IsNullOrWhiteSpace(EntryAnoFim.Text) &&
            !string.IsNullOrWhiteSpace(EntryCilindrada.Text) &&
            !string.IsNullOrWhiteSpace(EntryPotencia.Text) &&
            !string.IsNullOrWhiteSpace(EntryTorque.Text) &&
            !string.IsNullOrWhiteSpace(EntryCilindros.Text);

        BtnSalvar.IsEnabled = camposValidos;
        BtnSalvar.BackgroundColor = camposValidos ?
            Color.FromArgb("#2196F3") : Color.FromArgb("#9E9E9E");
    }

    private void OnFormChanged(object sender, EventArgs e)
    {
        ValidarFormulario();
    }

    private async void OnCancelarClicked(object sender, EventArgs e)
    {
        bool confirmar = await DisplayAlert("Cancelar",
            "Deseja cancelar? As altera" + "\u00e7" + "\u00f5es ser\u00e3o perdidas.",
            "Sim", "N\u00e3o");

        if (confirmar)
            await Navigation.PopAsync();
    }

    private async void OnSalvarClicked(object sender, EventArgs e)
    {
        try
        {
            if (!int.TryParse(EntryAnoInicio.Text, out int anoInicio))
            {
                await DisplayAlert("Erro", "Ano in\u00edcio inv\u00e1lido", "OK");
                return;
            }

            if (!int.TryParse(EntryAnoFim.Text, out int anoFim))
            {
                await DisplayAlert("Erro", "Ano fim inv\u00e1lido", "OK");
                return;
            }

            if (anoFim < anoInicio)
            {
                await DisplayAlert("Erro", "Ano fim n\u00e3o pode ser menor que ano in\u00edcio", "OK");
                return;
            }

            Motor motor = _modoEdicao && _motorEditando != null ?
                _motorEditando : new Motor();

            motor.Modelo = EntryModelo.Text;
            motor.Codigo = EntryCodigo.Text;
            motor.AnoInicio = anoInicio;
            motor.AnoFim = anoFim;
            motor.Cilindrada = int.Parse(EntryCilindrada.Text);
            motor.Potencia = int.Parse(EntryPotencia.Text);
            motor.Torque = int.Parse(EntryTorque.Text);
            motor.Cilindros = int.Parse(EntryCilindros.Text);
            motor.Combustivel = PickerCombustivel.SelectedItem?.ToString() ?? "Gasolina";
            motor.Cambio = PickerCambio.SelectedItem?.ToString() ?? "Manual 5 marchas";
            motor.Tracao = "Dianteira";
            motor.Alimentacao = "Inje\u00e7\u00e3o Multiponto";

            MotorSalvo?.Invoke(motor);

            await DisplayAlert("Sucesso!",
                _modoEdicao ? "Motor atualizado!" : "Motor adicionado!",
                "OK");

            await Navigation.PopAsync();
        }
        catch (FormatException)
        {
            await DisplayAlert("Erro",
                "Verifique se todos os campos num\u00e9ricos est\u00e3o corretos",
                "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro",
                $"Erro: {ex.Message}",
                "OK");
        }
    }
}
