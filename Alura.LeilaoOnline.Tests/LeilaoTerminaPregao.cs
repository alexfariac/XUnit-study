using System;
using Alura.LeilaoOnline.Core;
using Xunit;

namespace Alura.LeilaoOnline.Tests
{
    public class LeilaoTerminaPregao
    {
        [Theory]
        [InlineData(1200, 1250, new double[] { 800, 1150, 1400, 1250} )]
        [InlineData(1200, 1250, new double[] { 800, 700, 1100, 1250 })]
        public void RetornaValorSuperiorMaisProximoDadoLeilaoNessaModalidade(double valorDestino, double valorEsperado, double[] ofertas)
        {
            //Arranje - Cenario de entrada
            var modalidade = new OfertaSuperiorMaisProxima(valorDestino);
            var leilao = new Leilao("Van Gogh", modalidade);
            var fulano = new Interessada("Fulano", leilao);
            var maria = new Interessada("Maria", leilao);

            leilao.IniciaPregao();

            for (int i = 0; i < ofertas.Length; i++)
            {
                var lanceDe = i % 2 == 0 ? fulano : maria;
                leilao.RecebeLance(lanceDe, ofertas[i]);
            }

            //Act - metodo sob teste
            leilao.TerminaPregao();

            //Assert - validacoes
            var valorObtido = leilao.Ganhador.Valor;
            Assert.Equal(valorEsperado, valorObtido);
        }

        [Fact]
        public void RetornaZeroDadoLeilaoSemLances()
        {
            //Arranje - Cenario de entrada
            IModalidadeAvaliacao modalidade = new MaiorValor();
            var leilao = new Leilao("Van Gogh", modalidade);

            leilao.IniciaPregao();
            leilao.TerminaPregao();

            //Assert - validacoes
            var valorEsperado = 0;
            var valorObtido = leilao.Ganhador.Valor;
            Assert.Equal(valorEsperado, valorObtido);
        }

        [Fact]
        public void LancaInvalidOperationExceptionDadoPregaoNaoIniciado()
        {
            //Arranje - Cenario de entrada
            var modalidade = new MaiorValor();
            var leilao = new Leilao("Van Gogh", modalidade);

            //Assert
            var excessaoObtida = Assert.Throws<System.InvalidOperationException>(
                //Act
                () => leilao.TerminaPregao()
            );

            var msgEsperada = "Não é possivel finalizar um pregão não iniciado";
            Assert.Equal(msgEsperada, excessaoObtida.Message);

        }

        [Theory]
        [InlineData(1200, new Double[] { 800, 900, 1000, 1200 })]
        [InlineData(1000, new Double[] { 800, 900, 1000, 990 })]
        [InlineData(800, new Double[] { 800 })]
        public void RetornaMaiorValorDadoLeilaoComPeloMenosUmLance(double valorEsperado, double[] ofertas)
        {
            //Arranje - Cenario de entrada
            var modalidade = new MaiorValor();
            var leilao = new Leilao("Van Gogh", modalidade);
            var fulano = new Interessada("Fulano", leilao);
            var maria = new Interessada("Maria", leilao);

            leilao.IniciaPregao();

            for (int i = 0; i < ofertas.Length; i++)
            {
                var lanceDe = i % 2 == 0 ? fulano : maria;
                leilao.RecebeLance(lanceDe, ofertas[i]);
            }

            //Act - metodo sob teste
            leilao.TerminaPregao();

            //Assert - validacoes
            var valorObtido = leilao.Ganhador.Valor;
            Assert.Equal(valorEsperado, valorObtido);
        }
    }
}
