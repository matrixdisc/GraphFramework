﻿using System;
using System.Linq;
using NUnit.Framework;

namespace GraphFramework
{
    class GraphUnitTests
    {
        private Graph _graph;
        private Vertex _v1;
        private Vertex _v2;

        [SetUp]
        public void Init()
        {
            _graph = new Graph();
            _v1 = new Vertex();
            _v2 = new Vertex();
        }

        [Test]
        public void EmptyGraphHasNoVertices()
        {
            Assert.AreEqual(0, _graph.vertices.Count);
        }


        [Test]
        public void AddsVertexToGraph()
        {
            _graph.AddVertex(_v1);
            Assert.Contains(_v1, _graph.vertices);
        }

        [Test]
        public void RemovesVertexFromGraph()
        {
            _graph.AddVertex(_v1);
            Assert.IsTrue(_graph.vertices.Contains(_v1));
            _graph.RemoveVertex(_v1);
            Assert.IsFalse(_graph.vertices.Contains(_v1));
        }

        [Test]
        public void RemovingNonexistentVertexThrowsException()
        {
            Assert.Throws<NoVertexException>( () => _graph.RemoveVertex(_v1));
        }

        [Test]
        public void RemovingVertexRemovesInboundArcs()
        {
            _graph.AddVertex(_v1);
            _graph.AddVertex(_v2);
            _v2.AddOutboundArc(_v1);
            _graph.RemoveVertex(_v1);
            Assert.AreEqual(0, _v2.OutboundArcs.Select(arc => arc.Start == _v1 && arc.End == _v1).Count());
        }

        [Test]
        public void RemovingVertexRemovesOutboundArcsToNeighbours()
        {
            _graph.AddVertex(_v1);
            _graph.AddVertex(_v2);
            _v1.AddOutboundArc(_v2);
            _graph.RemoveVertex(_v1);
            Assert.AreEqual(0, _v2.InboundArcs.Select(arc => arc.Start == _v1 && arc.End == _v2).Count());
        }

        [Test]
        public void RemovingVertexResetsItsGraphToNull()
        {
            _graph.AddVertex(_v1);
            _graph.RemoveVertex(_v1);
            Assert.IsNull(_v1.Graph);
        }

        [Test]
        public void NewGraphHasNoArcs()
        {
            Assert.AreEqual(0, _graph.arcs.Count());
        }


        [Test]
        public void VertexKnowsWhichGraphItIsAddedTo()
        {
            _graph.AddVertex(_v1);
            Assert.AreSame(_graph, _v1.Graph);
        }

        [Test]
        public void AddsArcBetweenVertices()
        {
            _graph.AddVertex(_v1);
            _graph.AddVertex(_v2);
            _graph.AddArc(_v1, _v2);
            Assert.IsTrue(ArcHelper.DoesArcExist(_v1, _v2, _graph.arcs));
            Assert.IsNotNull(_v1.OutboundArcs.Single(arc => arc.Start == _v1 && arc.End == _v2));
            Assert.IsNotNull(_v2.InboundArcs.Single(arc => arc.Start == _v1 && arc.End == _v2));
            Assert.AreSame(_graph.arcs.Single(arc => arc.Start == _v1 && arc.End == _v2), _v1.OutboundArcs.Single(arc => arc.Start == _v1 && arc.End == _v2));
        }

        [Test]
        public void RemovesArcBetweenVertices()
        {
            _graph.AddVertex(_v1);
            _graph.AddVertex(_v2);
            _graph.AddArc(_v1, _v2);
            _graph.RemoveArc(_v1, _v2);
            Assert.IsNull(_graph.arcs.FirstOrDefault(arc => arc.Start == _v1 && arc.End == _v2));
            Assert.IsNull(_v1.OutboundArcs.FirstOrDefault(arc => arc.Start == _v1 && arc.End == _v2));
            Assert.IsNull(_v2.InboundArcs.FirstOrDefault(arc => arc.Start == _v1 && arc.End == _v2));
        }

        [Test]
        public void AddsEdgeBetweenVertices()
        {
            _graph.AddVertex(_v1);
            _graph.AddVertex(_v2);
            _graph.AddEdge(_v1, _v2);
            Assert.IsTrue(ArcHelper.DoesArcExist(_v1, _v2, _graph.arcs));
            Assert.IsNotNull(_v1.OutboundArcs.Single(arc => arc.Start == _v1 && arc.End == _v2));
            Assert.IsNotNull(_v2.InboundArcs.Single(arc => arc.Start == _v1 && arc.End == _v2));
            Assert.AreSame(_graph.arcs.Single(arc => arc.Start == _v1 && arc.End == _v2), _v1.OutboundArcs.Single(arc => arc.Start == _v1 && arc.End == _v2));

            Assert.IsTrue(ArcHelper.DoesArcExist(_v2, _v1, _graph.arcs));
            Assert.IsNotNull(_v2.OutboundArcs.Single(arc => arc.Start == _v2 && arc.End == _v1));
            Assert.IsNotNull(_v1.InboundArcs.Single(arc => arc.Start == _v2 && arc.End == _v1));
            Assert.AreSame(_graph.arcs.Single(arc => arc.Start == _v2 && arc.End == _v1), _v2.OutboundArcs.Single(arc => arc.Start == _v2 && arc.End == _v1));
        }

        [Test]
        public void RemovesEdgeBetweenVertices()
        {
            _graph.AddVertex(_v1);
            _graph.AddVertex(_v2);
            _graph.AddEdge(_v1, _v2);
            _graph.RemoveEdge(_v1, _v2);

            Func<Arc, bool> arcV1V2 = arc => arc.Start == _v1 && arc.End == _v2;
            Assert.IsNull(_graph.arcs.FirstOrDefault(arcV1V2));
            Assert.IsNull(_v1.OutboundArcs.FirstOrDefault(arcV1V2));
            Assert.IsNull(_v2.InboundArcs.FirstOrDefault(arcV1V2));

            Func<Arc, bool> arcV2V1 = arc => arc.Start == _v2 && arc.End == _v1;
            Assert.IsNull(_graph.arcs.FirstOrDefault(arcV2V1));
            Assert.IsNull(_v1.OutboundArcs.FirstOrDefault(arcV2V1));
            Assert.IsNull(_v2.InboundArcs.FirstOrDefault(arcV2V1));
        }
    }
}