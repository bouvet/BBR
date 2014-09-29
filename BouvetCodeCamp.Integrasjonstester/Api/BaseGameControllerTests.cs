namespace BouvetCodeCamp.Integrasjonstester.Api
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;

    using BouvetCodeCamp.Domene;
    using BouvetCodeCamp.Domene.Entiteter;
    using BouvetCodeCamp.Domene.InputModels;
    using BouvetCodeCamp.Domene.OutputModels;

    using FizzWare.NBuilder;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Newtonsoft.Json;

    using Should;

    [TestClass]
    public class BaseGameControllerTests : ApiTest
    {
        [TestMethod]
        [TestCategory(Testkategorier.Api)]
        public async Task HentPifPosisjon_GyldigModell_F�rHttpStatusKodeOk()
        {
            // Arrange
            this.S�rgForAtEtLagMedEnPifPosisjonFinnes();

            const string ApiEndPointAddress = ApiBaseAddress + "/api/game/base/hentpifposisjon/" + LagId;

            bool isSuccessStatusCode;

            // Act
            using (var httpClient = new HttpClient())
            {
                var httpResponseMessage = await httpClient.GetAsync(ApiEndPointAddress);

                isSuccessStatusCode = httpResponseMessage.IsSuccessStatusCode;
            }

            // Assert
            isSuccessStatusCode.ShouldBeTrue();
        }

        [TestMethod]
        [TestCategory(Testkategorier.Api)]
        public async Task HentPifPosisjon_LagetHarEnPifPosisjon_F�rPifPosisjon()
        {
            // Arrange
            this.S�rgForAtEtLagMedEnPifPosisjonFinnes();

            const string ApiEndPointAddress = ApiBaseAddress + "/api/game/base/hentpifposisjon/" + LagId;

            PifPosisjonModel pifPosisjon;

            // Act
            using (var httpClient = new HttpClient())
            {
                var httpResponseMessage = await httpClient.GetAsync(ApiEndPointAddress);
                var content = await httpResponseMessage.Content.ReadAsStringAsync();

                pifPosisjon = JsonConvert.DeserializeObject<PifPosisjonModel>(content);
            }

            // Assert
            pifPosisjon.LagId.ShouldNotBeEmpty();
            pifPosisjon.Latitude.ShouldNotBeEmpty();
            pifPosisjon.Longitude.ShouldNotBeEmpty();
            pifPosisjon.Tid.ShouldNotEqual(null);
        }

        [TestMethod]
        [TestCategory(Testkategorier.Api)]
        public async Task HentPifPosisjon_LagetHarIngenPifPosisjoner_F�rTomPifPosisjon()
        {
            // Arrange
            this.S�rgForAtEtLagMedUtenPifPosisjonFinnes();

            const string ApiEndPointAddress = ApiBaseAddress + "/api/game/base/hentpifposisjon/" + LagId;

            PifPosisjonModel pifPosisjon;

            // Act
            using (var httpClient = new HttpClient())
            {
                var httpResponseMessage = await httpClient.GetAsync(ApiEndPointAddress);
                var content = await httpResponseMessage.Content.ReadAsStringAsync();

                pifPosisjon = JsonConvert.DeserializeObject<PifPosisjonModel>(content);
            }

            // Assert
            pifPosisjon.LagId.ShouldEqual(null);
            pifPosisjon.Latitude.ShouldEqual(null);
            pifPosisjon.Longitude.ShouldEqual(null);
            pifPosisjon.Tid.ShouldEqual(null);
        }

        [TestMethod]
        [TestCategory(Testkategorier.Api)]
        public async Task SendPifMelding_GyldigModell_F�rHttpStatusCodeCreated()
        {
            // Arrange
            this.S�rgForAtEtLagFinnes();

            const string ApiEndPointAddress = ApiBaseAddress + "/api/game/base/sendpifmelding";
            bool isSuccessStatusCode;
            HttpStatusCode responseCode;

            // Act
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var modell = new MeldingModel
                {
                    LagId = LagId,
                    Tekst = "Heihei",
                    Type = MeldingType.Fritekst
                };

                var modellSomJson = JsonConvert.SerializeObject(modell);

                var httpResponseMessage = await httpClient.PostAsync(
                    ApiEndPointAddress,
                    new StringContent(modellSomJson, Encoding.UTF8, "application/json"));

                isSuccessStatusCode = httpResponseMessage.IsSuccessStatusCode;
                responseCode = httpResponseMessage.StatusCode;
            }

            // Assert
            isSuccessStatusCode.ShouldBeTrue();
            responseCode.ShouldEqual(HttpStatusCode.Created);
        }

        [TestMethod]
        [TestCategory(Testkategorier.Api)]
        public async Task SendPifMelding_UgyldigModell_F�rHttpStatusCodeBadRequest()
        {
            // Arrange
            this.S�rgForAtEtLagFinnes();

            const string ApiEndPointAddress = ApiBaseAddress + "/api/game/base/sendpifmelding";
            bool isSuccessStatusCode;
            HttpStatusCode responseCode;

            // Act
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                
                var modellSomJson = string.Empty;

                var httpResponseMessage = await httpClient.PostAsync(
                    ApiEndPointAddress,
                    new StringContent(modellSomJson, Encoding.UTF8, "application/json"));

                isSuccessStatusCode = httpResponseMessage.IsSuccessStatusCode;
                responseCode = httpResponseMessage.StatusCode;
            }

            // Assert
            isSuccessStatusCode.ShouldBeFalse();
            responseCode.ShouldEqual(HttpStatusCode.BadRequest);
        }

        [TestMethod]
        [TestCategory(Testkategorier.Api)]
        public async Task HentRegistrerteKoder_LagHarRegistrerteKoder_F�rKoder()
        {
            // Arrange
            this.S�rgForAtEtLagMedRegistrerteKoderFinnes();

            const string ApiEndPointAddress = ApiBaseAddress + "/api/game/base/hentregistrertekoder/" + LagId;

            IEnumerable<KodeOutputModel> kodeModeller;

            // Act
            using (var httpClient = new HttpClient())
            {
                var httpResponseMessage = await httpClient.GetAsync(ApiEndPointAddress);
                var content = await httpResponseMessage.Content.ReadAsStringAsync();

                kodeModeller = JsonConvert.DeserializeObject<IEnumerable<KodeOutputModel>>(content);
            }

            // Assert
            kodeModeller.ShouldNotBeEmpty();
        }

        [TestMethod]
        [TestCategory(Testkategorier.Api)]
        public async Task HentRegistrerteKoder_LagHarIngenRegistrerteKoder_F�rTomt()
        {
            // Arrange
            this.S�rgForAtEtLagFinnes();

            const string ApiEndPointAddress = ApiBaseAddress + "/api/game/base/hentregistrertekoder/" + LagId;

            IEnumerable<KodeOutputModel> kodeModeller;

            // Act
            using (var httpClient = new HttpClient())
            {
                var httpResponseMessage = await httpClient.GetAsync(ApiEndPointAddress);
                var content = await httpResponseMessage.Content.ReadAsStringAsync();

                kodeModeller = JsonConvert.DeserializeObject<IEnumerable<KodeOutputModel>>(content);
            }

            // Assert
            kodeModeller.ShouldBeEmpty();
        }

        private void S�rgForAtEtLagMedEnPifPosisjonFinnes()
        {
            var pifPosisjoner = new List<PifPosisjon>
                                    {
                                                        new PifPosisjon {
                                                            LagId = LagId,
                                                            Latitude = "12.1",
                                                            Longitude = "12.1",
                                                            Tid = DateTime.Now
                                                        }
                                    };

            var lag = Builder<Lag>.CreateNew()
                .With(o => o.LagId = LagId)
                .With(o => o.PifPosisjoner = pifPosisjoner)
                .Build();

            var lagOpprettet = this.OpprettLagViaApi(lag).Result;

            if (!lagOpprettet)
                Assert.Fail();
        }

        private void S�rgForAtEtLagMedUtenPifPosisjonFinnes()
        {
            var pifPosisjoner = new List<PifPosisjon>();

            var lag = Builder<Lag>.CreateNew()
                .With(o => o.LagId = LagId)
                .With(o => o.PifPosisjoner = pifPosisjoner)
                .Build();

            var lagOpprettet = this.OpprettLagViaApi(lag).Result;

            if (!lagOpprettet)
                Assert.Fail();
        }

        private void S�rgForAtEtLagMedRegistrerteKoderFinnes()
        {
            var registrerteKoder = new List<LagPost>
                                       {
                                           new LagPost()
                                               {
                                                   Kode = "akje",
                                                   Posisjon = new Koordinat("12", "12"),
                                                   PostTilstand = PostTilstand.Oppdaget
                                               }
                                       };

            var lag = Builder<Lag>.CreateNew()
                .With(o => o.LagId = LagId)
                .With(o => o.Poster = registrerteKoder)
                .Build();

            var lagOpprettet = this.OpprettLagViaApi(lag).Result;

            if (!lagOpprettet)
                Assert.Fail();
        }
    }
}