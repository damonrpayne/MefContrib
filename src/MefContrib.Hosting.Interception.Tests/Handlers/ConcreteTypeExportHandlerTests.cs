﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.ComponentModel.Composition.ReflectionModel;
using System.Linq;
using MefContrib.Hosting.Interception.Handlers;
using MefContrib.Tests;
using NUnit.Framework;

namespace MefContrib.Hosting.Interception.Tests.Handlers
{
    namespace ConcreteTypeExportHandlerTests
    {
        [TestFixture]
        public class When_querying_using_a_concrete_order_repository : ConcreteTypeExportHandlerContext
        {
            [Test]
            public void Order_repository_part_is_created()
            {
                partType.ShouldBeOfType<OrderRepository>();
            }

            public override void Context()
            {
                var exports = new List<Tuple<ComposablePartDefinition, ExportDefinition>>();
                var export = ConcreteTypeHandler.GetExports(RepositoryImportDefinition, exports).FirstOrDefault();
                partType = ReflectionModelServices.GetPartType(export.Item1).Value;
            }

            private Type partType;

        }

        public class ConcreteTypeExportHandlerContext
        {
            public ConcreteTypeExportHandler ConcreteTypeHandler;
            public ImportDefinition RepositoryImportDefinition;

            public ConcreteTypeExportHandlerContext()
            {
                ConcreteTypeHandler = new ConcreteTypeExportHandler();
                var typeCatalog = new TypeCatalog(typeof(OrderProcessor));
                var orderProcessorContract = AttributedModelServices.GetContractName(typeof(OrderProcessor));
                var orderProcessPartDefinition = typeCatalog.Parts.Single(p => p.ExportDefinitions.Any(d => d.ContractName == orderProcessorContract));
                RepositoryImportDefinition = orderProcessPartDefinition.ImportDefinitions.First();
                Context();
            }

            public virtual void Context()
            {
            }
        }

        [Export]
        public class OrderRepository
        {
        }

        [Export]
        public class OrderProcessor
        {
            [Import]
            OrderRepository OrderRepository { get; set; }
        }
    }
}
