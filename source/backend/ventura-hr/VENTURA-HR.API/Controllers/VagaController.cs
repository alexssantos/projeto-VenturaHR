﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using VENTURA_HR.Services.Dtos.Requests;
using VENTURA_HR.Services.VagaServices;

namespace VENTURA_HR.API.Controllers
{
	[Route("api/vaga")]
	[ApiController]
	[Authorize(Roles = "EMPRESA")]
	public class VagaController : ControllerBase
	{
		private IVagaService VagaService { get; set; }

		public VagaController(IVagaService vagaService)
		{
			VagaService = vagaService;
		}

		[HttpGet]
		public ActionResult PegarTodas()
		{
			var result = VagaService.PegarTodosComInclusos();
			return Ok(result);
		}

		[HttpGet("{vagaId}")]
		public ActionResult PegarVaga(Guid vagaId)
		{
			//não retorna empresa agregada, somente empresaId.
			var result = VagaService.Pegar(vagaId);
			return Ok(result);
		}

		[HttpPost]
		public ActionResult Post([FromBody] CadastroVagaRequest vagaNova)
		{
			if (ModelState.IsValid)
			{
				var vaga = VagaService.CadastrarVaga(vagaNova);

				return Ok(new
				{
					message = "Vaga criada com sucesso.",
					id = vaga.Id
				});
			}
			return BadRequest(vagaNova);
		}

		[HttpGet]
		[Route("criterios/{empresaId}")]
		public ActionResult PegarCriteriosEPesos(Guid empresaId)
		{
			var result = VagaService.PegarCriteriosEPesos(empresaId);
			return Ok(new
			{
				criterios = result.Item1,
				pesos = result.Item2
			});
		}

		//// PUT api/<VagasController>/5
		//[HttpPut("{id}")]
		//public void Put(int id, [FromBody] string value)
		//{
		//}

		//// DELETE api/<VagasController>/5
		//[HttpDelete("{id}")]
		//public void Delete(int id)
		//{
		//}
	}
}
