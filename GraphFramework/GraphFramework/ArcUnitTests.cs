﻿using NUnit.Framework;

namespace GraphFramework
{
    [TestFixture]
    public class ArcUnitTests
    {
        private Vertex _v1;
        private Vertex _v2;
        private Graph _g;
        private Arc _a;

        [SetUp]
        public void Init()
        {
            _v1 = new Vertex();
            _v2 = new Vertex();
            _g = new Graph();
        }

        public class TheConstructor3 : ArcUnitTests 
        {
            [SetUp]
            public void DerivedInit()
            {
                base.Init();
                _a = new Arc(_g, _v1, _v2);
            }

            [Test]
            public void SetsStartVertexOnCreate()
            {
                Assert.That(_a.Start, Is.SameAs(_v1));
            }

            [Test]
            public void SetsEndVertexOnCreate()
            {
                Assert.That(_a.End, Is.SameAs(_v2));
            }

            [Test]
            public void SetsGraphOnCreate()
            {
                Assert.AreSame(_g, _a.Graph);
            }
        }

        public class TheConstructor4 : ArcUnitTests
        {
            [Test]
            public void CanCreateArcInMatching()
            {
                _a = new Arc(_g, _v1, _v2, true);
                Assert.That(_a.IsInMatching, Is.True);
            }
        }

        public class TheAddToMatchingMethod : ArcUnitTests
        {
            [Test]
            public void KnowsItIsInMatching()
            {
                _a = new Arc(_g, _v1, _v2);
                _a.AddToMatching();
                Assert.AreEqual(true, _a.IsInMatching);
            }     
        }
    }
}
