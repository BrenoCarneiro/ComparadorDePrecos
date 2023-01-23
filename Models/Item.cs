using System.ComponentModel;

namespace ComparadorDePrecos.Models
{
    public class Item : INotifyPropertyChanged
    {
        
        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisedOnPropertyChanged(string _PropertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(_PropertyName));
            }
        }


        private int posicao = 1;
        public int Posicao
        {
            get => posicao;
            set
            {
                if (posicao != value)
                {
                    posicao = value;
                    this.RaisedOnPropertyChanged("Posicao");
                }
            }
        }
        private decimal quantidade;
        public decimal Quantidade
        {
            get { return quantidade; }
            set
            {
                if (quantidade != value)
                {
                    quantidade = value;
                    this.RaisedOnPropertyChanged("Quantidade");
                }
            }
        }

        private decimal valorTotal;
        public decimal ValorTotal
        {
            get { return valorTotal; }
            set
            {
                if (valorTotal != value)
                {
                    valorTotal = value;
                    this.RaisedOnPropertyChanged("ValorTotal");
                }
            }
        }
        private decimal valorUnitario;
        public decimal ValorUnitario
        {
            get { return valorUnitario; }
            set
            {
                if (valorUnitario != value)
                {
                    valorUnitario = value;
                    this.RaisedOnPropertyChanged("ValorUnitario");
                }
            }
        }

        private double progresso = 1;
        public double Progresso
        {
            get { return progresso; }
            set
            {
                if (progresso != value)
                {
                    progresso = value;
                    this.RaisedOnPropertyChanged("Progresso");
                }
            }
        }

        private string unidadeDeMedida;

        public string UnidadeDeMedida
        {
            get { return unidadeDeMedida; }
            set
            {
                if (unidadeDeMedida != value)
                {
                    unidadeDeMedida = value;
                    this.RaisedOnPropertyChanged("UnidadeDeMedida");
                }
            }
        }
        public Item()
        {
            
        }
        public Item(decimal quantidade, decimal valorTotal)
        {
            Quantidade = quantidade;
            ValorTotal = valorTotal;
            ValorUnitario = valorTotal / quantidade;
    
        }

        public Item(decimal quantidade, decimal valorTotal, int posicao)
        {
            Quantidade = quantidade;
            ValorTotal = valorTotal;
            ValorUnitario = valorTotal / quantidade;
            Posicao = posicao;
        }

        public Item(decimal quantidade, decimal valorTotal, int posicao, string unidadeDeMedida)
        {
            ValorUnitario = valorTotal / quantidade;
            UnidadeDeMedida = unidadeDeMedida;
            ValorTotal = valorTotal;
            Quantidade = quantidade;
            Posicao = posicao;

        }


    }

}
