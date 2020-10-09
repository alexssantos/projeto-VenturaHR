﻿using System.Collections.Generic;
using VENTURA_HR.DOMAIN.VagaAggregate.Entities;

namespace VENTURA_HR.DOMAIN.UsuarioAggregate.Entities
{
	public class Empresa : Usuario
	{
		public string CNPJ { get; set; }
		public IList<Vaga> Vagas { get; set; }
	}
}