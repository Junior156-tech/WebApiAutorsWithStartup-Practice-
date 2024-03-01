using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebApiAutoresConStartup.Controllers.V1;
using WebApiAutoresConStartup.Test.Mocks;

namespace WebApiAutoresConStartup.Test.PruebasUnitarias
{
    [TestClass]
    public class RootControllerTests
    {
        [TestMethod]
        public async Task SiUsuarioEsAdmin_Obtener4Links()
        {
            //Preparacion
            var authorizationService = new AuthorizationServiceMock();
            authorizationService.result = AuthorizationResult.Success();
            var rootController =  new RootController(authorizationService);
            rootController.Url = new UrlHelperMock();


            //Ejecucion
            var resultado = await rootController.Get();


            //Verificacion
            Assert.AreEqual(4, resultado.Value.Count());
        }

        [TestMethod]
        public async Task SiUsuarioNoEsAdmin_Obtener2Links()
        {
            //Preparacion
            var authorizationService = new AuthorizationServiceMock();
            authorizationService.result = AuthorizationResult.Failed();

            var rootController = new RootController(authorizationService);
            rootController.Url = new UrlHelperMock();


            //Ejecucion
            var resultado = await rootController.Get();


            //Verificacion
            Assert.AreEqual(2, resultado.Value.Count());
        }

        [TestMethod]
        public async Task SiUsuarioNoEsAdmin_Obtener2LinksConMoq()
        {
            //Preparacion

            var mockAuthorization = new  Mock<IAuthorizationService>();
            mockAuthorization.Setup(x => x.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(),
                                             It.IsAny<object>(), 
                                             It.IsAny<IEnumerable<IAuthorizationRequirement>>()
                                             )).Returns(Task.FromResult(AuthorizationResult.Failed()));

            mockAuthorization.Setup(x => x.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(),
                                          It.IsAny<object>(),
                                          It.IsAny<string>()
                                          )).Returns(Task.FromResult(AuthorizationResult.Failed()));



            var rootController = new RootController(mockAuthorization.Object);

            var mockUrlHelper = new Mock<IUrlHelper>();

            mockUrlHelper.Setup(x => x.Link(It.IsAny<string>(), 
                                            It.IsAny<object>()))
                                            .Returns(string.Empty);

            rootController.Url = mockUrlHelper.Object;


            //Ejecucion
            var resultado = await rootController.Get();


            //Verificacion
            Assert.AreEqual(2, resultado.Value.Count());
        }

    }
}
