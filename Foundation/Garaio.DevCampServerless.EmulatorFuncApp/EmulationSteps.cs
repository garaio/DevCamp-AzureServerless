using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using Garaio.DevCampServerless.Common.Model;

namespace Garaio.DevCampServerless.EmulatorFuncApp
{
    public class EmulationSteps : List<Func<EmulationContext, Task<EmulationContext>>>
    {
        private readonly Lazy<EntityFaker> _faker;

        public EmulationSteps(Lazy<EntityFaker> faker)
        {
            _faker = faker;

            Add(GetUserProfile);
            Add(SearchRequest);
            Add(CalculatePrimeNumber);

            Add(GetAllTechnologies);

            Add(GetAllPersons);
            Add(CreatePerson);
            Add(UpdatePerson);
            Add(DeletePerson);

            Add(GetAllProjects);
            Add(CreateProject);
            Add(UpdateProject);
            Add(DeleteProject);
        }

        private async Task<EmulationContext> GetUserProfile(EmulationContext ec)
        {
            var result = await Environment.GetEnvironmentVariable(Constants.Configurations.ServiceFuncUrl)
            .AppendPathSegment("user")
            .SetQueryParams(new { code = Environment.GetEnvironmentVariable(Constants.Configurations.ServiceFuncKey) })
            .GetStringAsync();

            return ec;
        }

        private async Task<EmulationContext> SearchRequest(EmulationContext ec)
        {
            var term = _faker.Value.Lorem.Word();

            var result = await Environment.GetEnvironmentVariable(Constants.Configurations.ServiceFuncUrl)
            .AppendPathSegment("search")
            .SetQueryParams(new { code = Environment.GetEnvironmentVariable(Constants.Configurations.ServiceFuncKey) })
            .PostJsonAsync(term);

            return ec;
        }
        
        private async Task<EmulationContext> CalculatePrimeNumber(EmulationContext ec)
        {
            var number = _faker.Value.Random.Int(100000);

            var result = await Environment.GetEnvironmentVariable(Constants.Configurations.ServiceFuncUrl)
            .AppendPathSegment("search")
            .SetQueryParams(new { code = Environment.GetEnvironmentVariable(Constants.Configurations.ServiceFuncKey) })
            .PostJsonAsync(number);

            return ec;
        }

        private async Task<EmulationContext> GetAllTechnologies(EmulationContext ec)
        {
            var result = await Environment.GetEnvironmentVariable(Constants.Configurations.ServiceFuncUrl)
            .AppendPathSegment("technologies")
            .SetQueryParams(new { code = Environment.GetEnvironmentVariable(Constants.Configurations.ServiceFuncKey) })
            .GetAsync()
            .ReceiveJson<ICollection<Technology>>();

            if (result?.Any() == true)
                ec.Entities.AddRange(result);

            return ec;
        }

        private async Task<EmulationContext> GetAllPersons(EmulationContext ec)
        {
            var result = await Environment.GetEnvironmentVariable(Constants.Configurations.ServiceFuncUrl)
            .AppendPathSegment("persons")
            .SetQueryParams(new { code = Environment.GetEnvironmentVariable(Constants.Configurations.ServiceFuncKey) })
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

            var result = await Environment.GetEnvironmentVariable(Constants.Configurations.ServiceFuncUrl)
            .AppendPathSegment("persons")
            .SetQueryParams(new { code = Environment.GetEnvironmentVariable(Constants.Configurations.ServiceFuncKey) })
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
            person = await Environment.GetEnvironmentVariable(Constants.Configurations.ServiceFuncUrl)
            .AppendPathSegment("persons")
            .AppendPathSegment($"{person.RowKey}")
            .SetQueryParams(new { code = Environment.GetEnvironmentVariable(Constants.Configurations.ServiceFuncKey) })
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
            var result = await Environment.GetEnvironmentVariable(Constants.Configurations.ServiceFuncUrl)
            .AppendPathSegment("persons")
            .AppendPathSegment($"{person.RowKey}")
            .SetQueryParams(new { code = Environment.GetEnvironmentVariable(Constants.Configurations.ServiceFuncKey) })
            .PutJsonAsync(person)
            .ReceiveJson<Person>();

            if (result != null)
                ec.Entities.Add(result);

            return ec;
        }
        
        private async Task<EmulationContext> DeletePerson(EmulationContext ec)
        {
            var persons = ec.Entities.OfType<Person>();
            if (!persons.Any())
                return ec;

            var person = _faker.Value.PickRandom(persons);

            var result = await Environment.GetEnvironmentVariable(Constants.Configurations.ServiceFuncUrl)
            .AppendPathSegment("persons")
            .AppendPathSegment($"{person.RowKey}")
            .SetQueryParams(new { code = Environment.GetEnvironmentVariable(Constants.Configurations.ServiceFuncKey) })
            .DeleteAsync();
            
            return ec;
        }

        private async Task<EmulationContext> GetAllProjects(EmulationContext ec)
        {
            var result = await Environment.GetEnvironmentVariable(Constants.Configurations.ServiceFuncUrl)
            .AppendPathSegment("projects")
            .SetQueryParams(new { code = Environment.GetEnvironmentVariable(Constants.Configurations.ServiceFuncKey) })
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

            var result = await Environment.GetEnvironmentVariable(Constants.Configurations.ServiceFuncUrl)
            .AppendPathSegment("projects")
            .SetQueryParams(new { code = Environment.GetEnvironmentVariable(Constants.Configurations.ServiceFuncKey) })
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
            project = await Environment.GetEnvironmentVariable(Constants.Configurations.ServiceFuncUrl)
            .AppendPathSegment("projects")
            .AppendPathSegment($"{project.RowKey}")
            .SetQueryParams(new { code = Environment.GetEnvironmentVariable(Constants.Configurations.ServiceFuncKey) })
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
            var result = await Environment.GetEnvironmentVariable(Constants.Configurations.ServiceFuncUrl)
            .AppendPathSegment("projects")
            .AppendPathSegment($"{project.RowKey}")
            .SetQueryParams(new { code = Environment.GetEnvironmentVariable(Constants.Configurations.ServiceFuncKey) })
            .PutJsonAsync(project)
            .ReceiveJson<Project>();

            if (result != null)
                ec.Entities.Add(result);

            return ec;
        }

        private async Task<EmulationContext> DeleteProject(EmulationContext ec)
        {
            var projects = ec.Entities.OfType<Project>();
            if (!projects.Any())
                return ec;

            var project = _faker.Value.PickRandom(projects);

            var result = await Environment.GetEnvironmentVariable(Constants.Configurations.ServiceFuncUrl)
            .AppendPathSegment("projects")
            .AppendPathSegment($"{project.RowKey}")
            .SetQueryParams(new { code = Environment.GetEnvironmentVariable(Constants.Configurations.ServiceFuncKey) })
            .DeleteAsync();

            return ec;
        }
    }
}
