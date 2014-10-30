using System.Threading.Tasks;
using Autofac;
using BouvetCodeCamp.Domene;
using BouvetCodeCamp.Domene.Entiteter;

using FizzWare.NBuilder;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BouvetCodeCamp.Integrasjonstester.DataAksess
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Threading;

    using Bouvet.BouvetBattleRoyale.Applikasjon.Owin;
    using Bouvet.BouvetBattleRoyale.Domene;
    using Bouvet.BouvetBattleRoyale.Domene.Entiteter;
    using Bouvet.BouvetBattleRoyale.Tjenester.Interfaces;

    using Microsoft.Azure.Documents;

    using Should;

    [TestClass]
    public class LagRepositoryIntegrasjonstester : BaseRepositoryIntegrasjonstest
    {
        [TestMethod]
        [TestCategory(Testkategorier.DataAksess)]
        public async Task Hent_LagMed10Poeng_LagHar10Poeng()
        {
            // Arrange
            var repository = Resolve<IRepository<Lag>>();

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
            var repository = Resolve<IRepository<Lag>>();

            var post = new LagPost { PostTilstand = PostTilstand.Oppdaget, Kode = "a", Posisjon = new Koordinat("10", "90") };
            var poster = new List<LagPost>
                            {
                                post
                            };

            var melding = Builder<Lag>.CreateNew()
                .With(o => o.Poster = poster)
                .Build();

            var documentId = await repository.Opprett(melding);

            // Act
            var lagretLag = repository.Hent(documentId);

            // Assert
            lagretLag.Poster.FirstOrDefault().PostTilstand.ShouldEqual(post.PostTilstand);
            lagretLag.Poster.FirstOrDefault().Kode.ShouldEqual(post.Kode);
            lagretLag.Poster.FirstOrDefault().Posisjon.Latitude.ShouldEqual(post.Posisjon.Latitude);
            lagretLag.Poster.FirstOrDefault().Posisjon.Longitude.ShouldEqual(post.Posisjon.Longitude);
        }

        [TestMethod]
        [TestCategory(Testkategorier.DataAksess)]
        public async Task Hent_LagMedLagId_LagHarLagId()
        {
            // Arrange
            var repository = Resolve<IRepository<Lag>>();

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
            var repository = Resolve<IRepository<Lag>>();

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
            var repository = Resolve<IRepository<Lag>>();
            
            var pifPosisjon = new PifPosisjon {
                                      LagId = "abc",
                                      Posisjon = new Koordinat
                                      {
                                          Latitude = "12.12",
                                          Longitude = "10.123121"
                                      },
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
            lagretLag.PifPosisjoner.FirstOrDefault().Posisjon.Latitude.ShouldEqual(pifPosisjon.Posisjon.Latitude);
            lagretLag.PifPosisjoner.FirstOrDefault().Posisjon.Longitude.ShouldEqual(pifPosisjon.Posisjon.Longitude);
            lagretLag.PifPosisjoner.FirstOrDefault().Tid.ShouldEqual(pifPosisjon.Tid);
        }

        [TestMethod]
        [TestCategory(Testkategorier.DataAksess)]
        public async Task Oppdater_LagMedFlerePoeng_LagHarFlerePoeng()
        {
            // Arrange
            var repository = Resolve<IRepository<Lag>>();

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
        public async Task Slett_HarFlereLagSomSlettes_GirIngenLag()
        {
            // Arrange
            var repository = Resolve<IRepository<Lag>>();

            var lagSomSkalOpprettes = Builder<Lag>.CreateListOfSize(5).All().Build();

            foreach (var lag in lagSomSkalOpprettes)
            {
                await repository.Opprett(lag);
            }

            // Act
            var alleLagForSletting = repository.HentAlle();

            foreach (var lag in alleLagForSletting)
            {
                await repository.Slett(lag);
            }

            // Assert
            var ingenLag = repository.HentAlle();

            ingenLag.ShouldBeEmpty();
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), AllowDerivedTypes = true)]
        public async Task VerifiserOptimisticConcurrency_OppdatererLagetUten�HenteNyF�rst_KasterConcurrencyException()
        {
            // Arrange
            var repository = Resolve<IRepository<Lag>>();

            var lag = Builder<Lag>.CreateNew()
                .Build();

            var documentId = await repository.Opprett(lag);

            // Hent ut to instanser av lag
            var lagMedMelding = repository.Hent(documentId);
            var lagMedPosisjon = repository.Hent(documentId);

            var pifPosisjon = new PifPosisjon
            {
                Posisjon = new Koordinat { Latitude = "59.10", Longitude = "10.6" },
                LagId = "TestlagID",
                Tid = DateTime.Now,
                Infisert = false
            };

            lagMedPosisjon.PifPosisjoner.Add(pifPosisjon);

            var melding = new Melding
            {
                LagId = "TestlagID",
                Tekst = "1",
                Tid = DateTime.Now,
                Type = MeldingType.Lengde
            };

            lagMedMelding.Meldinger.Add(melding);

            // Act

            // Lagre melding
            await repository.Oppdater(lagMedMelding);
            
            // Vent godt for � sikre seg at alt er p� plass
            Thread.Sleep(5000);

            var lagEtterMeldingsLagring = repository.Hent(documentId);

            // Assert
            Assert.AreEqual(1, lagEtterMeldingsLagring.Meldinger.Count, "Meldingen skulle blitt lagret.");

            // Lagrer s� posisjonen
            await repository.Oppdater(lagMedPosisjon);

            Thread.Sleep(5000);

            var lagEtterPosisjonsLagring = repository.Hent(documentId);

            Assert.AreEqual(1, lagEtterPosisjonsLagring.PifPosisjoner.Count, "Posisjonen skulle blitt lagret.");

            //MEN:
            Assert.AreEqual(1, lagEtterPosisjonsLagring.Meldinger.Count, "WHAAAT? Meldingen ble slettet :(");
        }

        [TestMethod]
        public async Task VerifiserOptimisticConcurrency_OppdatererLagetVed�HenteNyF�rstOgDeretterOppdatere_GirLagMedMeldingOgPifPosisjon()
        {
            // Arrange
            var repository = Resolve<IRepository<Lag>>();

            var lag = Builder<Lag>.CreateNew()
                .Build();

            var documentId = await repository.Opprett(lag);

            // Hent ut to instanser av lag
            var lagMedMelding = repository.Hent(documentId);
            var lagMedPosisjon = repository.Hent(documentId);

            var pifPosisjon = new PifPosisjon
            {
                Posisjon = new Koordinat { Latitude = "59.10", Longitude = "10.6" },
                LagId = "TestlagID",
                Tid = DateTime.Now,
                Infisert = false
            };

            lagMedPosisjon.PifPosisjoner.Add(pifPosisjon);

            var melding = new Melding
            {
                LagId = "TestlagID",
                Tekst = "1",
                Tid = DateTime.Now,
                Type = MeldingType.Lengde
            };

            lagMedMelding.Meldinger.Add(melding);

            // Act

            // Lagre melding
            await repository.Oppdater(lagMedMelding);

            // Vent godt for � sikre seg at alt er p� plass
            Thread.Sleep(5000);

            var lagEtterMeldingsLagring = repository.Hent(documentId);

            // Assert
            Assert.AreEqual(1, lagEtterMeldingsLagring.Meldinger.Count, "Meldingen skulle blitt lagret.");

            // Hent ny versjon av laget f�r oppdatering. Lagrer s� posisjonen
            var lagSomErNyligHentet = repository.Hent(documentId);
            lagSomErNyligHentet.PifPosisjoner = lagMedPosisjon.PifPosisjoner;

            await repository.Oppdater(lagSomErNyligHentet);

            Thread.Sleep(5000);

            var lagEtterPosisjonsLagring = repository.Hent(documentId);

            Assert.AreEqual(1, lagEtterPosisjonsLagring.PifPosisjoner.Count, "Posisjonen skulle blitt lagret.");

            //MEN:
            Assert.AreEqual(1, lagEtterPosisjonsLagring.Meldinger.Count, "WHAAAT? Meldingen ble slettet :(");
        }
    }
}