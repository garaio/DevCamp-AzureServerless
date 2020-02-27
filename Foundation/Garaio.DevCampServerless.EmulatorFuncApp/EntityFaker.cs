using System;
using System.Collections.Generic;
using System.Linq;
using AutoBogus;
using Bogus;
using Garaio.DevCampServerless.Common.Model;
using Person = Garaio.DevCampServerless.Common.Model.Person;

namespace Garaio.DevCampServerless.EmulatorFuncApp
{
    public class EntityFaker<T> : AutoFaker<T> where T : EntityBase
    {
        public EntityFaker()
        {
            AutoFaker.Configure(b => b.WithSkip<T>(x => x.ETag).WithSkip<T>(x => x.PartitionKey).WithSkip<T>(x => x.RowKey).WithSkip<T>(x => x.Timestamp));
        }
    }

    public class EntityFaker : Faker
    {
        public EntityFakerSet Entities { get; } = new EntityFakerSet();
    }

    public class EntityFakerSet
    {
        public Faker<Person> Person { get; } = new EntityFaker<Person>()
              .RuleFor(x => x.Firstname, (f, x) => f.Name.FirstName())
              .RuleFor(x => x.Lastname, (f, x) => f.Name.LastName())
              .RuleFor(x => x.JobTitle, (f, x) => f.Name.JobTitle())
              .RuleFor(x => x.Slogan, (f, x) => f.Hacker.Phrase())
              .RuleFor(x => x.EmployedSince, (f, x) => f.Date.PastOffset(10))
              .RuleFor(x => x.Skills, (f, x) => new List<Skill>())
              .RuleFor(x => x.Projects, (f, x) => new List<ProjectExperience>());

        public Faker<Project> Project { get; } = new EntityFaker<Project>()
              .RuleFor(x => x.CustomerName, (f, x) => f.Company.CompanyName())
              .RuleFor(x => x.ProjectName, (f, x) => f.Lorem.Word())
              .RuleFor(x => x.Description, (f, x) => f.Rant.Review(x.ProjectName))
              .RuleFor(x => x.ProjectUrl, (f, x) => f.Internet.Url())
              .RuleFor(x => x.IconUrl, (f, x) => f.Image.PicsumUrl(120, 120))
              .RuleFor(x => x.UsedTechnologies, (f, x) => new List<ProjectTechnology>());

        public Faker<ProjectExperience> ProjectExperience { get; } = new EntityFaker<ProjectExperience>()
              .RuleFor(x => x.PersonKey, (f, x) => null)
              .RuleFor(x => x.ProjectKey, (f, x) => null)
              .RuleFor(x => x.RoleInProject, (f, x) => f.Name.JobType())
              .RuleFor(x => x.Description, (f, x) => f.Lorem.Sentences());

        public Faker<ProjectTechnology> ProjectTechnology { get; } = new EntityFaker<ProjectTechnology>()
              .RuleFor(x => x.TechnologyKey, (f, x) => null)
              .RuleFor(x => x.ProjectKey, (f, x) => null)
              .RuleFor(x => x.Component, (f, x) => f.Lorem.Word());
        
        public Faker<Skill> Skill { get; } = new EntityFaker<Skill>()
              .RuleFor(x => x.TechnologyKey, (f, x) => null)
              .RuleFor(x => x.PersonKey, (f, x) => null);


        public void PopulatePersonSkills(Faker faker, Person person, IEnumerable<Technology> technologies)
        {
            if (person.Skills.Any() && faker.Random.Bool(0.3f))
            {
                person.Skills = faker.Random.ListItems(person.Skills);
            }

            foreach (var skill in person.Skills.Any() ? faker.Random.ListItems(person.Skills) : Enumerable.Empty<Skill>())
            {
                skill.Level = faker.Random.Bool(0.5f) ? faker.PickRandom<SkillLevel>() : skill.Level;
            }

            if (technologies.Any())
            {
                var technologiesList = technologies.ToList();
                var max = faker.Random.Bool(0.8f) ? Math.Min(technologiesList.Count, 99) : technologiesList.Count; // Maximal batch-size is 100 (going above this causes a HTTP 500)
                var number = faker.Random.Int(1, max);

                foreach (var technology in faker.Random.ListItems(technologiesList, number))
                {
                    var skill = Skill.Generate();
                    skill.PersonKey = person.RowKey;
                    skill.TechnologyKey = technology.RowKey;

                    person.Skills.Add(skill);
                }
            }
        }

        public void PopulatePersonProjects(Faker faker, Person person, IEnumerable<Project> projects)
        {
            if (person.Projects.Any() && faker.Random.Bool(0.3f))
            {
                person.Projects = faker.Random.ListItems(person.Projects);
            }

            foreach (var project in person.Projects.Any() ? faker.Random.ListItems(person.Projects) : Enumerable.Empty<ProjectExperience>())
            {
                project.Description = faker.Random.Bool(0.5f) ? faker.Lorem.Sentences() : project.Description;
                project.RoleInProject = faker.Random.Bool(0.3f) ? faker.Name.JobType() : project.RoleInProject;
                project.Status = faker.Random.Bool(0.3f) ? faker.PickRandom<PublishState>() : project.Status;
            }

            if (projects.Any())
            {
                var projectsList = projects.ToList();
                var max = faker.Random.Bool(0.8f) ? Math.Min(projectsList.Count, 99) : projectsList.Count; // Maximal batch-size is 100 (going above this causes a HTTP 500)
                var number = faker.Random.Int(1, max);

                foreach (var project in faker.Random.ListItems(projectsList, number))
                {
                    var projExp = ProjectExperience.Generate();
                    projExp.PersonKey = person.RowKey;
                    projExp.ProjectKey = project.RowKey;

                    person.Projects.Add(projExp);
                }
            }
        }

        public void PopulateProjectTechnologies(Faker faker, Project project, IEnumerable<Technology> technologies)
        {
            if (project.UsedTechnologies.Any() && faker.Random.Bool(0.3f))
            {
                project.UsedTechnologies = faker.Random.ListItems(project.UsedTechnologies);
            }

            foreach (var technology in project.UsedTechnologies.Any() ? faker.Random.ListItems(project.UsedTechnologies) : Enumerable.Empty<ProjectTechnology>())
            {
                technology.Component = faker.Random.Bool(0.5f) ? faker.Lorem.Word() : technology.Component;
            }

            if (technologies.Any())
            {
                var technologiesList = technologies.ToList();
                var max = faker.Random.Bool(0.8f) ? Math.Min(technologiesList.Count, 99) : technologiesList.Count; // Maximal batch-size is 100 (going above this causes a HTTP 500)
                var number = faker.Random.Int(1, max);
                
                foreach (var technology in faker.Random.ListItems(technologiesList, number))
                {
                    var projTechnology = ProjectTechnology.Generate();
                    projTechnology.ProjectKey = project.RowKey;
                    projTechnology.TechnologyKey = technology.RowKey;

                    project.UsedTechnologies.Add(projTechnology);
                }
            }
        }
    }
}
