﻿using System.Web.Http;
using BouvetCodeCamp.Domene.Entiteter;
using BouvetCodeCamp.DomeneTjenester.Interfaces;

namespace BouvetCodeCamp.AdminApi
{
    [RoutePrefix("api/infisert")]
    [Authorize]
    public class InfisertController : ApiController
    {
        private readonly IGameStateService _gameStateService;

        public InfisertController(IGameStateService gameStateService)
        {
            _gameStateService = gameStateService;
        }

        // GET api/infisert/get
        [HttpGet]
        public InfisertPolygon Get()
        {
            var gameState = _gameStateService.HentGameState();
            return gameState.InfisertPolygon;
        }

        // POST api/infisert/post
        [HttpPost]
        public void Post([FromBody] InfisertPolygon model)
        {
            var gameState = _gameStateService.HentGameState();
            gameState.InfisertPolygon = model;

            _gameStateService.OppdaterGameState(gameState);
        }
    }
}