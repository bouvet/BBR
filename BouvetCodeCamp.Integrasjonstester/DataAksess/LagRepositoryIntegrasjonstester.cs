using System.Threading.Tasks;

using BouvetCodeCamp.Domene;
using BouvetCodeCamp.Domene.Entiteter;

using FizzWare.NBuilder;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BouvetCodeCamp.Integrasjonstester.DataAksess
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using BouvetCodeCamp.Infrastruktur.DataAksess;
    using BouvetCodeCamp.Infrastruktur.DataAksess.Repositories;

    using Should;

    [TestClass]
    public class LagRepositoryIntegrasjonstester : BaseRepositoryIntegrasjonstest
    {
        [TestMethod]
        [TestCategory(Testkategorier.DataAksess)]
        public async Task Hent_LagMed10Poeng_LagHar10Poeng()
        {
            // Arrange
            var repository = OpprettRepository();

            const int Poeng = 10;

            var melding = Builder<Lag>.CreateNew()
                .With(o => o.Poeng = Poeng)
                .Build();

            var documentId = await repository.Opprett(melding);

            // Act
            var lagretLag = repository.Hent(documentId);

            // Assert
            lagretLag.Poeng.ShouldEqual(Poeng);
        }

        [TestMethod]
        [TestCategory(Testkategorier.DataAksess)]
        public async Task Hent_LagMedKode_LagHarKode()
        {
            // Arrange
            var repository = OpprettRepository();

            var kode = new Kode { PosisjonTilstand = PosisjonTilstand.Oppdaget, Bokstav = "a", Posisjon = new Coordinate("10", "90") };
            var koder = new List<Kode>
                            {
                                kode
                            };

            var melding = Builder<Lag>.CreateNew()
                .With(o => o.Koder = koder)
                .Build();

            var documentId = await repository.Opprett(melding);

            // Act
            var lagretLag = repository.Hent(documentId);

            // Assert
            lagretLag.Koder.FirstOrDefault().PosisjonTilstand.ShouldEqual(kode.PosisjonTilstand);
            lagretLag.Koder.FirstOrDefault().Bokstav.ShouldEqual(kode.Bokstav);
            lagretLag.Koder.FirstOrDefault().Posisjon.Latitude.ShouldEqual(kode.Posisjon.Latitude);
            lagretLag.Koder.FirstOrDefault().Posisjon.Longitude.ShouldEqual(kode.Posisjon.Longitude);
        }

        [TestMethod]
        [TestCategory(Testkategorier.DataAksess)]
        public async Task Hent_LagMedLagId_LagHarLagId()
        {
            // Arrange
            var repository = OpprettRepository();

            const string LagId = "abc";

            var melding = Builder<Lag>.CreateNew()
                .With(o => o.LagId = LagId)
                .Build();

            var documentId = await repository.Opprett(melding);

            // Act
            var lagretLag = repository.Hent(documentId);

            // Assert
            lagretLag.LagId.ShouldEqual(LagId);
        }

        [TestMethod]
        [TestCategory(Testkategorier.DataAksess)]
        public async Task Hent_LagMedMelding_LagHarMelding()
        {
            // Arrange
            var repository = OpprettRepository();

            var melding = new Melding {
                                  LagId = "abc",
                                  Tid = DateTime.Now,
                                  Tekst = "heihei���",
                                  Type = MeldingType.Fritekst
                              };

            var meldinger = new List<Melding> { melding };
            
            var lag = Builder<Lag>.CreateNew()
                .With(o => o.Meldinger = meldinger)
                .Build();

            var documentId = await repository.Opprett(lag);

            // Act
            var lagretLag = repository.Hent(documentId);

            // Assert
            lagretLag.Meldinger.FirstOrDefault().LagId.ShouldEqual(melding.LagId);
            lagretLag.Meldinger.FirstOrDefault().Tekst.ShouldEqual(melding.Tekst);
            lagretLag.Meldinger.FirstOrDefault().Tid.ShouldEqual(melding.Tid);
            lagretLag.Meldinger.FirstOrDefault().Type.ShouldEqual(melding.Type);
        }

        [TestMethod]
        [TestCategory(Testkategorier.DataAksess)]
        public async Task Hent_LagMedPifPosisjoner_LagHarPifPosisjon()
        {
            // Arrange
            var repository = OpprettRepository();
            
            var pifPosisjon = new PifPosisjon {
                                      LagId = "abc",
                                      Latitude = "12.12",
                                      Longitude = "10.123121",
                                      Tid = DateTime.Now
                                  };

            var pifPosisjoner = new List<PifPosisjon> { pifPosisjon };

            var lag = Builder<Lag>.CreateNew()
                .With(o => o.PifPosisjoner = pifPosisjoner)
                .Build();

            var documentId = await repository.Opprett(lag);

            // Act
            var lagretLag = repository.Hent(documentId);

            // Assert
            lagretLag.PifPosisjoner.FirstOrDefault().LagId.ShouldEqual(pifPosisjon.LagId);
            lagretLag.PifPosisjoner.FirstOrDefault().Latitude.ShouldEqual(pifPosisjon.Latitude);
            lagretLag.PifPosisjoner.FirstOrDefault().Longitude.ShouldEqual(pifPosisjon.Longitude);
            lagretLag.PifPosisjoner.FirstOrDefault().Tid.ShouldEqual(pifPosisjon.Tid);
        }

        [TestMethod]
        [TestCategory(Testkategorier.DataAksess)]
        public async Task Oppdater_LagMedFlerePoeng_LagHarFlerePoeng()
        {
            // Arrange
            var repository = OpprettRepository();

            const int poeng = 10;
            const int poeng�kning = 10;

            var lag = Builder<Lag>.CreateNew()
                .With(o => o.Poeng = poeng)
                .Build();

            var documentId = await repository.Opprett(lag);
            var opprettetLag = repository.Hent(documentId);

            // Act
            opprettetLag.Poeng += poeng�kning;
            await repository.Oppdater(opprettetLag);

            // Assert
            var oppdatertLag = repository.Hent(opprettetLag.DocumentId);

            oppdatertLag.Poeng.ShouldEqual(20);
        }

        [TestMethod]
        [TestCategory(Testkategorier.DataAksess)]
        public async Task SlettAlle_HarEttLag_HarIngenLag()
        {
            // Arrange
            var repository = OpprettRepository();
            
            var lag = Builder<Lag>.CreateNew()
                .Build();

            await repository.Opprett(lag);
            
            // Act
            await repository.SlettAlle();

            // Assert
            var alleLag = repository.HentAlle();

            alleLag.ShouldBeEmpty();
        }

        private LagRepository OpprettRepository()
        {
            return new LagRepository(new Konfigurasjon(), new DocumentDbContext(new Konfigurasjon()));
        }
    }
}