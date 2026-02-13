using ListEngineCorsaLite.Models;
using ListEngineCorsaLite.Services;
using System.Collections.ObjectModel;

namespace ListEngineCorsaLite;

// Este comentário foi adicionado para testar o Git!
// Pense no Git como uma máquina do tempo para o seu código.
// Agora vamos salvar esta alteração no histórico.
public partial class MainPage : ContentPage
{

    private ObservableCollection<Motor> _motores = new();
    private readonly DatabaseService _database;
    private string imagemSelecionada;

    public MainPage(DatabaseService database)
    {
        InitializeComponent();
        _database = database;
        CarregarMotores();
    }

    // ========== CREATE ==========
    private async void OnAdicionarClicked(object sender, EventArgs e)
    {
        // Pede o nome do novo motor
        var modelo = await DisplayPromptAsync("Novo Motor",
            "Nome do motor:", "Adicionar", "Cancelar");

        if (string.IsNullOrWhiteSpace(modelo))
            return;

        // Cria novo motor
        var novoMotor = new Motor
        {
            Modelo = modelo,
            Codigo = "NOVO",
            Cilindrada = 1600,
            Potencia = 100,
            Torque = 140,
            Cilindros = 4,
            Combustivel = "Gasolina",
            Cambio = "Manual 5 marchas",
            ImagemPath = imagemSelecionada
        };

        // Salva no banco
        await _database.SaveMotorAsync(novoMotor);

        // Adiciona na lista
        _motores.Add(novoMotor);
        AtualizarLista();
        imagemSelecionada = null;
        PreviewImagem.Source = null;


        await DisplayAlert("Sucesso", "Motor adicionado!", "OK");
    }

    // ========== READ ==========
    private async void CarregarMotores()
    {
        try
        {
            LoadingIndicator.IsVisible = true;
            var motores = await _database.GetAllMotoresAsync();
            _motores.Clear();
            foreach (var motor in motores)
            {
                _motores.Add(motor);
            }

            AtualizarLista();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", $"Erro ao carregar: {ex.Message}", "OK");
        }
        finally
        {
            LoadingIndicator.IsVisible = false;
        }
    }
    private async Task MudarCambio(Motor motor)
    {
        var cambio = await DisplayActionSheet("Câmbio",
            "Cancelar", null,
            "Manual 5 marchas",
            "Manual 6 marchas",
            "Automático 4 marchas",
            "Automático 6 marchas",
            "CVT",
            "Semi-automático");

        if (cambio != "Cancelar" && !string.IsNullOrEmpty(cambio))
        {
            motor.Cambio = cambio;
            await _database.SaveMotorAsync(motor);
            AtualizarLista();
        }
    }
    private void AtualizarLista()
    {
        listaMotores.ItemsSource = null;
        listaMotores.ItemsSource = _motores;
        EmptyView.IsVisible = _motores.Count == 0;
    }

    // ========== UPDATE ==========

    // Edição direta nos campos
    private async void OnCampoEditado(object sender, EventArgs e)
    {
        if (sender is Entry entry && entry.BindingContext is Motor motor)
        {
            // Pequeno delay para não salvar a cada tecla
            await Task.Delay(500);

            // Salva no banco
            await _database.SaveMotorAsync(motor);

            // Feedback visual
            entry.BackgroundColor = Color.FromArgb("#E8F5E8");
            _ = Task.Delay(300).ContinueWith(_ =>
                MainThread.BeginInvokeOnMainThread(() =>
                    entry.BackgroundColor = Colors.Transparent));
        }
    }

    // Menu de edição avançada
    private async void OnMenuClicked(object sender, EventArgs e)
    {
        if (sender is Button btn && btn.CommandParameter is Motor motor)
        {
            var acao = await DisplayActionSheet($"Editar: {motor.Modelo}",
                "Cancelar", null,
                "✏️ Editar Tudo",
                "⛽ Mudar Combustível",
                "🔧 Mudar Câmbio",
                "📊 Ver Detalhes",
                "🗑️ Deletar");

            switch (acao)
            {
                case "✏️ Editar Tudo":
                    await EditarMotorCompleto(motor);
                    break;

                case "⛽ Mudar Combustível":
                    await MudarCombustivel(motor);
                    break;

                case "🔧 Mudar Câmbio":
                    await MudarCambio(motor);
                    break;

                case "📊 Ver Detalhes":
                    await VerDetalhes(motor);
                    break;

                case "🗑️ Deletar":
                    await DeletarMotor(motor);
                    break;
            }
        }
    }

    private async Task EditarMotorCompleto(Motor motor)
    {
        // Modelo
        var novoModelo = await DisplayPromptAsync("Editar", "Modelo:",
            initialValue: motor.Modelo);
        if (!string.IsNullOrWhiteSpace(novoModelo))
            motor.Modelo = novoModelo;

        // Código
        var novoCodigo = await DisplayPromptAsync("Editar", "Código:",
            initialValue: motor.Codigo);
        if (!string.IsNullOrWhiteSpace(novoCodigo))
            motor.Codigo = novoCodigo;

        // Potência
        var potenciaStr = await DisplayPromptAsync("Editar", "Potência (cv):",
            initialValue: motor.Potencia.ToString(), keyboard: Keyboard.Numeric);
        if (int.TryParse(potenciaStr, out int potencia))
            motor.Potencia = potencia;

        // Cilindrada
        var cilindradaStr = await DisplayPromptAsync("Editar", "Cilindrada (cc):",
            initialValue: motor.Cilindrada.ToString(), keyboard: Keyboard.Numeric);
        if (int.TryParse(cilindradaStr, out int cilindrada))
            motor.Cilindrada = cilindrada;

        // Salva
        await _database.SaveMotorAsync(motor);
        AtualizarLista();
    }

    private async Task MudarCombustivel(Motor motor)
    {
        var combustivel = await DisplayActionSheet("Combustível",
            "Cancelar", null, "Gasolina", "Flex", "Álcool", "Diesel");

        if (combustivel != "Cancelar" && !string.IsNullOrEmpty(combustivel))
        {
            motor.Combustivel = combustivel;
            await _database.SaveMotorAsync(motor);
        }
    }

    private async Task VerDetalhes(Motor motor)
    {
        var detalhes = $"""
            🏎️ {motor.Modelo}
            🔢 {motor.Codigo}
            
            ⚙️ ESPECIFICAÇÕES:
            • Cilindrada: {motor.Cilindrada} cc
            • Potência: {motor.Potencia} cv
            • Torque: {motor.Torque} Kg/fm
            • Cilindros: {motor.Cilindros}
            
            ⛽ COMBUSTÍVEL: {motor.Combustivel}
            🔧 CÂMBIO: {motor.Cambio}
            📅 PERÍODO: {motor.AnoInicio}-{motor.AnoFim}
            
            ⭐ Favorito: {(motor.Favorito ? "Sim" : "Não")}
            """;

        await DisplayAlert("Detalhes do Motor", detalhes, "OK");
    }

    // ========== DELETE ==========
    private async Task DeletarMotor(Motor motor)
    {
        // Proteção absoluta contra null
        if (motor == null)
        {
            await DisplayAlert("Erro", "Motor inválido.", "OK");
            return;
        }

        bool confirmar = await DisplayAlert(
            "Confirmar",
            $"Deletar motor '{motor.Modelo}'?",
            "Sim",
            "Não");

        if (!confirmar)
            return;

        try
        {
            // Deleta do banco
            await _database.DeleteMotorAsync(motor);

            // Remove da lista SEM crash (UI thread garantida)
            MainThread.BeginInvokeOnMainThread(() =>
            {
                if (_motores.Contains(motor))
                    _motores.Remove(motor);

                AtualizarLista();
            });

            await DisplayAlert("Sucesso", "Motor deletado!", "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro",
                $"Falha ao deletar motor:\n{ex.Message}",
                "OK");
        }
    }


    // ========== FAVORITO ==========
    private async void OnFavoritoClicked(object sender, EventArgs e)
    {
        if (sender is Button btn && btn.CommandParameter is Motor motor)
        {
            motor.Favorito = !motor.Favorito;
            await _database.SaveMotorAsync(motor);

            // Atualiza visualmente
            btn.Text = motor.Favorito ? "★" : "☆";
        }
    }

    // ========== PESQUISA ==========
    private async void OnPesquisarClicked(object sender, EventArgs e)
    {
        var termo = await DisplayPromptAsync("Pesquisar",
            "Digite o nome ou código:", "Buscar", "Cancelar");

        if (!string.IsNullOrWhiteSpace(termo))
        {
            // Filtra a lista local
            var filtrados = _motores.Where(m =>
                m.Modelo.Contains(termo, StringComparison.OrdinalIgnoreCase) ||
                m.Codigo.Contains(termo, StringComparison.OrdinalIgnoreCase))
                .ToList();

            listaMotores.ItemsSource = filtrados;

            if (filtrados.Count == 0)
            {
                await DisplayAlert("Pesquisa", "Nenhum motor encontrado", "OK");
            }
        }
    }
    async void SelecionarImagem_Clicked(object sender, EventArgs e)
    {
        var result = await FilePicker.PickAsync(new PickOptions
        {
            PickerTitle = "Selecione uma imagem",
            FileTypes = FilePickerFileType.Images
        });

        if (result != null)
        {
            var novoCaminho = Path.Combine(FileSystem.AppDataDirectory, result.FileName);

            using var stream = await result.OpenReadAsync();
            using var newStream = File.OpenWrite(novoCaminho);
            await stream.CopyToAsync(newStream);

            imagemSelecionada = novoCaminho;
            PreviewImagem.Source = imagemSelecionada;
        }
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        CarregarMotores();
    }
    private async Task AlterarImagemMotor(Motor motor)
    {
        var result = await FilePicker.PickAsync(new PickOptions
        {
            PickerTitle = "Selecione a nova imagem",
            FileTypes = FilePickerFileType.Images
        });

        if (result == null)
            return;

        var novoCaminho = Path.Combine(FileSystem.AppDataDirectory, result.FileName);

        using var stream = await result.OpenReadAsync();
        using var newStream = File.OpenWrite(novoCaminho);
        await stream.CopyToAsync(newStream);

        motor.ImagemPath = novoCaminho;

        await _database.SaveMotorAsync(motor);
    }

    private async void OnItemTapped(object sender, TappedEventArgs e)
    {
        if (e.Parameter is not Motor motor)
            return;

        await Shell.Current.GoToAsync(nameof(DetalhesMotorPage), new Dictionary<string, object>
        {
            { "Motor", motor }
        });
    }

}
