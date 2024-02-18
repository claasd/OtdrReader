using System.Text;
using CdIts.OtdrReader;
using CdIts.OtdrWriter;
using FluentAssertions;
using NUnit.Framework.Internal.Commands;

namespace OtdrTests;

public class Tests
{
    [Test]
    public void GenericBlockTest()
    {
        var writer = new OtdrWriter();
        var str = "some data to put there";
        writer.AddBlock("Test", Encoding.UTF8.GetBytes($"Test\0{str}"));
        var memStream = new MemoryStream();
        writer.ToStream(memStream);
        var data = memStream.ToArray();
        var reader = OtdrReader.ReadByteArray(data);
        var blockData = reader.GetBlockData("Test", true);
        Encoding.UTF8.GetString(blockData).Should().Be(str);
    }
    
    [Test]
    public void Test1()
    {
        var writer = new OtdrWriter();
        var genParams = new GeneralParameters()
        {
            LanguageCode = "DE",
            LocationA = "LocA",
            LocationB = "LocB"
        };
        var fxdParams = new FixedParameters()
        {
            Timestamp = new DateTimeOffset(2024, 2, 18, 18, 10, 0, TimeSpan.Zero),
            X2 = 100,
            Y2 = 99
        };
        var supParams = new SupplierParameters()
        {
            Other = "Test"
        };
        var keyEvents = new KeyEvents()
        {
            OpticalReturnLoss = 22
        };
        keyEvents.Events.Add(new KeyEvent()
        {
            EventNumber = 1,
            Comment = "Event1",
        });
        keyEvents.Events.Add(new KeyEvent()
        {
            EventNumber = 2,
            Comment = "Event 2",
        });
        var trc = new DataPointsBlock();
        trc.Traces.Add(new TraceDataPoints()
        {
            DataPoints = { 1,2,3,4 }
        });
        trc.Traces.Add(new TraceDataPoints()
        {
            DataPoints = { 99,100 }
        });
        writer.AddBlock(genParams);
        writer.AddBlock(fxdParams);
        writer.AddBlock(supParams);
        writer.AddBlock(keyEvents);
        writer.AddBlock(trc);
        var memStream = new MemoryStream();
        writer.ToStream(memStream);
        var data = memStream.ToArray();
        var reader = OtdrReader.ReadByteArray(data);
        reader.GetGeneralParameters().LocationA.Should().Be("LocA");
        reader.GetFixedParameters().X2.Should().Be(100);
        reader.GetFixedParameters().Y2.Should().Be(99);
        reader.GetSupplierParameters().Other.Should().Be("Test");
        var events = reader.GetKeyEvents();
        events.OpticalReturnLoss.Should().Be(22);
        events.Events.Should().HaveCount(2);
        events.Events[0].EventNumber.Should().Be(1);
        events.Events[0].Comment.Should().Be("Event1");
        events.Events[1].EventNumber.Should().Be(2);
        events.Events[1].Comment.Should().Be("Event 2");
        var traces = reader.GetDataPoints();
        traces.Should().HaveCount(2);
        traces[0].DataPoints.Should().BeEquivalentTo(new List<ushort> { 1, 2, 3, 4 });
        traces[1].DataPoints.Should().BeEquivalentTo(new List<ushort> { 99, 100 });
    }
}