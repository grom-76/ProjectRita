namespace RitaTests;

using System;
using NUnit.Framework;

using RitaEngine.Base;

public class BaseTests : IDisposable
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Check_Is_System_x64()
    {
        int excpected = 8;
        var x = BaseHelper.FORCE_ALIGNEMENT;
        Assert.That(x, Is.EqualTo(excpected) );
    }

    [TearDown]
    public void Dispose()
    {

    }
}