using Microsoft.AspNetCore.Mvc;
using KevinMain.API.Models;

namespace KevinMain.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CVController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        var cvData = new CVData
        {
            PersonalInfo = new PersonalInfo
            {
                Name = "Kevin Main",
                Title = "Lead Developer / Senior Software Engineer",
                Email = "KevMain@gmail.com",
                Phone = "07739688271",
                Location = "Northwich, Cheshire, UK"
            },
            Profile = "With over 20 years of commercial experience across a variety of Microsoft platforms, I specialize in full-stack development using .NET and various front-end technologies like Angular and React. My expertise spans the entire software development lifecycle, from architecture and design to deployment, with a focus on delivering high-performance, scalable web applications. I have extensive experience in building modern, cloud-based applications, leveraging the power of Azure to enhance performance, scalability, and security.\n\nAs a team leader, I currently manage a group of five developers, overseeing the complete software development lifecycle from planning and architecture to deployment. I collaborate with product managers and stakeholders to define project scope, prioritize features, and ensure the timely delivery of solutions. My organizational skills allow me to work autonomously, while my leadership fosters a collaborative and efficient team environment.",
            KeySkills = new List<string>
            {
                ".NET Core, ASP.NET MVC",
                "SOLID, DRY, TDD, IoC, CI and Dependency Injection",
                "LINQ / Entity Framework / ORM",
                "React, Angular, jQuery and TypeScript",
                "Web technologies including XML, CSS, and HTML5",
                "REST APIs and WCF web services",
                "SQL, database architecture and performance as well as MongoDB",
                "Some experience of C++ and Java"
            },
            Tools = new List<string>
            {
                "Azure and cloud technologies",
                "Visual Studio, Rider and VSCode",
                "Git / TFS / SVN",
                "NUnit / MS Test / xUnit",
                "NHibernate / Dapper",
                "SQL Server, MySQL and Oracle",
                "Windows and OSX"
            },
            WorkExperience = new List<WorkExperience>
            {
                new WorkExperience
                {
                    Company = "iSAMS Independent Ltd",
                    Position = "Lead Developer / Software Developer",
                    StartDate = "Aug 2015",
                    EndDate = "Present",
                    Location = "Northampton, UK",
                    Description = "Key contributor and team lead in Agile development of enterprise-grade education software. Specialize in back-end development with ASP.NET (MVC/Web API) and C#, with full-stack capabilities across SQL Server, React, and Azure.",
                    Highlights = new List<string>
                    {
                        "Led successful delivery of critical features and services across the full SDLC, collaborating with stakeholders and product teams to meet business goals",
                        "Enforced SOLID principles, clean architecture and DDD. Used CI/CD (Azure DevOps), unit testing and code analysis",
                        "Promoted to Lead Developer; mentor to junior developers, responsible for technical direction and code reviews as well as performance reviews",
                        "Gained hands-on experience deploying and maintaining cloud-based solutions using Microsoft Azure services",
                        "Contributed to the migration of legacy AngularJS front-end components to a modern React-based architecture",
                        "Successfully modernized a legacy Classic ASP school registration system by integrating a new Polymer-based front end"
                    },
                    TechStack = ".NET Framework / .NET Core, C#, ASP.NET MVC/Web API, Entity Framework, React, AngularJS, SQL Server, Azure, JavaScript, HTML/CSS, Git, Azure DevOps"
                },
                new WorkExperience
                {
                    Company = "Advanced Legal",
                    Position = "Senior Software Engineer",
                    StartDate = "Sept 2014",
                    EndDate = "Aug 2015",
                    Location = "Knutsford, UK",
                    Description = "Worked in an Agile environment and followed practices such as pair programming, test driven development (TDD) and behaviour driven development to deliver updates and enhancements to an existing well adopted Legal software product.",
                    Highlights = new List<string>
                    {
                        "Refactored ageing WPF code using more modern techniques such as SOLID principles, dependency injections and unit testing",
                        "Produced a more maintainable and future proofed product"
                    },
                    TechStack = ".NET, WPF, C#, SOLID, TDD, BDD"
                },
                new WorkExperience
                {
                    Company = "Sofaworks",
                    Position = "Senior C# Developer",
                    StartDate = "May 2014",
                    EndDate = "Aug 2014",
                    Location = "Golborne, UK",
                    Description = "Responsible for developing interactive applications. Main achievement involved building a Stock Inventory system rolled out across 6 depots around the country.",
                    Highlights = new List<string>
                    {
                        "Built Stock Inventory system allowing tracking of stock at all times across 6 depots",
                        "Developed .NET Web API called via custom iOS app using 3rd party barcode scanner",
                        "Used SQL Server with EF Code First using migrations",
                        "Full system built using TDD from the ground up"
                    },
                    TechStack = ".NET Web API, iOS, SQL Server, Entity Framework Code First, TDD"
                },
                new WorkExperience
                {
                    Company = "Havas Lynx",
                    Position = "Software Engineer",
                    StartDate = "Nov 2013",
                    EndDate = "Apr 2014",
                    Location = "Manchester, UK",
                    Description = "Worked in a full agile based software house developing innovative healthcare solutions to be delivered into health organisations including the NHS.",
                    Highlights = new List<string>
                    {
                        "Worked on a Microsoft Surface application using Xamarin which could also be ported to iPad/Android",
                        "Used cutting edge technologies such as NoSQL database, Nancy API with heavy use of JavaScript (KnockoutJS, NodeJS)",
                        "Worked on user stories, bug fixing and architectural decisions"
                    },
                    TechStack = "Xamarin, NoSQL, Nancy API, JavaScript, KnockoutJS, NodeJS"
                },
                new WorkExperience
                {
                    Company = "AMEC",
                    Position = "Analyst Developer",
                    StartDate = "Sept 2012",
                    EndDate = "Nov 2013",
                    Location = "Knutsford, UK",
                    Description = "Worked within the Global Business Systems team across the full project development lifecycle to deliver all Applications/Software to the business.",
                    Highlights = new List<string>
                    {
                        "Analysed business needs and reached solutions acceptable to all stakeholders",
                        "Worked with Microsoft technologies and development stack"
                    },
                    TechStack = "ASP.NET MVC, SQL Server, SharePoint"
                },
                new WorkExperience
                {
                    Company = "CC Electronics Europe",
                    Position = "Software Developer",
                    StartDate = "July 2011",
                    EndDate = "Aug 2012",
                    Location = "Winsford, UK",
                    Description = "Managed all of the online systems for the company including an innovative quoting tool.",
                    Highlights = new List<string>
                    {
                        "Built innovative quoting tool in ASP.NET MVC",
                        "Created highly complex rule engine built using TDD from scratch"
                    },
                    TechStack = "ASP.NET MVC, TDD"
                },
                new WorkExperience
                {
                    Company = "Human-eCreative",
                    Position = "Senior Programmer/Developer",
                    StartDate = "May 2006",
                    EndDate = "June 2011",
                    Location = "Knutsford, UK",
                    Description = "Built web sites mainly using ASP.NET. Worked on a variety of multi-tiered projects including some large web applications.",
                    Highlights = new List<string>
                    {
                        "Worked on static HTML websites and converted existing sites to make accessible and cross browser compliant",
                        "Exposed to Microsoft SharePoint products and performed many custom modifications using .NET",
                        "Worked on a social networking site using .NET AJAX components and jQuery libraries"
                    },
                    TechStack = "ASP.NET, SharePoint, .NET AJAX, jQuery, HTML, CSS"
                }
            },
            Education = new Education
            {
                Higher = new HigherEducation
                {
                    University = "University Of Huddersfield",
                    Course = "Interactive Multimedia",
                    Grade = "2-1 (BA Hons)",
                    Dates = "10/01 - 05/05"
                },
                Secondary = new List<SecondaryEducation>
                {
                    new SecondaryEducation
                    {
                        Institution = "Lowestoft College",
                        Qualification = "BTEC National Diploma - Computer Studies",
                        Grade = "3 Distinctions, 1 Merit",
                        Date = "7/99"
                    },
                    new SecondaryEducation
                    {
                        Institution = "Benjamin Britten High School",
                        Qualification = "GCSE",
                        Grade = "8 A-C grades",
                        Date = "7/97"
                    }
                }
            },
            LeisureActivities = "I am a dedicated runner with a strong passion for fitness. I have completed many races, including the London Marathon, and continually strive to improve my personal bests. My commitment to fitness extends beyond running; I actively engage in a variety of training routines to enhance strength, mobility, and endurance.\n\nIn addition to my athletic pursuits, I stay informed on the latest trends and advancements in fitness and wellness, blending my love for fitness with my interest in science. I am an active participant in the tech community, attending events such as DDD and Umbraco meetups to network with industry professionals and stay up to date on emerging technologies.\n\nIn my spare time, I am passionate about hands-on development and have built several projects to continuously improve my technical skills. Currently, I am refactoring a large, legacy WebForms application, transitioning it to a modern architecture."
        };

        return Ok(cvData);
    }
}
