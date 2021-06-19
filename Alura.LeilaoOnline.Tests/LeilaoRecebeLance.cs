using Xunit;
using Alura.LeilaoOnline.Core;
using System.Linq;

namespace Alura.LeilaoOnline.Tests
{
    public class LeilaoRecebeLance
    {
        [Theory]
        [InlineData(4, new double[] { 1000, 900, 1200, 1300 })]
        [InlineData(2, new double[] { 800, 900 })]
        public void NaoPermiteNovosLancesDadoLeilaoFinalizado(int valorEsperado, double[] ofertas)
        {
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

            leilao.TerminaPregao();

            //Act
            leilao.RecebeLance(fulano, 1000);

            //Assert
            var valorObtido = leilao.Lances.Count();

            Assert.Equal(valorEsperado, valorObtido);
        }

        [Theory]
        [InlineData(new double[] { 200, 300, 400, 500 })]
        [InlineData(new double[] { 200 })]
        [InlineData(new double[] { 200, 300, 400 })]
        [InlineData(new double[] { 200, 300, 400, 500, 600, 700 })]
        public void QtdPermaneceZeroDadoQuePregaoNaoFoiIniciado(double[] ofertas)
        {
            var modalidade = new MaiorValor();
            var leilao = new Leilao("Van Gogh", modalidade);
            var fulano = new Interessada("Fulano", leilao);

            foreach(var valor in ofertas)
            {
                leilao.RecebeLance(fulano, valor);
            }

            Assert.Empty(leilao.Lances);

        }

        [Fact]
        public void NaoAceitaProximoLanceDadoMesmoClienteRealizouUltimoLance()
        {
            var modalidade = new MaiorValor();
            var leilao = new Leilao("Van Gogh", modalidade);
            var fulano = new Interessada("Fulano", leilao);

            leilao.IniciaPregao();
            leilao.RecebeLance(fulano, 800);

            //Act
            leilao.RecebeLance(fulano, 1000);

            //Assert
            var valorEsperado = 1;
            var valorObtido = leilao.Lances.Count();
            Assert.Equal(valorEsperado, valorObtido);
        }
    }
}
