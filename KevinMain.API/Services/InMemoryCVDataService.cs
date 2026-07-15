using KevinMain.API.Models;

namespace KevinMain.API.Services;

/// <summary>
/// In-memory implementation of CV data service.
/// This can easily be replaced with a database-backed implementation (e.g., DatabaseCVDataService)
/// by creating a new class that implements ICVDataService and swapping it in Program.cs.
/// 
/// Future database implementation could:
/// - Use Entity Framework Core with SQL Server
/// - Use MongoDB with the MongoDB.Driver package
/// - Use Cosmos DB for Azure-native solution
/// - Use any other data store by implementing the same interface
/// </summary>
public class InMemoryCVDataService : ICVDataService
{
    public Task<CVData> GetCVDataAsync()
    {
        // TODO: When implementing database version, replace this with:
        // return await _dbContext.CVData.Include(x => x.WorkExperience).FirstOrDefaultAsync();

        var cvData = new CVData
        {
            PersonalInfo = new PersonalInfo
            {
                Name = "Kevin Main",
                Title = "Technical Lead | Principal Software Engineer | .NET & Azure",
                Email = "KevMain@gmail.com",
                Phone = "07739 688271",
                Location = "Northwich, Cheshire"
            },
            Profile = "Technical Lead with over 20 years of commercial experience delivering enterprise software across the Microsoft ecosystem. Currently leading a team of five engineers responsible for technical direction, architecture and delivery of a SaaS platform used by more than 1,500 schools and thousands of users. Equally comfortable defining architecture, mentoring engineers, collaborating with stakeholders and writing production code. Passionate about modernising legacy platforms, improving engineering practices and delivering measurable business value.\n\nI believe the role of a Technical Lead is to enable teams to deliver consistently through clear technical direction, mentoring, pragmatic decision-making and engineering excellence. I enjoy coaching developers, encouraging ownership, improving delivery practices and balancing long-term architectural goals with day-to-day business needs.",
            KeySkills = new List<string>
            {
                "Leadership: Technical Leadership • Mentoring • Architecture • Stakeholder Management • Agile",
                "Backend: C# • .NET • ASP.NET Core • REST APIs • Entity Framework • Dapper",
                "Cloud: Azure • Azure DevOps • CI/CD",
                "Frontend: React • Angular • TypeScript",
                "Data: SQL Server • MongoDB • MySQL • Oracle"
            },
            Tools = new List<string>
            {
                "20+ years commercial software development experience",
                "Lead a team of five software engineers and have personally mentored two developers",
                "Platform supports 1,500+ schools and thousands of end users",
                "Converted parts of a monolith into independently deployable microservices",
                "Championed an API-first architecture",
                "Deployment frequency improved from every few weeks to several times each week",
                "Reduced production defects by approximately 50%",
                "Reduced feature delivery time by approximately 25%"
            },
            WorkExperience = new List<WorkExperience>
            {
                new WorkExperience
                {
                    Company = "iSAMS Independent Ltd",
                    Position = "Technical Lead (Lead Software Developer)",
                    StartDate = "Aug 2015",
                    EndDate = "Present",
                    Location = "Remote",
                    Description = "Lead technical delivery of a large education SaaS platform while remaining hands-on across architecture, cloud services and software engineering.",
                    Highlights = new List<string>
                    {
                        "Leadership: Lead, mentor and performance-manage a team of five developers",
                        "Leadership: Mentored two developers to strengthen technical capability and confidence",
                        "Leadership: Work closely with Product Managers and stakeholders to shape technical strategy, prioritise work and align delivery with business goals",
                        "Leadership: Lead technical design discussions, code reviews and engineering standards",
                        "Architecture: Designed Azure-hosted services supporting more than 1,500 schools and thousands of users",
                        "Architecture: Converted areas of a monolithic application into independently deployable microservices",
                        "Architecture: Championed an API-first design approach to improve scalability and integration",
                        "Architecture: Led migration from AngularJS to React",
                        "Architecture: Modernised legacy Classic ASP systems while maintaining business continuity",
                        "Architecture: Championed SOLID principles, clean architecture, automated testing and CI/CD",
                        "Impact: Increased deployment frequency from every few weeks to several deployments per week",
                        "Impact: Reduced production defects by approximately 50%",
                        "Impact: Reduced delivery time by approximately 25%",
                        "Impact: Delivered projects ranging from one-week enhancements to year-long strategic initiatives"
                    },
                    TechStack = ".NET, C#, ASP.NET Core, Azure, React, Angular, SQL Server, Entity Framework, REST APIs, CI/CD, Azure DevOps"
                },
                new WorkExperience
                {
                    Company = "Advanced Legal",
                    Position = "Senior Software Engineer",
                    StartDate = "Sep 2014",
                    EndDate = "Aug 2015",
                    Location = "Knutsford, UK",
                    Description = "Modernised a mature WPF application using SOLID, dependency injection, pair programming, TDD and BDD, improving maintainability and engineering quality.",
                    Highlights = new List<string>
                    {
                        "Refactored ageing WPF code using SOLID principles and dependency injection",
                        "Followed pair programming, TDD and BDD practices",
                        "Produced a more maintainable and future-proof product"
                    },
                    TechStack = ".NET, WPF, C#, SOLID, TDD, BDD, Dependency Injection"
                },
                new WorkExperience
                {
                    Company = "Sofaworks",
                    Position = "Senior C# Developer",
                    StartDate = "May 2014",
                    EndDate = "Aug 2014",
                    Location = "Golborne, UK",
                    Description = "Designed and delivered a nationwide stock management platform deployed across six UK depots with .NET Web APIs, Entity Framework Code First and iOS integration.",
                    Highlights = new List<string>
                    {
                        "Built Stock Inventory system allowing tracking of stock across 6 depots nationwide",
                        "Developed .NET Web API called via custom iOS app using barcode scanner",
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
                    Description = "Developed healthcare software for NHS organisations using Xamarin, JavaScript, NoSQL technologies and modern API-driven solutions.",
                    Highlights = new List<string>
                    {
                        "Worked on Microsoft Surface application using Xamarin (portable to iPad/Android)",
                        "Used NoSQL database, Nancy API with heavy JavaScript (KnockoutJS, NodeJS)",
                        "Worked on user stories, bug fixing and architectural decisions"
                    },
                    TechStack = "Xamarin, NoSQL, Nancy API, JavaScript, KnockoutJS, NodeJS"
                },
                new WorkExperience
                {
                    Company = "AMEC",
                    Position = "Analyst Developer",
                    StartDate = "Sep 2012",
                    EndDate = "Aug 2013",
                    Location = "Knutsford, UK",
                    Description = "Delivered enterprise business applications using ASP.NET MVC, SQL Server and SharePoint while collaborating closely with business stakeholders.",
                    Highlights = new List<string>
                    {
                        "Analysed business needs and reached solutions acceptable to all stakeholders",
                        "Delivered applications across the full project development lifecycle"
                    },
                    TechStack = "ASP.NET MVC, SQL Server, SharePoint"
                },
                new WorkExperience
                {
                    Company = "CC Electronics Europe",
                    Position = "Software Developer",
                    StartDate = "Jul 2011",
                    EndDate = "Jun 2012",
                    Location = "Winsford, UK",
                    Description = "Designed business-critical quoting and workflow systems using ASP.NET MVC.",
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
                    Position = "Senior Developer",
                    StartDate = "May 2006",
                    EndDate = "Aug 2011",
                    Location = "Knutsford, UK",
                    Description = "Delivered enterprise web applications, SharePoint solutions and accessibility improvements for a wide range of clients.",
                    Highlights = new List<string>
                    {
                        "Built web applications and converted sites to be accessible and cross browser compliant",
                        "Performed custom SharePoint modifications using .NET",
                        "Worked on social networking site using .NET AJAX and jQuery"
                    },
                    TechStack = "ASP.NET, SharePoint, .NET AJAX, jQuery, HTML, CSS"
                },
                new WorkExperience
                {
                    Company = "Easy Computers",
                    Position = "Software Developer",
                    StartDate = "Apr 2005",
                    EndDate = "May 2006",
                    Location = "Lowestoft, UK",
                    Description = "Developed business automation software including an automated customer notification system that reduced support calls by around 20%.",
                    Highlights = new List<string>
                    {
                        "Built automated customer notification system reducing support calls by ~20%",
                        "Developed business process automation tools"
                    },
                    TechStack = "ASP.NET, VB.NET, SQL Server"
                },
                new WorkExperience
                {
                    Company = "3T Productions",
                    Position = "Web Developer",
                    StartDate = "Jun 2003",
                    EndDate = "Aug 2004",
                    Location = "Lowestoft, UK",
                    Description = "Built dynamic education and government websites using ASP.",
                    Highlights = new List<string>
                    {
                        "Developed dynamic websites for education and government sectors",
                        "Used Classic ASP and early web technologies"
                    },
                    TechStack = "Classic ASP, JavaScript, HTML, CSS"
                }
            },
            Education = new Education
            {
                Higher = new HigherEducation
                {
                    University = "University of Huddersfield",
                    Course = "Interactive Multimedia",
                    Grade = "2:1 (BA Hons)",
                    Dates = "2001 - 2005"
                },
                Secondary = new List<SecondaryEducation>
                {
                    new SecondaryEducation
                    {
                        Institution = "Lowestoft College",
                        Qualification = "BTEC National Diploma - Computer Studies",
                        Grade = "Merit/Distinction",
                        Date = "1999"
                    }
                }
            },
            LeisureActivities = "Outside of work I'm passionate about continuous improvement, whether that's training for endurance events, studying software architecture or experimenting with new technologies. I've completed the London Marathon and regularly compete in running events. I attend DDD and Umbraco community events and maintain personal projects focused on modernising legacy applications using contemporary architectural patterns."
        };

        return Task.FromResult(cvData);
    }
}
