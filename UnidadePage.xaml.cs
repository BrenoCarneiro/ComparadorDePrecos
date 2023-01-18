using System.Collections.ObjectModel;
using System.Text;

namespace ComparadorDePrecos;

public partial class UnidadePage : ContentPage
{
    public UnidadePage()
    {
        InitializeComponent();
        ListaDeItens = new ObservableCollection<Item>();       

    }
    public class Item
    {
        public int Posicao { get; private set; }
        public int Quantidade { get; private set; }
        public decimal ValorTotal { get; private set; }

        public decimal ValorUnitario { get; private set; }

        public Item(int quantidade, decimal valorTotal, int posicao)
        {
            Quantidade = quantidade;
            ValorTotal = valorTotal;
            ValorUnitario = valorTotal / quantidade;
            Posicao = posicao;
        }
    }

    private ObservableCollection<Item> _listaDeItens;
    public ObservableCollection<Item> ListaDeItens
    {
        get => _listaDeItens;
        set
        {
            _listaDeItens = value;
            OnPropertyChanged(nameof(ListaDeItens));
        }
    }
    public void AdicionarItem(object sender, EventArgs e)
    {
        ListaDeItens.Add(new Item(int.Parse(quantidade.Text), decimal.Parse(valor.Text), ListaDeItens.Count+1));
        
    }
    public void RemoverItem(object sender, EventArgs e)
    {
        var posicaoDoItem = (Item)sender;
        Console.WriteLine(posicaoDoItem);
        Console.WriteLine(posicaoDoItem.Posicao);

    }
}