using System.Collections.ObjectModel;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Alerts;
using ComparadorDePrecos.Models;
using System.Globalization;

namespace ComparadorDePrecos.Views;

public partial class CapacidadePage : ContentPage
{
	public CapacidadePage()
	{
		InitializeComponent();
        ListaCapacidade = new ObservableCollection<Item>();
    }

    private ObservableCollection<Item> _listaCapacidade;
    public ObservableCollection<Item> ListaCapacidade
    {
        get => _listaCapacidade;
        set
        {
            _listaCapacidade = value;
            OnPropertyChanged(nameof(ListaCapacidade));
        }
    }

    public Command<Item> Remove { get => new Command<Item>((item) => ListaCapacidade.Remove(item)); }
    public void AtualizarUnidadeDeMedida()
    {
        if (ListaCapacidade.Count > 1)
        {
            if (ListaCapacidade.Any(x => x.UnidadeDeMedida == "L"))
            {
                foreach (Item item in ListaCapacidade.Where(x => x.UnidadeDeMedida == "ML"))
                {
                    item.Quantidade = item.Quantidade / 1000;
                    item.UnidadeDeMedida = "L";
                    item.ValorUnitario = item.ValorUnitario * 1000;
                }
            }
        }
    }
    public void AtualizarProgresso()
    {
        if (ListaCapacidade.Count > 1)
        {
            decimal somaValorUnitario = ListaCapacidade.Sum(o => o.ValorUnitario);

            foreach (Item item in ListaCapacidade)
            {
                item.Progresso = Convert.ToDouble(item.ValorUnitario / somaValorUnitario);
            }
        }
        else
        {
            foreach (Item item in ListaCapacidade)
            {
                item.Progresso = 1;
            }
        }
    }

    public void AdicionarItem(object sender, EventArgs e)
    {
        if(quantidade.Text != null && valor.Text != null && picker.SelectedIndex > -1 && quantidade.Text != "" && valor.Text != "") 
        {
            decimal quantidadeText = decimal.Parse(quantidade.Text, new NumberFormatInfo() { NumberDecimalSeparator = "." });
            decimal valorText = decimal.Parse(valor.Text, new NumberFormatInfo() { NumberDecimalSeparator = "." });
            int posicao = ListaCapacidade.Count + 1;
            string unidadeDeMedida = picker.Items[picker.SelectedIndex];

            ListaCapacidade.Add(new Item(quantidadeText, valorText, posicao, unidadeDeMedida));
            AtualizarUnidadeDeMedida();
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

    private void RemoverItem(object sender, EventArgs e)
	{
		var botao = sender as Button;
		var item = botao?.BindingContext as Item;
		Remove.Execute(item);
        for (int i = 1; i <= ListaCapacidade.Count; i++)
        {
            ListaCapacidade[i - 1].Posicao = i;
        }
        AtualizarProgresso();
    }


    private void ProgressBar_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        var progress = sender as ProgressBar;
        if (ListaCapacidade.Count > 1)
        {
            var item = progress?.BindingContext as Item;
            var minimo = ListaCapacidade.MinBy(x => x.ValorUnitario);
            var maximo = ListaCapacidade.MaxBy(x => x.ValorUnitario);
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
        else if (ListaCapacidade.Count == 1)
        {
            progress.ProgressColor = Color.FromHex("3E8EED");
        }
    }
}