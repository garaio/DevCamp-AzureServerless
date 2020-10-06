using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using Garaio.DevCampServerless.Common.Model;

namespace Garaio.DevCampServerless.EmulatorFuncApp.Emulation
{
    public class EmulationSteps : List<EmulationStep>
    {
        private readonly Lazy<EntityFaker> _faker;

        public EmulationSteps(Lazy<EntityFaker> faker)
        {
            _faker = faker;

            Add(2, GetUserProfile);
            Add(2, SearchRequest);
            Add(3, CalculatePrimeNumber);

            Add(1, GetAllTechnologies);

            Add(1, GetAllPersons);
            Add(2, CreatePerson);
            Add(2, UpdatePerson);
            Add(3, DeletePerson);

            Add(1, GetAllProjects);
            Add(2, CreateProject);
            Add(2, UpdateProject);
            Add(3, DeleteProject);
        }

        public void Add(int priority, Func<EmulationContext, Task<EmulationContext>> method)
        {
            Add(new EmulationStep { Priority = priority, Method = method });
        }

        private async Task<EmulationContext> GetUserProfile(EmulationContext ec)
        {
            await Configurations.ServiceFuncUrl
            .AppendPathSegment("user")
            .SetQueryParams(new { code = Configurations.ServiceFuncKey })
            .GetStringAsync();

            return ec;
        }

        private async Task<EmulationContext> SearchRequest(EmulationContext ec)
        {
            var term = _faker.Value.Lorem.Word();

            await Configurations.ServiceFuncUrl
            .AppendPathSegment("search")
            .SetQueryParams(new { code = Configurations.ServiceFuncKey })
            .PostJsonAsync(term);

            return ec;
        }

        private async Task<EmulationContext> CalculatePrimeNumber(EmulationContext ec)
        {
            var number = _faker.Value.Random.Int(100000);

            await Configurations.ServiceFuncUrl
            .AppendPathSegment("search")
            .SetQueryParams(new { code = Configurations.ServiceFuncKey })
            .PostJsonAsync(number);

            return ec;
        }

        private async Task<EmulationContext> GetAllTechnologies(EmulationContext ec)
        {
            var result = await Configurations.ServiceFuncUrl
            .AppendPathSegment("technologies")
            .SetQueryParams(new { code = Configurations.ServiceFuncKey })
            .GetAsync()
            .ReceiveJson<ICollection<Technology>>();

            if (result?.Any() == true)
                ec.Entities.AddRange(result);

            return ec;
        }

        private async Task<EmulationContext> GetAllPersons(EmulationContext ec)
        {
            var result = await Configurations.ServiceFuncUrl
            .AppendPathSegment("persons")
            .SetQueryParams(new { code = Configurations.ServiceFuncKey })
            .GetAsync()
            .ReceiveJson<ICollection<Person>>();

            if (result?.Any() == true)
                ec.Entities.AddRange(result);

            return ec;
        }

        private async Task<EmulationContext> CreatePerson(EmulationContext ec)
        {
            var person = _faker.Value.Entities.Person.Generate();

            _faker.Value.Entities.PopulatePersonSkills(_faker.Value, person, ec.Entities.OfType<Technology>());
            _faker.Value.Entities.PopulatePersonProjects(_faker.Value, person, ec.Entities.OfType<Project>());

            var result = await Configurations.ServiceFuncUrl
            .AppendPathSegment("persons")
            .SetQueryParams(new { code = Configurations.ServiceFuncKey })
            .PostJsonAsync(person)
            .ReceiveJson<Person>();

            if (result != null)
                ec.Entities.Add(result);

            return ec;
        }

        private async Task<EmulationContext> UpdatePerson(EmulationContext ec)
        {
            var persons = ec.Entities.OfType<Person>();
            if (!persons.Any())
                return ec;

            var generated = _faker.Value.Entities.Person.Generate();
            var person = _faker.Value.PickRandom(persons);

            // Load all data to person
            person = await Configurations.ServiceFuncUrl
            .AppendPathSegment("persons")
            .AppendPathSegment($"{person.RowKey}")
            .SetQueryParams(new { code = Configurations.ServiceFuncKey })
            .GetAsync()
            .ReceiveJson<Person>();

            // Manipulate data
            person.RowKey = _faker.Value.Random.Bool(0.1f) ? generated.RowKey : person.RowKey;
            person.Firstname = _faker.Value.Random.Bool(0.3f) ? generated.Firstname : person.Firstname;
            person.Lastname = _faker.Value.Random.Bool(0.3f) ? generated.Lastname : person.Lastname;
            person.JobTitle = _faker.Value.Random.Bool(0.6f) ? generated.JobTitle : person.JobTitle;
            person.Slogan = _faker.Value.Random.Bool(0.5f) ? generated.Slogan : person.Slogan;
            person.EmployedSince = _faker.Value.Random.Bool(0.3f) ? generated.EmployedSince : person.EmployedSince;
            person.Status = _faker.Value.Random.Bool(0.3f) ? generated.Status : person.Status;

            _faker.Value.Entities.PopulatePersonSkills(_faker.Value, person, ec.Entities.OfType<Technology>());
            _faker.Value.Entities.PopulatePersonProjects(_faker.Value, person, ec.Entities.OfType<Project>());

            // Store person
            var result = await Configurations.ServiceFuncUrl
            .AppendPathSegment("persons")
            .AppendPathSegment($"{person.RowKey}")
            .SetQueryParams(new { code = Configurations.ServiceFuncKey })
            .PutJsonAsync(person)
            .ReceiveJson<Person>();

            if (result != null)
                ec.Entities.Add(result);

            return ec;
        }

        private async Task<EmulationContext> DeletePerson(EmulationContext ec)
        {
            var key = _faker.Value.PickRandom(ec.Entities.OfType<Person>().Select(p => p.RowKey).DefaultIfEmpty(EntityBase.NewRowKey));

            var result = await Configurations.ServiceFuncUrl
            .AppendPathSegment("persons")
            .AppendPathSegment($"{key}")
            .SetQueryParams(new { code = Configurations.ServiceFuncKey })
            .DeleteAsync();

            return ec;
        }

        private async Task<EmulationContext> GetAllProjects(EmulationContext ec)
        {
            var result = await Configurations.ServiceFuncUrl
            .AppendPathSegment("projects")
            .SetQueryParams(new { code = Configurations.ServiceFuncKey })
            .GetAsync()
            .ReceiveJson<ICollection<Project>>();

            if (result?.Any() == true)
                ec.Entities.AddRange(result);

            return ec;
        }

        private async Task<EmulationContext> CreateProject(EmulationContext ec)
        {
            var project = _faker.Value.Entities.Project.Generate();

            _faker.Value.Entities.PopulateProjectTechnologies(_faker.Value, project, ec.Entities.OfType<Technology>());

            var result = await Configurations.ServiceFuncUrl
            .AppendPathSegment("projects")
            .SetQueryParams(new { code = Configurations.ServiceFuncKey })
            .PostJsonAsync(project)
            .ReceiveJson<Project>();

            if (result != null)
                ec.Entities.Add(result);

            return ec;
        }

        private async Task<EmulationContext> UpdateProject(EmulationContext ec)
        {
            var projects = ec.Entities.OfType<Project>();
            if (!projects.Any())
                return ec;

            var generated = _faker.Value.Entities.Project.Generate();
            var project = _faker.Value.PickRandom(projects);

            // Load all data to entity
            project = await Configurations.ServiceFuncUrl
            .AppendPathSegment("projects")
            .AppendPathSegment($"{project.RowKey}")
            .SetQueryParams(new { code = Configurations.ServiceFuncKey })
            .GetAsync()
            .ReceiveJson<Project>();

            // Manipulate data
            project.RowKey = _faker.Value.Random.Bool(0.1f) ? generated.RowKey : project.RowKey;
            project.CustomerName = _faker.Value.Random.Bool(0.3f) ? generated.CustomerName : project.CustomerName;
            project.ProjectName = _faker.Value.Random.Bool(0.3f) ? generated.ProjectName : project.ProjectName;
            project.Description = _faker.Value.Random.Bool(0.6f) ? generated.Description : project.Description;
            project.ProjectUrl = _faker.Value.Random.Bool(0.3f) ? generated.ProjectUrl : project.ProjectUrl;
            project.IconUrl = _faker.Value.Random.Bool(0.3f) ? generated.IconUrl : project.IconUrl;
            project.Status = _faker.Value.Random.Bool(0.3f) ? generated.Status : project.Status;

            _faker.Value.Entities.PopulateProjectTechnologies(_faker.Value, project, ec.Entities.OfType<Technology>());

            // Store entity
            var result = await Configurations.ServiceFuncUrl
            .AppendPathSegment("projects")
            .AppendPathSegment($"{project.RowKey}")
            .SetQueryParams(new { code = Configurations.ServiceFuncKey })
            .PutJsonAsync(project)
            .ReceiveJson<Project>();

            if (result != null)
                ec.Entities.Add(result);

            return ec;
        }

        private async Task<EmulationContext> DeleteProject(EmulationContext ec)
        {
            var key = _faker.Value.PickRandom(ec.Entities.OfType<Project>().Select(p => p.RowKey).DefaultIfEmpty(EntityBase.NewRowKey));

            var result = await Configurations.ServiceFuncUrl
            .AppendPathSegment("projects")
            .AppendPathSegment($"{key}")
            .SetQueryParams(new { code = Configurations.ServiceFuncKey })
            .DeleteAsync();

            return ec;
        }
    }

    public class EmulationStep
    {
        public Guid Id { get; } = Guid.NewGuid();

        public int Priority { get; set; }

        public Func<EmulationContext, Task<EmulationContext>> Method { get; set; }
    }
}
