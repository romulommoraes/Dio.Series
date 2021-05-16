using Dio.Series.Interfaces;
using System;

namespace Dio.Series
{
    class Program
    {
        static SerieRepositorio repositorio = new SerieRepositorio();

        static void Main(string[] args)
        {
            
            AppInfo(); //informações do app
            AddSeriesTeste(); //duas séries adicionadas para a lista nao ficar vazia

            string opcaoUsuario = ObterOpcaoUsuario();

            while (opcaoUsuario.ToUpper() != "X")
            {
                switch (opcaoUsuario)
                {
                    case "1":
                        ListarSeries();
                        break;
                    case "2":
                        InserirSerie();
                        break;
                    case "3":
                        AtualizarSerie();
                        break;
                    case "4":
                        ExcluirSerie();
                        break;
                    case "5":
                        VisualizarSerie();
                        break;
                    case "C":
                        Console.Clear();
                        break;
                    default:
                        WriteLineCor(ConsoleColor.Red, "Digite uma opção válida");
                        break;
                }
                opcaoUsuario = ObterOpcaoUsuario();
            }

            WriteLineCor(ConsoleColor.Blue, "Obrigado por utilizar nossos serviços");
            Console.ReadLine();
        }


        //------------------- MÉTODOS ------------------------------//

        //LISTA SÉRIES PRESENTES
        private static void ListarSeries() 
        {
            Console.WriteLine("Listar Séries");

            var lista = repositorio.Lista();

            if(lista.Count == 0)
            {
                Console.WriteLine("Nenhuma Série Cadastrada");
                return;
            }

            foreach (var serie in lista)
            {
                var excluido = serie.retornaExcluido();
                string exclusao;
                if (excluido)
                {
                    exclusao = ", *Excluído*";
                }
                else
                {
                    exclusao = "";
                }
                Console.WriteLine($"ID: {serie.retornaId()}, {serie.retornaTitulo()}{exclusao}");
            }
        }


        //INSERE NOVAS SÉRIES
        private static void InserirSerie() 
        {
            WriteLineCor(ConsoleColor.DarkBlue, "Inserir nova série");
            var novaSerie = CarregaSerie(repositorio.ProximoId());
            repositorio.Insere(novaSerie);
        }

        //ATUALIZA AS SÉRIES

        private static void AtualizarSerie() 
        {
            WriteLineCor(ConsoleColor.Yellow, "Atualizar série");

            int indiceSerie = CarregaID();
            var atualizaSerie = CarregaSerie(indiceSerie);
            repositorio.Atualiza(indiceSerie, atualizaSerie);
        }

        //EXCLUI SÉRIES
        private static void ExcluirSerie() //tratar exceções
        {
            int indiceSerie = CarregaID();
            WriteLineCor(ConsoleColor.Yellow, $"Digite S para excluir {repositorio.RetornaNome(indiceSerie)} OU qualquer tecla para sair");
            string confirm = Console.ReadLine();
            if (confirm.ToUpper() == "S")
            {
                repositorio.Exclui(indiceSerie);
                WriteLineCor(ConsoleColor.Red, $"Serie {repositorio.RetornaNome(indiceSerie)} excluida com sucesso");
            }
            else
            {
                return;
            }
        }

        //VISUALIZA AS SÉRIES SALVAS
        private static void VisualizarSerie() 
        {
            int indiceSerie = CarregaID();
            var serie = repositorio.RetornaPorId(indiceSerie);
            WriteLineCor(ConsoleColor.Blue, $"{serie}");
        }

        //LOOP PARA GERAR A LISTA INICIAL
        private static string ObterOpcaoUsuario()
        {
            Console.WriteLine();
            WriteLineCor(ConsoleColor.DarkGreen, "DIO Series a seu dispor!");
            WriteLineCor(ConsoleColor.DarkGreen, "Informe a opção desejada:");

            WriteLineCor(ConsoleColor.DarkGreen, "1 - Listar séries");
            WriteLineCor(ConsoleColor.DarkGreen, "2 - Inserir nova série");
            WriteLineCor(ConsoleColor.DarkGreen, "3 - Atualizar série");
            WriteLineCor(ConsoleColor.DarkGreen, "4 - Excluir série");
            WriteLineCor(ConsoleColor.DarkGreen, "5 - Visualizar série");
            WriteLineCor(ConsoleColor.DarkGreen, "C - Limpar Tela");
            WriteLineCor(ConsoleColor.DarkGreen, "X - Sair");
            Console.WriteLine("");


            string opcaoUsuario = Console.ReadLine().ToUpper();
            Console.WriteLine();
            return opcaoUsuario;
        }

        //CARREGA O ID DAS SÉRIES
        private static int CarregaID() 
        {

            int indiceSerie = SolicitaID();

            while (indiceSerie == -9)
            {
                WriteCor(ConsoleColor.Red, "Digite um número de conta válido ou X para sair");
                indiceSerie = SolicitaID();
            }

            if (indiceSerie == -1) // SAÍDA PELO "X"
            {
                return -1;
            }

            static int SolicitaID() //FUNÇÃO INTERNA QUE SOLICITA O ID AO USUÁRIO
            {
                WriteCor(ConsoleColor.DarkGreen, "Digite o Id da série: ");
                string indiceEntrada = Console.ReadLine();
                return ParseID(indiceEntrada);
            }

            return indiceSerie;
        }

        //CARREGA AS SÉRIES PRA AS FUNÇÕES INSERIR E ATUALIZAR
        private static Serie CarregaSerie(int IndexSerie) 
        {
            //--------------- GENERO -------------//
            foreach (int i in Enum.GetValues(typeof(Genero)))
            {
                Console.WriteLine("{0}-{1}", i, Enum.GetName(typeof(Genero), i));
            }

            string[] enumArr = Enum.GetNames(typeof(Genero));//pega todos os itens do enum e joga num array

            string entradaParseGenero;
            int entradaGenero;
            do
            {
                WriteCor(ConsoleColor.DarkGreen, "Digite o gênero entre as opções acima: ");
                entradaParseGenero = Console.ReadLine();
                int parseresultado;
                bool parsing = int.TryParse(entradaParseGenero, out parseresultado);
                if (parsing)
                {
                    entradaGenero = parseresultado;
                    if (entradaGenero < 0 || entradaGenero > enumArr.Length - 1)
                    {
                        WriteCor(ConsoleColor.Red, "Gênero inválido");
                    }
                }
                else
                {
                    WriteCor(ConsoleColor.Red, "Gênero inválido");
                    entradaGenero = -9;
                }
            } while (entradaGenero < 0 || entradaGenero > enumArr.Length -1 );//se o genero inserido estiver fora do range do enum

            //--------------- TÍTULO -------------//
            WriteCor(ConsoleColor.DarkGreen, "Digite o título da série: ");
            string entradaTitulo = Console.ReadLine();

            //-----------------ANO---------------//
            int entradaAno;
            do
            {
                entradaAno = SolicitaAno();
            } while (entradaAno == -9);


            //-------------DESCRIÇAO-------------//
            WriteCor(ConsoleColor.DarkGreen, "Digite a descrição da Série: ");
            string entradaDescricao = Console.ReadLine();            


            Serie ObjSerie = new Serie(
                                            id: IndexSerie,
                                            genero: (Genero)entradaGenero,
                                            titulo: entradaTitulo,
                                            ano: entradaAno,
                                            descricao: entradaDescricao
                                        );

            return ObjSerie; //RETORNA UM OBJETO DA SÉRIE QUE VAI SER USADO PELAS FUNÇÕES ATUALIZAR E INSERIR... FIZ ISSO PRA EVITAR REPETIÇÃO DE CÓDIGO
        }


        //PROCESSA O ID TRATA ERROS DE ENTRADA
        static int ParseID(string entradaParse)
        {
            if (entradaParse.ToUpper() == "X")
            {
                return -1;
            }
            int parseresultado;

            bool parsing = int.TryParse(entradaParse, out parseresultado);
            if (parsing)
            {
                int indiceSerie = parseresultado;
                if (indiceSerie < 0 || indiceSerie > (repositorio.RetornaCount() - 1))
                {
                    WriteCor(ConsoleColor.Red, "Índice inválido");
                    return -9;
                }
                else
                {
                    return indiceSerie;
                }
            }
            else
            {
                WriteCor(ConsoleColor.Red, "Índice inválido");
                return -9;
            }
        }

        //SOLICITA O ANO DA SÉRIE PRA PERMITIR UM DO-WHILE CASO HAJA UM ERRO DE ENTRADA
        static int SolicitaAno()
        {
            WriteCor(ConsoleColor.DarkGreen, "Digite o Ano de Início da Série: ");
            string ano = Console.ReadLine();

            return ParseANO(ano);
        }


        //PROCESSA O ANO DA SÉRIE TRATA ERROS DE ENTRADA
        static int ParseANO(string entradaParse)
        {
            if (entradaParse.ToUpper() == "X")
            {
                return -1;
            }
            int parseresultado;

            bool parsing = int.TryParse(entradaParse, out parseresultado);
            if (parsing)
            {
                int anoSerie = parseresultado;
                return anoSerie;
            }
            else
            {
                WriteCor(ConsoleColor.Red, "Ano inválido");
                return -9;
            }
        }


        // ---------------------------------FUNÇÕES PRA COLORIR O TEXTO -----------------------------------//
        public static void WriteLineCor(ConsoleColor cor, string mensagem) //colore o texto do WriteLine
        {
            //muda cor do texto
            Console.ForegroundColor = cor;
            Console.WriteLine(mensagem);
            //reseta cor do texto
            Console.ResetColor();
            //pergunta nome do usuário
        }

        public static void WriteCor(ConsoleColor cor, string mensagem) //colore o texto do Write
        {
            //muda cor do texto
            Console.ForegroundColor = cor;
            Console.WriteLine(mensagem);
            //reseta cor do texto
            Console.ResetColor();
            //pergunta nome do usuário
        }

        //----------------------------------------FUNÇÃO INFORMAÇÕES DO APP-------------------------------//
        static void AppInfo()
        {
            string NomeApp = "DIO.Series";
            string versao = "1.0.0";
            string autor = "Romulus";

            // Muda cor do texto
            Console.ForegroundColor = ConsoleColor.Blue;
            // informações sobre o programa
            Console.WriteLine("{0}: Versão {1} por {2}", NomeApp, versao, autor);
            Console.WriteLine();
            //reseta cor do texto
            Console.ResetColor();

        }
        static void AddSeriesTeste()
        {
            Serie californication = new Serie(repositorio.ProximoId(), genero: (Genero)5, "Californication", "Escritor que gosta de encher a cara e seduzir mulheres", 2007);
            repositorio.Insere(californication);
            Serie himym = new Serie(repositorio.ProximoId(), genero: (Genero)3, "How I met your mother", "Ted se ferrando por 9 anos", 2005);
            repositorio.Insere(himym);
        }
    }
}
