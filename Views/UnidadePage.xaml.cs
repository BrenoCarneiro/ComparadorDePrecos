using System.Collections.ObjectModel;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Alerts;
using ComparadorDePrecos.Models;
using System.Globalization;

namespace ComparadorDePrecos.Views;

public partial class UnidadePage : ContentPage
{

    public UnidadePage()
    {
        InitializeComponent();
        ListaUnidade = new ObservableCollection<Item>();
    }

    private ObservableCollection<Item> _listaUnidade;
    public ObservableCollection<Item> ListaUnidade
    {
        get => _listaUnidade;       
        set
        {   
            _listaUnidade = value;
           OnPropertyChanged(nameof(ListaUnidade));        
        }
    }

    public void AtualizarProgresso()
    {
        if(ListaUnidade.Count > 1) 
        {
            decimal somaValorUnitario = ListaUnidade.Sum(o => o.ValorUnitario);

            foreach(Item item in ListaUnidade)
            {
                item.Progresso = Convert.ToDouble(item.ValorUnitario / somaValorUnitario);
            }
        }
        else
        {
            foreach (Item item in ListaUnidade)
            {
                item.Progresso = 1;
            }
        }
    }
    

    public Command<Item> Remove { get => new Command<Item>((item) => ListaUnidade.Remove(item)); }
    public void AdicionarItem(object sender, EventArgs e)
    {
        if (quantidade.Text != null && valor.Text != null && quantidade.Text != "" && valor.Text != "")
        {
            decimal quantidadeText = decimal.Parse(quantidade.Text, new NumberFormatInfo() { NumberDecimalSeparator = "." });
            decimal valorText = decimal.Parse(valor.Text, new NumberFormatInfo() { NumberDecimalSeparator = "." });
            int posicao = ListaUnidade.Count + 1;

            ListaUnidade.Add(new Item(quantidadeText, valorText, posicao));
            AtualizarProgresso();
            valor.Text = "";
            quantidade.Text = "";
        }
        else
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            var toast = Toast.Make("Preencha os campos corretamente!", ToastDuration.Short, 20);
            toast.Show(cancellationTokenSource.Token);
        }   
    }

    public void RemoverItem(object sender, EventArgs e)
    {
        var botao = sender as Button;
        var item = botao?.BindingContext as Item;
        Remove.Execute(item);
        for (int i = 1; i <= ListaUnidade.Count; i++)
        {
            ListaUnidade[i-1].Posicao = i;
        }
        AtualizarProgresso();
    }
    private void ProgressBar_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        var progress = sender as ProgressBar;
        if (ListaUnidade.Count > 1)
        {
            var item = progress?.BindingContext as Item;
            var minimo = ListaUnidade.MinBy(x => x.ValorUnitario);
            var maximo = ListaUnidade.MaxBy(x => x.ValorUnitario);
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
        else if (ListaUnidade.Count == 1)
        {
            progress.ProgressColor = Color.FromHex("3E8EED");
        }
    }
}