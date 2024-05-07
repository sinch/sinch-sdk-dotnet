using FluentAssertions;
using Sinch.Core;
using Xunit;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Sinch.Tests.Core
{
    public class JsonInterfaceConverterTests
    {
        [JsonInterfaceConverter(typeof(InterfaceConverter<IPerson>))]
        private interface IPerson
        {
        }

        private class Student : IPerson
        {
            public string Name { get; set; }
        }

        private class Teacher : IPerson
        {
            public int Age { get; set; }
        }

        [Fact]
        public void WriteIPersonStudent()
        {
            IPerson person = new Student()
            {
                Name = "Adam"
            };
            var json = JsonSerializer.Serialize(person);
            json.Should().BeEquivalentTo("{\"Name\":\"Adam\"}");
        }

        [Fact]
        public void WriteIPersonTeacher()
        {
            IPerson person = new Teacher()
            {
                Age = 42
            };
            var json = JsonSerializer.Serialize(person);
            json.Should().BeEquivalentTo("{\"Age\":42}");
        }

        [Fact]
        public void WriteIPersonNull()
        {
            var json = JsonSerializer.Serialize((IPerson)null);
            json.Should().BeEquivalentTo("null");
        }

        [Fact]
        public void ReadIPersonStudent()
        {
            var person = JsonSerializer.Deserialize<IPerson>("{\"Name\":\"Adam\"}");
            person.Should().BeOfType<Student>().Which.Name.Should().Be("Adam");
        }

        [Fact]
        public void ReadIPersonTeacher()
        {
            var person = JsonSerializer.Deserialize<IPerson>("{\"Age\":42}");
            person.Should().BeOfType<Teacher>().Which.Age.Should().Be(42);
        }

        [Fact]
        public void ReadIPersonNull()
        {
            var person = JsonSerializer.Deserialize<IPerson>("null");
            person.Should().BeNull();
        }
    }
}
