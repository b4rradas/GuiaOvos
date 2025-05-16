using System;
using System.Threading;

// ======== CLASSE ABSTRATA (ABSTRAÇÃO) =========
// Classe que outras classes irão herdar os comportamentos e propriedades
abstract class ReceitaDeOvo {
    // ======== ENCAPSULAMENTO =========
    // Cada receita implementa suas próprias versões dessas propriedades.
    public abstract string Nome { get; }
    public abstract string OndeFazer { get; }
    public abstract string Temperatura { get; }
    public abstract int TempoPreparo { get; } // em segundos
    public abstract string Dicas { get; }

    // Método base que pode ser sobrescrito - POLIMORFISMO
    public virtual void MostrarReceita() {
        Console.Clear(); // -limpa terminal
        Console.WriteLine($"=== Receita: {Nome} ===\n"); //-comando para escrever no terminal
        Console.WriteLine($"📍 Onde fazer: {OndeFazer}");
        Console.WriteLine($"🌡️ Temperatura: {Temperatura}");
        Console.WriteLine($"⏱️ Tempo de preparo: {TempoPreparo / 60:D2}:{TempoPreparo % 60:D2}");
        Console.WriteLine($"✨ Dicas: {Dicas}\n");

        Console.WriteLine("Pressione ENTER para iniciar o timer ou ESC para voltar...");
        var key = Console.ReadKey(true); // -lê tecla pressionada sem mostrar no terminal
        if (key.Key == ConsoleKey.Enter)
            IniciarTemporizador();
    }

    // Método de contagem regressiva do Temporizador, com animação de carregamento e controle para pausar, retomar e sair
    protected virtual void IniciarTemporizador() {
        char[] spinner = { '|', '/', '-', '\\' }; // caracteres para a animação de carregamento
        int s = 0;
        int tempoRestante = TempoPreparo;
        bool pausado = false;

        while (tempoRestante > 0) {
            if (Console.KeyAvailable) { // Verifica se alguma tecla foi pressionada
                var key = Console.ReadKey(true).Key;

                if (key == ConsoleKey.P) {
                    pausado = true;
                    Console.WriteLine("\n⏸️ Temporizador pausado. Pressione 'R' para retomar ou 'S' para sair:");
                }
                else if (key == ConsoleKey.R && pausado) {
                    pausado = false;
                    Console.WriteLine("\n▶️ Temporizador retomado.");
                }
                else if (key == ConsoleKey.S || key == ConsoleKey.Escape) {
                    Console.WriteLine("\n❌ Temporizador cancelado.");
                    Thread.Sleep(1500); // -adiciona um tempo de espera para realizar a proxima linha de comando
                    return;
                }
            }

            if (!pausado) {
                string tempoFormatado = $"{tempoRestante / 60:D2}:{tempoRestante % 60:D2}"; //Tempo em segundos formatado para minutos
                Console.Clear();
                Console.WriteLine($"{spinner[s]} Cozinhando -=-> Tempo restante: {tempoFormatado}");
                Console.WriteLine("\nPressione: \n'P' - Pausar\n'R' - Retomar\n'S' - Sair.");
                Thread.Sleep(1000); // Espera de 1 segundo para o temporizador ficar certo
                tempoRestante--;
                s = (s + 1) % spinner.Length; // troca o símbolo da animação
            } else {
                Thread.Sleep(200); // tempo de espera para evitar sobrecarregar CPU quando pausado
            }
        }

        Console.WriteLine("\n✅ Pronto! Pode servir.");
        Console.WriteLine("Pressione qualquer tecla para voltar...");
        Console.ReadKey();
    }
}

// ======== HERANÇA =========
// classes filhas que herdam de ReceitaDeOvo, implementando seus respectivos detalhes

class OvoFrito : ReceitaDeOvo {
    public override string Nome => "Ovo Frito";
    public override string OndeFazer => "Frigideira";
    public override string Temperatura => "Fogo médio";
    public override int TempoPreparo => 120; // 2 minutos
    public override string Dicas => "Use manteiga ou óleo na frigideira, frite até a clara firmar e vire o ovo com a ajuda de uma espatula.";
}

class OvoMexido : ReceitaDeOvo {
    public override string Nome => "Ovo Mexido";
    public override string OndeFazer => "Frigideira";
    public override string Temperatura => "Fogo baixo";
    public override int TempoPreparo => 240; // 4 minutos
    public override string Dicas => "Mexa constantemente para manter cremoso.";
}

// ======== CLASSE DERIVADA ESPECIFICA =========
// crição de uma classse base OvoCozidoBase para adicionar diferentes opções de gema a ser escolhidas

abstract class OvoCozidoBase : ReceitaDeOvo {
    public abstract string TipoGema { get; }

    public override string Nome => $"Ovo Cozido - Gema {TipoGema}";
    public override string OndeFazer => "Panela com água";
    public override string Temperatura => "Água fervente";
    public override string Dicas => "Coloque os ovos com cuidado e acompanhe o tempo.";
}

class OvoMole : OvoCozidoBase {
    public override string TipoGema => "Mole";
    public override int TempoPreparo => 240; // 4 minutos
}

class OvoMedio : OvoCozidoBase {
    public override string TipoGema => "Média";
    public override int TempoPreparo => 360; // 6 minutos
}

class OvoDuro : OvoCozidoBase {
    public override string TipoGema => "Dura";
    public override int TempoPreparo => 480; // 8 minutos
}

// Classe para exibe as opções de gema para o ovo cozido
class OvoCozido : ReceitaDeOvo {
    public override string Nome => "Ovo Cozido";
    public override string OndeFazer => "";
    public override string Temperatura => "";
    public override int TempoPreparo => 0;
    public override string Dicas => "";

    public override void MostrarReceita() {
        while (true) {
            Console.Clear();
            Console.WriteLine("=== Preparo de Ovo Cozido ===\n");
            Console.WriteLine("Escolha o ponto da gema:");
            Console.WriteLine("1 - Gema Mole (4 min)");
            Console.WriteLine("2 - Gema Média (6 min)");
            Console.WriteLine("3 - Gema Dura (8 min)");
            Console.WriteLine("0 - Voltar");

            Console.Write("\nDigite sua opção: ");
            string op = Console.ReadLine(); // -lê o que usuario escreve no terminal

            ReceitaDeOvo receita = op switch { // -switch case para verificar qual tipo de gema é requirido
                "1" => new OvoMole(),
                "2" => new OvoMedio(),
                "3" => new OvoDuro(),
                "0" => null,
                _ => null
            };

            if (receita == null) return;
            receita.MostrarReceita();
        }
    }
}

// ======== MENU PRINCIPAL DO TERMINAL =========
class Program {    //C# é uma linguagem totalmente baseada em uma programação orientada a objetos. Tudo precisa estar dentro de uma Classe ou Struct
    static void Main() {
        while (true) {
            Console.Clear();
            Console.WriteLine("=== GUIA DE PREPARO DE OVOS ===\n");
            Console.WriteLine("Escolha o Ovo que deseja preparar:");
            Console.WriteLine("1 - Ovo Cozido");
            Console.WriteLine("2 - Ovo Frito");
            Console.WriteLine("3 - Ovo Mexido");
            Console.WriteLine("0 - Sair");

            Console.Write("\nDigite sua opção: ");
            string op = Console.ReadLine();

            // SWITCH EXPRESSIONS - forma moderna de switch em C#
            ReceitaDeOvo receita = op switch {
                "1" => new OvoCozido(),
                "2" => new OvoFrito(),
                "3" => new OvoMexido(),
                "0" => null,
                _ => null
            };

            if (receita == null) {
                Console.WriteLine("\nSaindo...");
                break;
            }

            receita.MostrarReceita(); // cada classe possui uma forma diferente - POLIMORFISMO
        }
    }
}
