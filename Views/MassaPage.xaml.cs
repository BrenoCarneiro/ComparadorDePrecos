using System.Collections.ObjectModel;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Alerts;
using ComparadorDePrecos.Models;
using System.Globalization;
using System;

namespace ComparadorDePrecos.Views;

public partial class MassaPage : ContentPage
{
	public MassaPage()
	{
        InitializeComponent();
        ListaMassa = [];
    }

    private ObservableCollection<Item> _listaMassa;
    public ObservableCollection<Item> ListaMassa
    {
        get => _listaMassa;
        set
        {
            _listaMassa = value;
            OnPropertyChanged(nameof(ListaMassa));
        }
    }

    public Command<Item> Remove { get => new((item) => ListaMassa.Remove(item)); }
    public void AtualizarUnidadeDeMedida()
    {
        if (ListaMassa.Count > 1)
        {
            if (ListaMassa.Any(x => x.UnidadeDeMedida == "KG"))
            {
                foreach (Item item in ListaMassa.Where(x => x.UnidadeDeMedida == "G"))
                {
                    item.Quantidade /= 1000;
                    item.UnidadeDeMedida = "KG";
                    item.ValorUnitario *= 1000;
                }
            }
        }
    }
    public void AtualizarProgresso()
    {
        if (ListaMassa.Count > 1)
        {
            decimal somaValorUnitario = ListaMassa.Sum(o => o.ValorUnitario);
           
            foreach (Item item in ListaMassa)
            {
                item.Progresso = Convert.ToDouble(item.ValorUnitario / somaValorUnitario);
            }
        }
        else
        {
            foreach (Item item in ListaMassa)
            {
                item.Progresso = 1;
            }
        }
    }

    public void AdicionarItem(object sender, EventArgs e)
    {
        if (quantidade.Text != null && valor.Text != null && picker.SelectedIndex > -1 && quantidade.Text != "" && valor.Text != "")
        {
            decimal quantidadeText = decimal.Parse(quantidade.Text, new NumberFormatInfo() { NumberDecimalSeparator = "." });
            decimal valorText = decimal.Parse(valor.Text, new NumberFormatInfo() { NumberDecimalSeparator = "." });
            int posicao = ListaMassa.Count + 1;
            string unidadeDeMedida = picker.Items[picker.SelectedIndex];

            ListaMassa.Add(new Item(quantidadeText, valorText, posicao, unidadeDeMedida));
            AtualizarUnidadeDeMedida();
            AtualizarProgresso();
            valor.Text = "";
            quantidade.Text = "";

        }
        else
        {
            CancellationTokenSource cancellationTokenSource = new();
            var toast = Toast.Make("Preencha os campos corretamente!", ToastDuration.Short, 20);
            toast.Show(cancellationTokenSource.Token);
        }
    }

    private void RemoverItem(object sender, EventArgs e)
    {
        var botao = sender as Button;
        var item = botao?.BindingContext as Item;
        Remove.Execute(item);
        for (int i = 1; i <= ListaMassa.Count; i++)
        {
            ListaMassa[i - 1].Posicao = i;
        }
        AtualizarProgresso();
    }

    private void ProgressBar_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        var progress = sender as ProgressBar;
        if (ListaMassa.Count > 1)
        {
            var item = progress?.BindingContext as Item;
            var minimo = ListaMassa.MinBy(x => x.ValorUnitario);
            var maximo = ListaMassa.MaxBy(x => x.ValorUnitario);
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
        else if (ListaMassa.Count == 1)
        {
            progress.ProgressColor = Color.FromHex("3E8EED");
        }
    }
}