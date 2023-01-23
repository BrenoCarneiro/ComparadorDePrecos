using System.Collections.ObjectModel;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using System.Globalization;
using ComparadorDePrecos.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ComparadorDePrecos.Views;

public partial class ComprimentoPage : ContentPage
{
	public ComprimentoPage()
	{
		InitializeComponent();
        ListaComprimento = new ObservableCollection<Item>();
    }

    private ObservableCollection<Item> _listaComprimento;
    public ObservableCollection<Item> ListaComprimento
    {
        get => _listaComprimento;
        set
        {
            _listaComprimento = value;
            OnPropertyChanged(nameof(ListaComprimento));
        }
    }

    public Command<Item> Remove { get => new Command<Item>((item) => ListaComprimento.Remove(item)); }
    public void AtualizarUnidadeDeMedida()
    {
        if (ListaComprimento.Count > 1)
        {
            if (ListaComprimento.Any(x => x.UnidadeDeMedida == "M"))
            {
                foreach (Item item in ListaComprimento.Where(x => x.UnidadeDeMedida == "CM"))
                {
                    item.Quantidade = item.Quantidade / 100;
                    item.UnidadeDeMedida = "M";
                    item.ValorUnitario = item.ValorUnitario * 100;
                }
            }
        }
    }
    public void AtualizarProgresso()
    {
        if (ListaComprimento.Count > 1)
        {
            decimal somaValorUnitario = ListaComprimento.Sum(o => o.ValorUnitario);

            foreach (Item item in ListaComprimento)
            {
                item.Progresso = Convert.ToDouble(item.ValorUnitario / somaValorUnitario);
            }
        }
        else
        {
            foreach (Item item in ListaComprimento)
            {
                item.Progresso = 1;
            }
        }
    }

    public void AdicionarItem(object sender, EventArgs e)
    {
        if (quantidade.Text != null && valor.Text != null && picker.SelectedIndex > -1)
        {
            int quantidadeDeItensText = 1;
            if (quantidadeDeItens.Text != null && quantidadeDeItens.Text != "")
            {
                quantidadeDeItensText = int.Parse(quantidadeDeItens.Text);
            }
            decimal quantidadeText = decimal.Parse(quantidade.Text, new NumberFormatInfo() { NumberDecimalSeparator = "." });
            decimal valorText = decimal.Parse(valor.Text, new NumberFormatInfo() { NumberDecimalSeparator = "." });
            int posicao = ListaComprimento.Count + 1;
            string unidadeDeMedida = picker.Items[picker.SelectedIndex];

            ListaComprimento.Add(new Item((quantidadeText * quantidadeDeItensText), valorText, posicao, unidadeDeMedida));
            AtualizarUnidadeDeMedida();
            AtualizarProgresso();
            valor.Text = "";
            quantidade.Text = "";
            quantidadeDeItens.Text = "";

        }
        else
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            var toast = Toast.Make("Preencha os campos corretamente!", ToastDuration.Short, 20);
            toast.Show(cancellationTokenSource.Token);
        }
    }

    private void RemoverItem(object sender, EventArgs e)
    {
        var botao = sender as Button;
        var item = botao?.BindingContext as Item;
        Remove.Execute(item);
        for (int i = 1; i <= ListaComprimento.Count; i++)
        {
            ListaComprimento[i - 1].Posicao = i;
        }
        AtualizarProgresso();
    }

    private void ProgressBar_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        var progress = sender as ProgressBar;
        if (ListaComprimento.Count > 1)
        {
            var item = progress?.BindingContext as Item;
            var minimo = ListaComprimento.MinBy(x => x.ValorUnitario);
            var maximo = ListaComprimento.MaxBy(x => x.ValorUnitario);
            if (maximo.ValorUnitario > minimo.ValorUnitario && item != null)
            {
                if (item.ValorUnitario <= minimo.ValorUnitario)
                {
                    progress.ProgressColor = Colors.Green;
                }
                if (item.ValorUnitario >= maximo.ValorUnitario)
                {
                    progress.ProgressColor = Colors.Red;
                }
            }
            else
            {
                progress.ProgressColor = Color.FromHex("3E8EED");
            }
        }
        else if (ListaComprimento.Count == 1)
        {
            progress.ProgressColor = Color.FromHex("3E8EED");
        }
    }
}