using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cargueiro.Application
{
    public class CargueiroDTO
    {
        public string Origem { get; set; }
        public string Destino { get; set; }
        public int Preco { get; set; }
        public DateTime DataPartida { get; set; }

        public CargueiroDTO(string origem, string destino, int preco, DateTime dataPartida)
        {
            Origem = origem;
            Destino = destino;
            Preco = preco;
            DataPartida = dataPartida;
        }

        public override string ToString()
        {
            return $"{Origem}|{Destino}|{Preco}|{DataPartida}";
        }

        public static CargueiroDTO FromString(string message)
        {
            string[] values = message.Split('|');
            string origem = values[0].Trim();
            string destino = values[1].Trim();
            int preco = int.Parse(values[2].Trim());
            DateTime dataPartida = DateTime.Parse(values[3].Trim());
            return new CargueiroDTO(origem, destino, preco, dataPartida);
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        public static CargueiroDTO FromJson(string json)
        {
            return JsonConvert.DeserializeObject<CargueiroDTO>(json);
        }
    }
}
