﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VENTURA_HR.DOMAIN.VagaAggregate.Entities;

namespace VENTURA_HR.Repository.MapEntities
{
	public class CriterioMap : IEntityTypeConfiguration<Criterio>
	{
		public void Configure(EntityTypeBuilder<Criterio> entity)
		{
			entity.ToTable("Criterio");

			entity.HasKey(x => x.Id);
			entity.Property(x => x.Id)
				.HasColumnName("id_criterio")
				.ValueGeneratedNever()
				.IsRequired();

			entity.Property(x => x.Titulo)
				.HasColumnName("str_titulo")
				.IsRequired();

			entity.Property(x => x.Descricao)
				.HasColumnName("str_desc");


			entity.Property(x => x.Ativo)
				.HasColumnName("bl_ativo")
				.IsRequired();

			entity.Property(x => x.Peso)
				.HasColumnName("int_peso")
				.IsRequired();

			entity.Property(x => x.PMD)
				.HasColumnName("bl_pmd")
				.IsRequired();

			entity.Property(x => x.VagaId)
				.HasColumnName("id_vagaid")
				.IsRequired();


			// ==== aditamento =====

			entity.Property(x => x.DataCriacao)
				.HasColumnName("dt_criacao")
				.IsRequired();

			entity.Property(x => x.DataUltimaAtualizacao)
				.HasColumnName("dt_atualizacao");


			// ==== relatoinsiops =====

			entity.HasMany(x => x.RespostaCriterios)
					.WithOne(x => x.Criterio)
					.HasForeignKey(x => x.CriterioId)
					.OnDelete(DeleteBehavior.Cascade);

			// ==== Keys =====

			//entity.HasIndex(p => new { p.Peso, p.RespostaCriterios, p.Titulo }).IsUnique();
		}
	}
}
