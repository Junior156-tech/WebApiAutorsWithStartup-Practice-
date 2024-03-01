using System.ComponentModel.DataAnnotations;
using WebApiAutoresConStartup.Validations;

namespace WebApiAutoresConStartup.Test.PruebasUnitarias
{
    [TestClass]
    public class FirstLetterAttributeTests
    {
        [TestMethod]
        public void PrimeraLetraMinuscula_NoDevuelveError()
        {

            //Preparacion
            var primeraLetraMayuscula = new FirstLetterAttribute();
            var valor = "junior";
            var valContext = new ValidationContext(new { Nombre = valor });

            //Ejecucion
            var resultado = primeraLetraMayuscula.GetValidationResult(valor, valContext);

            //Verificacion
            Assert.AreEqual("Ta malo", resultado.ErrorMessage);
        }

        [TestMethod]
        public void ValorNulo_NoDevuelveError()
        {

            //Preparacion
            var primeraLetraMayuscula = new FirstLetterAttribute();
            string valor = null;
            var valContext = new ValidationContext(new { Nombre = valor });

            //Ejecucion
            var resultado = primeraLetraMayuscula.GetValidationResult(valor, valContext);

            //Verificacion
            Assert.IsNull(resultado);
        }


        [TestMethod]
        public void ValorConPrimeraLetraMinuscula_NoDevuelveError()
        {

            //Preparacion
            var primeraLetraMayuscula = new FirstLetterAttribute();
            string valor = null;
            var valContext = new ValidationContext(new { Nombre = valor });

            //Ejecucion
            var resultado = primeraLetraMayuscula.GetValidationResult(valor, valContext);

            //Verificacion
            Assert.IsNull(resultado);
        }
    }
}