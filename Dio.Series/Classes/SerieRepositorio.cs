using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dio.Series.Interfaces;

namespace Dio.Series
{
    public class SerieRepositorio : IRepositorio<Serie>
    {
        private List<Serie> listaSerie = new List<Serie>();

        public void Atualiza(int id, Serie objeto)
        {
            listaSerie[id] = objeto;
        }

        public void Exclui(int id)
        {
            listaSerie[id].Excluir();
        }

        public void Insere(Serie objeto)
        {
            listaSerie.Add(objeto);
        }

        public List<Serie> Lista()
        {
            return listaSerie;
        }

        public int ProximoId()
        {
            return listaSerie.Count;
        }

        //FUNÇÃO PRA RETORNAR O NOME DA SÉRIE
        public string RetornaNome(int id)
        {
            return listaSerie[id].retornaTitulo();
        }

        public Serie RetornaPorId(int id)
        {
            return listaSerie[id];
        }
         //FUNÇÃO PRA RETORNAR O TOTAL DE SÉRIES SALVAS
        public int RetornaCount()
        {
            return listaSerie.Count;
        }

    }
}
